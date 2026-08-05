using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.Specifications;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService;

public class DisponibilidadeTecnicoTratamentoService(
  IRepositoryAsync repository,
  ICurrentClinicaService currentClinica
) : IDisponibilidadeTecnicoTratamentoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly ICurrentClinicaService _currentClinica = currentClinica;

  public const string MsgFolgaClinica = "Dia de Folga Clínica";
  public const string MsgFeriado = "Dia de Feriado";

  public async Task<Response<UnidadesTempoTecnicoResponse>> GetUnidadesTempoAsync(Guid tecnicoId)
  {
    Tecnico? tecnico = await _repository.GetByIdAsync<Tecnico, Guid>(tecnicoId);
    if (tecnico is null)
    {
      return ResponseFactory.Fail<UnidadesTempoTecnicoResponse>("Técnico não encontrado.");
    }

    int max = Math.Max(1, tecnico.MaxTratamentos);
    List<int> unidades = Enumerable.Range(1, max).ToList();

    return ResponseFactory.Success(
      new UnidadesTempoTecnicoResponse
      {
        TecnicoId = tecnicoId,
        MaxTratamentos = max,
        UnidadesTempo = unidades,
      }
    );
  }

  public async Task<Response<HorasPossiveisTecnicoResponse>> GetHorasPossiveisAsync(
    HorasPossiveisTecnicoRequest request
  )
  {
    Tecnico? tecnico = await _repository.GetByIdAsync<Tecnico, Guid>(request.TecnicoId);
    if (tecnico is null)
    {
      return ResponseFactory.Fail<HorasPossiveisTecnicoResponse>("Técnico não encontrado.");
    }

    int max = Math.Max(1, tecnico.MaxTratamentos);
    if (request.UnidadeTempo < 1 || request.UnidadeTempo > max)
    {
      return ResponseFactory.Fail<HorasPossiveisTecnicoResponse>(
        $"Unidade de tempo inválida. Máximo permitido: {max}."
      );
    }

    Response<HorasPossiveisTecnicoResponse>? bloqueio =
      await TentarBloquearPorFolgaOuFeriadoAsync(request.Data);
    if (bloqueio is not null)
    {
      return bloqueio;
    }

    (string duracao, List<string> horas) =
      await DisponibilidadeTecnicoTratamentoHelper.ObterHorasPossiveisAsync(
        _repository,
        tecnico,
        request.Data,
        request.UnidadeTempo,
        request.IgnorarSessaoId
      );

    return ResponseFactory.Success(
      new HorasPossiveisTecnicoResponse { Duracao = duracao, Horas = horas }
    );
  }

  /// <summary>
  /// Legado: ClinFolg → ReturnErro(MarcacoesErrorFolgaClinica).
  /// Extra newCC: Feriado activo na data da clínica corrente.
  /// </summary>
  private async Task<Response<HorasPossiveisTecnicoResponse>?> TentarBloquearPorFolgaOuFeriadoAsync(
    DateTime data
  )
  {
    await _currentClinica.SetClinicaAsync();

    if (
      string.IsNullOrWhiteSpace(_currentClinica.ClinicaId)
      || !Guid.TryParse(_currentClinica.ClinicaId, out Guid clinicaId)
      || clinicaId == Guid.Empty
    )
    {
      return null;
    }

    Clinica? clinica = await _repository.GetByIdAsync<Clinica, Guid>(clinicaId);
    if (clinica is not null
        && DisponibilidadeTecnicoTratamentoHelper.EhFolgaSemanalClinica(clinica, data))
    {
      return ResponseFactory.Fail<HorasPossiveisTecnicoResponse>(MsgFolgaClinica);
    }

    FeriadoAtivoNaDataSpec feriadoSpec = new(clinicaId, data);
    IEnumerable<Feriado> feriados =
      await _repository.GetListAsync<Feriado, Guid>(feriadoSpec);
    Feriado? feriado = feriados.OrderBy(f => f.Designacao).FirstOrDefault();
    if (feriado is not null)
    {
      string msg = string.IsNullOrWhiteSpace(feriado.Designacao)
        ? MsgFeriado
        : $"{MsgFeriado}: {feriado.Designacao.Trim()}";
      return ResponseFactory.Fail<HorasPossiveisTecnicoResponse>(msg);
    }

    return null;
  }
}