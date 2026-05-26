using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService.DTOs;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;

public class FechoDiarioAdministrativoService(
  IRepositoryAsync repository,
  ICurrentClinicaService currentClinicaService,
  IRequisicaoEspFechoUpdater requisicaoEspFechoUpdater
) : IFechoDiarioAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;
  private readonly IRequisicaoEspFechoUpdater _requisicaoEspFechoUpdater = requisicaoEspFechoUpdater;

  public async Task<Response<FechoDiarioResultDTO>> ExecutarFechoAsync(FechoDiarioRequest request)
  {
    DateTime data = request.Data!.Value.Date;
    Guid clinicaId = await ObterClinicaAtualIdAsync();

    List<Admissao> admissoes = (
      await _repository.GetListAsync<Admissao, Guid>(new AdmissoesParaFechoSpec(data, clinicaId))
    ).ToList();

    var result = new FechoDiarioResultDTO { TotalElegiveis = admissoes.Count };

    if (admissoes.Count == 0)
    {
      return ResponseFactory.Success(result);
    }

    try
    {
      foreach (Admissao admissao in admissoes)
      {
        result.TotalProcessadas++;

        _ = await AdmissaoPromocaoRunner.PromoverAsync(
          admissao,
          _repository,
          _requisicaoEspFechoUpdater
        );
        result.TotalConsultasCriadas++;
      }

      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(result);
    }
    catch (Exception ex)
    {
      _repository.ClearChangeTracker();
      return ResponseFactory.Fail<FechoDiarioResultDTO>($"Fecho diário cancelado: {ex.Message}");
    }
  }

  public async Task<Response<int>> ContarElegiveisAsync(DateTime data)
  {
    Guid clinicaId = await ObterClinicaAtualIdAsync();
    List<Admissao> admissoes = (
      await _repository.GetListAsync<Admissao, Guid>(new AdmissoesParaFechoSpec(data.Date, clinicaId))
    ).ToList();
    return ResponseFactory.Success(admissoes.Count);
  }

  private async Task<Guid> ObterClinicaAtualIdAsync()
  {
    await _currentClinicaService.SetClinicaAsync();
    if (
      Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId)
      && clinicaId != Guid.Empty
    )
    {
      return clinicaId;
    }

    throw new InvalidOperationException("Clínica atual inválida.");
  }
}
