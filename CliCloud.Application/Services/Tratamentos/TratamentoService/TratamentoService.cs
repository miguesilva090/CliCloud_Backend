using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos;
using CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.TratamentoService.Filters;
using CliCloud.Application.Services.Tratamentos.TratamentoService.Specifications;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Specifications;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Application.Services.Core.EmailService;
using CliCloud.Application.Services.Core.EmailService.DTOs;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Specifications;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService;
using CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.DTOs;


namespace CliCloud.Application.Services.Tratamentos.TratamentoService
{
  public class TratamentoService : ITratamentoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguracaoEmailService _configuracaoEmailService;
    private readonly IDisponibilidadeTecnicoTratamentoService _disponibilidadeTecnicoTratamentoService;

    public TratamentoService(IRepositoryAsync repository, IMapper mapper, IConfiguracaoEmailService configuracaoEmailService, IDisponibilidadeTecnicoTratamentoService disponibilidadeTecnicoTratamentoService)
    {
      _repository = repository;
      _mapper = mapper;
      _configuracaoEmailService = configuracaoEmailService;
      _disponibilidadeTecnicoTratamentoService = disponibilidadeTecnicoTratamentoService;
    }

    public async Task<Response<IEnumerable<TratamentoDTO>>> GetTratamentoAsync(string keyword = "")
    {
      var spec = new TratamentoSearchList(keyword);
      var list = await _repository.GetListAsync<Tratamento, TratamentoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<TratamentoLightDTO>>> GetTratamentoLightAsync(string keyword = "")
    {
      var spec = new TratamentoSearchList(keyword);
      var list = await _repository.GetListAsync<Tratamento, TratamentoLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<TratamentoTableDTO>> GetTratamentoPaginatedAsync(TratamentoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new TratamentoSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Tratamento, TratamentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<TratamentoTableDTO>>> GetAllTratamentoAsync(TratamentoAllFilter? filter)
    {
      try
      {
        filter ??= new TratamentoAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new TratamentoSearchTable(filters, order);
        var list = await _repository.GetListAsync<Tratamento, TratamentoTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<TratamentoTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<TratamentoDTO>> GetTratamentoAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<Tratamento, TratamentoDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<TratamentoDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateTratamentoAsync(CreateTratamentoRequest request)
    {
      var entity = _mapper.Map<Tratamento>(request);
      try
      {
        TratamentoIntegridadeHelper.NormalizarTratamento(entity);
        var created = await _repository.CreateAsync<Tratamento, Guid>(entity);
        _ = await _repository.SaveChangesAsync();

        await TratamentoIntegridadeHelper.GarantirSessoesPlaneadasAsync(created, _repository);
        _ = await _repository.SaveChangesAsync();

        await TratamentoIntegridadeHelper.RecalcularFaltasAsync(created.Id, _repository);
        _ = await _repository.SaveChangesAsync();
        if (request.SendEmail)
          await TentarDispararEmailFluxoAsync(created);
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateMarcacaoManualAsync(CreateMarcacaoManualTratamentoRequest request)
    {
      try
      {
        DisponibilidadeValidacaoResult validacaoDisponibilidade =
          await ValidarDisponibilidadeSessoesAsync(request);
        if (!validacaoDisponibilidade.IsSuccess)
        {
          return ResponseFactory.Fail<Guid>(validacaoDisponibilidade.ErrorMessage!);
        }

        Guid? listaEsperaId = string.IsNullOrWhiteSpace(request.ListaEsperaTratamentoId) ? null : Guid.Parse(request.ListaEsperaTratamentoId);
        ListaEsperaTratamento? listaEspera = null;
          if (listaEsperaId.HasValue)
          {
            listaEspera = await _repository.GetByIdAsync<ListaEsperaTratamento, Guid>(
              listaEsperaId.Value);
          if (listaEspera == null || listaEspera.DeletedOn != null)
            return ResponseFactory.Fail<Guid>("Registo da lista de espera não encontrado");

          var jaConvertidos = await _repository.GetListAsync<Tratamento, Guid>(
            new TratamentoByListaEsperaTratamentoIdSpec(listaEsperaId.Value));
            if (jaConvertidos.Any())
            {
              var msg = "Esta prescrição já foi inserida num tratamento!";
              var existente = jaConvertidos.First();
              if (existente.Provisorio == 1)
                msg += " Encontra-se no estado provisório";
              return ResponseFactory.Fail<Guid>(msg);
            }
          }
        
        var datas = request.Sessoes
          .Where(s => s.Data.HasValue)
          .Select(s => s.Data!.Value.Date)
          .OrderBy(d => d)
          .ToList();

        var tratamento = new Tratamento
        {
          UtenteId = Guid.Parse(request.UtenteId),
          OrganismoId = Guid.Parse(request.OrganismoId),
          MedicoId = ToNullableGuid(request.MedicoId),
          FisioterapeutaId = ToNullableGuid(request.FisioterapeutaId),
          AuxiliarId = ToNullableGuid(request.AuxiliarId),
          OutroTecnicoId = ToNullableGuid(request.OutroTecnicoId),
          LocalTratamentoId = ToNullableGuid(request.LocalTratamentoId),
          LocalOrigemId = ToNullableGuid(request.LocalOrigemId),
          Designacao = request.Designacao,
          NomePatologia = request.NomePatologia,
          NumSessao = request.NumSessao ?? request.Sessoes.Count,
          DataInic = request.DataInic ?? (datas.Count > 0 ? datas.First() : null),
          DataFim = request.DataFim ?? (datas.Count > 0 ? datas.Last() : null),
          Data = DateTime.UtcNow.Date,
          DuracaoTotal = request.DuracaoTotal,
          UnidadeTempoFisio = request.UnidadeTempoFisio,
          UnidadeTempoAux = request.UnidadeTempoAux,
          UnidadeTempoOutro = request.UnidadeTempoOutro,
          Credencial = request.Credencial,
          NumBenif = request.NumBenif,
          Apolice = request.Apolice,
          NFaltMax = request.NFaltMax,
          NFaltComax = request.NFaltComax,
          TaxaMod = request.TaxaMod,
          Isencao = request.Isencao,
          ConfDfim = request.ConfDfim ?? 0,
          CredencialExterna = request.CredencialExterna,
          NumCartao = request.NumCartao,
          HoraFisio = request.HoraFisio,
          HoraAux = request.HoraAux,
          HoraOutro = request.HoraOutro,
          Provisorio = request.Provisorio ?? 0,
          Obs = request.Obs,
          TecObs = request.TecObs,
          SinistroId = ToNullableGuid(request.SinistroId),
          SeguradoraId = ToNullableGuid(request.SeguradoraId),
          ListaEsperaTratamentoId = listaEsperaId,
          NFalta = 0,
          NFaltaCons = 0,
          NAltSess = 0,
          Lotes = 0,
          TerapiaFala = request.TerapiaFala,
        };

        TratamentoIntegridadeHelper.NormalizarTratamento(tratamento);

        var created = await _repository.CreateAsync<Tratamento, Guid>(tratamento);

        var ordem = 1;
        foreach ( var svc in request.Servicos)
        {
          _ = await _repository.CreateAsync<ServicoTratamento, Guid>(new ServicoTratamento
          {
            TratamentoId = created.Id,
            ServicoId = Guid.Parse(svc.ServicoId),
            Duracao = svc.Duracao,
            Ordem = svc.Ordem ?? ordem,
            UsaFisioter = svc.UsaFisioter ?? 1,
            UsaAuxiliar = svc.UsaAuxiliar ?? 0,
            UsaOutro = svc.UsaOutro ?? 0,
            Preco = svc.Preco,
            DescInst = svc.DescInst,
            ValorUt = svc.ValorUt,
            Obs = svc.Obs,
          });
          ordem++;
        }

        var num = 1;
        foreach( var sessao in request.Sessoes.OrderBy(s => s.Data))
        {
          _ = await _repository.CreateAsync<SessaoTratamento, Guid>(new SessaoTratamento
          {
            TratamentoId = created.Id,
            NumSessao = sessao.NumSessao ?? num,
            Data = sessao.Data!.Value.Date,
            HoraInic = sessao.HoraInic,
            Duracao = sessao.Duracao ?? request.DuracaoTotal,
            FisioterapeutaId = ToNullableGuid(sessao.FisioterapeutaId) ?? created.FisioterapeutaId,
            AuxiliarId = ToNullableGuid(sessao.AuxiliarId) ?? created.AuxiliarId,
            OutroTecnicoId = ToNullableGuid(sessao.OutroTecnicoId) ?? created.OutroTecnicoId,
            Faltou = 0,
            CompensaFalta = 0,
            Desmarcado = 0, 
            Confirmado = 0,
            Efetuado = 0,
          });
          num++;
        }

        // Persistir antes de recalcular (GetList das sessões vai à BD)
        _ = await _repository.SaveChangesAsync();

        await TratamentoIntegridadeHelper.RecalcularFaltasAsync(created.Id, _repository);

        if (listaEspera != null && (request.Provisorio ?? 0) != 1)
        {
          await _repository.RemoveByIdAsync<ListaEsperaTratamento, Guid>(listaEspera.Id);
        }

        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private static Guid? ToNullableGuid(string? value ) => 
      string.IsNullOrWhiteSpace(value) ? null : Guid.Parse(value);

    private sealed record DisponibilidadeValidacaoResult(bool IsSuccess, string? ErrorMessage)
    {
      public static DisponibilidadeValidacaoResult Ok() => new(true, null);
      public static DisponibilidadeValidacaoResult Fail(string message) => new(false, message);
    }

    private async Task<DisponibilidadeValidacaoResult> ValidarDisponibilidadeSessoesAsync(
      CreateMarcacaoManualTratamentoRequest request)
    {
      foreach (CreateMarcacaoManualSessaoItem sessao in request.Sessoes)
      {
        if (!sessao.Data.HasValue || string.IsNullOrWhiteSpace(sessao.HoraInic))
          continue;

        DateTime data = sessao.Data.Value.Date;
        string hora = NormalizarHora(sessao.HoraInic);

        var checks = new List<(string? tecnicoId, int? unidadeTempo)>
        {
          (sessao.FisioterapeutaId ?? request.FisioterapeutaId, request.UnidadeTempoFisio),
          (sessao.AuxiliarId ?? request.AuxiliarId, request.UnidadeTempoAux),
          (sessao.OutroTecnicoId ?? request.OutroTecnicoId, request.UnidadeTempoOutro),
        };

        foreach ((string? tecnicoIdRaw, int? unidadeTempoRaw) in checks)
        {
          if (string.IsNullOrWhiteSpace(tecnicoIdRaw) || !Guid.TryParse(tecnicoIdRaw, out Guid tecnicoId))
            continue;

          int unidadeTempo = unidadeTempoRaw.GetValueOrDefault(1);
          if (unidadeTempo < 1) unidadeTempo = 1;

          Response<HorasPossiveisTecnicoResponse> disponibilidade =
            await _disponibilidadeTecnicoTratamentoService.GetHorasPossiveisAsync(
              new HorasPossiveisTecnicoRequest
              {
                TecnicoId = tecnicoId,
                Data = data,
                UnidadeTempo = unidadeTempo,
              });

          if (disponibilidade.Status == ResponseStatus.Failure)
          {
            string msg = disponibilidade.Messages.Values.SelectMany(x => x).FirstOrDefault()
              ?? "Sessão indisponível.";
            return DisponibilidadeValidacaoResult.Fail(msg);
          }

          IReadOnlyList<string> horas = disponibilidade.Data?.Horas ?? [];
          bool horaLivre = horas.Any(h => string.Equals(NormalizarHora(h), hora, StringComparison.Ordinal));

          if (!horaLivre)
          {
            return DisponibilidadeValidacaoResult.Fail(
              $"Sessão dia {data:dd/MM/yyyy} às {hora} já não se encontra disponível."
            );
          }
        }
      }

      return DisponibilidadeValidacaoResult.Ok();
    }

    private static string NormalizarHora(string hora)
    {
      if (TimeSpan.TryParse(hora, out TimeSpan ts))
        return $"{(int)ts.TotalHours:00}:{ts.Minutes:00}";

      return hora.Trim();
    }

    public async Task<Response<Guid>> UpdateTratamentoAsync(UpdateTratamentoRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<Tratamento, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Tratamento não encontrado.");

      _ = _mapper.Map(request, existing);
      try
      {
        TratamentoIntegridadeHelper.NormalizarTratamento(existing);
        var updated = await _repository.UpdateAsync<Tratamento, Guid>(existing);
        await TratamentoIntegridadeHelper.GarantirSessoesPlaneadasAsync(updated, _repository);
        await TratamentoIntegridadeHelper.RecalcularFaltasAsync(updated.Id, _repository);
        _ = await _repository.SaveChangesAsync();
        if (request.SendEmail)
          await TentarDispararEmailFluxoAsync(updated);
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteTratamentoAsync(Guid id)
    {
      try
      {
        var block = await GetBloqueioEliminacaoPorReciboAsync(id);
        if (block != null)
          return ResponseFactory.Fail<Guid>(block);

        var entity = await _repository.RemoveByIdAsync<Tratamento, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTratamentoAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var block = await GetBloqueioEliminacaoPorReciboAsync(id);
          if (block != null)
          {
            fail.Add(block);
            continue;
          }

          var e = await _repository.GetByIdAsync<Tratamento, Guid>(id);
          if (e == null) { fail.Add($"Tratamento {id} não encontrado."); continue; }
          var removed = await _repository.RemoveByIdAsync<Tratamento, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"Tratamento {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }

    /// <summary>
    /// Paridade TRATAMENDel_Execute: bloqueia se tratamento ou sessões tiverem recibo/documento.
    /// </summary>
    private async Task<string?> GetBloqueioEliminacaoPorReciboAsync(Guid tratamentoId)
    {
      const string msg = "Tratamento já tem recibo associado";

      var tratamento = await _repository.GetByIdAsync<Tratamento, Guid>(tratamentoId);
      if (tratamento == null)
        return $"Tratamento {tratamentoId} não encontrado.";

      if (tratamento.ReciboId.HasValue || tratamento.DocumentoId.HasValue)
        return msg;

      var sessoes = (
        await _repository.GetListAsync<SessaoTratamento, Guid>(
          new SessoesTratamentoByTratamentoIdSpec(tratamentoId)
        )
      ).ToList();

      if (sessoes.Any(s => s.ReciboId.HasValue || s.DocumentoId.HasValue))
        return msg;

      return null;
    }
    public async Task<Response<Guid>> UpdateTratamentoAltaAsync(Guid id, bool alta)
    {
      var existing = await _repository.GetByIdAsync<Tratamento, Guid>(id);
      if (existing == null)
      {
        return ResponseFactory.Fail<Guid>("Tratamento não encontrado.");
      }

      if (alta)
      {
        existing.DataFim ??= DateTime.UtcNow;
      }
      else
      {
        existing.DataFim = null;
      }

      // Sincronizar o estado de Alta também na última Evolução de Tratamento, quando existir.
      try
      {
        var evolucaoSpec = new EvolucaoTratamentoByTratamentoIdUltima(existing.Id);
        IEnumerable<EvolucaoTratamento> evolucoes =
          await _repository.GetListAsync<EvolucaoTratamento, Guid>(evolucaoSpec);

        EvolucaoTratamento? ultimaEvolucao = evolucoes.FirstOrDefault();
        if (ultimaEvolucao != null)
        {
          if (alta)
          {
            ultimaEvolucao.DataAlta ??= DateTime.UtcNow;
          }
          else
          {
            ultimaEvolucao.DataAlta = null;
          }

          _ = await _repository.UpdateAsync<EvolucaoTratamento, Guid>(ultimaEvolucao);
        }
      }
      catch
      {
        // Se falhar a sincronização da evolução, não impedimos a atualização do tratamento.
      }

      try
      {
        var updated = await _repository.UpdateAsync<Tratamento, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private async Task TentarDispararEmailFluxoAsync(Tratamento tratamento)
    {
      try
      {
        if (!tratamento.UtenteId.HasValue) return;

        var clinica = (await _repository.GetListAsync<Clinica, Guid>(new ClinicaPorDefeitoSelected())).FirstOrDefault();
        if (clinica is null) return;

        var utente = await _repository.GetByIdAsync<Utente, Guid>(tratamento.UtenteId.Value);
        if (utente is null || string.IsNullOrWhiteSpace(utente.Email)) return;

        string profissionalNome = string.Empty;
        if (tratamento.FisioterapeutaId.HasValue)
        {
          var fisio = await _repository.GetByIdAsync<Tecnico, Guid>(tratamento.FisioterapeutaId.Value);
          if (fisio is not null) profissionalNome = fisio.Nome ?? string.Empty;
        }
        else if (tratamento.MedicoId.HasValue)
        {
          var medico = await _repository.GetByIdAsync<Medico, Guid>(tratamento.MedicoId.Value);
          if (medico is not null) profissionalNome = medico.Nome ?? string.Empty;
        }

        var emailRequest = new EnviarEmailPorCodigoRequest
        {
          CodigoConfiguracao = "8.2",
          EmailDestino = utente.Email.Trim(),
          NomeUtente = utente.Nome ?? string.Empty,
          NomeMedicoOuProfissional = profissionalNome,
          NomeEspecialidade = tratamento.Designacao ?? string.Empty,
          NumeroSessao = tratamento.NumSessao?.ToString(),
          Data = tratamento.Data ?? tratamento.DataInic,
          Hora = tratamento.HoraFisio,
          HoraNova = tratamento.HoraFisio,
          Modulo = "Tratamento",
        };

        _ = await _configuracaoEmailService.EnviarEmailPorCodigoAsync(clinica.Id, emailRequest);
      }
      catch
      {
        // Não bloquear o fluxo principal por falha de Email.
      }
    }
  }
}

