using System;

namespace API.DTOs;

public class CreateUserDto
{
    public string Name { get; set; }  // Nome do usuário
    public string Email { get; set; }  // Email do usuário
    public string Password { get; set; }  // Senha do usuário
    public string Role { get; set; }  // Role do usuário (PT, Client)
}
