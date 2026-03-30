using Microsoft.AspNetCore.Identity;

namespace API.Models;

/// <summary>
/// Representa uma Role do sistema, herdando IdentityRole.
/// </summary>
public class AppRole : IdentityRole
{
    /// <summary>
    /// Relação entre usuários e esta role.
    /// </summary>
    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
}