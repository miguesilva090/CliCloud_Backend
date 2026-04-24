using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Domain.Entities.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CliCloud.WebApi.Services
{
  public class CurrentClinicaService : ICurrentClinicaService
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepositoryAsync _repository;

    public CurrentClinicaService(
      IHttpContextAccessor httpContextAccessor,
      IRepositoryAsync repository
    )
    {
      _httpContextAccessor = httpContextAccessor;
      _repository = repository;
    }

    public async Task SetClinicaAsync()
    {
      // Ordem alinhada ao TokenService.ResolveClinicaForTokenAsync: BD primeiro, depois JWT.
      // Tokens emitidos pelo serviço de licenças podem não trazer "clinica_id".
      ClinicaId = null;

      try
      {
        var defaultClinica = (await _repository
            .GetListAsync<Clinica, Guid>(new ClinicaPorDefeitoSelected()))
          .FirstOrDefault();

        if (defaultClinica != null)
          ClinicaId = defaultClinica.Id.ToString();
      }
      catch
      {
      }

      if (string.IsNullOrWhiteSpace(ClinicaId) || !Guid.TryParse(ClinicaId, out _))
      {
        string? fromClaims = ResolveClinicaIdFromClaims(_httpContextAccessor?.HttpContext?.User);
        if (!string.IsNullOrWhiteSpace(fromClaims) && Guid.TryParse(fromClaims.Trim(), out _))
          ClinicaId = fromClaims.Trim();
      }

      if (string.IsNullOrWhiteSpace(ClinicaId) || !Guid.TryParse(ClinicaId, out _))
      {
        try
        {
          var anyClinica = (await _repository
              .GetListAsync<Clinica, Guid>(new ClinicaFallbackParaContextoAtualSpec()))
            .FirstOrDefault();

          if (anyClinica != null)
            ClinicaId = anyClinica.Id.ToString();
        }
        catch
        {
        }
      }
    }

    /// <summary>
    /// Lê Guid de clínica de claims (CliCloud + possíveis nomes do Access Control / licenças).
    /// </summary>
    private static string? ResolveClinicaIdFromClaims(ClaimsPrincipal? user)
    {
      if (user?.Identities.Any(i => i.IsAuthenticated) != true)
        return null;

      string[] knownTypes =
      [
        "clinica_id",
        "ClinicaId",
        "clinicaId",
        "ClinicId",
        "client_clinica_id",
      ];

      foreach (string t in knownTypes)
      {
        string? v = user.FindFirstValue(t);
        if (!string.IsNullOrWhiteSpace(v) && Guid.TryParse(v.Trim(), out _))
          return v.Trim();
      }

      foreach (Claim c in user.Claims)
      {
        if (string.IsNullOrWhiteSpace(c.Value))
          continue;

        string tv = c.Value.Trim();
        if (!Guid.TryParse(tv, out _))
          continue;

        if (c.Type.EndsWith("clinica_id", StringComparison.OrdinalIgnoreCase)
            || c.Type.EndsWith("ClinicaId", StringComparison.OrdinalIgnoreCase))
          return tv;
      }

      return null;
    }

    public string? ClinicaId { get; set; }
  }
}
