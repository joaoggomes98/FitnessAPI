using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    /// <summary>
    /// Serviço responsável pela gestão dos planos de nutrição.
    /// Contém a lógica de negócio para criação, atualização, remoção e consulta de planos.
    /// </summary>
    public class NutritionPlanService (AppDbContext _context,IMapper _mapper): INutritionPlanService
    {
        /// <summary>
        /// Cria um novo plano de nutrição para um cliente.
        /// </summary>
        /// <param name="ptId">Identificador do Personal Trainer que cria o plano.</param>
        /// <param name="dto">Dados necessários para criação do plano de nutrição.</param>
        /// <returns>O plano de nutrição criado em formato DTO.</returns>
        /// <exception cref="Exception">Lançada quando o cliente não é encontrado.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// Lançada quando o cliente não pertence ao Personal Trainer.
        /// </exception>
        public async Task<NutritionPlanDto> CreateNutritionPlan(string ptId, string clientId, CreateNutritionPlanDto dto)
        {
            var client = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == clientId);

            if (client == null)
                throw new Exception("Client not found");

            // verificar se pertence ao PT
            if (client.TrainerId != ptId)
                throw new UnauthorizedAccessException("Client does not belong to this PT");

            var nutritionPlan = new NutritionPlan
            {
                Title = dto.Title,
                Description = dto.Description,
                ClientId = clientId,
                CreatedByPtId = ptId,
                CreatedAt = DateTime.UtcNow
            };

            _context.NutritionPlans.Add(nutritionPlan);
            await _context.SaveChangesAsync();

            return _mapper.Map<NutritionPlanDto>(nutritionPlan);
        }

        /// <summary>
        /// Atualiza um plano de nutrição existente.
        /// </summary>
        /// <param name="ptId">Identificador do Personal Trainer responsável pelo plano.</param>
        /// <param name="planId">Identificador do plano de nutrição.</param>
        /// <param name="dto">Dados atualizados do plano.</param>
        /// <returns>O plano atualizado em formato DTO ou null se não for encontrado.</returns>
        /// <exception cref="UnauthorizedAccessException">
        /// Lançada quando o PT não tem permissão para editar o plano.
        /// </exception>
        public async Task<NutritionPlanDto?> UpdateNutritionPlan(string ptId, int planId, UpdateNutritionPlanDto dto)
        {
            var plan = await _context.NutritionPlans
                .FirstOrDefaultAsync(np => np.Id == planId);

            if (plan == null)
                return null;

            if (plan.CreatedByPtId != ptId)
                throw new UnauthorizedAccessException("You do not have permission to edit this plan");

            plan.Title = dto.Title ?? plan.Title;
            plan.Description = dto.Description ?? plan.Description;

            await _context.SaveChangesAsync();

            return _mapper.Map<NutritionPlanDto>(plan);
        }

        /// <summary>
        /// Remove um plano de nutrição existente.
        /// </summary>
        /// <param name="ptId">Identificador do Personal Trainer responsável pelo plano.</param>
        /// <param name="planId">Identificador do plano de nutrição.</param>
        /// <returns>
        /// True se o plano foi removido com sucesso; False se o plano não foi encontrado.
        /// </returns>
        /// <exception cref="UnauthorizedAccessException">
        /// Lançada quando o PT não tem permissão para remover o plano.
        /// </exception>
        public async Task<bool> DeleteNutritionPlan(string ptId, int planId)
        {
            var plan = await _context.NutritionPlans
                .FirstOrDefaultAsync(np => np.Id == planId);

            if (plan == null)
                return false;

            if (plan.CreatedByPtId != ptId)
                throw new UnauthorizedAccessException("You do not have permission to delete this plan");

            _context.NutritionPlans.Remove(plan);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Obtém todos os planos de nutrição de um cliente criados por um determinado Personal Trainer.
        /// </summary>
        /// <param name="ptId">Identificador do Personal Trainer.</param>
        /// <param name="clientId">Identificador do cliente.</param>
        /// <returns>Lista de planos de nutrição em formato DTO.</returns>
        public async Task<IEnumerable<NutritionPlanDto>> GetAllNutritionPlans(string ptId, string clientId)
        {
            var plans = await _context.NutritionPlans
                .Where(np => np.ClientId == clientId && np.CreatedByPtId == ptId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NutritionPlanDto>>(plans);
        }

        /// <summary>
        /// Obtém todos os planos de nutrição associados a um cliente.
        /// </summary>
        /// <param name="clientId">Identificador do cliente.</param>
        /// <returns>Lista de planos de nutrição do cliente em formato DTO.</returns>
        public async Task<IEnumerable<NutritionPlanDto>> GetNutritionPlanForClient(string clientId)
        {
            var plans = await _context.NutritionPlans
                .Where(np => np.ClientId == clientId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NutritionPlanDto>>(plans);
        }
    }
}