using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService.Filters;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService.Specifications;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Application.Services.Core.SmsService;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Application.Services.Core.EmailService;
using CliCloud.Application.Services.Core.EmailService.DTOs;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Consultas.MarcacaoConsultaService
{
  public class MarcacaoConsultaService : IMarcacaoConsultaService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;
    private readonly IServicoSms _servicoSms;
    private readonly IConfiguracaoEmailService _configuracaoEmailService;

    public MarcacaoConsultaService(
      IRepositoryAsync repository,
      IMapper mapper,
      IServicoSms servicoSms,
      IConfiguracaoEmailService configuracaoEmailService)
    {
      _repository = repository;
      _mapper = mapper;
      _servicoSms = servicoSms;
      _configuracaoEmailService = configuracaoEmailService;
    }

    public async Task<Response<IEnumerable<MarcacaoConsultaDTO>>> GetMarcacaoConsultaAsync(string keyword = "")
    {
      var spec = new MarcacaoConsultaSearchList(keyword);
      var list = await _repository.GetListAsync<ConsultaMarcacao, MarcacaoConsultaDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<MarcacaoConsultaLightDTO>>> GetMarcacaoConsultaLightAsync(string keyword = "")
    {
      var spec = new MarcacaoConsultaSearchList(keyword);
      var list = await _repository.GetListAsync<ConsultaMarcacao, MarcacaoConsultaLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<MarcacaoConsultaTableDTO>> GetMarcacaoConsultaPaginatedAsync(MarcacaoConsultaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new MarcacaoConsultaSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<ConsultaMarcacao, MarcacaoConsultaTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<MarcacaoConsultaTableDTO>>> GetAllMarcacaoConsultaAsync(MarcacaoConsultaAllFilter? filter)
    {
      try
      {
        filter ??= new MarcacaoConsultaAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new MarcacaoConsultaSearchTable(filters, order);
        var list = await _repository.GetListAsync<ConsultaMarcacao, MarcacaoConsultaTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<MarcacaoConsultaTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<MarcacaoConsultaDTO>> GetMarcacaoConsultaAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<ConsultaMarcacao, MarcacaoConsultaDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<MarcacaoConsultaDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateMarcacaoConsultaAsync(CreateMarcacaoConsultaRequest request)
    {
      var entity = _mapper.Map<ConsultaMarcacao>(request);
      try
      {
        var created = await _repository.CreateAsync<ConsultaMarcacao, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        await TentarDispararSmsFluxoAsync(created, "6.1");
        if (request.SendEmail)
          await TentarDispararEmailFluxoAsync(created, "8.1");
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateMarcacaoConsultaAsync(UpdateMarcacaoConsultaRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("MarcacaoConsulta não encontrada.");

      _ = _mapper.Map(request, existing);
      try
      {
        var updated = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        await TentarDispararSmsFluxoAsync(updated, "6.2");
        if (request.SendEmail)
          await TentarDispararEmailFluxoAsync(updated, "8.1");
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteMarcacaoConsultaAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<ConsultaMarcacao, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMarcacaoConsultaAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(id);
          if (e == null) { fail.Add($"MarcacaoConsulta {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<ConsultaMarcacao, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"MarcacaoConsulta {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminadas {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }

    private async Task TentarDispararSmsFluxoAsync(ConsultaMarcacao marcacao, string codigoConfiguracao)
    {
      try
      {
        var clinica = (await _repository.GetListAsync<Clinica, Guid>(new ClinicaPorDefeitoSelected())).FirstOrDefault();
        if (clinica is null) return;

        var utente = await _repository.GetByIdAsync<Utente, Guid>(marcacao.UtenteId);
        if (utente is null) return;

        var contactos = await _repository.GetListAsync<EntidadeContacto, Guid>();
        var contacto = contactos
          .Where(x => x.EntidadeId == utente.Id && !string.IsNullOrWhiteSpace(x.Valor))
          .OrderByDescending(x => x.Principal)
          .Select(x => x.Valor!)
          .FirstOrDefault();
        if (string.IsNullOrWhiteSpace(contacto)) return;

        string medicoNome = string.Empty;
        if (marcacao.MedicoId.HasValue)
        {
          var medico = await _repository.GetByIdAsync<Medico, Guid>(marcacao.MedicoId.Value);
          if (medico is not null) medicoNome = medico.Nome ?? string.Empty;
        }

        string especialidadeNome = string.Empty;
        if (marcacao.EspecialidadeId.HasValue)
        {
          var especialidade = await _repository.GetByIdAsync<Especialidade, Guid>(marcacao.EspecialidadeId.Value);
          if (especialidade is not null) especialidadeNome = especialidade.Nome ?? string.Empty;
        }

        var smsRequest = new EnviarSmsPorCodigoRequest
        {
          CodigoConfiguracao = codigoConfiguracao,
          NumeroDestinatario = contacto,
          NomeUtente = utente.Nome ?? string.Empty,
          NomeMedicoOuProfissional = medicoNome,
          NomeEspecialidade = especialidadeNome,
          Data = marcacao.Data,
          Hora = marcacao.HoraMarcacao?.ToString(@"hh\:mm"),
          Modulo = "MarcacaoConsulta",
        };

        _ = await _servicoSms.EnviarSmsPorCodigoAsync(clinica.Id, smsRequest);
      }
      catch
      {
        // Não bloquear o fluxo principal por falha de SMS.
      }
    }

    private async Task TentarDispararEmailFluxoAsync(ConsultaMarcacao marcacao, string codigoConfiguracao)
    {
      try
      {
        var clinica = (await _repository.GetListAsync<Clinica, Guid>(new ClinicaPorDefeitoSelected())).FirstOrDefault();
        if (clinica is null) return;

        var utente = await _repository.GetByIdAsync<Utente, Guid>(marcacao.UtenteId);
        if (utente is null || string.IsNullOrWhiteSpace(utente.Email)) return;

        string medicoNome = string.Empty;
        if (marcacao.MedicoId.HasValue)
        {
          var medico = await _repository.GetByIdAsync<Medico, Guid>(marcacao.MedicoId.Value);
          if (medico is not null) medicoNome = medico.Nome ?? string.Empty;
        }

        string especialidadeNome = string.Empty;
        if (marcacao.EspecialidadeId.HasValue)
        {
          var especialidade = await _repository.GetByIdAsync<Especialidade, Guid>(marcacao.EspecialidadeId.Value);
          if (especialidade is not null) especialidadeNome = especialidade.Nome ?? string.Empty;
        }

        var emailRequest = new EnviarEmailPorCodigoRequest
        {
          CodigoConfiguracao = codigoConfiguracao,
          EmailDestino = utente.Email.Trim(),
          NomeUtente = utente.Nome ?? string.Empty,
          NomeMedicoOuProfissional = medicoNome,
          NomeEspecialidade = especialidadeNome,
          Data = marcacao.Data,
          Hora = marcacao.HoraMarcacao?.ToString(@"hh\:mm"),
          HoraNova = marcacao.HoraMarcacao?.ToString(@"hh\:mm"),
          Modulo = "MarcacaoConsulta",
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

