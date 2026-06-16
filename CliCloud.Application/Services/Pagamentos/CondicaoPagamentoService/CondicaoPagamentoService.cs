using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.DTOs;
using CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.Filters;
using CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.Specifications;
using CliCloud.Application.Utility;
using CondicaoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.CondicaoPagamento;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService;

public class CondicaoPagamentoService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentClinicaService currentClinicaService) : ICondicaoPagamentoService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    public async Task<Response<IEnumerable<CondicaoPagamentoDTO>>> GetAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<CondicaoPagamentoDTO>>("Clínica atual não definida");

        var spec = new CondicaoPagamentoSearchList(keyword, clinicaId.Value);
        var list = await _repository.GetListAsync<CondicaoPagamentoEntity, CondicaoPagamentoDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<CondicaoPagamentoLightDTO>>> GetLightAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<CondicaoPagamentoLightDTO>>("Clínica atual não definida");

        var spec = new CondicaoPagamentoSearchList(keyword, clinicaId.Value);
        var list = await _repository.GetListAsync<CondicaoPagamentoEntity, CondicaoPagamentoLightDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<CondicaoPagamentoTableDTO>> GetPaginatedAsync(CondicaoPagamentoTableFilter filter)
    {
        if (filter.Filters != null && filter.Filters.Count > 0)
            filter.PageNumber = 1;

        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return new PaginatedResponse<CondicaoPagamentoTableDTO>([], 0, filter.PageNumber, filter.PageSize);

        string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
        var spec = new CondicaoPagamentoSearchTable(filter, clinicaId.Value, order);
        return await _repository.GetPaginatedResultsAsync<CondicaoPagamentoEntity, CondicaoPagamentoTableDTO, Guid>(
            filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<CondicaoPagamentoTableDTO>>> GetAllAsync(CondicaoPagamentoAllFilter? filter)
    {
        try
        {
            filter ??= new CondicaoPagamentoAllFilter();
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<IEnumerable<CondicaoPagamentoTableDTO>>("Clínica atual não definida");

            var tableFilter = new CondicaoPagamentoTableFilter
            {
                Filters = filter.Filters,
                FiltroBox = filter.FiltroBox,
                CodigoDe = filter.CodigoDe,
                CodigoAte = filter.CodigoAte,
                DescricaoDe = filter.DescricaoDe,
                DescricaoAte = filter.DescricaoAte
            };
            string order = filter.GetOrderByString();
            var spec = new CondicaoPagamentoSearchTable(tableFilter, clinicaId.Value, order);
            var list = await _repository.GetListAsync<CondicaoPagamentoEntity, CondicaoPagamentoTableDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<CondicaoPagamentoTableDTO>>(ex.Message);
        }
    }

    public async Task<Response<CondicaoPagamentoDTO>> GetAsync(Guid id)
    {
        try
        {
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<CondicaoPagamentoDTO>("Clínica atual não definida");

            var spec = new CondicaoPagamentoByIdClinicaSpec(id, clinicaId.Value);
            var dto = (await _repository.GetListAsync<CondicaoPagamentoEntity, CondicaoPagamentoDTO, Guid>(spec)).FirstOrDefault();
            if (dto == null)
                return ResponseFactory.Fail<CondicaoPagamentoDTO>("Condição de pagamento não encontrada");

            return ResponseFactory.Success(dto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<CondicaoPagamentoDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CreateAsync(CreateCondicaoPagamentoRequest request)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        var entity = _mapper.Map<CondicaoPagamentoEntity>(request);
        entity.ClinicaId = clinicaId;
        entity.Codigo = await ObterProximoCodigoAsync(clinicaId);
        entity.Descricao = request.Descricao.Trim();

        try
        {
            var created = await _repository.CreateAsync<CondicaoPagamentoEntity, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateCondicaoPagamentoRequest request, Guid id)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        var spec = new CondicaoPagamentoByIdClinicaSpec(id, clinicaId);
        var existing = (await _repository.GetListAsync<CondicaoPagamentoEntity, Guid>(spec)).FirstOrDefault();
        if (existing == null)
            return ResponseFactory.Fail<Guid>("Condição de pagamento não encontrada");

        _mapper.Map(request, existing);
        existing.ClinicaId = clinicaId;
        existing.Descricao = request.Descricao.Trim();

        try
        {
            _ = await _repository.UpdateAsync<CondicaoPagamentoEntity, Guid>(existing);
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

            var spec = new CondicaoPagamentoByIdClinicaSpec(id, clinicaIdOpt.Value);
            var entity = (await _repository.GetListAsync<CondicaoPagamentoEntity, Guid>(spec)).FirstOrDefault();
            if (entity == null)
                return ResponseFactory.Fail<Guid>("Condição de pagamento não encontrada");

            bool emUso = await _repository.ExistsAsync<CliCloud.Domain.Entities.Documentos.Documento, Guid>(new DocumentoByCondicaoPagamentoSpec(entity.Id))
                || await _repository.ExistsAsync<CliCloud.Domain.Entities.Organismos.Organismo, Guid>(new OrganismoByCondicaoPagamentoSpec(entity.Id))
                || await _repository.ExistsAsync<CliCloud.Domain.Entities.Fornecedores.Fornecedor, Guid>(new FornecedorByCondicaoPagamentoSpec(entity.Id))
                || await _repository.ExistsAsync<CliCloud.Domain.Entities.Empresas.Empresa, Guid>(new EmpresaByCondicaoPagamentoSpec(entity.Id));

            if (emUso)
                return ResponseFactory.Fail<Guid>("Condição de pagamento em uso e não pode ser eliminada.");

            await _repository.RemoveAsync<CondicaoPagamentoEntity, Guid>(entity);
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
        var items = await _repository.GetListAsync<CondicaoPagamentoEntity, Guid>(new CondicaoPagamentoByClinicaSpec(clinicaId));
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
