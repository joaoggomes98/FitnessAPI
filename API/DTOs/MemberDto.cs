using System;

namespace API.DTOs;

public class MemberDto
{
    public string Id { get; set; } = string.Empty;
    public required string DisplayName { get; set; }

    public required string Email { get; set; }
}
