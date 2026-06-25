using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.ZonaComercialService.DTOs;
using CliCloud.Application.Services.Faturacao.ZonaComercialService.Filters;
using CliCloud.Application.Services.Faturacao.ZonaComercialService.Specifications;
using CliCloud.Application.Utility;
using ZonaComercialEntity = CliCloud.Domain.Entities.Faturacao.ZonaComercial;

namespace CliCloud.Application.Services.Faturacao.ZonaComercialService;

public class ZonaComercialService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentClinicaService currentClinicaService) : IZonaComercialService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    public async Task<Response<IEnumerable<ZonaComercialDTO>>> GetAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<ZonaComercialDTO>>("Clínica atual não definida");

        var spec = new ZonaComercialSearchList(clinicaId.Value, keyword);
        var list = await _repository.GetListAsync<ZonaComercialEntity, ZonaComercialDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ZonaComercialLightDTO>>> GetLightAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<ZonaComercialLightDTO>>("Clínica atual não definida");

        var spec = new ZonaComercialSearchList(clinicaId.Value, keyword);
        var list = await _repository.GetListAsync<ZonaComercialEntity, ZonaComercialLightDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ZonaComercialTableDTO>> GetPaginatedAsync(ZonaComercialTableFilter filter)
    {
        if (filter.Filters != null && filter.Filters.Count > 0)
            filter.PageNumber = 1;

        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return new PaginatedResponse<ZonaComercialTableDTO>([], 0, filter.PageNumber, filter.PageSize);

        string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
        var spec = new ZonaComercialSearchTable(filter, clinicaId.Value, order);
        return await _repository.GetPaginatedResultsAsync<ZonaComercialEntity, ZonaComercialTableDTO, Guid>(
            filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ZonaComercialTableDTO>>> GetAllAsync(ZonaComercialAllFilter? filter)
    {
        try
        {
            filter ??= new ZonaComercialAllFilter();
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<IEnumerable<ZonaComercialTableDTO>>("Clínica atual não definida");

            var tableFilter = new ZonaComercialTableFilter
            {
                Filters = filter.Filters,
                FiltroBox = filter.FiltroBox,
                CodigoDe = filter.CodigoDe,
                CodigoAte = filter.CodigoAte,
                DescricaoDe = filter.DescricaoDe,
                DescricaoAte = filter.DescricaoAte,
            };
            string order = filter.GetOrderByString();
            var spec = new ZonaComercialSearchTable(tableFilter, clinicaId.Value, order);
            var list = await _repository.GetListAsync<ZonaComercialEntity, ZonaComercialTableDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<ZonaComercialTableDTO>>(ex.Message);
        }
    }

    public async Task<Response<ZonaComercialDTO>> GetAsync(Guid id)
    {
        try
        {
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<ZonaComercialDTO>("Clínica atual não definida");

            var spec = new ZonaComercialByIdClinicaSpec(id, clinicaId.Value);
            var dto = (await _repository.GetListAsync<ZonaComercialEntity, ZonaComercialDTO, Guid>(spec)).FirstOrDefault();
            if (dto == null)
                return ResponseFactory.Fail<ZonaComercialDTO>("Zona não encontrada");

            return ResponseFactory.Success(dto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<ZonaComercialDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CreateAsync(CreateZonaComercialRequest request)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        var entity = _mapper.Map<ZonaComercialEntity>(request);
        entity.ClinicaId = clinicaId;
        entity.Codigo = await ObterProximoCodigoAsync(clinicaId);
        entity.Descricao = request.Descricao.Trim();

        try
        {
            var created = await _repository.CreateAsync<ZonaComercialEntity, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateZonaComercialRequest request, Guid id)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        var spec = new ZonaComercialByIdClinicaSpec(id, clinicaId);
        var existing = (await _repository.GetListAsync<ZonaComercialEntity, Guid>(spec)).FirstOrDefault();
        if (existing == null)
            return ResponseFactory.Fail<Guid>("Zona não encontrada");

        existing.Descricao = request.Descricao.Trim();

        try
        {
            _ = await _repository.UpdateAsync<ZonaComercialEntity, Guid>(existing);
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

            var spec = new ZonaComercialByIdClinicaSpec(id, clinicaIdOpt.Value);
            var entity = (await _repository.GetListAsync<ZonaComercialEntity, Guid>(spec)).FirstOrDefault();
            if (entity == null)
                return ResponseFactory.Fail<Guid>("Zona não encontrada");

            await _repository.RemoveAsync<ZonaComercialEntity, Guid>(entity);
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
        var items = await _repository.GetListAsync<ZonaComercialEntity, Guid>(new ZonaComercialByClinicaSpec(clinicaId));
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