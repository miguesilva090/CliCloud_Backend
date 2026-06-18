using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;
using CliCloud.Application.Services.Stocks.FamiliaArtigoService.Filters;
using CliCloud.Application.Services.Stocks.FamiliaArtigoService.Specifications;
using CliCloud.Application.Utility;
using FamiliaArtigoEntity = CliCloud.Domain.Entities.Stocks.FamiliaArtigo;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService;

public class FamiliaArtigoService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentClinicaService currentClinicaService) : IFamiliaArtigoService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    public async Task<Response<IEnumerable<FamiliaArtigoLightDTO>>> GetLightAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<FamiliaArtigoLightDTO>>("Clínica atual não definida");

        var spec = new FamiliaArtigoSearchList(keyword, clinicaId.Value);
        var entities = (await _repository.GetListAsync<FamiliaArtigoEntity, Guid>(spec)).ToList();
        var result = new List<FamiliaArtigoLightDTO>();

        foreach (var entity in entities)
        {
            result.Add(new FamiliaArtigoLightDTO
            {
                Id = entity.Id,
                Codigo = entity.Codigo,
                Descricao = entity.Descricao,
                Path = await BuildPathAsync(entity, clinicaId.Value)
            });
        }

        return ResponseFactory.Success<IEnumerable<FamiliaArtigoLightDTO>>(result);
    }

    public async Task<PaginatedResponse<FamiliaArtigoTableDTO>> GetPaginatedAsync(FamiliaArtigoTableFilter filter)
    {
        if (filter.Filters?.Count > 0) filter.PageNumber = 1;

        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return new PaginatedResponse<FamiliaArtigoTableDTO>([], 0, filter.PageNumber, filter.PageSize);

        var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
        var spec = new FamiliaArtigoSearchTable(filter, clinicaId.Value, order);
        var page = await _repository.GetPaginatedResultsAsync<FamiliaArtigoEntity, FamiliaArtigoTableDTO, Guid>(
            filter.PageNumber, filter.PageSize, spec);

        foreach (var row in page.Data)
        {
            row.TemFilhos = await _repository.ExistsAsync<FamiliaArtigoEntity, Guid>(
                new FamiliaArtigoChildrenSpec(row.Id));
        }

        return page;
    }

    public async Task<Response<IEnumerable<FamiliaArtigoTableDTO>>> GetAllAsync(FamiliaArtigoAllFilter? filter)
    {
        filter ??= new FamiliaArtigoAllFilter();
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<FamiliaArtigoTableDTO>>("Clínica atual não definida");

        var tableFilter = new FamiliaArtigoTableFilter
        {
            Filters = filter.Filters,
            FiltroBox = filter.FiltroBox,
            CodigoDe = filter.CodigoDe,
            CodigoAte = filter.CodigoAte,
            DescricaoDe = filter.DescricaoDe,
            DescricaoAte = filter.DescricaoAte,
            ParentId = filter.ParentId
        };

        var spec = new FamiliaArtigoSearchTable(tableFilter, clinicaId.Value, filter.GetOrderByString());
        var list = (await _repository.GetListAsync<FamiliaArtigoEntity, FamiliaArtigoTableDTO, Guid>(spec)).ToList();

        foreach (var row in list)
        {
            row.TemFilhos = await _repository.ExistsAsync<FamiliaArtigoEntity, Guid>(
                new FamiliaArtigoChildrenSpec(row.Id));
        }

        return ResponseFactory.Success<IEnumerable<FamiliaArtigoTableDTO>>(list);
    }

    public async Task<Response<FamiliaArtigoDTO>> GetAsync(Guid id)
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<FamiliaArtigoDTO>("Clínica atual não definida");

        var entity = await GetEntityAsync(id, clinicaId.Value);
        if (entity == null)
            return ResponseFactory.Fail<FamiliaArtigoDTO>("Família de artigo não encontrada");

        var dto = _mapper.Map<FamiliaArtigoDTO>(entity);
        dto.Path = await BuildPathAsync(entity, clinicaId.Value);
        return ResponseFactory.Success(dto);
    }

    public async Task<Response<IEnumerable<FamiliaArtigoBreadcrumbDTO>>> GetAncestorsAsync(Guid? parentId)
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<FamiliaArtigoBreadcrumbDTO>>("Clínica atual não definida");

        if (!parentId.HasValue)
            return ResponseFactory.Success<IEnumerable<FamiliaArtigoBreadcrumbDTO>>([]);

        var chain = new List<FamiliaArtigoBreadcrumbDTO>();
        var current = await GetEntityAsync(parentId.Value, clinicaId.Value);
        if (current == null)
            return ResponseFactory.Fail<IEnumerable<FamiliaArtigoBreadcrumbDTO>>("Registo pai não encontrado");

        while (current != null)
        {
            chain.Insert(0, new FamiliaArtigoBreadcrumbDTO
            {
                Id = current.Id,
                Descricao = current.Descricao,
                Nivel = current.Nivel
            });

            current = current.ParentId.HasValue
                ? await GetEntityAsync(current.ParentId.Value, clinicaId.Value)
                : null;
        }

        return ResponseFactory.Success<IEnumerable<FamiliaArtigoBreadcrumbDTO>>(chain);
    }

    public async Task<Response<Guid>> CreateAsync(CreateFamiliaArtigoRequest request)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        int nivel = 1;
        Guid? parentId = request.ParentId;

        if (parentId.HasValue)
        {
            var parent = await GetEntityAsync(parentId.Value, clinicaId);
            if (parent == null)
                return ResponseFactory.Fail<Guid>("Registo pai não encontrado.");
            if (parent.Nivel >= 3)
                return ResponseFactory.Fail<Guid>("Não é possível criar filhos abaixo do nível 3.");
            nivel = parent.Nivel + 1;
        }

        var entity = new FamiliaArtigoEntity
        {
            ClinicaId = clinicaId,
            Codigo = await ObterProximoCodigoAsync(clinicaId),
            ParentId = parentId,
            Nivel = nivel,
            Descricao = request.Descricao.Trim(),
            UrlFoto = string.IsNullOrWhiteSpace(request.UrlFoto) ? null : request.UrlFoto.Trim()
        };

        try
        {
            var created = await _repository.CreateAsync<FamiliaArtigoEntity, Guid>(entity);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateFamiliaArtigoRequest request, Guid id)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        var existing = await GetEntityAsync(id, clinicaIdOpt.Value);
        if (existing == null)
            return ResponseFactory.Fail<Guid>("Família de artigo não encontrada");

        existing.Descricao = request.Descricao.Trim();
        existing.UrlFoto = string.IsNullOrWhiteSpace(request.UrlFoto) ? null : request.UrlFoto.Trim();

        try
        {
            await _repository.UpdateAsync<FamiliaArtigoEntity, Guid>(existing);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(existing.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> DeleteAsync(Guid id)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        var entity = await GetEntityAsync(id, clinicaIdOpt.Value);
        if (entity == null)
            return ResponseFactory.Fail<Guid>("Família de artigo não encontrada");

        if (await _repository.ExistsAsync<FamiliaArtigoEntity, Guid>(new FamiliaArtigoChildrenSpec(id)))
            return ResponseFactory.Fail<Guid>("Registo referenciado por filhos e não pode ser eliminado.");

        try
        {
            await _repository.RemoveAsync<FamiliaArtigoEntity, Guid>(entity);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(entity.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids)
    {
        var ok = new List<Guid>();
        foreach (var id in ids.ToList())
        {
            var result = await DeleteAsync(id);
            if (result.Status == ResponseStatus.Success) ok.Add(id);
            else _repository.ClearChangeTracker();
        }

        if (ok.Count == ids.Count())
            return ResponseFactory.Success<IEnumerable<Guid>>(ok);
        if (ok.Count > 0)
            return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {ids.Count()}.");
        return ResponseFactory.Fail<IEnumerable<Guid>>("Nenhum eliminado.");
    }

    private async Task<FamiliaArtigoEntity?> GetEntityAsync(Guid id, Guid clinicaId)
    {
        var spec = new FamiliaArtigoByIdClinicaSpec(id, clinicaId);
        return (await _repository.GetListAsync<FamiliaArtigoEntity, Guid>(spec)).FirstOrDefault();
    }

    private async Task<string> BuildPathAsync(FamiliaArtigoEntity node, Guid clinicaId)
    {
        var parts = new List<string>();
        FamiliaArtigoEntity? current = node;

        while (current != null)
        {
            parts.Insert(0, current.Descricao);
            current = current.ParentId.HasValue
                ? await GetEntityAsync(current.ParentId.Value, clinicaId)
                : null;
        }

        return "/" + string.Join("/", parts);
    }

    private async Task<int> ObterProximoCodigoAsync(Guid clinicaId)
    {
        var items = await _repository.GetListAsync<FamiliaArtigoEntity, Guid>(new FamiliaArtigoByClinicaSpec(clinicaId));
        return items.Any() ? items.Max(x => x.Codigo) + 1 : 1;
    }

    private async Task<Guid?> ResolveClinicaIdAsync()
    {
        await _currentClinicaService.SetClinicaAsync();
        if (Guid.TryParse(_currentClinicaService.ClinicaId, out var clinicaId) && clinicaId != Guid.Empty)
            return clinicaId;
        return null;
    }
}