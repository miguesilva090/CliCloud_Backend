using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;
using CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.Filters;
using CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService;

public class ListaEsperaAdministrativoService(
  IRepositoryAsync repository,
  IMapper mapper,
  IUtilizadorDisplayNameResolver utilizadorDisplayNameResolver
) : IListaEsperaAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly IMapper _mapper = mapper;
  private readonly IUtilizadorDisplayNameResolver _utilizadorDisplayNameResolver =
    utilizadorDisplayNameResolver;

  public async Task<PaginatedResponse<ListaEsperaTableDTO>> GetPaginatedAsync(
    ListaEsperaTableFilter filter
  )
  {
    if (filter.Filters?.Count > 0)
    {
      filter.PageNumber = 1;
    }

    if (filter.MedicoAgendaId.HasValue)
    {
      Medico? medicoAgenda = await _repository.GetByIdAsync<Medico, Guid>(filter.MedicoAgendaId.Value);
      filter.MedicoAgendaEspecialidadeId = medicoAgenda?.EspecialidadeId;
    }

    string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
    var spec = new ListaEsperaSearchTable(filter, order);
    PaginatedResponse<ListaEsperaTableDTO> result =
      await _repository.GetPaginatedResultsAsync<ListaEsperaConsulta, ListaEsperaTableDTO, Guid>(
        filter.PageNumber,
        filter.PageSize,
        spec
      );

    await HydrateUtenteNumerosAsync(result.Data);
    await HydrateUtenteTelefonesAsync(result.Data);
    return result;
  }

  public async Task<Response<ListaEsperaDTO>> GetByIdAsync(Guid id)
  {
    ListaEsperaConsulta? entity = await _repository.GetByIdAsync<ListaEsperaConsulta, Guid>(id);
    if (entity == null || entity.DeletedOn != null)
    {
      return ResponseFactory.Fail<ListaEsperaDTO>("Registo de lista de espera não encontrado.");
    }

    ListaEsperaDTO dto = _mapper.Map<ListaEsperaDTO>(entity);
    await HydrateUtenteNumeroAsync(dto, entity.UtenteId);
    return ResponseFactory.Success(dto);
  }

  public async Task<Response<Guid>> CreateAsync(CreateListaEsperaRequest request)
  {
    var entity = _mapper.Map<ListaEsperaConsulta>(request);
    entity.Id = Guid.NewGuid();
    entity.Data = request.Data.Date;

    ListaEsperaConsulta created =
      await _repository.CreateAsync<ListaEsperaConsulta, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(created.Id);
  }

  public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateListaEsperaRequest request)
  {
    ListaEsperaConsulta? entity = await _repository.GetByIdAsync<ListaEsperaConsulta, Guid>(id);
    if (entity == null || entity.DeletedOn != null)
    {
      return ResponseFactory.Fail<Guid>("Registo de lista de espera não encontrado.");
    }

    if (entity.ConsultaMarcacaoId.HasValue)
    {
      return ResponseFactory.Fail<Guid>(
        "Não é possível alterar um registo já convertido em marcação."
      );
    }

    string? obs = entity.Obs;
    _mapper.Map(request, entity);
    entity.Obs = obs;
    entity.Data = request.Data.Date;

    _ = await _repository.UpdateAsync<ListaEsperaConsulta, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<Guid>> DeleteAsync(Guid id)
  {
    ListaEsperaConsulta? entity = await _repository.GetByIdAsync<ListaEsperaConsulta, Guid>(id);
    if (entity == null || entity.DeletedOn != null)
    {
      return ResponseFactory.Fail<Guid>("Registo de lista de espera não encontrado.");
    }

    await _repository.RemoveByIdAsync<ListaEsperaConsulta, Guid>(id);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids)
  {
    List<Guid> list = ids.Distinct().ToList();
    if (list.Count == 0)
    {
      return ResponseFactory.Fail<IEnumerable<Guid>>("Selecione pelo menos um registo.");
    }

    foreach (Guid id in list)
    {
      await _repository.RemoveByIdAsync<ListaEsperaConsulta, Guid>(id);
    }

    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success<IEnumerable<Guid>>(list);
  }

  public async Task<Response<ListaEsperaObservacoesDTO>> GetObservacoesAsync(Guid id)
  {
    ListaEsperaConsulta entity = await _repository.GetByIdAsync<ListaEsperaConsulta, Guid>(id);
    return ResponseFactory.Success(
      new ListaEsperaObservacoesDTO { Observacoes = entity.Obs ?? string.Empty }
    );
  }

  public async Task<Response<Guid>> AppendObservacaoAsync(
    Guid id,
    AppendListaEsperaObservacaoRequest request
  )
  {
    if (string.IsNullOrWhiteSpace(request.Texto))
    {
      return ResponseFactory.Fail<Guid>("Indique o texto da observação.");
    }

    ListaEsperaConsulta entity = await _repository.GetByIdAsync<ListaEsperaConsulta, Guid>(id);
    string nomeAutor = await _utilizadorDisplayNameResolver.ResolveAsync();
    entity.Obs = AdmissaoObservacoesHelper.FormatarObservacaoAppend(
      request.Texto,
      nomeAutor,
      entity.Obs
    );

    _ = await _repository.UpdateAsync<ListaEsperaConsulta, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<ConverterListaEsperaMarcacaoResultDTO>> ConverterParaMarcacaoAsync(
    Guid id,
    ConverterListaEsperaMarcacaoRequest request
  )
  {
    ListaEsperaConsulta? entity = await _repository.GetByIdAsync<ListaEsperaConsulta, Guid>(id);
    if (entity == null || entity.DeletedOn != null)
    {
      return ResponseFactory.Fail<ConverterListaEsperaMarcacaoResultDTO>(
        "Registo de lista de espera não encontrado."
      );
    }

    if (entity.ConsultaMarcacaoId.HasValue)
    {
      return ResponseFactory.Fail<ConverterListaEsperaMarcacaoResultDTO>(
        "Este registo já foi convertido em marcação."
      );
    }

    var marcacao = new ConsultaMarcacao
    {
      UtenteId = entity.UtenteId,
      MedicoId = entity.MedicoId,
      EspecialidadeId = entity.EspecialidadeId,
      Data = request.DataMarcacao?.Date ?? entity.Data.Date,
      HoraMarcacao = request.HoraInicio,
      TipoConsultaId = entity.TipoConsultaId,
      TipoAdmissaoId = request.TipoAdmissaoId,
      Obs = string.IsNullOrWhiteSpace(request.Obs) ? entity.Obs : request.Obs,
    };

    ConsultaMarcacao created =
      await _repository.CreateAsync<ConsultaMarcacao, Guid>(marcacao);

    await MarcacaoAdmissaoSyncHelper.SyncAdmissaoFromMarcacaoAsync(
      _repository,
      created,
      new MarcacaoAdmissaoSyncHelper.MarcacaoAdmissaoFields
      {
        OrganismoId = entity.OrganismoId,
        Credencial = entity.Credencial,
        HoraFim = entity.HoraFim,
      }
    );

    Guid listaEsperaId = entity.Id;
    entity.ConsultaMarcacaoId = created.Id;
    entity.ConvertidoEm = DateTime.UtcNow;
    _ = await _repository.UpdateAsync<ListaEsperaConsulta, Guid>(entity);

    if (!request.ManterNaListaEspera)
    {
      await _repository.RemoveByIdAsync<ListaEsperaConsulta, Guid>(entity.Id);
    }

    _ = await _repository.SaveChangesAsync();

    return ResponseFactory.Success(
      new ConverterListaEsperaMarcacaoResultDTO
      {
        ListaEsperaId = listaEsperaId,
        ConsultaMarcacaoId = created.Id,
      }
    );
  }

  private async Task HydrateUtenteTelefonesAsync(IReadOnlyCollection<ListaEsperaTableDTO> rows)
  {
    List<Guid> ids = rows.Select(r => r.UtenteId).Distinct().ToList();
    if (ids.Count == 0)
    {
      return;
    }

    var spec = new EntidadeContactosByEntidadeIdsSpecification(ids);
    List<EntidadeContacto> contactos = (
      await _repository.GetListAsync<EntidadeContacto, Guid>(spec)
    ).ToList();

    foreach (ListaEsperaTableDTO row in rows)
    {
      string? tel = contactos
        .Where(c => c.EntidadeId == row.UtenteId && !string.IsNullOrWhiteSpace(c.Valor))
        .OrderByDescending(c => c.Principal)
        .Select(c => c.Valor)
        .FirstOrDefault();
      row.UtenteTelefone = tel;
    }
  }

  private async Task HydrateUtenteNumerosAsync(IReadOnlyCollection<ListaEsperaTableDTO> rows)
  {
    List<Guid> ids = rows.Select(r => r.UtenteId).Distinct().ToList();
    if (ids.Count == 0)
    {
      return;
    }

    var spec = new UtenteNumerosByIdsSpecification(ids);
    List<UtenteNumeroLookupDTO> lookups = (
      await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
    ).ToList();

    foreach (ListaEsperaTableDTO row in rows)
    {
      UtenteNumeroLookupDTO? u = lookups.FirstOrDefault(x => x.Id == row.UtenteId);
      if (u != null)
      {
        row.UtenteNumero = u.NumeroUtente;
      }
    }
  }

  private async Task HydrateUtenteNumeroAsync(ListaEsperaDTO dto, Guid utenteId)
  {
    var spec = new UtenteNumerosByIdsSpecification([utenteId]);
    List<UtenteNumeroLookupDTO> lookups = (
      await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
    ).ToList();
    UtenteNumeroLookupDTO? u = lookups.FirstOrDefault();
    if (u != null)
    {
      dto.UtenteNumero = u.NumeroUtente;
    }
  }
}
