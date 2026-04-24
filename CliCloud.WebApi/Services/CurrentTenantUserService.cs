using System.Security.Claims;
using CliCloud.Application.Common;

namespace CliCloud.WebApi.Services
{
  public class CurrentTenantUserService : ICurrentTenantUserService
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTenantUserService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public void SetUser()
    {
      var user = _httpContextAccessor?.HttpContext?.User;
      if (user == null)
      {
        UserId = null;
        return;
      }

      // Prefer Identity user id (teleconsulta, Medico.IdUtilizador, etc.). Fallback: legacy tokens só com uid (= clínica).
      string? aspNet = user.FindFirstValue("aspnet_user_id");
      if (string.IsNullOrWhiteSpace(aspNet))
      {
        foreach (var c in user.Claims)
        {
          if (string.Equals(c.Type, "aspnet_user_id", StringComparison.OrdinalIgnoreCase))
          {
            aspNet = c.Value;
            break;
          }
        }
      }

      UserId = !string.IsNullOrWhiteSpace(aspNet) ? aspNet.Trim() : user.FindFirstValue("uid");
    }

    public string? UserId { get; set; }
  }
}
