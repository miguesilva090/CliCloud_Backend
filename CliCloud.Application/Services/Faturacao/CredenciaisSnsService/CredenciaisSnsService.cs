using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService.DTOs;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService.Filters;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService;

public sealed class CredenciaisSnsService(
    IRepositoryAsync repository,
    ICredenciaisSnsAgregadoDeleteExecutor deleteExecutor,
    ICredenciaisSnsFisioterapiaGateway fisioterapiaGateway,
    ICredenciaisSnsFisioterapiaDeleteExecutor fisioterapiaDeleteExecutor,
    ICredenciaisSnsLegadoLookup legadoLookup,
    ICurrentClinicaService currentClinicaService
) : ICredenciaisSnsService
{
    private static readonly string[] MesesNomes =
    [
        "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
        "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro",
    ];

    private readonly IRepositoryAsync _repository = repository;
    private readonly ICredenciaisSnsAgregadoDeleteExecutor _deleteExecutor = deleteExecutor;
    private readonly ICredenciaisSnsFisioterapiaGateway _fisioterapiaGateway = fisioterapiaGateway;
    private readonly ICredenciaisSnsFisioterapiaDeleteExecutor _fisioterapiaDeleteExecutor = fisioterapiaDeleteExecutor;
    private readonly ICredenciaisSnsLegadoLookup _legadoLookup = legadoLookup;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    public async Task<PaginatedResponse<CredenciaisSnsLoteTableDTO>> GetPaginatedAsync(CredenciaisSnsTableFilter filter)
    {
        if (!CredenciaisSnsModulo.TryParse(filter.Modulo, out string modulo))
        {
            return new PaginatedResponse<CredenciaisSnsLoteTableDTO>(
                [],
                0,
                filter.PageNumber,
                filter.PageSize);
        }

        if (modulo == CredenciaisSnsModulo.Fisioterapia)
        {
            if (filter.Filters?.Count > 0)
                filter.PageNumber = 1;

            string orderFisio = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            int? filtroLegado = await ResolverFiltroLegadoAsync().ConfigureAwait(false);
            PaginatedResponse<CredenciaisSnsLoteTableDTO> pageFisio = await _fisioterapiaGateway
                .GetPaginatedAsync(
                    filter.Filters ?? [],
                    filter.PageNumber,
                    filter.PageSize,
                    orderFisio,
                    filtroLegado)
                .ConfigureAwait(false);

            await PreencherEnriquecimentosAsync(pageFisio.Data).ConfigureAwait(false);
            return pageFisio;
        }

        if (modulo != CredenciaisSnsModulo.Especialidades)
        {
            return new PaginatedResponse<CredenciaisSnsLoteTableDTO>(
                [],
                0,
                filter.PageNumber,
                filter.PageSize);
        }

        if (filter.Filters?.Count > 0)
            filter.PageNumber = 1;

        string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
        CredenciaisSnsAgregadoSearchSpec spec = new(modulo, filter.Filters ?? [], order);
        PaginatedResponse<CredenciaisSnsLoteTableDTO> page = await _repository
            .GetPaginatedResultsAsync<LoteDirectAgregado, CredenciaisSnsLoteTableDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                spec)
            .ConfigureAwait(false);

        await PreencherEnriquecimentosAsync(page.Data).ConfigureAwait(false);
        return page;
    }

    public async Task<Response<bool>> DeleteAsync(
        string modulo,
        DeleteCredenciaisSnsRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!CredenciaisSnsModulo.TryParse(modulo, out string moduloNormalizado))
            return ResponseFactory.Fail<bool>("Módulo inválido.");

        if (moduloNormalizado == CredenciaisSnsModulo.Fisioterapia)
        {
            try
            {
                await _fisioterapiaDeleteExecutor
                    .ExecutarAsync(request.Indices, cancellationToken)
                    .ConfigureAwait(false);
                return ResponseFactory.Success(true);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<bool>($"Não foi possível eliminar o(s) lote(s): {ex.Message}");
            }
        }

        if (moduloNormalizado != CredenciaisSnsModulo.Especialidades)
            return ResponseFactory.Fail<bool>("Módulo ainda não disponível.");

        if (request.Indices is not { Count: > 0 })
            return ResponseFactory.Fail<bool>("Nenhum lote selecionado.");

        try
        {
            await _deleteExecutor.ExecutarAsync(request.Indices, cancellationToken).ConfigureAwait(false);
            return ResponseFactory.Success(true);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<bool>($"Não foi possível eliminar o(s) lote(s): {ex.Message}");
        }
    }

    private async Task PreencherEnriquecimentosAsync(List<CredenciaisSnsLoteTableDTO> linhas)
    {
        if (linhas is not { Count: > 0 })
            return;

        int[] codigosOrganismo = linhas
            .Select(x => x.CodigoOrganismo)
            .Distinct()
            .ToArray();

        Dictionary<int, OrganismoEnriquecimento> porCodigoUls =
            await ObterEnriquecimentosOrganismoPorCodigoUlsAsync(codigosOrganismo).ConfigureAwait(false);

        int[] tipoLoteIds = linhas
            .Select(x => x.TipoLote)
            .Distinct()
            .ToArray();

        Dictionary<int, string?> tipoLoteDesignacoes = [];
        HashSet<int> tipoLoteIdSet = tipoLoteIds.ToHashSet();
        if (tipoLoteIdSet.Count > 0)
        {
            List<TipoLote> tipos = (await _repository
                .GetListAsync<TipoLote, int>(new TipoLoteSearchList())
                .ConfigureAwait(false))
                .Where(x => tipoLoteIdSet.Contains(x.Id))
                .ToList();

            foreach (TipoLote tipo in tipos)
                tipoLoteDesignacoes[tipo.Id] = tipo.Designa;
        }

        int[] tipoServicoCodigos = linhas
            .Select(x => x.TipoServico)
            .Distinct()
            .ToArray();

        int[] indicesLote = linhas
            .Select(x => x.Indice)
            .Distinct()
            .ToArray();

        int? filtroLegado = await ResolverFiltroLegadoAsync().ConfigureAwait(false);
        IReadOnlyDictionary<int, string> nomesTipoServicoLegado = await _legadoLookup
            .ObterNomesTipoServicoAsync(tipoServicoCodigos, filtroLegado)
            .ConfigureAwait(false);

        Dictionary<int, string> nomesTipoServicoPorIndice =
            await ObterTipoServicoDesignacaoPorIndiceLoteAsync(indicesLote).ConfigureAwait(false);

        foreach (CredenciaisSnsLoteTableDTO row in linhas)
        {
            if (row.Mes is >= 1 and <= 12)
                row.MesNome = MesesNomes[row.Mes - 1];

            if (porCodigoUls.TryGetValue(row.CodigoOrganismo, out OrganismoEnriquecimento? org))
            {
                if (!string.IsNullOrWhiteSpace(org.Sigla))
                    row.OrganismoSigla = org.Sigla;

                if (!string.IsNullOrWhiteSpace(org.Nome))
                    row.OrganismoNome = org.Nome;
            }

            if (tipoLoteDesignacoes.TryGetValue(row.TipoLote, out string? designaLote)
                && !string.IsNullOrWhiteSpace(designaLote))
                row.TipoLoteDesignacao = designaLote;

            if (nomesTipoServicoLegado.TryGetValue(row.TipoServico, out string? nomeTipoServicoLegado)
                && !string.IsNullOrWhiteSpace(nomeTipoServicoLegado))
                row.TipoServicoDesignacao = nomeTipoServicoLegado;
            else if (nomesTipoServicoPorIndice.TryGetValue(row.Indice, out string? nomePorIndice)
                && !string.IsNullOrWhiteSpace(nomePorIndice))
                row.TipoServicoDesignacao = nomePorIndice;
            else if (string.IsNullOrWhiteSpace(row.TipoServicoDesignacao))
                row.TipoServicoDesignacao = row.TipoServico.ToString();
        }
    }

    private async Task<int?> ResolverFiltroLegadoAsync()
    {
        if (!Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId) || clinicaId == Guid.Empty)
            return null;

        Clinica? clinica = await _repository
            .GetByIdAsync<Clinica, Guid>(clinicaId)
            .ConfigureAwait(false);

        return clinica?.Cid;
    }

    private async Task<Dictionary<int, OrganismoEnriquecimento>> ObterEnriquecimentosOrganismoPorCodigoUlsAsync(
        int[] codigosUls)
    {
        if (codigosUls is not { Length: > 0 })
            return [];

        OrganismosByCodigoULSNovaSpec orgSpec = new(codigosUls);
        List<Organismo> organismos = (await _repository
            .GetListAsync<Organismo, Guid>(orgSpec)
            .ConfigureAwait(false))
            .ToList();

        Dictionary<int, OrganismoEnriquecimento> porCodigoUls = [];

        foreach (Organismo o in organismos.Where(x => x.CodigoULSNova.HasValue))
        {
            int c = o.CodigoULSNova!.Value;
            if (porCodigoUls.ContainsKey(c))
                continue;

            string? abv = string.IsNullOrWhiteSpace(o.Abreviatura) ? null : o.Abreviatura.Trim();
            string? nome = string.IsNullOrWhiteSpace(o.Nome) ? null : o.Nome.Trim();

            porCodigoUls[c] = new OrganismoEnriquecimento
            {
                Sigla = abv,
                Nome = nome,
            };
        }

        return porCodigoUls;
    }

    private async Task<Dictionary<int, string>> ObterTipoServicoDesignacaoPorIndiceLoteAsync(int[] indices)
    {
        if (indices is not { Length: > 0 })
            return [];

        LoteDirectByIndiceLoteInSpec spec = new(indices);
        List<LoteDirect> lotes = (await _repository
            .GetListAsync<LoteDirect, Guid>(spec)
            .ConfigureAwait(false))
            .ToList();

        Dictionary<int, string> porIndice = [];

        foreach (IGrouping<int, LoteDirect> grupo in lotes
                     .Where(x => x.IndiceLote.HasValue)
                     .GroupBy(x => x.IndiceLote!.Value))
        {
            string? descricao = grupo
                .Select(x => x.TipoServicoRegisto?.Descricao)
                .FirstOrDefault(d => !string.IsNullOrWhiteSpace(d));

            if (!string.IsNullOrWhiteSpace(descricao))
                porIndice[grupo.Key] = descricao.Trim();
        }

        return porIndice;
    }

    private sealed class OrganismoEnriquecimento
    {
        public string? Sigla { get; init; }
        public string? Nome { get; init; }
    }
}
