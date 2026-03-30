using System;

namespace API.DTOs;

public class UpdateUserDto
{
    public string Name { get; set; }  // Novo nome do usuário (opcional)
    public string Email { get; set; }  // Novo email do usuário (opcional)
    public string Password { get; set; }  // Nova senha do usuário (opcional)
    public string Role { get; set; }  // Nova role do usuário (opcional)
}
