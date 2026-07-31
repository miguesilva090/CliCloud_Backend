using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Application.Services.Credenciais.LoteDirectService.Filters;
using CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Organismos;
using Microsoft.Extensions.Logging;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService
{
    public class LoteDirectService(
        IRepositoryAsync repository, 
        IMapper mapper,
        ILoteDirectCorrecaoLotesExecutor correcaoLotesExecutor,
        ILoteDirectCorrecaoLotesValidator correcaoLotesValidator,
        ILoteDirectLinhasSyncRepository loteDirectLinhasSyncRepository,
        ILoteDirectSaveValidator saveValidator,
        ILoteDirectPassarHistoricoExecutor passarHistoricoExecutor,
        ILoteDirectPassarAtivoExecutor passarAtivoExecutor,
        ILoteDirectNovoLoteResolver novoLoteResolver,
        ILoteDirectEspHistoricoGateway espHistoricoGateway,
        ICredenciaisSnsLegadoLookup legadoLookup,
        ICurrentClinicaService currentClinicaService,
        ILogger<LoteDirectService> logger
        ) : ILoteDirectService
    {
        private const int TipoLoteValorExamesSemPapel = 97;

        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILoteDirectCorrecaoLotesExecutor _correcaoLotesExecutor = correcaoLotesExecutor;
        private readonly ILoteDirectCorrecaoLotesValidator _correcaoLotesValidator = correcaoLotesValidator;
        private readonly ILoteDirectLinhasSyncRepository _loteDirectLinhasSyncRepository = loteDirectLinhasSyncRepository;
        private readonly ILoteDirectSaveValidator _saveValidator = saveValidator;
        private readonly ILoteDirectPassarHistoricoExecutor _passarHistoricoExecutor = passarHistoricoExecutor;
        private readonly ILoteDirectPassarAtivoExecutor _passarAtivoExecutor = passarAtivoExecutor;
        private readonly ILoteDirectNovoLoteResolver _novoLoteResolver = novoLoteResolver;
        private readonly ILoteDirectEspHistoricoGateway _espHistoricoGateway = espHistoricoGateway;
        private readonly ICredenciaisSnsLegadoLookup _legadoLookup = legadoLookup;
        private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;
        private readonly ILogger<LoteDirectService> _logger = logger;

        public async Task<PaginatedResponse<LoteDirectTableDTO>> GetPaginatedAsync(LoteDirectTableFilter filter)
        {
            if (filter.Filters?.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new LoteDirectSearchTable(filter.Filters ?? [], order);
            PaginatedResponse<LoteDirectTableDTO> page = await _repository.GetPaginatedResultsAsync<LoteDirect, LoteDirectTableDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                spec);
            await PreencherEnriquecimentosListagemAsync(page.Data).ConfigureAwait(false);
            return page;
        }

        public async Task<Response<LoteDirectDTO>> GetByIdAsync(Guid id)
        {
            var spec = new LoteDirectGetById();
            var dto = await _repository.GetByIdAsync<LoteDirect, LoteDirectDTO, Guid>(id, spec);
            if (dto is null)
                return ResponseFactory.Fail<LoteDirectDTO>("Registo não encontrado.");
            if (dto.CodigoOrganismo is int cod)
            {
                Dictionary<int, string?> map = await ObterSiglasOrganismoPorCodigoUlsAsync([cod]).ConfigureAwait(false);
                if (map.TryGetValue(cod, out string? s) && !string.IsNullOrWhiteSpace(s))
                    dto.OrganismoSigla = s;
            }
            List<LoteDirectLinha> linhas = (await _repository
                .GetListAsync<LoteDirectLinha, Guid>(new LoteDirectLinhasByCabecalhoSpec(id))
                .ConfigureAwait(false))
                .ToList();
            List<LoteDirectLinha789> linhas789 = (await _repository
                .GetListAsync<LoteDirectLinha789, Guid>(new LoteDirectLinhas789ByCabecalhoSpec(id))
                .ConfigureAwait(false))
                .ToList();
            dto.Linhas = _mapper.Map<List<LoteDirectLinhaDTO>>(linhas);
            dto.Linhas789 = _mapper.Map<List<LoteDirectLinhaDTO>>(linhas789);
            return ResponseFactory.Success(dto);
        }

        public async Task<Response<Guid>> CreateAsync(CreateLoteDirectRequest request)
        {
            string? erro = await _saveValidator.ValidateAsync(request).ConfigureAwait(false);
            if (erro is not null)
                return ResponseFactory.Fail<Guid>(erro);

            var entity = _mapper.Map<LoteDirect>(request);
            entity.Id = Guid.NewGuid();
            await _repository.CreateAsync<LoteDirect, Guid>(entity);
            await SincronizarLinhasAsync(entity.Id, request.Linhas, request.Linhas789);
            await _repository.SaveChangesAsync();
            await SincronizarEspMedicoSeNecessarioAsync(request).ConfigureAwait(false);
            await TentarAutoCorrigirAgregadosAsync(entity.Ano, entity.Mes).ConfigureAwait(false);
            return ResponseFactory.Success(entity.Id);
        }

        public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateLoteDirectRequest request)
        {
            string? erro = await _saveValidator.ValidateAsync(request, id).ConfigureAwait(false);
            if (erro is not null)
                return ResponseFactory.Fail<Guid>(erro);

            var entity = await _repository.GetByIdAsync<LoteDirect, Guid>(id);
            _mapper.Map(request, entity);
            await _repository.UpdateAsync<LoteDirect, Guid>(entity);
            await SincronizarLinhasAsync(entity.Id, request.Linhas, request.Linhas789);
            await _repository.SaveChangesAsync();
            await SincronizarEspMedicoSeNecessarioAsync(request).ConfigureAwait(false);
            await TentarAutoCorrigirAgregadosAsync(entity.Ano, entity.Mes).ConfigureAwait(false);
            return ResponseFactory.Success(entity.Id);
        }

        public async Task<Response<Guid>> DeleteAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<LoteDirect, Guid>(id);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(id);
        }

        public async Task<Response<CorrigirLotesResultDTO>> CorrigirLotesAsync(CorrigirLotesRequest request)
        {
            if (request.Mes is < 1 or > 12)
                return ResponseFactory.Fail<CorrigirLotesResultDTO>("Mês inválido.");

            if (request.Ano < 1900)
                return ResponseFactory.Fail<CorrigirLotesResultDTO>("Ano inválido.");

            try
            {
                CorrigirLotesResultDTO result = await _correcaoLotesExecutor.ExecutarAsync(request.Ano, request.Mes);
                return ResponseFactory.Success(result);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<CorrigirLotesResultDTO>($"Não foi possível corrigir os lotes: {ex.Message}");
            }
        }

        public async Task<Response<ValidarCorrigirLotesDTO>> ValidarCorrigirLotesAsync(CorrigirLotesRequest request)
        {
            if (request.Mes is < 1 or > 12)
                return ResponseFactory.Fail<ValidarCorrigirLotesDTO>("Mês inválido.");

            if (request.Ano < 1900)
                return ResponseFactory.Fail<ValidarCorrigirLotesDTO>("Ano inválido.");

            ValidarCorrigirLotesDTO result = await _correcaoLotesValidator.ValidarAsync(request.Ano, request.Mes);
            return ResponseFactory.Success(result);
        }

        public async Task<PaginatedResponse<LoteDirectAgregadoTableDTO>> GetAgregadosPaginatedAsync(LoteDirectAgregadoTableFilter filter)
        {
            if (filter.Filters?.Count > 0) filter.PageNumber = 1;
            string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            LoteDirectAgregadoSearchTable spec = new(filter.Filters ?? [], order);
            PaginatedResponse<LoteDirectAgregadoTableDTO> page = await _repository.GetPaginatedResultsAsync<LoteDirectAgregado, LoteDirectAgregadoTableDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                spec);
            await PreencherAgregadosEnriquecimentosAsync(page.Data).ConfigureAwait(false);
            return page;
        }

        public async Task<Response<IEnumerable<TipoLoteLightDTO>>> GetTiposLoteLightAsync()
        {
            var spec = new TipoLoteSearchList();
            var list = await _repository.GetListAsync<TipoLote, TipoLoteLightDTO, int>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<PassarParaHistoricoResultDTO>> PassarParaHistoricoAsync(
            PassarParaHistoricoRequest request)
        {
            try
            {
                LoteDirect origem = await _repository.GetByIdAsync<LoteDirect, Guid>(request.LoteDirectId);

                if (origem.Historico)
                    return ResponseFactory.Fail<PassarParaHistoricoResultDTO>("O registo já está em histórico.");

                if (origem is not { CodigoOrganismo: int org, Mes: int mes, Ano: int ano })
                    return ResponseFactory.Fail<PassarParaHistoricoResultDTO>("Organismo, mês ou ano em falta.");

                PassarParaHistoricoResultDTO result = await _passarHistoricoExecutor
                    .ExecutarAsync(org, mes, ano)
                    .ConfigureAwait(false);

                return ResponseFactory.Success(result);
            }
            catch (InvalidOperationException)
            {
                return ResponseFactory.Fail<PassarParaHistoricoResultDTO>("Registo não encontrado.");
            }
        }

        public async Task<Response<PassarParaAtivoResultDTO>> PassarParaAtivoAsync(
            PassarParaAtivoRequest request)
        {
            if (request.NovoMes is < 1 or > 12)
                return ResponseFactory.Fail<PassarParaAtivoResultDTO>("Mês inválido.");

            if (request.NovoAno < 1900)
                return ResponseFactory.Fail<PassarParaAtivoResultDTO>("Ano inválido.");

            try
            {
                LoteDirect origem = await _repository.GetByIdAsync<LoteDirect, Guid>(request.LoteDirectId);

                if (!origem.Historico)
                    return ResponseFactory.Fail<PassarParaAtivoResultDTO>("O registo já está ativo.");

                if (origem.CodigoOrganismo is not int org)
                    return ResponseFactory.Fail<PassarParaAtivoResultDTO>("Organismo em falta.");

                bool destinoEmHistorico = await _repository
                    .ExistsAsync<LoteDirect, Guid>(
                        new LoteDirectOrganismoMesAnoHistoricoSpec(org, request.NovoMes, request.NovoAno))
                    .ConfigureAwait(false);

                if (destinoEmHistorico)
                    return ResponseFactory.Fail<PassarParaAtivoResultDTO>("O mês/ano destino deste organismo já está em histórico.");

                PassarParaAtivoResultDTO result = await _passarAtivoExecutor
                    .ExecutarAsync(request.LoteDirectId, request.NovoMes, request.NovoAno)
                    .ConfigureAwait(false);

                return ResponseFactory.Success(result);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseFactory.Fail<PassarParaAtivoResultDTO>(ex.Message);
            }
        }

        private async Task SincronizarLinhasAsync(
            Guid loteDirectId,
            IEnumerable<LoteDirectLinhaUpsertRequest>? linhas,
            IEnumerable<LoteDirectLinhaUpsertRequest>? linhas789)
        {
            await _loteDirectLinhasSyncRepository.SincronizarAsync(loteDirectId, linhas, linhas789);
        }

        private async Task TentarAutoCorrigirAgregadosAsync(int? ano, int? mes)
        {
            if (!ano.HasValue || !mes.HasValue)
                return;
            if (ano.Value < 1900 || mes.Value is < 1 or > 12)
                return;

            try
            {
                await _correcaoLotesExecutor.ExecutarAsync(ano.Value, mes.Value).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Falha ao auto-corrigir agregados de lotes para {Ano}/{Mes}.",
                    ano.Value,
                    mes.Value);
            }
        }

        private async Task SincronizarEspMedicoSeNecessarioAsync(CreateLoteDirectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Credencial) || string.IsNullOrWhiteSpace(request.CodigoMedico))
                return;

            if (!await IsTipoExamesSemPapelAsync(request.TipoLote).ConfigureAwait(false))
                return;

            LoteDirectEspRequisicaoData? esp = await _espHistoricoGateway
                .ObterPorCredencialAsync(request.Credencial.Trim())
                .ConfigureAwait(false);

            if (esp is null
                || string.Equals(esp.CodigoMedico, request.CodigoMedico.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            await _espHistoricoGateway
                .UpdateMedicoAsync(request.Credencial.Trim(), request.CodigoMedico.Trim())
                .ConfigureAwait(false);
        }

        private async Task<bool> IsTipoExamesSemPapelAsync(int? tipoLoteId)
        {
            if (tipoLoteId is null or <= 0)
                return false;

            List<TipoLote> tipos = (await _repository
                .GetListAsync<TipoLote, int>(new TipoLoteByIdsSpec([tipoLoteId.Value]))
                .ConfigureAwait(false))
                .ToList();

            return tipos.FirstOrDefault()?.Valor == TipoLoteValorExamesSemPapel;
        }

        /// <summary>Código no lote = <see cref="Organismo.CodigoULSNova"/>; exibir abreviatura e nome na listagem.</summary>
        private async Task PreencherEnriquecimentosListagemAsync(List<LoteDirectTableDTO> linhas)
        {
            if (linhas is not { Count: > 0 })
                return;

            int[] codigos = linhas
                .Select(x => x.CodigoOrganismo)
                .Where(c => c.HasValue)
                .Select(c => c!.Value)
                .Distinct()
                .ToArray();

            Dictionary<int, OrganismoListagemEnriquecimento> porCodigoUls =
                await ObterOrganismoEnriquecimentosPorCodigoUlsAsync(codigos).ConfigureAwait(false);

            int[] tipoLoteIds = linhas
                .Where(x => x.TipoLote.HasValue)
                .Select(x => x.TipoLote!.Value)
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
                .Where(x => x.TipoServico.HasValue)
                .Select(x => x.TipoServico!.Value)
                .Distinct()
                .ToArray();

            int? filtroLegado = await ResolverFiltroLegadoAsync().ConfigureAwait(false);
            IReadOnlyDictionary<int, string> nomesTipoServicoLegado = await _legadoLookup
                .ObterNomesTipoServicoAsync(tipoServicoCodigos, filtroLegado)
                .ConfigureAwait(false);

            foreach (LoteDirectTableDTO row in linhas)
            {
                if (row.CodigoOrganismo is null)
                    continue;

                if (porCodigoUls.TryGetValue(row.CodigoOrganismo.Value, out OrganismoListagemEnriquecimento? org))
                {
                    if (!string.IsNullOrWhiteSpace(org.Sigla))
                        row.OrganismoSigla = org.Sigla;
                    if (!string.IsNullOrWhiteSpace(org.Nome))
                        row.OrganismoNome = org.Nome;
                }

                if (row.TipoLote is int tipoLoteId
                    && tipoLoteDesignacoes.TryGetValue(tipoLoteId, out string? designaLote)
                    && !string.IsNullOrWhiteSpace(designaLote))
                    row.TipoLoteDesignacao = designaLote;

                if (row.TipoServico is int tipoServico
                    && nomesTipoServicoLegado.TryGetValue(tipoServico, out string? nomeTipoServico)
                    && !string.IsNullOrWhiteSpace(nomeTipoServico))
                    row.TipoServicoDesignacao = nomeTipoServico;
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

        private async Task<Dictionary<int, OrganismoListagemEnriquecimento>> ObterOrganismoEnriquecimentosPorCodigoUlsAsync(
            IReadOnlyCollection<int> codigosUls)
        {
            if (codigosUls is not { Count: > 0 })
                return [];

            var orgSpec = new OrganismosByCodigoULSNovaSpec(codigosUls);
            List<Organismo> organismos = (await _repository.GetListAsync<Organismo, Guid>(orgSpec).ConfigureAwait(false)).ToList();
            Dictionary<int, OrganismoListagemEnriquecimento> porCodigoUls = [];
            foreach (Organismo o in organismos.Where(x => x.CodigoULSNova.HasValue))
            {
                int c = o.CodigoULSNova!.Value;
                if (porCodigoUls.ContainsKey(c))
                    continue;

                string? abv = string.IsNullOrWhiteSpace(o.Abreviatura) ? null : o.Abreviatura.Trim();
                string? nome = string.IsNullOrWhiteSpace(o.Nome) ? null : o.Nome.Trim();
                porCodigoUls[c] = new OrganismoListagemEnriquecimento
                {
                    Sigla = abv,
                    Nome = nome,
                };
            }

            return porCodigoUls;
        }

        private sealed class OrganismoListagemEnriquecimento
        {
            public string? Sigla { get; init; }
            public string? Nome { get; init; }
        }

        private async Task<Dictionary<int, string?>> ObterSiglasOrganismoPorCodigoUlsAsync(IReadOnlyCollection<int> codigosUls)
        {
            Dictionary<int, OrganismoListagemEnriquecimento> enriquecimentos =
                await ObterOrganismoEnriquecimentosPorCodigoUlsAsync(codigosUls).ConfigureAwait(false);

            return enriquecimentos.ToDictionary(x => x.Key, x => x.Value.Sigla);
        }

        private async Task PreencherAgregadosEnriquecimentosAsync(List<LoteDirectAgregadoTableDTO> linhas)
        {
            if (linhas is not { Count: > 0 })
                return;

            int[] codigos = linhas
                .Select(x => x.CodigoOrganismo)
                .Distinct()
                .ToArray();

            Dictionary<int, string?> porCodigoUls = await ObterSiglasOrganismoPorCodigoUlsAsync(codigos).ConfigureAwait(false);

            int[] tipoLoteIds = linhas
                .Select(x => x.TipoLote)
                .Distinct()
                .ToArray();

            Dictionary<int, string?> tipoLoteDesignacoes = [];
            HashSet<int> tipoLoteIdSet = tipoLoteIds.ToHashSet();
            if (tipoLoteIdSet.Count > 0)
            {
                // Tabela pequena; filtro em memória evita OPENJSON no SQL Server 2014.
                List<TipoLote> tipos = (await _repository
                    .GetListAsync<TipoLote, int>(new TipoLoteSearchList())
                    .ConfigureAwait(false))
                    .Where(x => tipoLoteIdSet.Contains(x.Id))
                    .ToList();
                foreach (TipoLote tipo in tipos)
                    tipoLoteDesignacoes[tipo.Id] = tipo.Designa;
            }

            foreach (LoteDirectAgregadoTableDTO row in linhas)
            {
                if (porCodigoUls.TryGetValue(row.CodigoOrganismo, out string? sigla) && !string.IsNullOrWhiteSpace(sigla))
                    row.OrganismoSigla = sigla;

                if (tipoLoteDesignacoes.TryGetValue(row.TipoLote, out string? designa) && !string.IsNullOrWhiteSpace(designa))
                    row.TipoLoteDesignacao = designa;
            }
        }

        public async Task<Response<ObterNovoLoteResultDTO>> ObterNovoLoteAsync(ObterNovoLoteRequest request)
        {
            if (request.CodigoOrganismo <= 0)
                return ResponseFactory.Fail<ObterNovoLoteResultDTO>("Codigo de organismo inválido");

            if (request.Mes is < 1 or > 12)
                return ResponseFactory.Fail<ObterNovoLoteResultDTO>("Mês inválido");

            if (request.Ano < 1900)
                return ResponseFactory.Fail<ObterNovoLoteResultDTO>("Ano inválido");

            if (request.TipoLote <= 0)
                return ResponseFactory.Fail<ObterNovoLoteResultDTO>("Tipo de lote inválido");

            if (request.TipoServico <= 0)
                return ResponseFactory.Fail<ObterNovoLoteResultDTO>("Tipo de serviço inválido");

            (int novoIndice, int novoLote) = await _novoLoteResolver
                .ResolverAsync(
                    request.CodigoOrganismo,
                    request.TipoLote,
                    request.TipoServico,
                    request.Mes,
                    request.Ano)
                .ConfigureAwait(false);

            return ResponseFactory.Success(new ObterNovoLoteResultDTO
            {
                NovoIndice = novoIndice,
                NovoLote = novoLote,
            });
        }
    }
}