using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Stocks.SubsistemaArtigoService.DTOs;
using CliCloud.Application.Services.Stocks.SubsistemaArtigoService.Filters;
using CliCloud.Application.Services.Stocks.SubsistemaArtigoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Stocks;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService;

public class SubsistemaArtigoService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentClinicaService currentClinicaService
) : ISubsistemaArtigoService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    public async Task<PaginatedResponse<SubsistemaArtigoTableDTO>> GetPaginatedAsync(SubsistemaArtigoTableFilter filter)
    {
        if (filter.ArtigoId.HasValue || filter.OrganismoId.HasValue || filter.Filters?.Count > 0) 
            filter.PageNumber = 1;

        Guid? clinicaId = await ResolveClinicaIdAsync();
        if(!clinicaId.HasValue)
            return new PaginatedResponse<SubsistemaArtigoTableDTO>([], 0, filter.PageNumber, filter.PageSize);

        string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
        var spec = new SubsistemaArtigoSearchTable(filter, clinicaId.Value, order);
        return await _repository.GetPaginatedResultsAsync<SubsistemaArtigo, SubsistemaArtigoTableDTO, Guid>(
            filter.PageNumber, filter.PageSize, spec
        );
    }

    public async Task<Response<SubsistemaArtigoDTO>> GetAsync(Guid id)
    {
        try
        {
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<SubsistemaArtigoDTO>("Clínica atual não definida");

            var spec = new SubsistemaArtigoByIdClinicaSpec(id, clinicaId.Value);
            var dto = (await _repository.GetListAsync<SubsistemaArtigo, SubsistemaArtigoDTO, Guid>(spec)).FirstOrDefault();
            if(dto == null)
                return ResponseFactory.Fail<SubsistemaArtigoDTO>("Subsistema de artigo não encontrado");

            return ResponseFactory.Success(dto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<SubsistemaArtigoDTO>(ex.Message);
        }
    }
    

    public async Task<Response<Guid>> CreateAsync(CreateSubsistemaArtigoRequest request)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if(!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        var dupSpec = new SubsistemaArtigoMatchCompositeSpec(clinicaId, request.ArtigoId, request.OrganismoId);
        if ( await _repository.ExistsAsync<SubsistemaArtigo, Guid>(dupSpec))
            return ResponseFactory.Fail<Guid>("Já existe acordo para este artigo e organismo");
        
        var entity = _mapper.Map<SubsistemaArtigo>(request);
        entity.ClinicaId = clinicaId;
        entity.CodigoCartaoInstituicao = request.CodigoCartaoInstituicao.Trim();

        try
        {
            var created = await _repository.CreateAsync<SubsistemaArtigo, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }

        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateSubsistemaArtigoRequest request, Guid id)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if(!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        var spec = new SubsistemaArtigoByIdClinicaSpec(id, clinicaIdOpt.Value);
        var existing = (await _repository.GetListAsync<SubsistemaArtigo, Guid>(spec)).FirstOrDefault();
        if(existing == null)
            return ResponseFactory.Fail<Guid>("Subsistema de artigo não encontrado");

        existing.CodigoCartaoInstituicao = request.CodigoCartaoInstituicao.Trim();
        existing.ValorServico = request.ValorServico;
        existing.MargemOrganismoPercent = request.MargemOrganismoPercent;
        existing.ValorOrganismo = request.ValorOrganismo;
        existing.ValorUtente = request.ValorUtente;
        existing.Inativo = request.Inativo;
        existing.CodigoComplementarAdse = request.CodigoComplementarAdse?.Trim();

        try
        {
            _ = await _repository.UpdateAsync<SubsistemaArtigo, Guid>(existing);
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

            var spec = new SubsistemaArtigoByIdClinicaSpec(id, clinicaIdOpt.Value);
            var entity = (await _repository.GetListAsync<SubsistemaArtigo, Guid>(spec)).FirstOrDefault();
            if(entity == null)
                return ResponseFactory.Fail<Guid>("Subsistema de artigo não encontrado");

            await _repository.RemoveByIdAsync<SubsistemaArtigo, Guid>(id);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids)
    {
        var ok = new List<Guid>();
        foreach(var id in ids.ToList())
        {
            var result = await DeleteAsync(id);
            if (result.Status == ResponseStatus.Success)
                ok.Add(id);
            else
                _repository.ClearChangeTracker();
        }

        if(ok.Count == ids.Count())
            return ResponseFactory.Success<IEnumerable<Guid>>(ok);
        if(ok.Count > 0)
            return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {ids.Count()}");
        return ResponseFactory.Fail<IEnumerable<Guid>>("Nenhum eliminado");
    }

    private async Task<Guid?> ResolveClinicaIdAsync()
    {
        await _currentClinicaService.SetClinicaAsync();
        if (Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId) && clinicaId != Guid.Empty)
            return clinicaId;
        return null;
    }
}