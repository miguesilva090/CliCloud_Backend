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
      ClinicaId = null;

      var httpContext = _httpContextAccessor?.HttpContext;
      var user = httpContext?.User;

      if(user?.Identities.Any(i => i.IsAuthenticated) != true)
        return;
      // Claim UID (User ID)
      var uidRaw = user.FindFirstValue("uid");
      if(string.IsNullOrWhiteSpace(uidRaw) || !Guid.TryParse(uidRaw, out var userIdLicencas))
        return;

      // Header enviado pelo Frontend
      var clientIdRaw = httpContext?.Request?.Headers["X-Client-Id"].FirstOrDefault();
      if(string.IsNullOrWhiteSpace(clientIdRaw) || !Guid.TryParse(clientIdRaw, out var clientIdLicencas))
        return;
  
      // default ativa
      var mapDefault = ( await _repository.GetListAsync<LicencaUserClinicaMap, Guid>(
        new LicencaUserClinicaMapDefaultSpec(clientIdLicencas, userIdLicencas)))
        .FirstOrDefault();
      
      if(mapDefault is not null)
      {
        ClinicaId = mapDefault.ClinicaId.ToString();
        return;
      }

      // fallback: unica ativa
      var mapAtivas = (await _repository.GetListAsync<LicencaUserClinicaMap, Guid>(
        new LicencaUserClinicaMapAtivasSpec(clientIdLicencas, userIdLicencas)))
      .ToList();

      if(mapAtivas.Count == 1)
      { 
        ClinicaId = mapAtivas[0].ClinicaId.ToString();
        return;
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
