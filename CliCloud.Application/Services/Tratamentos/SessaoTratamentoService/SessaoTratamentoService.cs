using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Application.Services.Core.EmailService;
using CliCloud.Application.Services.Core.EmailService.DTOs;
using CliCloud.Application.Services.Tratamentos;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Filters;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService
{
  public class SessaoTratamentoService : ISessaoTratamentoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguracaoEmailService _configuracaoEmailService;

    public SessaoTratamentoService(IRepositoryAsync repository, IMapper mapper, IConfiguracaoEmailService configuracaoEmailService)
    {
      _repository = repository;
      _mapper = mapper;
      _configuracaoEmailService = configuracaoEmailService;
    }

    public async Task<Response<IEnumerable<SessaoTratamentoDTO>>> GetSessaoTratamentoAsync(string keyword = "")
    {
      var spec = new SessaoTratamentoSearchList(keyword);
      var list = await _repository.GetListAsync<SessaoTratamento, SessaoTratamentoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<SessaoTratamentoLightDTO>>> GetSessaoTratamentoLightAsync(string keyword = "")
    {
      var spec = new SessaoTratamentoSearchList(keyword);
      var list = await _repository.GetListAsync<SessaoTratamento, SessaoTratamentoLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<SessaoTratamentoTableDTO>> GetSessaoTratamentoPaginatedAsync(SessaoTratamentoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new SessaoTratamentoSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<SessaoTratamento, SessaoTratamentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<SessaoTratamentoTableDTO>>> GetAllSessaoTratamentoAsync(SessaoTratamentoAllFilter? filter)
    {
      try
      {
        filter ??= new SessaoTratamentoAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new SessaoTratamentoSearchTable(filters, order);
        var list = await _repository.GetListAsync<SessaoTratamento, SessaoTratamentoTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<SessaoTratamentoTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<SessaoTratamentoDTO>> GetSessaoTratamentoAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<SessaoTratamento, SessaoTratamentoDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<SessaoTratamentoDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CompensarFaltaAsync(CompensarFaltaSessaoTratamentoRequest request)
    {
      if (!request.Data.HasValue)
        return ResponseFactory.Fail<Guid>("A data da sessão é obrigatória");

      var tratamentoId = Guid.Parse(request.TratamentoId);
      var tratamento = await _repository.GetByIdAsync<Tratamento, Guid>(tratamentoId);
      if (tratamento == null)
        return ResponseFactory.Fail<Guid>("Tratamento não encontrado");
      
      var sessoes = ( 
        await _repository.GetListAsync<SessaoTratamento, Guid>(
          new SessoesTratamentoByTratamentoIdSpec(tratamentoId)
        )
      ).ToList();

      int faltas = sessoes.Count(s => s.Faltou == 1 && s.Desmarcado != 1);
      int comps = sessoes.Count(s => s.CompensaFalta == 1 && s.Desmarcado != 1);
      if (faltas <= comps)
        return ResponseFactory.Fail<Guid>("Não existem faltas por compensar");

      if (await ExisteSessaoNaDataAsync(tratamentoId, request.Data.Value.Date, null))
        return ResponseFactory.Fail<Guid>("Já existe sessão nesta data");

      static Guid? Ng(string? v) => 
        string.IsNullOrWhiteSpace(v) ? null : Guid.Parse(v);

      var horas = new List<string>();
      if (!string.IsNullOrWhiteSpace(request.HoraFisio)) horas.Add(request.HoraFisio.Trim());
      if (!string.IsNullOrWhiteSpace(request.HoraAux)) horas.Add(request.HoraAux.Trim());
      if (!string.IsNullOrWhiteSpace(request.HoraOutro)) horas.Add(request.HoraOutro.Trim());
      if (!string.IsNullOrWhiteSpace(request.HoraInic)) horas.Add(request.HoraInic.Trim());
      var horaInic = horas.OrderBy(h => h).FirstOrDefault()
        ?? tratamento.HoraFisio
        ?? tratamento.HoraAux
        ?? tratamento.HoraOutro;

      var duracao = request.Duracao
        ?? request.DuracaoFisio
        ?? request.DuracaoAux
        ?? request.DuracaoOutro
        ?? tratamento.DuracaoTotal;

      try
      {
        var created = await _repository.CreateAsync<SessaoTratamento, Guid>(new SessaoTratamento
        {
          TratamentoId = tratamentoId,
          Data = request.Data.Value.Date,
          HoraInic = horaInic,
          Duracao = duracao,
          FisioterapeutaId = Ng(request.FisioterapeutaId),
          AuxiliarId = Ng(request.AuxiliarId),
          OutroTecnicoId = Ng(request.OutroTecnicoId),
          HoraFisio = request.HoraFisio,
          HoraAux = request.HoraAux,
          HoraOutro = request.HoraOutro,
          DuracaoFisio = request.DuracaoFisio,
          DuracaoAux = request.DuracaoAux,
          DuracaoOutro = request.DuracaoOutro,
          NumSessao = sessoes.Count + 1,
          Faltou = 0,
          CompensaFalta = 1,
          Desmarcado = 0,
          Confirmado = 0,
          Efetuado = 0,
          Pago = 0,
        });

        // Incluir a sessão criada (ainda pode não vir no GetList sem SaveChanges)
        var ordenadas = sessoes
          .Where(s => s.Id != created.Id)
          .Append(created)
          .OrderBy(s => s.Data ?? DateTime.MaxValue)
          .ThenBy(s => s.NumSessao ?? int.MaxValue)
          .ThenBy(s => s.Id)
          .ToList();

        int n = 1;
        foreach (var s in ordenadas)
        {
          if (s.NumSessao != n)
          {
            s.NumSessao = n;
            _ = await _repository.UpdateAsync<SessaoTratamento, Guid>(s);
          }
          n++;
        }

        tratamento.NumSessao = Math.Max(tratamento.NumSessao ?? 0, ordenadas.Count);
        if (!tratamento.DataFim.HasValue
          || tratamento.DataFim.Value.Date < request.Data.Value.Date)
        {
          tratamento.DataFim = request.Data.Value.Date;
        }

        _ = await _repository.UpdateAsync<Tratamento, Guid>(tratamento);

        await TratamentoIntegridadeHelper.RecalcularFaltasAsync(tratamentoId, _repository);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }


    public async Task<Response<Guid>> CreateSessaoTratamentoAsync(CreateSessaoTratamentoRequest request)
    {
      if (!request.Data.HasValue)
        return ResponseFactory.Fail<Guid>("A data da sessão é obrigatória.");

      var tratamentoId = Guid.Parse(request.TratamentoId);
      var dup = await ExisteSessaoNaDataAsync(tratamentoId, request.Data.Value.Date, excludeId: null);
      if (dup)
        return ResponseFactory.Fail<Guid>("Já existe sessão nesta data.");

      var entity = _mapper.Map<SessaoTratamento>(request);
      try
      {
        var created = await _repository.CreateAsync<SessaoTratamento, Guid>(entity);
        await TratamentoIntegridadeHelper.RecalcularFaltasAsync(created.TratamentoId, _repository);
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

    public async Task<Response<Guid>> UpdateSessaoTratamentoAsync(UpdateSessaoTratamentoRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<SessaoTratamento, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Sessão de tratamento não encontrada.");

      if (!request.Data.HasValue)
        return ResponseFactory.Fail<Guid>("A data da sessão é obrigatória.");

      var tratamentoId = Guid.Parse(request.TratamentoId);
      var dup = await ExisteSessaoNaDataAsync(tratamentoId, request.Data.Value.Date, excludeId: id);
      if (dup)
        return ResponseFactory.Fail<Guid>("Já existe sessão nesta data.");

      var tratamentoAnteriorId = existing.TratamentoId;
      _ = _mapper.Map(request, existing);
      try
      {
        var updated = await _repository.UpdateAsync<SessaoTratamento, Guid>(existing);
        await TratamentoIntegridadeHelper.RecalcularFaltasAsync(updated.TratamentoId, _repository);
        if (tratamentoAnteriorId != updated.TratamentoId)
        {
          await TratamentoIntegridadeHelper.RecalcularFaltasAsync(tratamentoAnteriorId, _repository);
        }
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

    public async Task<Response<Guid>> DeleteSessaoTratamentoAsync(Guid id)
    {
      try
      {
        var existing = await _repository.GetByIdAsync<SessaoTratamento, Guid>(id);
        if (existing == null)
          return ResponseFactory.Fail<Guid>("Sessão de tratamento não encontrada.");

        if (existing.ReciboId.HasValue || existing.DocumentoId.HasValue)
          return ResponseFactory.Fail<Guid>("Sessão já tem recibo associado");

        var entity = await _repository.RemoveByIdAsync<SessaoTratamento, Guid>(id);
        await TratamentoIntegridadeHelper.RecalcularFaltasAsync(entity.TratamentoId, _repository);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private async Task<bool> ExisteSessaoNaDataAsync(
      Guid tratamentoId,
      DateTime data,
      Guid? excludeId
    )
    {
      var list = (
        await _repository.GetListAsync<SessaoTratamento, Guid>(
          new SessoesTratamentoByTratamentoIdSpec(tratamentoId)
        )
      ).ToList();

      return list.Any(s =>
        s.Data.HasValue
        && s.Data.Value.Date == data.Date
        && (!excludeId.HasValue || s.Id != excludeId.Value)
      );
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleSessaoTratamentoAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();
      var tratamentosParaRecalcular = new HashSet<Guid>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<SessaoTratamento, Guid>(id);
          if (e == null) { fail.Add($"SessaoTratamento {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<SessaoTratamento, Guid>(id);
          tratamentosParaRecalcular.Add(removed.TratamentoId);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"SessaoTratamento {id}.");
          _repository.ClearChangeTracker();
        }
      }

      foreach (var tratamentoId in tratamentosParaRecalcular)
      {
        await TratamentoIntegridadeHelper.RecalcularFaltasAsync(tratamentoId, _repository);
      }
      if (tratamentosParaRecalcular.Count > 0)
      {
        _ = await _repository.SaveChangesAsync();
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminadas {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }

    private async Task TentarDispararEmailFluxoAsync(SessaoTratamento sessao)
    {
      try
      {
        var clinica = (await _repository.GetListAsync<Clinica, Guid>(new ClinicaPorDefeitoSelected())).FirstOrDefault();
        if (clinica is null) return;

        var tratamento = await _repository.GetByIdAsync<Tratamento, Guid>(sessao.TratamentoId);
        if (tratamento is null || !tratamento.UtenteId.HasValue) return;

        var utente = await _repository.GetByIdAsync<Utente, Guid>(tratamento.UtenteId.Value);
        if (utente is null || string.IsNullOrWhiteSpace(utente.Email)) return;

        string profissionalNome = string.Empty;
        if (sessao.FisioterapeutaId.HasValue)
        {
          var fisio = await _repository.GetByIdAsync<Tecnico, Guid>(sessao.FisioterapeutaId.Value);
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
          NumeroSessao = sessao.NumSessao?.ToString() ?? tratamento.NumSessao?.ToString(),
          Data = sessao.Data ?? tratamento.Data ?? tratamento.DataInic,
          Hora = sessao.HoraFisio ?? sessao.HoraInic ?? tratamento.HoraFisio,
          HoraNova = sessao.HoraFisio ?? sessao.HoraInic ?? tratamento.HoraFisio,
          Modulo = "SessaoTratamento",
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

