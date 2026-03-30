using System.Security.Claims;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PTController(UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager, ITrainingPlanService _trainingPlanService, INutritionPlanService _nutritionPlanService) : BaseApiController
    {
        /// <summary>
        /// Obtém todos os clientes atribuídos ao Personal Trainer autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/PT/clients
        /// Requer role "PT".
        /// </remarks>
        /// <returns>Lista de clientes em formato ClientDto.</returns>
        [HttpGet("clients")]
        [Authorize(Roles = "PT")]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ptId == null)
                return Unauthorized();

            var clients = await _userManager.Users
                .Where(u => u.TrainerId == ptId)
                .Select(u => new ClientDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email ?? string.Empty
                })
                .ToListAsync();

            return Ok(clients);
        }

        /// <summary>
        /// Obtém um cliente específico atribuído ao PT autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/PT/clients/{id}
        /// </remarks>
        /// <param name="id">Identificador do cliente.</param>
        /// <returns>Cliente em formato ClientDto.</returns>
        [HttpGet("clients/{id}")]
        [Authorize(Roles = "PT")]
        public async Task<ActionResult<ClientDto>> GetClientById(string id)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(ptId))
                return Unauthorized();
            var client = await _userManager.Users
                .Where(u => u.TrainerId == ptId && u.Id == id)
                .Select(u => new ClientDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email ?? string.Empty
                })
                .FirstOrDefaultAsync();

            if (client == null)
                return Unauthorized("Você não tem acesso a esse cliente.");

            return Ok(client);
        }

        /// <summary>
        /// Cria um novo cliente e associa ao Personal Trainer autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: POST /api/PT/clients
        /// Requer role "PT".
        /// </remarks>
        /// <param name="createClientDto">Dados do cliente a ser criado.</param>
        /// <returns>Cliente criado em formato ClientDto.</returns>
        [HttpPost("clients")]
        [Authorize(Roles = "PT")]
        public async Task<ActionResult<ClientDto>> CreateClient([FromBody] CreateClientDto createClientDto)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized("PT não autenticado.");

            var pt = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == ptId);

            if (pt == null)
                return Unauthorized("PT não encontrado.");

            var emailExists = await _userManager.FindByEmailAsync(createClientDto.Email);
            if (emailExists != null)
                return BadRequest("Já existe um utilizador com esse email.");

            var clientRole = "Client";

            if (!await _roleManager.RoleExistsAsync(clientRole))
                return BadRequest("A role Client não existe.");

            var client = new AppUser
            {
                UserName = createClientDto.Email,
                Email = createClientDto.Email,
                Name = createClientDto.Name,
                TrainerId = pt.Id,
                SuperAdminId = pt.SuperAdminId
            };

            var result = await _userManager.CreateAsync(client, createClientDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var addToRoleResult = await _userManager.AddToRoleAsync(client, clientRole);

            if (!addToRoleResult.Succeeded)
                return BadRequest(addToRoleResult.Errors);

            var clientDto = new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email ?? string.Empty
            };

            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, clientDto);
        }

        /// <summary>
        /// Atualiza os dados de um cliente atribuído ao PT autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: PUT /api/PT/clients/{id}
        /// Requer role "PT".
        /// </remarks>
        /// <param name="id">Identificador do cliente a ser atualizado.</param>
        /// <param name="updateClientDto">Dados atualizados do cliente.</param>
        /// <returns>Cliente atualizado em formato ClientDto.</returns>
        [HttpPut("clients/{id}")]
        [Authorize(Roles = "PT")]
        public async Task<IActionResult> UpdateClient(string id, [FromBody] UpdateClientDto updateClientDto)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var client = await _userManager.Users
                .Where(u => u.TrainerId == ptId && u.Id == id)
                .FirstOrDefaultAsync();

            if (client == null)
                return Unauthorized("Você não tem acesso a esse cliente.");

            client.Name = updateClientDto.Name ?? client.Name;
            client.Email = updateClientDto.Email ?? client.Email;

            var result = await _userManager.UpdateAsync(client);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email ?? string.Empty
            });
        }

        /// <summary>
        /// Remove um cliente atribuído ao PT autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: DELETE /api/PT/clients/{id}
        /// Requer role "PT".
        /// </remarks>
        /// <param name="id">Identificador do cliente a ser removido.</param>
        /// <returns>204 No Content se removido com sucesso.</returns>
        [HttpDelete("clients/{id}")]
        [Authorize(Roles = "PT")]
        public async Task<IActionResult> DeleteClient(string id)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var client = await _userManager.Users
                .Where(u => u.TrainerId == ptId && u.Id == id)
                .FirstOrDefaultAsync();

            if (client == null)
                return Unauthorized("Você não tem acesso a esse cliente.");

            var result = await _userManager.DeleteAsync(client);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return NoContent();
        }

        /// <summary>
        /// Obtém todos os planos de treino de um cliente atribuído ao PT autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/PT/training/{clientId}
        /// Requer role "PT".
        /// </remarks>
        /// <param name="clientId">Identificador do cliente.</param>
        /// <returns>Lista de planos de treino do cliente.</returns>
        [HttpGet("training/{clientId}")]
        [Authorize(Roles = "PT")]
        public async Task<ActionResult<TrainingPlanDto>> GetTrainingPlan(string clientId)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized();

            var clientBelongsToPt = await _userManager.Users
                .AnyAsync(u => u.Id == clientId && u.TrainerId == ptId);

            if (!clientBelongsToPt)
                return Unauthorized("Você não tem acesso a esse cliente.");

            var trainingPlan = await _trainingPlanService.GetTrainingPlanForClient(clientId);

            if (trainingPlan == null)
                return NotFound("Plano de treino não encontrado.");

            return Ok(trainingPlan);
        }

        /// <summary>
        /// Obtém todos os planos de nutrição de um cliente atribuídos ao PT autenticado.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/PT/nutrition/{clientId}
        /// Requer role "PT".
        /// </remarks>
        /// <param name="clientId">Identificador do cliente.</param>
        /// <returns>Lista de planos de nutrição do cliente.</returns>
        [HttpGet("nutrition/{clientId}")]
        [Authorize(Roles = "PT")]
        public async Task<ActionResult<IEnumerable<NutritionPlanDto>>> GetNutritionPlansForClient(string clientId)
        {
            var ptId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ptId))
                return Unauthorized("Usuário PT não está autenticado.");

            var nutritionPlans = await _nutritionPlanService.GetAllNutritionPlans(ptId, clientId);

            if (nutritionPlans == null || !nutritionPlans.Any())
                return NotFound("Plano de nutrição não encontrado para o cliente.");

            return Ok(nutritionPlans);
        }
    }

}