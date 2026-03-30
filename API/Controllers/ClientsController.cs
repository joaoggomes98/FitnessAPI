using System.Security.Claims;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsável pelos endpoints de clientes, retornando planos de treino e nutrição.
    /// </summary>
    [Authorize(Roles = "Client")]
    public class ClientsController(ITrainingPlanService _trainingPlanService, INutritionPlanService _nutritionPlanService) : BaseApiController
    {
        /// <summary>
        /// Endpoint GET para retornar o plano de treino do cliente autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/clients/training
        /// Necessita autenticação (JWT)
        /// </remarks>
        [HttpGet("training-plans")]
        public async Task<ActionResult<TrainingPlanDto>> GetClientTrainingPlan()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Usuário não identificado.");
            }
            var trainingPlan = await _trainingPlanService.GetTrainingPlanForClient(userId);

            if (trainingPlan == null) return NotFound("Plano de treino não encontrado.");

            return Ok(trainingPlan);
        }
        /// <summary>
        /// Endpoint GET para retornar os planos de nutrição do cliente autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/clients/my-plans
        /// Necessita autenticação (JWT)
        /// </remarks>
        [HttpGet("nutrition-plans")]
        public async Task<ActionResult<IEnumerable<NutritionPlanDto>>> GetClientNutritionPlans()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Usuário não identificado.");
            }
            var nutritionPlans = await _nutritionPlanService.GetNutritionPlanForClient(userId);

            if (nutritionPlans == null || !nutritionPlans.Any())
                return NotFound("Nenhum plano de nutrição encontrado.");

            return Ok(nutritionPlans);
        }
    }
}
