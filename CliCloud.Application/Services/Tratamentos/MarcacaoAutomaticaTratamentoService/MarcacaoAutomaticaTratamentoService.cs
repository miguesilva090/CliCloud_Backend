using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService;
using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.MarcacaoAutomaticaTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.TratamentoService;
using CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs;

namespace CliCloud.Application.Services.Tratamentos.MarcacaoAutomaticaTratamentoService;

public class MarcacaoAutomaticaTratamentoService(
  IListaEsperaTratamentoAdministrativoService listaEsperaService,
  IDisponibilidadeTecnicoTratamentoService disponibilidadeService,
  ITratamentoService tratamentoService
) : IMarcacaoAutomaticaTratamentoService
{
  private readonly IListaEsperaTratamentoAdministrativoService _listaEsperaService = listaEsperaService;
  private readonly IDisponibilidadeTecnicoTratamentoService _disponibilidadeService = disponibilidadeService;
  private readonly ITratamentoService _tratamentoService = tratamentoService;

  public async Task<Response<MarcacaoAutomaticaPreviewResponse>> PreviewAsync(
    MarcacaoAutomaticaPreviewRequest request
  )
  {
    Response<ListaEsperaTratamentoDTO> leResult =
      await _listaEsperaService.GetByIdAsync(request.ListaEsperaTratamentoId);

    if (leResult.Status == ResponseStatus.Failure || leResult.Data is null)
      return ResponseFactory.Fail<MarcacaoAutomaticaPreviewResponse>("Registo da lista de espera não encontrado.");

    ListaEsperaTratamentoDTO le = leResult.Data;

    if (le.OrganismoId is null)
      return ResponseFactory.Fail<MarcacaoAutomaticaPreviewResponse>(
        "Certifique-se que o organismo está preenchido."
      );

    var sessoes = new List<MarcacaoAutomaticaSessaoPreviewDTO>();
    DateTime cursor = request.DataInicio.Date;

    // limite defensivo para não entrar em loop infinito
    int limiteDiasPesquisa = Math.Max(60, request.NumeroSessoes * 10);

    for (int i = 0; i < limiteDiasPesquisa && sessoes.Count < request.NumeroSessoes; i++)
    {
      if (!DiaPermitido(cursor, request.DiasSemanaPermitidos))
      {
        cursor = cursor.AddDays(request.IntervaloDias);
        continue;
      }

      var horasIntersecao = await ObterIntersecaoHorasAsync(request, cursor);
      string? horaEscolhida = horasIntersecao.OrderBy(x => x).FirstOrDefault();

      if (!string.IsNullOrWhiteSpace(horaEscolhida))
      {
        sessoes.Add(new MarcacaoAutomaticaSessaoPreviewDTO
        {
          NumSessao = sessoes.Count + 1,
          Data = cursor,
          HoraInic = horaEscolhida,
        });
      }

      cursor = cursor.AddDays(request.IntervaloDias);
    }

    if (sessoes.Count == 0)
    {
      return ResponseFactory.Fail<MarcacaoAutomaticaPreviewResponse>(
        "Não foi possível encontrar disponibilidade para os critérios selecionados."
      );
    }

    string duracao = le.DuracaoTotal ?? "01:00";

    return ResponseFactory.Success(
      new MarcacaoAutomaticaPreviewResponse
      {
        ListaEsperaTratamentoId = le.Id,
        UtenteNome = le.UtenteNome ?? string.Empty,
        NumeroSessoesPedido = request.NumeroSessoes,
        NumeroSessoesGeradas = sessoes.Count,
        Duracao = duracao,
        Sessoes = sessoes,
      }
    );
  }

  public async Task<Response<Guid>> ConfirmAsync(MarcacaoAutomaticaConfirmRequest request)
  {
    Response<MarcacaoAutomaticaPreviewResponse> preview = await PreviewAsync(request);
    if (preview.Status == ResponseStatus.Failure || preview.Data is null)
    {
      string msg = preview.Messages.Values.SelectMany(x => x).FirstOrDefault()
        ?? "Não foi possível gerar marcação automática.";
      return ResponseFactory.Fail<Guid>(msg);
    }

    Response<ListaEsperaTratamentoDTO> leResult =
      await _listaEsperaService.GetByIdAsync(request.ListaEsperaTratamentoId);

    if (leResult.Status == ResponseStatus.Failure || leResult.Data is null)
      return ResponseFactory.Fail<Guid>("Registo da lista de espera não encontrado.");

    ListaEsperaTratamentoDTO le = leResult.Data;
    if (le.OrganismoId is null)
      return ResponseFactory.Fail<Guid>("Certifique-se que o organismo está preenchido.");

    List<CreateMarcacaoManualServicoItem> servicos = (le.Servicos ?? [])
      .Where(s => s.ServicoId.HasValue && s.ServicoId.Value != Guid.Empty)
      .OrderBy(s => s.Ordem)
      .Select(s => new CreateMarcacaoManualServicoItem
      {
        ServicoId = s.ServicoId!.Value.ToString(),
        Duracao = s.Duracao,
        Ordem = s.Ordem,
        UsaFisioter = request.FisioterapeutaId.HasValue ? 1 : 0,
        UsaAuxiliar = request.AuxiliarId.HasValue ? 1 : 0,
        UsaOutro = request.OutroTecnicoId.HasValue ? 1 : 0,
      })
      .ToList();

    if (servicos.Count == 0)
      return ResponseFactory.Fail<Guid>("Não existem serviços selecionados");

    if (preview.Data.Sessoes.Count == 0)
      return ResponseFactory.Fail<Guid>("Não foram inseridas sessões selecionados");

    var createRequest = new CreateMarcacaoManualTratamentoRequest
    {
      ListaEsperaTratamentoId = le.Id.ToString(),
      UtenteId = le.UtenteId.ToString(),
      OrganismoId = le.OrganismoId.Value.ToString(),
      MedicoId = le.MedicoId?.ToString(),
      FisioterapeutaId = request.FisioterapeutaId?.ToString(),
      AuxiliarId = request.AuxiliarId?.ToString(),
      OutroTecnicoId = request.OutroTecnicoId?.ToString(),
      LocalTratamentoId = le.LocalTratamentoId?.ToString(),
      LocalOrigemId = le.LocalTratamentoId?.ToString(),
      Designacao = le.Designacao,
      NomePatologia = le.PatologiaDesignacao,
      NumSessao = preview.Data.Sessoes.Count,
      DataInic = preview.Data.Sessoes.Min(s => s.Data),
      DataFim = preview.Data.Sessoes.Max(s => s.Data),
      DuracaoTotal = le.DuracaoTotal,
      Credencial = le.Credencial,
      NFaltMax = le.NFaltMax,
      NFaltComax = le.NFaltComax,
      TaxaMod = le.TaxaModeradora,
      Provisorio = request.Provisorio ? 1 : 0,
      TecObs = le.TecObs,
      CredencialExterna = le.CredencialExterna ? 1 : 0,
      TerapiaFala = 0,
      UnidadeTempoFisio = request.UnidadeTempoFisio,
      UnidadeTempoAux = request.UnidadeTempoAux,
      UnidadeTempoOutro = request.UnidadeTempoOutro,
      Servicos = servicos,
      Sessoes = preview.Data.Sessoes
        .Select(s => new CreateMarcacaoManualSessaoItem
        {
          NumSessao = s.NumSessao,
          Data = s.Data,
          HoraInic = s.HoraInic,
          Duracao = le.DuracaoTotal,
          FisioterapeutaId = request.FisioterapeutaId?.ToString(),
          AuxiliarId = request.AuxiliarId?.ToString(),
          OutroTecnicoId = request.OutroTecnicoId?.ToString(),
        })
        .ToList(),
    };

    return await _tratamentoService.CreateMarcacaoManualAsync(createRequest);
  }

  private async Task<HashSet<string>> ObterIntersecaoHorasAsync(
    MarcacaoAutomaticaPreviewRequest request,
    DateTime data)
  {
    List<HashSet<string>> bolsas = [];

    if (request.FisioterapeutaId.HasValue)
    {
      bolsas.Add(await ObterHorasTecnicoAsync(
        request.FisioterapeutaId.Value,
        data,
        request.UnidadeTempoFisio ?? 1));
    }

    if (request.AuxiliarId.HasValue)
    {
      bolsas.Add(await ObterHorasTecnicoAsync(
        request.AuxiliarId.Value,
        data,
        request.UnidadeTempoAux ?? 1));
    }

    if (request.OutroTecnicoId.HasValue)
    {
      bolsas.Add(await ObterHorasTecnicoAsync(
        request.OutroTecnicoId.Value,
        data,
        request.UnidadeTempoOutro ?? 1));
    }

    if (bolsas.Count == 0) return [];

    HashSet<string> intersecao = new(bolsas[0], StringComparer.Ordinal);
    foreach (HashSet<string> b in bolsas.Skip(1))
      intersecao.IntersectWith(b);

    return intersecao;
  }

  private async Task<HashSet<string>> ObterHorasTecnicoAsync(
    Guid tecnicoId,
    DateTime data,
    int unidadeTempo)
  {
    Response<HorasPossiveisTecnicoResponse> horasResult =
      await _disponibilidadeService.GetHorasPossiveisAsync(new HorasPossiveisTecnicoRequest
      {
        TecnicoId = tecnicoId,
        Data = data,
        UnidadeTempo = unidadeTempo,
      });

    if (horasResult.Status == ResponseStatus.Failure || horasResult.Data is null)
      return [];

    return new HashSet<string>(
      horasResult.Data.Horas.Select(NormalizarHora),
      StringComparer.Ordinal
    );
  }

  private static bool DiaPermitido(DateTime data, List<int>? diasSemanaPermitidos)
  {
    if (diasSemanaPermitidos == null || diasSemanaPermitidos.Count == 0)
      return true;

    int dia = (int)data.DayOfWeek;
    return diasSemanaPermitidos.Contains(dia);
  }

  private static string NormalizarHora(string hora)
  {
    if (TimeSpan.TryParse(hora, out TimeSpan ts))
      return $"{(int)ts.TotalHours:00}:{ts.Minutes:00}";
    return hora.Trim();
  }
}