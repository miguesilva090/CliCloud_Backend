using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Filters;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

public class AdmissaoAdministrativoService(IRepositoryAsync repository, IMapper mapper)
  : IAdmissaoAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly IMapper _mapper = mapper;

  public async Task<PaginatedResponse<AdmissaoTableDTO>> GetPaginatedAsync(AdmissaoTableFilter filter)
  {
    if (filter.Filters?.Count > 0)
    {
      filter.PageNumber = 1;
    }

    string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
    var spec = new AdmissaoSearchTable(filter.Modo, filter.Filters ?? [], order);
    PaginatedResponse<AdmissaoTableDTO> result =
      await _repository.GetPaginatedResultsAsync<Admissao, AdmissaoTableDTO, Guid>(
        filter.PageNumber,
        filter.PageSize,
        spec
      );

    await HydrateUtenteNumerosAsync(result.Data);
    return result;
  }

  public async Task<Response<AdmissaoDTO>> GetByIdAsync(Guid id)
  {
    List<Admissao> list = (await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByIdWithServicosSpec(id))).ToList();
    Admissao? entity = list.FirstOrDefault();
    if (entity == null)
    {
      return ResponseFactory.Fail<AdmissaoDTO>("Admissão não encontrada.");
    }

    AdmissaoDTO dto = _mapper.Map<AdmissaoDTO>(entity);
    await HydrateUtenteNumeroAsync(dto, entity.UtenteId);
    return ResponseFactory.Success(dto);
  }

  public async Task<Response<Guid>> CreateAsync(CreateAdmissaoRequest request)
  {
    if (request.ConsultaMarcacaoId.HasValue)
    {
      List<Admissao> existentes = (await _repository.GetListAsync<Admissao, Guid>()).ToList();
      if (existentes.Any(a =>
            a.ConsultaMarcacaoId == request.ConsultaMarcacaoId && a.DeletedOn == null))
      {
        return ResponseFactory.Fail<Guid>("Já existe uma admissão para esta marcação.");
      }
    }

    Admissao entity = _mapper.Map<Admissao>(request);
    entity.Id = Guid.NewGuid();
    NormalizeServicos(entity);
    _ = await _repository.CreateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateAdmissaoRequest request)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    _mapper.Map(request, entity);
    NormalizeServicos(entity);
    _ = await _repository.UpdateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<Guid>> DeleteAsync(Guid id)
  {
    await _repository.RemoveByIdAsync<Admissao, Guid>(id);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> ConfirmarAsync(Guid id, bool confirmado)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    entity.Confirmado = confirmado;
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> SetEfetuadoAsync(Guid id, bool efetuado)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    entity.Efetuado = efetuado;
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> PromoverParaConsultaAsync(Guid id)
  {
    List<Admissao> list = (await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByIdWithServicosSpec(id))).ToList();
    Admissao? admissao = list.FirstOrDefault();
    if (admissao == null)
    {
      return ResponseFactory.Fail<Guid>("Admissão não encontrada.");
    }

    List<Consulta> consultas = (await _repository.GetListAsync<Consulta, Guid>()).ToList();
    if (consultas.Any(c => c.AdmissaoId == id && c.DeletedOn == null))
    {
      return ResponseFactory.Fail<Guid>("Esta admissão já foi promovida para consulta.");
    }

    Consulta consulta = AdmissaoPromocaoHelper.CriarConsultaDesdeAdmissao(admissao);
    Consulta created = await _repository.CreateAsync<Consulta, Guid>(consulta);

    foreach (ServicoConsulta servico in AdmissaoPromocaoHelper.MapearServicos(admissao, created.Id))
    {
      servico.Id = Guid.NewGuid();
      _ = await _repository.CreateAsync<ServicoConsulta, Guid>(servico);
    }

    if (admissao.ConsultaMarcacaoId.HasValue)
    {
      ConsultaMarcacao marcacao =
        await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(admissao.ConsultaMarcacaoId.Value);
      marcacao.ConsultaId = created.Id;
      _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
    }

    await _repository.RemoveByIdAsync<Admissao, Guid>(admissao.Id);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(created.Id);
  }

  private static void NormalizeServicos(Admissao entity)
  {
    int linha = 1;
    foreach (AdmissaoServico servico in entity.Servicos.OrderBy(s => s.Linha))
    {
      if (servico.Id == Guid.Empty)
      {
        servico.Id = Guid.NewGuid();
      }

      servico.AdmissaoId = entity.Id;
      if (servico.Linha <= 0)
      {
        servico.Linha = linha;
      }

      linha++;
    }
  }

  private async Task HydrateUtenteNumerosAsync(IReadOnlyCollection<AdmissaoTableDTO> rows)
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

    foreach (AdmissaoTableDTO row in rows)
    {
      UtenteNumeroLookupDTO? u = lookups.FirstOrDefault(x => x.Id == row.UtenteId);
      if (u != null)
      {
        row.UtenteNumero = u.NumeroUtente;
      }
    }
  }

  private async Task HydrateUtenteNumeroAsync(AdmissaoDTO dto, Guid utenteId)
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
