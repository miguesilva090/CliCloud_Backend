using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Filters;
using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService;

public class ListaEsperaTratamentoAdministrativoService(
    IRepositoryAsync repository,
    IMapper mapper,
    IUtilizadorDisplayNameResolver utilizadorDisplayNameResolver
) : IListaEsperaTratamentoAdministrativoService
{
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IUtilizadorDisplayNameResolver _utilizadorDisplayNameResolver =
        utilizadorDisplayNameResolver;

    public async Task<PaginatedResponse<ListaEsperaTratamentoTableDTO>> GetPaginatedAsync(
        ListaEsperaTratamentoTableFilter filter
    )
    {
        if (filter.Filters?.Count > 0)
        {
            filter.PageNumber = 1;
        }

        string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
        var spec = new ListaEsperaTratamentoSearchTable(filter, order);

        return await _repository.GetPaginatedResultsAsync<
            ListaEsperaTratamento,
            ListaEsperaTratamentoTableDTO,
            Guid
        >(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<ListaEsperaTratamentoDTO>> GetByIdAsync(Guid id)
    {
        ListaEsperaTratamento? entity = (
            await _repository.GetListAsync<ListaEsperaTratamento, Guid>(
                new ListaEsperaTratamentoByIdSpec(id)
            )
        ).FirstOrDefault();

        if (entity == null)
        {
            return ResponseFactory.Fail<ListaEsperaTratamentoDTO>(
                "Registo de lista de espera não encontrado."
            );
        }

        return ResponseFactory.Success(MapEntityToDto(entity));
    }

    public async Task<Response<ListaEsperaTratamentoProximoIdentificadorDTO>> GetProximoIdentificadorAsync()
    {
        return ResponseFactory.Success(
            new ListaEsperaTratamentoProximoIdentificadorDTO
            {
                CodigoListaEspera = await ResolveNextCodigoLegadoAsync(),
                ProximaOrdem = await ResolveNextOrdemAsync(),
            }
        );
    }

    public async Task<Response<bool>> VerificarOrdemDisponivelAsync(int ordem, Guid? excludeId = null)
    {
        if (ordem <= 0)
        {
            return ResponseFactory.Fail<bool>("N.º ordem inválido.");
        }

        bool emUso = await IsOrdemEmUsoAsync(ordem, excludeId);
        return ResponseFactory.Success(!emUso);
    }

    public async Task<Response<Guid>> CreateAsync(CreateListaEsperaTratamentoRequest request)
    {
        int ordem = request.Ordem ?? await ResolveNextOrdemAsync();
        if (await IsOrdemEmUsoAsync(ordem, null))
        {
            return ResponseFactory.Fail<Guid>("N.º ordem indisponível.");
        }

        var entity = _mapper.Map<ListaEsperaTratamento>(request);
        entity.Id = Guid.NewGuid();
        entity.CodigoLegado = await ResolveNextCodigoLegadoAsync();
        entity.Ordem = ordem;
        entity.OrdemOrigem = ordem;
        entity.DataEntrada = DateTime.UtcNow.Date;
        entity.Historico = false;

        if (!string.IsNullOrWhiteSpace(request.Obs))
        {
            string nomeAutor = await _utilizadorDisplayNameResolver.ResolveAsync();
            entity.Obs = AdmissaoObservacoesHelper.FormatarObservacaoAppend(
                request.Obs,
                nomeAutor,
                null
            );
        }

        ListaEsperaTratamento created =
            await _repository.CreateAsync<ListaEsperaTratamento, Guid>(entity);
        await ReplaceServicosAsync(created.Id, request.Servicos);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
    }

    public async Task<Response<Guid>> UpdateAsync(
        Guid id,
        UpdateListaEsperaTratamentoRequest request
    )
    {
        ListaEsperaTratamento? entity =
            await _repository.GetByIdAsync<ListaEsperaTratamento, Guid>(id);

        if (entity == null || entity.DeletedOn != null)
        {
            return ResponseFactory.Fail<Guid>("Registo de lista de espera não encontrado.");
        }

        string? obs = entity.Obs;
        int? ordemOrigem = entity.OrdemOrigem;
        _mapper.Map(request, entity);
        entity.Obs = obs;

        if (request.Ordem.HasValue && request.Ordem.Value != entity.Ordem)
        {
            if (await IsOrdemEmUsoAsync(request.Ordem.Value, id))
            {
                return ResponseFactory.Fail<Guid>("N.º ordem indisponível.");
            }

            entity.Ordem = request.Ordem.Value;
        }

        entity.OrdemOrigem = ordemOrigem;

        _ = await _repository.UpdateAsync<ListaEsperaTratamento, Guid>(entity);
        if (request.Servicos != null)
        {
            await ReplaceServicosAsync(id, request.Servicos);
        }

        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
    }

    public async Task<Response<Guid>> DeleteAsync(Guid id)
    {
        ListaEsperaTratamento? entity =
            await _repository.GetByIdAsync<ListaEsperaTratamento, Guid>(id);

        if (entity == null || entity.DeletedOn != null)
        {
            return ResponseFactory.Fail<Guid>("Registo de lista de espera não encontrado.");
        }

        await _repository.RemoveByIdAsync<ListaEsperaTratamento, Guid>(id);
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
            await _repository.RemoveByIdAsync<ListaEsperaTratamento, Guid>(id);
        }

        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success<IEnumerable<Guid>>(list);
    }

    public async Task<Response<ListaEsperaTratamentoObservacoesDTO>> GetObservacoesAsync(Guid id)
    {
        ListaEsperaTratamento? entity =
            await _repository.GetByIdAsync<ListaEsperaTratamento, Guid>(id);

        if (entity == null || entity.DeletedOn != null)
        {
            return ResponseFactory.Fail<ListaEsperaTratamentoObservacoesDTO>(
                "Registo de lista de espera não encontrado."
            );
        }

        return ResponseFactory.Success(
            new ListaEsperaTratamentoObservacoesDTO { Observacoes = entity.Obs ?? string.Empty }
        );
    }

    public async Task<Response<Guid>> AppendObservacaoAsync(
        Guid id,
        AppendListaEsperaTratamentoObservacaoRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Texto))
        {
            return ResponseFactory.Fail<Guid>("Indique o texto da observação.");
        }

        ListaEsperaTratamento? entity =
            await _repository.GetByIdAsync<ListaEsperaTratamento, Guid>(id);

        if (entity == null || entity.DeletedOn != null)
        {
            return ResponseFactory.Fail<Guid>("Registo de lista de espera não encontrado.");
        }

        string nomeAutor = await _utilizadorDisplayNameResolver.ResolveAsync();
        entity.Obs = AdmissaoObservacoesHelper.FormatarObservacaoAppend(
            request.Texto,
            nomeAutor,
            entity.Obs
        );

        _ = await _repository.UpdateAsync<ListaEsperaTratamento, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
    }

    private async Task<int> ResolveNextOrdemAsync()
    {
        ListaEsperaTratamento? last = (
            await _repository.GetListAsync<ListaEsperaTratamento, Guid>(
                new ListaEsperaTratamentoMaxOrdemSpec()
            )
        ).FirstOrDefault();

        return (last?.Ordem ?? 0) + 1;
    }

    private async Task<int> ResolveNextCodigoLegadoAsync()
    {
        ListaEsperaTratamento? last = (
            await _repository.GetListAsync<ListaEsperaTratamento, Guid>(
                new ListaEsperaTratamentoMaxCodigoLegadoSpec()
            )
        ).FirstOrDefault();

        return (last?.CodigoLegado ?? 0) + 1;
    }

    private async Task<bool> IsOrdemEmUsoAsync(int ordem, Guid? excludeId)
    {
        List<ListaEsperaTratamento> emUso = (
            await _repository.GetListAsync<ListaEsperaTratamento, Guid>(
                new ListaEsperaTratamentoOrdemEmUsoSpec(ordem, excludeId)
            )
        ).ToList();

        return emUso.Count > 0;
    }

    private ListaEsperaTratamentoDTO MapEntityToDto(ListaEsperaTratamento entity)
    {
        ListaEsperaTratamentoDTO dto = _mapper.Map<ListaEsperaTratamentoDTO>(entity);
        dto.Servicos = entity
            .Servicos.Where(s => s.DeletedOn == null)
            .OrderBy(s => s.Ordem)
            .Select(s => _mapper.Map<ListaEsperaTratamentoServicoDTO>(s))
            .ToList();
        return dto;
    }

    private async Task ReplaceServicosAsync(
        Guid listaEsperaId,
        IReadOnlyList<ListaEsperaTratamentoServicoRequest>? servicos
    )
    {
        if (servicos == null)
        {
            return;
        }

        ListaEsperaTratamento? entity = (
            await _repository.GetListAsync<ListaEsperaTratamento, Guid>(
                new ListaEsperaTratamentoByIdSpec(listaEsperaId)
            )
        ).FirstOrDefault();

        if (entity == null)
        {
            return;
        }

        foreach (ListaEsperaTratamentoServico existing in entity.Servicos.ToList())
        {
            await _repository.RemoveByIdAsync<ListaEsperaTratamentoServico, Guid>(existing.Id);
        }

        foreach (ListaEsperaTratamentoServicoRequest item in servicos.OrderBy(s => s.Ordem))
        {
            var linha = new ListaEsperaTratamentoServico
            {
                Id = Guid.NewGuid(),
                ListaEsperaTratamentoId = listaEsperaId,
                ServicoId = item.ServicoId,
                SubsistemaServicoId = item.SubsistemaServicoId,
                CodigoServico = item.CodigoServico,
                Designacao = item.Designacao,
                SubsistemaDesignacao = item.SubsistemaDesignacao,
                Duracao = item.Duracao,
                Ordem = item.Ordem,
            };

            _ = await _repository.CreateAsync<ListaEsperaTratamentoServico, Guid>(linha);
        }
    }
}
