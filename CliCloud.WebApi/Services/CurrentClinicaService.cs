using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Domain.Entities.Core;
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
      // fallback: claim do JWT gerado por TokenService: "clinica_id"
      string? claimClinicaId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue("clinica_id");
      ClinicaId = claimClinicaId;

      try
      {
        // legado: quando alguém altera o pordefeito, o GET/PUT /current deve refletir isso imediatamente
        var defaultClinica = (await _repository
            .GetListAsync<Clinica, Guid>(new ClinicaPorDefeitoSelected()))
          .FirstOrDefault();

        if (defaultClinica != null)
          ClinicaId = defaultClinica.Id.ToString();
      }
      catch
      {
        // Se ainda não existir por defeito, mantemos fallback do JWT.
      }
    }

    public string? ClinicaId { get; set; }
  }
}

