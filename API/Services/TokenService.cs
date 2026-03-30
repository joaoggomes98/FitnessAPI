using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

/// <summary>
/// Serviço responsável por gerar tokens JWT para autenticação de usuários.
/// </summary>
public class TokenService(IConfiguration _config, UserManager<AppUser> _userManager) : ITokenService
{

    /// <summary>
    /// Cria um token JWT para o usuário fornecido, incluindo Id, Email e Roles.
    /// </summary>
    /// <param name="appUser">Usuário para quem o token será gerado.</param>
    /// <returns>Token JWT assinado em formato string.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada se a chave do token não estiver configurada ou for muito curta.
    /// </exception>
    public async Task<string> CreateToken(AppUser appUser)
    {
        // Validar chave JWT
        var tokenKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("Cannot get token key");
        if (tokenKey.Length < 64)
            throw new InvalidOperationException("Your token key needs to be >= 64 characters");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // Criar claims principais
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, appUser.Id),      // Sempre usar Id único
            new(ClaimTypes.Email, appUser.Email ?? string.Empty),
            new(ClaimTypes.Name, appUser.Name ?? string.Empty)
        };

        // Adicionar roles do usuário como claims
        var roles = await _userManager.GetRolesAsync(appUser);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Configurar descriptor do token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(double.TryParse(_config["Jwt:ExpireDays"], out var days) ? days : 7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}