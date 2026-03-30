using System;

namespace API.DTOs;

public class UpdateClientDto
{
    public string Name { get; set; }  // Nome do cliente
    public string Email { get; set; }  // Novo email do cliente (opcional)
}
