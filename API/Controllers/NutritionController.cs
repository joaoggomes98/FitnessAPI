using System.Security.Claims;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão de planos de nutrição.
    /// Permite criar, consultar, atualizar e remover planos alimentares.
    /// </summary>
    [Authorize]
    public class NutritionController(INutritionPlanService _nutritionPlanService) : BaseApiController
    {
        /// <summary>
        /// Endpoint GET para obter todos os planos de nutrição de um cliente criados por um PT.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/nutrition/client/{clientId}
        /// </remarks>
        /// <param name="clientId">Identificador do cliente.</param>
        /// <returns>Lista de planos de nutrição.</returns>
        [Authorize(Roles = "PT")]
        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<IEnumerable<NutritionPlanDto>>> GetAllNutritionPlansByClient(string clientId)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized("Personal Trainer não identificado.");

            var plans = await _nutritionPlanService.GetAllNutritionPlans(ptId, clientId);

            if (plans == null || !plans.Any())
                return NotFound("Nenhum plano de nutrição encontrado para este cliente.");

            return Ok(plans);
        }

        /// <summary>
        /// Endpoint GET para obter todos os planos de nutrição associados a um cliente.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/nutrition/user/{clientId}
        /// </remarks>
        /// <param name="clientId">Identificador do cliente.</param>
        /// <returns>Lista de planos de nutrição.</returns>
        [Authorize(Roles = "Client")]
        [HttpGet("my-plans")]
        public async Task<ActionResult<IEnumerable<NutritionPlanDto>>> GetNutritionPlansForUser()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(clientId))
                return Unauthorized("Cliente não identificado.");

            var plans = await _nutritionPlanService.GetNutritionPlanForClient(clientId);

            if (plans == null || !plans.Any())
                return NotFound("Nenhum plano de nutrição encontrado para este utilizador.");

            return Ok(plans);
        }
        /// <summary>
        /// Endpoint POST para criar um novo plano de nutrição para um cliente.
        /// </summary>
        /// <remarks>
        /// Rota: POST /api/nutrition
        /// Requer autenticação do Personal Trainer.
        /// </remarks>
        /// <param name="dto">Dados necessários para criação do plano de nutrição.</param>
        /// <returns>Plano de nutrição criado.</returns>
        [Authorize(Roles = "PT")]
        [HttpPost("{clientId}")]
        public async Task<ActionResult<NutritionPlanDto>> CreatePlan(string clientId, [FromBody] CreateNutritionPlanDto dto)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized("Personal Trainer não identificado.");

            var plan = await _nutritionPlanService.CreateNutritionPlan(ptId,clientId,    dto);

            return CreatedAtAction(nameof(GetAllNutritionPlansByClient), new { clientId = plan.ClientId }, plan);
        }
        /// <summary>
        /// Endpoint PUT para atualizar um plano de nutrição existente.
        /// </summary>
        /// <remarks>
        /// Rota: PUT /api/nutrition/{planId}
        /// </remarks>
        /// <param name="planId">Identificador do plano de nutrição.</param>
        /// <param name="dto">Dados atualizados do plano.</param>
        /// <returns>Plano de nutrição atualizado.</returns>
        [Authorize(Roles = "PT")]
        [HttpPut("{planId}")]
        public async Task<ActionResult<NutritionPlanDto>> UpdatePlan(int planId, [FromBody] UpdateNutritionPlanDto dto)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized("Personal Trainer não identificado.");

            var plan = await _nutritionPlanService.UpdateNutritionPlan(ptId, planId, dto);

            if (plan == null)
                return NotFound("Plano de nutrição não encontrado.");

            return Ok(plan);
        }

        /// <summary>
        /// Endpoint DELETE para remover um plano de nutrição.
        /// </summary>
        /// <remarks>
        /// Rota: DELETE /api/nutrition/{planId}
        /// </remarks>
        /// <param name="planId">Identificador do plano de nutrição.</param>
        /// <returns>NoContent se removido com sucesso.</returns>
        [Authorize(Roles = "PT")]
        [HttpDelete("{planId}")]
        public async Task<IActionResult> DeletePlan(int planId)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized("Personal Trainer não identificado.");

            var success = await _nutritionPlanService.DeleteNutritionPlan(ptId, planId);

            if (!success)
                return NotFound("Plano de nutrição não encontrado.");

            return NoContent();
        }
    }
}