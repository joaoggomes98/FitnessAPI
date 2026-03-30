using System;

namespace API.DTOs;

public class UpdateRoleDto
{
    public string RoleId { get; set; } = null!;
    public string NewName { get; set; } = null!;
}
