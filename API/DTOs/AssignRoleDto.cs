using System;

namespace API.DTOs;

public class AssignRoleDto
{
    public string UserId { get; set; } = null!;
    public string Role { get; set; } = null!;
}
