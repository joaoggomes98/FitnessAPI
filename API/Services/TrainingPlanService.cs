using API.Controllers;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services;
/// <summary>
/// Serviço responsável pela gestão de planos de treino.
/// Implementa a interface <see cref="ITrainingPlanService"/> e contém
/// a lógica de negócio para criação, atualização, remoção e consulta de planos de treino.
/// </summary>
public class TrainingPlanService(AppDbContext _context, IMapper _mapper) : ITrainingPlanService
{
    /// <summary>
    /// Retorna todos os planos de treino associados a um cliente específico.
    /// </summary>
    /// <param name="clientId">Identificador do cliente.</param>
    /// <returns>Lista de TrainingPlanDto pertencentes ao cliente.</returns>
    public async Task<IEnumerable<TrainingPlanDto>> GetTrainingPlanForClient(string clientId)
    {


        var plans = await _context.TrainingPlans
            .Where(p => p.ClientId == clientId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TrainingPlanDto>>(plans);
    }
    /// <summary>
    /// Retorna todos os planos de treino associados a um personal trainer,
    /// podendo opcionalmente filtrar por cliente.
    /// </summary>
    /// <param name="ptId">Identificador do personal trainer.</param>
    /// <param name="clientId">Identificador do cliente (opcional para filtragem).</param>
    /// <returns>Lista de TrainingPlanDto encontrados.</returns>
    public async Task<IEnumerable<TrainingPlanDto>> GetAllTrainingPlans(string ptId, string clientId)
    {
        // Buscar planos de nutrição do cliente atribuído ao PT
        var plans = await _context.TrainingPlans
            .Where(np => np.ClientId == clientId && np.CreatedByPtId == ptId)
            .ToListAsync();

        // Retornar os planos como DTOs
        return _mapper.Map<IEnumerable<TrainingPlanDto>>(plans);
    }
    /// <summary>
    /// Cria um novo plano de treino para um cliente específico.
    /// </summary>
    /// <param name="ptId">Identificador do personal trainer responsável pelo plano.</param>
    /// <param name="clientId">Identificador do cliente que receberá o plano.</param>
    /// <param name="dto">Dados necessários para criação do plano de treino.</param>
    /// <returns>TrainingPlanDto contendo os dados do plano criado.</returns>
    public async Task<TrainingPlanDto> CreateTrainingPlan(string ptId, string clientId, CreateTrainingPlanDto dto)
    {
        var client = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == clientId);

        if (client == null)
            throw new Exception("Client not found");

        // verificar se pertence ao PT
        if (client.TrainerId != ptId)
            throw new UnauthorizedAccessException("Client does not belong to this trainer");

        var trainingplan = new TrainingPlan
        {
            Title = dto.Title,
            Description = dto.Description,
            ClientId = clientId,
            CreatedByPtId = ptId,
            CreatedAt = DateTime.UtcNow
        };

        _context.TrainingPlans.Add(trainingplan);
        await _context.SaveChangesAsync();

        return _mapper.Map<TrainingPlanDto>(trainingplan);
    }
    /// <summary>
    /// Atualiza um plano de treino existente.
    /// </summary>
    /// <param name="ptId">Identificador do personal trainer responsável pelo plano.</param>
    /// <param name="clientId">Identificador do cliente dono do plano.</param>
    /// <param name="planId">Identificador do plano de treino a ser atualizado.</param>
    /// <param name="dto">Dados atualizados do plano de treino.</param>
    /// <returns>TrainingPlanDto contendo os dados atualizados do plano.</returns>
    public async Task<TrainingPlanDto> UpdateTrainingPlan(string ptId, string clientId, int planId, UpdateTrainingPlanDto dto)
    {
        var plan = await _context.TrainingPlans
            .FirstOrDefaultAsync(p =>
                p.Id == planId &&
                p.ClientId == clientId &&
                p.CreatedByPtId == ptId);

        if (plan == null)
            throw new KeyNotFoundException("Training plan not found");

        plan.Title = dto.Title ?? plan.Title;
        plan.Description = dto.Description ?? plan.Description;

        await _context.SaveChangesAsync();

        return _mapper.Map<TrainingPlanDto>(plan);
    }
    /// <summary>
    /// Remove um plano de treino existente.
    /// </summary>
    /// <param name="ptId">Identificador do personal trainer responsável pelo plano.</param>
    /// <param name="clientId">Identificador do cliente dono do plano.</param>
    /// <param name="planId">Identificador do plano de treino a ser removido.</param>
    /// <returns>True se o plano foi removido com sucesso; caso contrário, False.</returns>
    public async Task<bool> DeleteTrainingPlan(string ptId, string clientId, int planId)
    {
        var plan = await _context.TrainingPlans
            .FirstOrDefaultAsync(p =>
                p.Id == planId &&
                p.ClientId == clientId &&
                p.CreatedByPtId == ptId);

        if (plan == null)
            return false;

        _context.TrainingPlans.Remove(plan);

        await _context.SaveChangesAsync();

        return true;
    }

}