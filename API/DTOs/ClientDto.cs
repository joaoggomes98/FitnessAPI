using System;

namespace API.DTOs;

public class ClientDto
{
    public string Id { get; set; }  // ID do cliente (identificador único, geralmente uma string)
    public string Name { get; set; }  // Nome do cliente
    public string Email { get; set; }  // Email do cliente
    public string DisplayName { get; set; }  // Nome exibido do cliente (caso diferente do Name)
    public string Role { get; set; }  // Role (no caso de cliente será "Client")
    public DateTime CreatedAt { get; set; }  // Data de criação do usuário
}
