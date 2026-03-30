using System.Collections.Specialized;
using System.Data.SqlTypes;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsável pela administração de utilizadores e roles.
    /// Acesso restrito a utilizadores com role SuperAdmin.
    /// </summary>
    [Authorize(Roles = "SuperAdmin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController(
        UserManager<AppUser> _userManager,
        RoleManager<IdentityRole> _roleManager) : BaseApiController
    {
        /// <summary>
        /// Retorna todos os utilizadores cadastrados no sistema.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/admin/users
        /// </remarks>
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(u => new MemberDto
                {
                    Id = u.Id,
                    DisplayName = u.Name,
                    Email = u.Email ?? string.Empty
                })
                .ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// Retorna um utilizador específico pelo Id.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/admin/users/{id}
        /// </remarks>
        /// <param name="id">Id do utilizador.</param>
        [HttpGet("users/{id}")]
        public async Task<ActionResult<MemberDto>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var memberDto = new MemberDto
            {
                Id = user.Id,
                DisplayName = user.Name,
                Email = user.Email ?? string.Empty
            };

            return Ok(memberDto);
        }

        /// <summary>
        /// Cria um novo utilizador.
        /// </summary>
        /// <remarks>
        /// Rota: POST /api/admin/users
        /// </remarks>
        /// <param name="createUserDto">Dados do utilizador a criar.</param>
        [HttpPost("users")]
        public async Task<ActionResult<MemberDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var emailExists = await _userManager.FindByEmailAsync(createUserDto.Email);
            if (emailExists != null)
                return BadRequest("Já existe um utilizador com esse email.");

            if (!await _roleManager.RoleExistsAsync(createUserDto.Role))
                return BadRequest("Role inválida.");

            var user = new AppUser
            {
                UserName = createUserDto.Email,
                Email = createUserDto.Email,
                Name = createUserDto.Name
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var addToRoleResult = await _userManager.AddToRoleAsync(user, createUserDto.Role);

            if (!addToRoleResult.Succeeded)
                return BadRequest(addToRoleResult.Errors);

            var memberDto = new MemberDto
            {
                Id = user.Id,
                DisplayName = user.Name,
                Email = user.Email ?? string.Empty
            };

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, memberDto);
        }

        /// <summary>
        /// Atualiza um utilizador existente pelo Id.
        /// </summary>
        /// <remarks>
        /// Rota: PUT /api/admin/users/{id}
        /// </remarks>
        /// <param name="id">Id do utilizador.</param>
        /// <param name="updateUserDto">Dados atualizados do utilizador.</param>
        [HttpPut("users/{id}")]
        public async Task<ActionResult<MemberDto>> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            if (!string.IsNullOrWhiteSpace(updateUserDto.Email) &&
                !string.Equals(user.Email, updateUserDto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailInUse = await _userManager.FindByEmailAsync(updateUserDto.Email);
                if (emailInUse != null && emailInUse.Id != user.Id)
                    return BadRequest("Já existe outro utilizador com esse email.");

                user.Email = updateUserDto.Email;
                user.UserName = updateUserDto.Email;
            }

            if (!string.IsNullOrWhiteSpace(updateUserDto.Name))
                user.Name = updateUserDto.Name;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var memberDto = new MemberDto
            {
                Id = user.Id,
                DisplayName = user.Name,
                Email = user.Email ?? string.Empty
            };

            return Ok(memberDto);
        }

        /// <summary>
        /// Remove um utilizador pelo Id.
        /// </summary>
        /// <remarks>
        /// Rota: DELETE /api/admin/users/{id}
        /// </remarks>
        /// <param name="id">Id do utilizador.</param>
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return NoContent();
        }

        /// <summary>
        /// Lista todas as roles existentes.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/admin/roles
        /// </remarks>
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles
                .Select(role => role.Name)
                .Where(name => name != null)
                .ToList();

            return Ok(roles);
        }

        /// <summary>
        /// Cria uma nova role.
        /// </summary>
        /// <remarks>
        /// Rota: POST /api/admin/roles
        /// </remarks>
        /// <param name="dto">Nome da role a criar.</param>
        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RoleName))
                return BadRequest("O nome da role é obrigatório.");

            if (await _roleManager.RoleExistsAsync(dto.RoleName))
                return BadRequest("Role já existe.");

            var result = await _roleManager.CreateAsync(new IdentityRole(dto.RoleName));

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"Role '{dto.RoleName}' criada com sucesso.");
        }

        /// <summary>
        /// Atualiza o nome de uma role.
        /// </summary>
        /// <remarks>
        /// Rota: PUT /api/admin/roles
        /// </remarks>
        /// <param name="dto">Id da role e novo nome.</param>
        [HttpPut("roles")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(dto.RoleId);

            if (role == null)
                return NotFound("Role não encontrada.");

            var existingRole = await _roleManager.FindByNameAsync(dto.NewName);
            if (existingRole != null && existingRole.Id != role.Id)
                return BadRequest("Já existe uma role com esse nome.");

            role.Name = dto.NewName;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"Role '{dto.RoleId}' atualizada para '{dto.NewName}' com sucesso.");
        }

        /// <summary>
        /// Remove uma role pelo Id.
        /// </summary>
        /// <remarks>
        /// Rota: DELETE /api/admin/roles/{roleId}
        /// </remarks>
        /// <param name="roleId">Id da role.</param>
        [HttpDelete("roles/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return NotFound("Role não encontrada.");

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"Role '{role.Name}' removida com sucesso.");
        }

        /// <summary>
        /// Atribui uma role a um utilizador.
        /// </summary>
        /// <remarks>
        /// Rota: POST /api/admin/assign-role
        /// </remarks>
        /// <param name="dto">Id do utilizador e role a atribuir.</param>
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            if (!await _roleManager.RoleExistsAsync(dto.Role))
                return BadRequest("Role inválida.");

            var alreadyInRole = await _userManager.IsInRoleAsync(user, dto.Role);
            if (alreadyInRole)
                return BadRequest("O utilizador já possui essa role.");

            var result = await _userManager.AddToRoleAsync(user, dto.Role);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"Role '{dto.Role}' atribuída ao utilizador '{user.UserName}'.");
        }

        /// <summary>
        /// Remove uma role de um utilizador.
        /// </summary>
        /// <remarks>
        /// Rota: POST /api/admin/remove-role
        /// </remarks>
        /// <param name="dto">Id do utilizador e role a remover.</param>
        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] RemoveRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var isInRole = await _userManager.IsInRoleAsync(user, dto.Role);
            if (!isInRole)
                return BadRequest("O utilizador não possui essa role.");

            var result = await _userManager.RemoveFromRoleAsync(user, dto.Role);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"Role '{dto.Role}' removida do utilizador '{user.UserName}'.");
        }

        /// <summary>
        /// Retorna todas as roles de um utilizador.
        /// </summary>
        /// <remarks>
        /// Rota: GET /api/admin/users/{id}/roles
        /// </remarks>
        /// <param name="id">Id do utilizador.</param>
        [HttpGet("users/{id}/roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("Usuario não encontrado!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

    }
}