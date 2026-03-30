using System;

namespace API.DTOs;

public class RemoveRoleDto
{
    public string UserId { get; set; }  // ID do usuário do qual a role será removida
    public string Role { get; set; }  // Nome da role a ser removida
}
