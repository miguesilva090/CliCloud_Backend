using CliCloud.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Contexts
{
  /// <summary>
  /// DbContext dedicado ao Identity (Auth Server).
  /// Mantém as tabelas AspNet* separadas do ApplicationDbContext de negócio.
  /// </summary>
  public class AuthDbContext(DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole, string>(options)
  {
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // (Opcional) Se quiseres, podes trocar nomes/tabelas/schemas aqui.
      // Por agora usamos os defaults do Identity (AspNetUsers, AspNetRoles, ...).
    }
  }
}

