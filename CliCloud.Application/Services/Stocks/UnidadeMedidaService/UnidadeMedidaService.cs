using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Stocks.UnidadeMedidaService.DTOs;
using CliCloud.Application.Services.Stocks.UnidadeMedidaService.Filters;
using CliCloud.Application.Services.Stocks.UnidadeMedidaService.Specifications;
using CliCloud.Application.Utility;
using UnidadeMedidaEntity = CliCloud.Domain.Entities.Stocks.UnidadeMedida;

namespace CliCloud.Application.Services.Stocks.UnidadeMedidaService;

public class UnidadeMedidaService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentClinicaService currentClinicaService) : IUnidadeMedidaService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    public async Task<Response<IEnumerable<UnidadeMedidaDTO>>> GetAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<UnidadeMedidaDTO>>("Clínica atual não definida");

        var spec = new UnidadeMedidaSearchList(keyword, clinicaId.Value);
        var list = await _repository.GetListAsync<UnidadeMedidaEntity, UnidadeMedidaDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<UnidadeMedidaLightDTO>>> GetLightAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<UnidadeMedidaLightDTO>>("Clínica atual não definida");

        var spec = new UnidadeMedidaSearchList(keyword, clinicaId.Value);
        var list = await _repository.GetListAsync<UnidadeMedidaEntity, UnidadeMedidaLightDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<UnidadeMedidaTableDTO>> GetPaginatedAsync(UnidadeMedidaTableFilter filter)
    {
        if (filter.Filters != null && filter.Filters.Count > 0)
            filter.PageNumber = 1;

        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return new PaginatedResponse<UnidadeMedidaTableDTO>([], 0, filter.PageNumber, filter.PageSize);

        string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
        var spec = new UnidadeMedidaSearchTable(filter, clinicaId.Value, order);
        return await _repository.GetPaginatedResultsAsync<UnidadeMedidaEntity, UnidadeMedidaTableDTO, Guid>(
            filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<UnidadeMedidaTableDTO>>> GetAllAsync(UnidadeMedidaAllFilter? filter)
    {
        try
        {
            filter ??= new UnidadeMedidaAllFilter();
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<IEnumerable<UnidadeMedidaTableDTO>>("Clínica atual não definida");

            var tableFilter = new UnidadeMedidaTableFilter
            {
                Filters = filter.Filters,
                FiltroBox = filter.FiltroBox,
                CodigoDe = filter.CodigoDe,
                CodigoAte = filter.CodigoAte,
                DescricaoDe = filter.DescricaoDe,
                DescricaoAte = filter.DescricaoAte,
            };
            string order = filter.GetOrderByString();
            var spec = new UnidadeMedidaSearchTable(tableFilter, clinicaId.Value, order);
            var list = await _repository.GetListAsync<UnidadeMedidaEntity, UnidadeMedidaTableDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<UnidadeMedidaTableDTO>>(ex.Message);
        }
    }

    public async Task<Response<UnidadeMedidaDTO>> GetAsync(Guid id)
    {
        try
        {
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<UnidadeMedidaDTO>("Clínica atual não definida");

            var spec = new UnidadeMedidaByIdClinicaSpec(id, clinicaId.Value);
            var dto = (await _repository.GetListAsync<UnidadeMedidaEntity, UnidadeMedidaDTO, Guid>(spec)).FirstOrDefault();
            if (dto == null)
                return ResponseFactory.Fail<UnidadeMedidaDTO>("Unidade de medida não encontrada");

            return ResponseFactory.Success(dto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<UnidadeMedidaDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CreateAsync(CreateUnidadeMedidaRequest request)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        var entity = _mapper.Map<UnidadeMedidaEntity>(request);
        entity.ClinicaId = clinicaId;
        entity.Codigo = await ObterProximoCodigoAsync(clinicaId);
        entity.Descricao = request.Descricao.Trim();

        try
        {
            var created = await _repository.CreateAsync<UnidadeMedidaEntity, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateUnidadeMedidaRequest request, Guid id)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        var spec = new UnidadeMedidaByIdClinicaSpec(id, clinicaId);
        var existing = (await _repository.GetListAsync<UnidadeMedidaEntity, Guid>(spec)).FirstOrDefault();
        if (existing == null)
            return ResponseFactory.Fail<Guid>("Unidade de medida não encontrada");

        existing.Descricao = request.Descricao.Trim();

        try
        {
            _ = await _repository.UpdateAsync<UnidadeMedidaEntity, Guid>(existing);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(existing.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> DeleteAsync(Guid id)
    {
        try
        {
            Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
            if (!clinicaIdOpt.HasValue)
                return ResponseFactory.Fail<Guid>("Clínica atual inválida");

            var spec = new UnidadeMedidaByIdClinicaSpec(id, clinicaIdOpt.Value);
            var entity = (await _repository.GetListAsync<UnidadeMedidaEntity, Guid>(spec)).FirstOrDefault();
            if (entity == null)
                return ResponseFactory.Fail<Guid>("Unidade de medida não encontrada");

            await _repository.RemoveAsync<UnidadeMedidaEntity, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
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
            if (result.Status == ResponseStatus.Success)
                ok.Add(id);
            else
                _repository.ClearChangeTracker();
        }

        if (ok.Count == ids.Count())
            return ResponseFactory.Success<IEnumerable<Guid>>(ok);
        if (ok.Count > 0)
            return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {ids.Count()}.");
        return ResponseFactory.Fail<IEnumerable<Guid>>("Nenhum eliminado.");
    }

    private async Task<int> ObterProximoCodigoAsync(Guid clinicaId)
    {
        var items = await _repository.GetListAsync<UnidadeMedidaEntity, Guid>(new UnidadeMedidaByClinicaSpec(clinicaId));
        return items.Any() ? items.Max(x => x.Codigo) + 1 : 1;
    }

    private async Task<Guid?> ResolveClinicaIdAsync()
    {
        await _currentClinicaService.SetClinicaAsync();
        if (Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId) && clinicaId != Guid.Empty)
            return clinicaId;
        return null;
    }
}
