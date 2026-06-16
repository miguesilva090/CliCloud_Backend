using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Pagamentos.ModoPagamentoService.DTOs;
using CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Filters;
using CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Specifications;
using CliCloud.Application.Services.Pagamentos.TipoPagamentoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Bancos;
using ModoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.ModoPagamento;
using TipoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.TipoPagamento;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService;

public class ModoPagamentoService(
    IRepositoryAsync repository,
    IMapper mapper,
    ICurrentClinicaService currentClinicaService) : IModoPagamentoService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    public async Task<Response<IEnumerable<ModoPagamentoDTO>>> GetAsync(string keyword = "")
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<ModoPagamentoDTO>>("Clínica atual não definida");

        var spec = new ModoPagamentoSearchList(keyword, clinicaId.Value, apenasAtivos: false);
        var list = await _repository.GetListAsync<ModoPagamentoEntity, ModoPagamentoDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ModoPagamentoLightDTO>>> GetLightAsync(string keyword = "", bool apenasAtivos = false)
    {
        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return ResponseFactory.Fail<IEnumerable<ModoPagamentoLightDTO>>("Clínica atual não definida");

        var spec = new ModoPagamentoSearchList(keyword, clinicaId.Value, apenasAtivos);
        var list = await _repository.GetListAsync<ModoPagamentoEntity, ModoPagamentoLightDTO, Guid>(spec);
        return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ModoPagamentoTableDTO>> GetPaginatedAsync(ModoPagamentoTableFilter filter)
    {
        if (filter.Filters != null && filter.Filters.Count > 0)
            filter.PageNumber = 1;

        Guid? clinicaId = await ResolveClinicaIdAsync();
        if (!clinicaId.HasValue)
            return new PaginatedResponse<ModoPagamentoTableDTO>([], 0, filter.PageNumber, filter.PageSize);

        string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
        var spec = new ModoPagamentoSearchTable(filter, clinicaId.Value, order);
        return await _repository.GetPaginatedResultsAsync<ModoPagamentoEntity, ModoPagamentoTableDTO, Guid>(
            filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ModoPagamentoTableDTO>>> GetAllAsync(ModoPagamentoAllFilter? filter)
    {
        try
        {
            filter ??= new ModoPagamentoAllFilter();
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<IEnumerable<ModoPagamentoTableDTO>>("Clínica atual não definida");

            var tableFilter = new ModoPagamentoTableFilter
            {
                Filters = filter.Filters,
                FiltroBox = filter.FiltroBox,
                CodigoDe = filter.CodigoDe,
                CodigoAte = filter.CodigoAte,
                DescricaoDe = filter.DescricaoDe,
                DescricaoAte = filter.DescricaoAte,
                FiltrarHistorico = filter.FiltrarHistorico
            };
            string order = filter.GetOrderByString();
            var spec = new ModoPagamentoSearchTable(tableFilter, clinicaId.Value, order);
            var list = await _repository.GetListAsync<ModoPagamentoEntity, ModoPagamentoTableDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<IEnumerable<ModoPagamentoTableDTO>>(ex.Message);
        }
    }

    public async Task<Response<ModoPagamentoDTO>> GetAsync(Guid id)
    {
        try
        {
            Guid? clinicaId = await ResolveClinicaIdAsync();
            if (!clinicaId.HasValue)
                return ResponseFactory.Fail<ModoPagamentoDTO>("Clínica atual não definida");

            var spec = new ModoPagamentoByIdClinicaSpec(id, clinicaId.Value);
            var dto = (await _repository.GetListAsync<ModoPagamentoEntity, ModoPagamentoDTO, Guid>(spec)).FirstOrDefault();
            if (dto == null)
                return ResponseFactory.Fail<ModoPagamentoDTO>("Modo de pagamento não encontrado");

            return ResponseFactory.Success(dto);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<ModoPagamentoDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> CreateAsync(CreateModoPagamentoRequest request)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        Response<Guid>? validation = await ValidarRequestAsync(request.Abreviatura, request.TemContaBancaria, request.ContaBancariaId);
        if (validation != null)
            return validation;

        var entity = _mapper.Map<ModoPagamentoEntity>(request);
        entity.ClinicaId = clinicaId;
        entity.Codigo = await ObterProximoCodigoAsync(clinicaId);
        entity.Descricao = request.Descricao.Trim();
        entity.Abreviatura = request.Abreviatura.Trim().ToUpperInvariant();
        entity.Historico = false;
        NormalizarContaBancaria(entity);

        try
        {
            var created = await _repository.CreateAsync<ModoPagamentoEntity, Guid>(entity);
            _ = await _repository.SaveChangesAsync();
            return ResponseFactory.Success(created.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateModoPagamentoRequest request, Guid id)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        Guid clinicaId = clinicaIdOpt.Value;
        var spec = new ModoPagamentoByIdClinicaSpec(id, clinicaId);
        var existing = (await _repository.GetListAsync<ModoPagamentoEntity, Guid>(spec)).FirstOrDefault();
        if (existing == null)
            return ResponseFactory.Fail<Guid>("Modo de pagamento não encontrado");

        if (existing.Historico)
            return ResponseFactory.Fail<Guid>("Registo em histórico não pode ser editado. Retire do histórico primeiro.");

        Response<Guid>? validation = await ValidarRequestAsync(request.Abreviatura, request.TemContaBancaria, request.ContaBancariaId);
        if (validation != null)
            return validation;

        _mapper.Map(request, existing);
        existing.ClinicaId = clinicaId;
        existing.Descricao = request.Descricao.Trim();
        existing.Abreviatura = request.Abreviatura.Trim().ToUpperInvariant();
        NormalizarContaBancaria(existing);

        try
        {
            _ = await _repository.UpdateAsync<ModoPagamentoEntity, Guid>(existing);
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

            var spec = new ModoPagamentoByIdClinicaSpec(id, clinicaIdOpt.Value);
            var entity = (await _repository.GetListAsync<ModoPagamentoEntity, Guid>(spec)).FirstOrDefault();
            if (entity == null)
                return ResponseFactory.Fail<Guid>("Modo de pagamento não encontrado");

            bool emUso = await _repository.ExistsAsync<CliCloud.Domain.Entities.Documentos.Documento, Guid>(new DocumentoByModoPagamentoSpec(entity.Id))
                || await _repository.ExistsAsync<CliCloud.Domain.Entities.Organismos.Organismo, Guid>(new OrganismoByModoPagamentoSpec(entity.Id))
                || await _repository.ExistsAsync<CliCloud.Domain.Entities.Fornecedores.Fornecedor, Guid>(new FornecedorByModoPagamentoSpec(entity.Id))
                || await _repository.ExistsAsync<CliCloud.Domain.Entities.Empresas.Empresa, Guid>(new EmpresaByModoPagamentoSpec(entity.Id));

            if (emUso)
                return ResponseFactory.Fail<Guid>("Modo de pagamento em uso e não pode ser eliminado.");

            await _repository.RemoveAsync<ModoPagamentoEntity, Guid>(entity);
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

    public Task<Response<Guid>> PassarHistoricoAsync(Guid id) => AlterarHistoricoAsync(id, true);

    public Task<Response<Guid>> RetirarHistoricoAsync(Guid id) => AlterarHistoricoAsync(id, false);

    private async Task<Response<Guid>> AlterarHistoricoAsync(Guid id, bool historico)
    {
        Guid? clinicaIdOpt = await ResolveClinicaIdAsync();
        if (!clinicaIdOpt.HasValue)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida");

        var spec = new ModoPagamentoByIdClinicaSpec(id, clinicaIdOpt.Value);
        var entity = (await _repository.GetListAsync<ModoPagamentoEntity, Guid>(spec)).FirstOrDefault();
        if (entity == null)
            return ResponseFactory.Fail<Guid>("Modo de pagamento não encontrado");

        entity.Historico = historico;
        _ = await _repository.UpdateAsync<ModoPagamentoEntity, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
    }

    private async Task<Response<Guid>?> ValidarRequestAsync(string abreviatura, bool temContaBancaria, Guid? contaBancariaId)
    {
        string codigo = abreviatura.Trim().ToUpperInvariant();
        if (!await _repository.ExistsAsync<TipoPagamentoEntity, Guid>(new TipoPagamentoMatchCodigo(codigo)))
            return ResponseFactory.Fail<Guid>("Abreviatura inválida: tipo de pagamento SAFT não encontrado.");

        if (temContaBancaria && contaBancariaId.HasValue)
        {
            try
            {
                _ = await _repository.GetByIdAsync<ContaBancaria, Guid>(contaBancariaId.Value);
            }
            catch
            {
                return ResponseFactory.Fail<Guid>("Conta bancária não encontrada.");
            }
        }

        return null;
    }

    private static void NormalizarContaBancaria(ModoPagamentoEntity entity)
    {
        if (!entity.TemContaBancaria)
            entity.ContaBancariaId = null;
    }

    private async Task<int> ObterProximoCodigoAsync(Guid clinicaId)
    {
        var items = await _repository.GetListAsync<ModoPagamentoEntity, Guid>(new ModoPagamentoByClinicaSpec(clinicaId));
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
