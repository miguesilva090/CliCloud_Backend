using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Stocks.ArtigoService.DTOs;
using CliCloud.Application.Services.Stocks.ArtigoService.Filters;
using CliCloud.Application.Services.Stocks.ArtigoService.Specifications;
using CliCloud.Application.Services.Stocks.ArmazemService.Specifications;
using CliCloud.Application.Services.Stocks.FamiliaArtigoService.Specifications;
using CliCloud.Application.Services.Stocks.UnidadeMedidaService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.TaxasIva;
using ArtigoEntity = CliCloud.Domain.Entities.Stocks.Artigo;
using ArmazemEntity = CliCloud.Domain.Entities.Stocks.Armazem;
using FamiliaArtigoEntity = CliCloud.Domain.Entities.Stocks.FamiliaArtigo;
using UnidadeMedidaEntity = CliCloud.Domain.Entities.Stocks.UnidadeMedida;

namespace CliCloud.Application.Services.Stocks.ArtigoService;

public class ArtigoService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentClinicaService currentClinicaService) : IArtigoService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    public async Task<Response<IEnumerable<ArtigoDTO>>> GetAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<ArtigoDTO>>("Clínica atual não definida");

        var spec = new ArtigoSearchList(keyword, clinicaId.Value);
        var list = await _repository.GetListAsync<ArtigoEntity, ArtigoDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ArtigoLightDTO>>> GetLightAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<ArtigoLightDTO>>("Clínica atual não definida");

        var spec = new ArtigoSearchList(keyword, clinicaId.Value);
        var list = await _repository.GetListAsync<ArtigoEntity, ArtigoLightDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ArtigoTableDTO>> GetPaginatedAsync(ArtigoTableFilter filter)
    {
        if (filter.Filters != null && filter.Filters.Count > 0)
            filter.PageNumber = 1;

        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return new PaginatedResponse<ArtigoTableDTO>([], 0, filter.PageNumber, filter.PageSize);

        string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
        var spec = new ArtigoSearchTable(filter, clinicaId.Value, order);
        return await _repository.GetPaginatedResultsAsync<ArtigoEntity, ArtigoTableDTO, Guid>(
            filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ArtigoTableDTO>>> GetAllAsync(ArtigoAllFilter? filter)
    {
        try
        {
            filter ??= new ArtigoAllFilter();
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<IEnumerable<ArtigoTableDTO>>("Clínica atual não definida");

            var tableFilter = new ArtigoTableFilter
            {
                Filters = filter.Filters,
                FiltroBox = filter.FiltroBox,
                CodigoDe = filter.CodigoDe,
                CodigoAte = filter.CodigoAte,
                NumeroArtigoDe = filter.NumeroArtigoDe,
                NumeroArtigoAte = filter.NumeroArtigoAte,
                DescricaoDe = filter.DescricaoDe,
                DescricaoAte = filter.DescricaoAte,
                Inativo = filter.Inativo,
                Descontinuado = filter.Descontinuado,
                TipoArtigo = filter.TipoArtigo,
            };
            string order = filter.GetOrderByString();
            var spec = new ArtigoSearchTable(tableFilter, clinicaId.Value, order);
            var list = await _repository.GetListAsync<ArtigoEntity, ArtigoTableDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<ArtigoTableDTO>>(ex.Message);
        }
    }

    public async Task<Response<ArtigoDTO>> GetAsync(Guid id)
    {
        try
        {
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<ArtigoDTO>("Clínica atual não definida");

            var spec = new ArtigoByIdClinicaSpec(id, clinicaId.Value);
            var dto = (await _repository.GetListAsync<ArtigoEntity, ArtigoDTO, Guid>(spec)).FirstOrDefault();
            if (dto == null)
                return ResponseFactory.Fail<ArtigoDTO>("Artigo não encontrado");

            return ResponseFactory.Success(dto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<ArtigoDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CreateAsync(CreateArtigoRequest request)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        Response<Guid>? validation = await ValidarReferenciasAsync(request, clinicaId);
        if (validation != null)
            return validation;

        var entity = _mapper.Map<ArtigoEntity>(request);
        entity.ClinicaId = clinicaId;
        entity.Codigo = await ObterProximoCodigoAsync(clinicaId);
        entity.Descricao = request.Descricao.Trim();
        entity.NumeroArtigo = string.IsNullOrWhiteSpace(request.NumeroArtigo)
            ? entity.Codigo.ToString()
            : request.NumeroArtigo.Trim();
        entity.StockReal = 0;

        try
        {
            var created = await _repository.CreateAsync<ArtigoEntity, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateArtigoRequest request, Guid id)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        var spec = new ArtigoByIdClinicaSpec(id, clinicaId);
        var existing = (await _repository.GetListAsync<ArtigoEntity, Guid>(spec)).FirstOrDefault();
        if (existing == null)
            return ResponseFactory.Fail<Guid>("Artigo não encontrado");

        Response<Guid>? validation = await ValidarReferenciasAsync(request, clinicaId);
        if (validation != null)
            return validation;

        _mapper.Map(request, existing);
        existing.ClinicaId = clinicaId;
        existing.Descricao = request.Descricao.Trim();
        existing.NumeroArtigo = string.IsNullOrWhiteSpace(request.NumeroArtigo)
            ? existing.Codigo.ToString()
            : request.NumeroArtigo.Trim();

        try
        {
            _ = await _repository.UpdateAsync<ArtigoEntity, Guid>(existing);
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

            var spec = new ArtigoByIdClinicaSpec(id, clinicaIdOpt.Value);
            var entity = (await _repository.GetListAsync<ArtigoEntity, Guid>(spec)).FirstOrDefault();
            if (entity == null)
                return ResponseFactory.Fail<Guid>("Artigo não encontrado");

            await _repository.RemoveAsync<ArtigoEntity, Guid>(entity);
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
        return ResponseFactory.Fail<IEnumerable<Guid>>("Nenhum artigo eliminado.");
    }

    private async Task<Response<Guid>?> ValidarReferenciasAsync(CreateArtigoRequest request, Guid clinicaId)
    {
        var unidade = (await _repository.GetListAsync<UnidadeMedidaEntity, Guid>(
            new UnidadeMedidaByIdClinicaSpec(request.UnidadeMedidaId, clinicaId))).FirstOrDefault();
        if (unidade == null)
            return ResponseFactory.Fail<Guid>("Unidade de medida não encontrada.");

        var armazem = (await _repository.GetListAsync<ArmazemEntity, Guid>(
            new ArmazemByIdClinicaSpec(request.ArmazemId, clinicaId))).FirstOrDefault();
        if (armazem == null)
            return ResponseFactory.Fail<Guid>("Armazém não encontrado.");

        if (request.FamiliaArtigoId.HasValue)
        {
            var familia = (await _repository.GetListAsync<FamiliaArtigoEntity, Guid>(
                new FamiliaArtigoByIdClinicaSpec(request.FamiliaArtigoId.Value, clinicaId))).FirstOrDefault();
            if (familia == null)
                return ResponseFactory.Fail<Guid>("Família de artigo não encontrada.");
        }

        TaxaIva taxaIva;
        try
        {
            taxaIva = await _repository.GetByIdAsync<TaxaIva, Guid>(request.TaxaIvaId);
        }
        catch
        {
            return ResponseFactory.Fail<Guid>("Taxa de IVA não encontrada.");
        }

        if (taxaIva.Taxa == 0 && !request.MotivoIsencaoId.HasValue)
            return ResponseFactory.Fail<Guid>("Motivo de isenção é obrigatório para taxa isenta.");

        if (request.MotivoIsencaoId.HasValue)
        {
            try
            {
                _ = await _repository.GetByIdAsync<MotivoIsencao, Guid>(request.MotivoIsencaoId.Value);
            }
            catch
            {
                return ResponseFactory.Fail<Guid>("Motivo de isenção não encontrado.");
            }
        }

        return null;
    }

    private async Task<int> ObterProximoCodigoAsync(Guid clinicaId)
    {
        var items = await _repository.GetListAsync<ArtigoEntity, Guid>(new ArtigoByClinicaSpec(clinicaId));
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
