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

namespace CliCloud.Application.Services.Tratamentos.TratamentoService
{
  public class TratamentoService : ITratamentoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguracaoEmailService _configuracaoEmailService;

    public TratamentoService(IRepositoryAsync repository, IMapper mapper, IConfiguracaoEmailService configuracaoEmailService)
    {
      _repository = repository;
      _mapper = mapper;
      _configuracaoEmailService = configuracaoEmailService;
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
        await TratamentoIntegridadeHelper.GarantirSessoesPlaneadasAsync(created, _repository);
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
    /// Atualiza o estado de "alta" de um tratamento.
    /// No modelo novo vamos usar DataFim como indicador de alta:
    /// - alta = true  => DataFim = DateTime.UtcNow (se ainda não tiver)
    /// - alta = false => DataFim = null
    /// </summary>
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

