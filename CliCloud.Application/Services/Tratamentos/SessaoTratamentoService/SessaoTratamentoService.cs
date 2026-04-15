using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Application.Services.Core.EmailService;
using CliCloud.Application.Services.Core.EmailService.DTOs;
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

    public async Task<Response<Guid>> CreateSessaoTratamentoAsync(CreateSessaoTratamentoRequest request)
    {
      var entity = _mapper.Map<SessaoTratamento>(request);
      try
      {
        var created = await _repository.CreateAsync<SessaoTratamento, Guid>(entity);
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
      if (existing == null) return ResponseFactory.Fail<Guid>("SessaoTratamento não encontrada.");

      _ = _mapper.Map(request, existing);
      try
      {
        var updated = await _repository.UpdateAsync<SessaoTratamento, Guid>(existing);
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
        var entity = await _repository.RemoveByIdAsync<SessaoTratamento, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleSessaoTratamentoAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<SessaoTratamento, Guid>(id);
          if (e == null) { fail.Add($"SessaoTratamento {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<SessaoTratamento, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"SessaoTratamento {id}.");
          _repository.ClearChangeTracker();
        }
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

