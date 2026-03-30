using System.Security.Claims;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de planos de treino.
    /// Permite criar, consultar, atualizar e remover planos.
    /// </summary>
    [Authorize]
    public class TrainingController(ITrainingPlanService _trainingPlanService) : BaseApiController
    {
        /// <summary>
        /// Endpoint GET para obter todos os planos de treino do cliente autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/training/my-plans
        /// </remarks>
        /// <returns>Lista de planos de treino.</returns>
        [Authorize(Roles = "Client")]
        [HttpGet("my-plans")]
        public async Task<ActionResult<IEnumerable<TrainingPlanDto>>> GetMyTrainingPlans()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Cliente não identificado.");

            var plans = await _trainingPlanService.GetTrainingPlanForClient(userId);

            if (plans == null || !plans.Any())
                return NotFound("Nenhum plano encontrado para este utilizador.");

            return Ok(plans);
        }

        /// <summary>
        /// Endpoint GET para obter todos os planos de treino de um cliente atribuídos ao PT autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/training/pt/{clientId}
        /// </remarks>
        /// <param name="clientId">Identificador do cliente.</param>
        /// <returns>Lista de planos de treino.</returns>
        [Authorize(Roles = "PT")]
        [HttpGet("pt/{clientId}")]
        public async Task<ActionResult<IEnumerable<TrainingPlanDto>>> GetAllTrainingPlansByClient(string clientId)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized("Personal Trainer não identificado.");

            var plans = await _trainingPlanService.GetAllTrainingPlans(ptId, clientId);

            if (plans == null || !plans.Any())
                return NotFound("Nenhum plano de treino encontrado para este cliente.");

            return Ok(plans);
        }

        /// <summary>
        /// Endpoint POST para criar um novo plano de treino para um cliente.
        /// </summary>
        /// <remarks>
        /// Rota: POST /api/training/{clientId}
        /// Requer autenticação do Personal Trainer.
        /// </remarks>
        /// <param name="clientId">Identificador do cliente que receberá o plano.</param>
        /// <param name="dto">Dados necessários para criação do plano.</param>
        /// <returns>Plano de treino criado.</returns>
        [Authorize(Roles = "PT")]
        [HttpPost("{clientId}")]
        public async Task<ActionResult<TrainingPlanDto>> CreatePlan(string clientId, [FromBody] CreateTrainingPlanDto dto)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized("Personal Trainer não identificado.");

            var plan = await _trainingPlanService.CreateTrainingPlan(ptId, clientId, dto);

            return Ok(plan);
        }

        /// <summary>
        /// Endpoint PUT para atualizar um plano de treino existente.
        /// </summary>
        /// <remarks>
        /// Rota: PUT /api/training/{planId}/client/{clientId}
        /// </remarks>
        /// <param name="planId">Identificador do plano de treino.</param>
        /// <param name="clientId">Identificador do cliente dono do plano.</param>
        /// <param name="dto">Dados atualizados do plano.</param>
        /// <returns>Plano de treino atualizado.</returns>
        [Authorize(Roles = "PT")]
        [HttpPut("{planId}/client/{clientId}")]
        public async Task<ActionResult<TrainingPlanDto>> UpdatePlan(int planId, string clientId, [FromBody] UpdateTrainingPlanDto dto)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized("Personal Trainer não identificado.");

            var plan = await _trainingPlanService.UpdateTrainingPlan(ptId, clientId, planId, dto);

            if (plan == null)
                return NotFound("Plano de treino não encontrado.");

            return Ok(plan);
        }

        /// <summary>
        /// Endpoint DELETE para remover um plano de treino.
        /// </summary>
        /// <remarks>
        /// Rota: DELETE /api/training/{planId}/client/{clientId}
        /// </remarks>
        /// <param name="planId">Identificador do plano de treino.</param>
        /// <param name="clientId">Identificador do cliente dono do plano.</param>
        /// <returns>NoContent se removido com sucesso.</returns>
        [Authorize(Roles = "PT")]
        [HttpDelete("{planId}/client/{clientId}")]
        public async Task<IActionResult> DeletePlan(int planId, string clientId)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized("Personal Trainer não identificado.");

            var success = await _trainingPlanService.DeleteTrainingPlan(ptId, clientId, planId);

            if (!success)
                return NotFound("Plano de treino não encontrado.");

            return NoContent();
        }
    }
}