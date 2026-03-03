using System;

namespace API.DTOs;

public class MemberDto
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required string DisplayName { get; set; } 
}
