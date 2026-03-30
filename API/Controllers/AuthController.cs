
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller responsável por autenticação de usuários (login e registro).
    /// </summary>
    public class AuthController(ITokenService tokenService, UserManager<AppUser> _userManager, IMapper _mapper) : BaseApiController
    {
        /// <summary>
        /// Endpoint POST para autenticar um usuário (login).
        /// Recebe LoginDto com Email e Password.
        /// Retorna UserDto com token JWT se autenticado com sucesso.
        /// </summary>
        /// <remarks>
        /// Rota: POST /api/auth/login
        /// </remarks>
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized("Invalid email address");

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordValid) return Unauthorized("Invalid password");

            var userDto = _mapper.Map<UserDto>(user);

            userDto.Token = await tokenService.CreateToken(user);

            var roles = await _userManager.GetRolesAsync(user);
            userDto.Role = roles.FirstOrDefault() ?? string.Empty;

            return Ok(userDto);
        }
        /// <summary>
        /// Endpoint POST para registrar um novo usuário.
        /// Recebe RegisterDto com Name, Email e Password.
        /// Retorna UserDto com token JWT se registro bem-sucedido.
        /// </summary>
        /// <remarks>
        /// Rota: POST /api/auth/register
        /// </remarks>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            // Verifica se o email já existe
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                return BadRequest("Email taken");

            // Cria o AppUser
            var user = new AppUser
            {
                Email = registerDto.Email ?? throw new ArgumentNullException("Email required"),
                UserName = registerDto.Email ?? throw new ArgumentNullException("Email required"),
                Name = registerDto.Name
            };

            // Cria o usuário com UserManager (gera hash da senha automaticamente)
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Opcional: atribuir role
            await _userManager.AddToRoleAsync(user, "Client");

            // Mapear para UserDto
            var userDto = _mapper.Map<UserDto>(user);

            // Usar await no método assíncrono
            userDto.Token = await tokenService.CreateToken(user);

            return Ok(userDto);
        }
    }
}
