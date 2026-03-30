using Microsoft.AspNetCore.Identity;

namespace API.Models;

/// <summary>
/// Entidade que representa a relação entre Usuário e Role.
/// Herdando IdentityUserRole para integração com Identity.
/// </summary>
public class AppUserRole : IdentityUserRole<string>
{
    /// <summary>
    /// Usuário associado a esta Role.
    /// </summary>
    public AppUser User { get; set; } = null!;

    /// <summary>
    /// Role associada ao usuário.
    /// </summary>
    public AppRole Role { get; set; } = null!;
}