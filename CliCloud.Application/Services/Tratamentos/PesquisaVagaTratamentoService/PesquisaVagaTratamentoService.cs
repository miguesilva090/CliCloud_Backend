using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.PesquisaVagaTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.PesquisaVagaTratamentoService.Specifications;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tratamentos.PesquisaVagaTratamentoService;

public class PesquisaVagaTratamentoService(
  IRepositoryAsync repository,
  IDisponibilidadeTecnicoTratamentoService disponibilidadeService
) : IPesquisaVagaTratamentoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly IDisponibilidadeTecnicoTratamentoService _disponibilidadeService =
    disponibilidadeService;

  public async Task<Response<PesquisaVagaResponse>> PesquisarAsync(PesquisaVagaRequest request)
  {
    string horaAlvo = NormalizarHora(request.Hora);
    if (string.IsNullOrWhiteSpace(horaAlvo))
      return ResponseFactory.Fail<PesquisaVagaResponse>("Hora inválida.");

    if (!request.DiasConsecutivos
        && (request.DiasSemana == null || request.DiasSemana.Count == 0))
    {
      return ResponseFactory.Fail<PesquisaVagaResponse>("Tem de selecionar os dias");
    }

    HashSet<int> diasPermitidos = request.DiasConsecutivos
      ? [0, 1, 2, 3, 4, 5, 6]
      : request.DiasSemana!.Distinct().ToHashSet();

    List<TipoTecnico> tipos = ResolverTipos(request.TiposTecnico);
    IEnumerable<Tecnico> tecnicos = await _repository.GetListAsync<Tecnico, Guid>(
      new TecnicosAtivosByTiposSpec(tipos)
    );

    var fisio = new List<PesquisaVagaTecnicoDTO>();
    var aux = new List<PesquisaVagaTecnicoDTO>();
    var outros = new List<PesquisaVagaTecnicoDTO>();

    foreach (Tecnico tecnico in tecnicos)
    {
      bool ok = await TecnicoConsegueNSessoesAsync(
        tecnico.Id,
        request.DataInicio.Date,
        horaAlvo,
        request.NumeroSessoes,
        request.UnidadeTempo,
        diasPermitidos
      );

      if (!ok)
        continue;

      var dto = new PesquisaVagaTecnicoDTO
      {
        TecnicoId = tecnico.Id,
        Nome = tecnico.Nome ?? string.Empty,
        TipoTecnico = (int)tecnico.TipoTecnico,
      };

      switch (tecnico.TipoTecnico)
      {
        case TipoTecnico.Auxiliar:
          aux.Add(dto);
          break;
        case TipoTecnico.Outro:
          outros.Add(dto);
          break;
        default:
          fisio.Add(dto);
          break;
      }
    }

    return ResponseFactory.Success(
      new PesquisaVagaResponse
      {
        Fisioterapeutas = fisio,
        Auxiliares = aux,
        Outros = outros,
      }
    );
  }

  /// <summary>
  /// Paridade legado: avança dia a dia; em dia permitido, se a hora não couber → rejeita técnico.
  /// Folga clínica / feriado (Response Failure) → salta o dia.
  /// </summary>
  private async Task<bool> TecnicoConsegueNSessoesAsync(
    Guid tecnicoId,
    DateTime dataInicio,
    string horaAlvo,
    int numeroSessoes,
    int unidadeTempo,
    HashSet<int> diasPermitidos
  )
  {
    int obtidas = 0;
    DateTime cursor = dataInicio;
    int limite = Math.Max(60, numeroSessoes * 14);

    for (int i = 0; i < limite && obtidas < numeroSessoes; i++)
    {
      int dow = (int)cursor.DayOfWeek;
      if (!diasPermitidos.Contains(dow))
      {
        cursor = cursor.AddDays(1);
        continue;
      }

      Response<HorasPossiveisTecnicoResponse> horasResult =
        await _disponibilidadeService.GetHorasPossiveisAsync(
          new HorasPossiveisTecnicoRequest
          {
            TecnicoId = tecnicoId,
            Data = cursor,
            UnidadeTempo = unidadeTempo,
          }
        );

      if (horasResult.Status == ResponseStatus.Failure)
      {
        cursor = cursor.AddDays(1);
        continue;
      }

      IReadOnlyList<string> horas = horasResult.Data?.Horas ?? [];
      bool livre = horas.Any(h =>
        string.Equals(NormalizarHora(h), horaAlvo, StringComparison.Ordinal)
      );

      if (!livre)
        return false;

      obtidas++;
      cursor = cursor.AddDays(1);
    }

    return obtidas >= numeroSessoes;
  }

  private static List<TipoTecnico> ResolverTipos(List<int>? tipos)
  {
    if (tipos == null || tipos.Count == 0)
    {
      return
      [
        TipoTecnico.Fisioterapeuta,
        TipoTecnico.Auxiliar,
        TipoTecnico.Outro,
      ];
    }

    return tipos
      .Where(t => Enum.IsDefined(typeof(TipoTecnico), t))
      .Select(t => (TipoTecnico)t)
      .Distinct()
      .ToList();
  }

  private static string NormalizarHora(string hora)
  {
    if (string.IsNullOrWhiteSpace(hora))
      return string.Empty;
    if (TimeSpan.TryParse(hora.Trim(), out TimeSpan ts))
      return $"{(int)ts.TotalHours:00}:{ts.Minutes:00}";
    return hora.Trim();
  }
}
