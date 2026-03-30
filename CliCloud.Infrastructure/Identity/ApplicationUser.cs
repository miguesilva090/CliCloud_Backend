using Microsoft.AspNetCore.Identity;

namespace CliCloud.Infrastructure.Identity
{
  /// <summary>
  /// Utilizador de autenticação (Identity).
  ///
  /// Nota: mantemos o Id como string, mas vamos gerar GUIDs em string para
  /// compatibilidade com o audit (que faz Guid.Parse no CurrentUserId).
  /// </summary>
  public class ApplicationUser : IdentityUser
  {
    public bool IsActive { get; set; } = true;

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
  }
}

