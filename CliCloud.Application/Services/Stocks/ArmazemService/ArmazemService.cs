using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Stocks.ArmazemService.DTOs;
using CliCloud.Application.Services.Stocks.ArmazemService.Filters;
using CliCloud.Application.Services.Stocks.ArmazemService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Utility;
using ArmazemEntity = CliCloud.Domain.Entities.Stocks.Armazem;

namespace CliCloud.Application.Services.Stocks.ArmazemService;

public class ArmazemService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentClinicaService currentClinicaService) : IArmazemService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    public async Task<Response<IEnumerable<ArmazemDTO>>> GetAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<ArmazemDTO>>("Clínica atual não definida");

        var spec = new ArmazemSearchList(keyword, clinicaId.Value);
        var list = await _repository.GetListAsync<ArmazemEntity, ArmazemDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ArmazemLightDTO>>> GetLightAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<ArmazemLightDTO>>("Clínica atual não definida");

        var spec = new ArmazemSearchList(keyword, clinicaId.Value);
        var list = await _repository.GetListAsync<ArmazemEntity, ArmazemLightDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ArmazemTableDTO>> GetPaginatedAsync(ArmazemTableFilter filter)
    {
        if (filter.Filters != null && filter.Filters.Count > 0)
            filter.PageNumber = 1;

        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return new PaginatedResponse<ArmazemTableDTO>([], 0, filter.PageNumber, filter.PageSize);

        string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
        var spec = new ArmazemSearchTable(filter, clinicaId.Value, order);
        return await _repository.GetPaginatedResultsAsync<ArmazemEntity, ArmazemTableDTO, Guid>(
            filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ArmazemTableDTO>>> GetAllAsync(ArmazemAllFilter? filter)
    {
        try
        {
            filter ??= new ArmazemAllFilter();
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<IEnumerable<ArmazemTableDTO>>("Clínica atual não definida");

            var tableFilter = new ArmazemTableFilter
            {
                Filters = filter.Filters,
                FiltroBox = filter.FiltroBox,
                CodigoDe = filter.CodigoDe,
                CodigoAte = filter.CodigoAte,
                NomeDe = filter.NomeDe,
                NomeAte = filter.NomeAte,
                ArmazemGeral = filter.ArmazemGeral
            };
            string order = filter.GetOrderByString();
            var spec = new ArmazemSearchTable(tableFilter, clinicaId.Value, order);
            var list = await _repository.GetListAsync<ArmazemEntity, ArmazemTableDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<ArmazemTableDTO>>(ex.Message);
        }
    }

    public async Task<Response<ArmazemDTO>> GetAsync(Guid id)
    {
        try
        {
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<ArmazemDTO>("Clínica atual não definida");

            var spec = new ArmazemByIdClinicaSpec(id, clinicaId.Value);
            var dto = (await _repository.GetListAsync<ArmazemEntity, ArmazemDTO, Guid>(spec)).FirstOrDefault();
            if (dto == null)
                return ResponseFactory.Fail<ArmazemDTO>("Armazém não encontrado");

            return ResponseFactory.Success(dto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<ArmazemDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CreateAsync(CreateArmazemRequest request)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        Response<Guid>? validation = await ValidarCodigoPostalAsync(request.CodigoPostalId);
        if (validation != null)
            return validation;

        var armazens = await ObterArmazensClinicaAsync(clinicaId);
        Response<Guid>? geralValidation = ValidarArmazemGeral(request.ArmazemGeral, null, armazens);
        if (geralValidation != null)
            return geralValidation;

        var entity = _mapper.Map<ArmazemEntity>(request);
        entity.ClinicaId = clinicaId;
        entity.Codigo = await ObterProximoCodigoAsync(clinicaId);
        entity.Nome = request.Nome.Trim();

        try
        {
            if (entity.ArmazemGeral)
                await DesmarcarOutrosArmazensGeraisAsync(clinicaId, null);

            var created = await _repository.CreateAsync<ArmazemEntity, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateArmazemRequest request, Guid id)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        var spec = new ArmazemByIdClinicaSpec(id, clinicaId);
        var existing = (await _repository.GetListAsync<ArmazemEntity, Guid>(spec)).FirstOrDefault();
        if (existing == null)
            return ResponseFactory.Fail<Guid>("Armazém não encontrado");

        Response<Guid>? validation = await ValidarCodigoPostalAsync(request.CodigoPostalId);
        if (validation != null)
            return validation;

        var armazens = await ObterArmazensClinicaAsync(clinicaId);
        Response<Guid>? geralValidation = ValidarArmazemGeral(request.ArmazemGeral, id, armazens);
        if (geralValidation != null)
            return geralValidation;

        _mapper.Map(request, existing);
        existing.ClinicaId = clinicaId;
        existing.Nome = request.Nome.Trim();

        try
        {
            if (existing.ArmazemGeral)
                await DesmarcarOutrosArmazensGeraisAsync(clinicaId, existing.Id);

            _ = await _repository.UpdateAsync<ArmazemEntity, Guid>(existing);
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

            var spec = new ArmazemByIdClinicaSpec(id, clinicaIdOpt.Value);
            var entity = (await _repository.GetListAsync<ArmazemEntity, Guid>(spec)).FirstOrDefault();
            if (entity == null)
                return ResponseFactory.Fail<Guid>("Armazém não encontrado");

            if (entity.ArmazemGeral)
            {
                var armazens = await ObterArmazensClinicaAsync(clinicaIdOpt.Value);
                if (!armazens.Any(x => x.Id != id && x.ArmazemGeral))
                    return ResponseFactory.Fail<Guid>("Tem de existir pelo menos um armazém geral.");
            }

            await _repository.RemoveAsync<ArmazemEntity, Guid>(entity);
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

    public static async Task EnsureArmazemGeralDefaultAsync(IRepositoryAsync repository, Guid clinicaId, string clinicaNome)
    {
        var existing = await repository.GetListAsync<ArmazemEntity, Guid>(new ArmazemByClinicaSpec(clinicaId));
        if (existing.Any())
            return;

        _ = await repository.CreateAsync<ArmazemEntity, Guid>(new ArmazemEntity
        {
            ClinicaId = clinicaId,
            Codigo = 1,
            Nome = $"Armazém Geral ({clinicaNome})",
            ArmazemGeral = true
        });
    }

    private async Task<List<ArmazemEntity>> ObterArmazensClinicaAsync(Guid clinicaId)
        => (await _repository.GetListAsync<ArmazemEntity, Guid>(new ArmazemByClinicaSpec(clinicaId))).ToList();

    private static Response<Guid>? ValidarArmazemGeral(bool armazemGeral, Guid? currentId, List<ArmazemEntity> armazens)
    {
        bool outrosComGeral = armazens.Any(x => x.ArmazemGeral && x.Id != currentId);
        if (!armazemGeral && !outrosComGeral)
            return ResponseFactory.Fail<Guid>("Tem de ter um armazém geral.");

        return null;
    }

    private async Task DesmarcarOutrosArmazensGeraisAsync(Guid clinicaId, Guid? exceptId)
    {
        var armazens = await ObterArmazensClinicaAsync(clinicaId);
        foreach (var armazem in armazens.Where(x => x.ArmazemGeral && x.Id != exceptId))
        {
            armazem.ArmazemGeral = false;
            _ = await _repository.UpdateAsync<ArmazemEntity, Guid>(armazem);
        }
    }

    private async Task<Response<Guid>?> ValidarCodigoPostalAsync(Guid? codigoPostalId)
    {
        if (!codigoPostalId.HasValue)
            return null;

        try
        {
            _ = await _repository.GetByIdAsync<CodigoPostal, Guid>(codigoPostalId.Value);
        }
        catch
        {
            return ResponseFactory.Fail<Guid>("Código postal não encontrado.");
        }

        return null;
    }

    private async Task<int> ObterProximoCodigoAsync(Guid clinicaId)
    {
        var items = await _repository.GetListAsync<ArmazemEntity, Guid>(new ArmazemByClinicaSpec(clinicaId));
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
