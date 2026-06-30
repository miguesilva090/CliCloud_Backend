using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Application.Services.Credenciais.LoteDirectService.Filters;
using CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService
{
    public class LoteDirectService(
        IRepositoryAsync repository, 
        IMapper mapper,
        ILoteDirectCorrecaoLotesExecutor correcaoLotesExecutor,
        ILoteDirectCorrecaoLotesValidator correcaoLotesValidator,
        ILoteDirectLinhasSyncRepository loteDirectLinhasSyncRepository,
        ILoteDirectSaveValidator saveValidator,
        ILoteDirectPassarHistoricoExecutor passarHistoricoExecutor
        ) : ILoteDirectService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILoteDirectCorrecaoLotesExecutor _correcaoLotesExecutor = correcaoLotesExecutor;
        private readonly ILoteDirectCorrecaoLotesValidator _correcaoLotesValidator = correcaoLotesValidator;
        private readonly ILoteDirectLinhasSyncRepository _loteDirectLinhasSyncRepository = loteDirectLinhasSyncRepository;
        private readonly ILoteDirectSaveValidator _saveValidator = saveValidator;
        private readonly ILoteDirectPassarHistoricoExecutor _passarHistoricoExecutor = passarHistoricoExecutor;

        public async Task<PaginatedResponse<LoteDirectTableDTO>> GetPaginatedAsync(LoteDirectTableFilter filter)
        {
            if (filter.Filters?.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new LoteDirectSearchTable(filter.Filters ?? [], order);
            PaginatedResponse<LoteDirectTableDTO> page = await _repository.GetPaginatedResultsAsync<LoteDirect, LoteDirectTableDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                spec);
            await PreencherSiglasOrganismoAsync(page.Data).ConfigureAwait(false);
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

        private async Task SincronizarLinhasAsync(
            Guid loteDirectId,
            IEnumerable<LoteDirectLinhaUpsertRequest>? linhas,
            IEnumerable<LoteDirectLinhaUpsertRequest>? linhas789)
        {
            await _loteDirectLinhasSyncRepository.SincronizarAsync(loteDirectId, linhas, linhas789);
        }

        /// <summary>Código no lote = <see cref="Organismo.CodigoULSNova"/>; exibir abreviatura na listagem.</summary>
        private async Task PreencherSiglasOrganismoAsync(List<LoteDirectTableDTO> linhas)
        {
            if (linhas is not { Count: > 0 })
                return;

            int[] codigos = linhas
                .Select(x => x.CodigoOrganismo)
                .Where(c => c.HasValue)
                .Select(c => c!.Value)
                .Distinct()
                .ToArray();
            if (codigos.Length == 0)
                return;

            Dictionary<int, string?> porCodigoUls = await ObterSiglasOrganismoPorCodigoUlsAsync(codigos).ConfigureAwait(false);

            foreach (LoteDirectTableDTO row in linhas)
            {
                if (row.CodigoOrganismo is null)
                    continue;
                if (porCodigoUls.TryGetValue(row.CodigoOrganismo.Value, out string? s) && !string.IsNullOrWhiteSpace(s))
                    row.OrganismoSigla = s;
            }
        }

        private async Task<Dictionary<int, string?>> ObterSiglasOrganismoPorCodigoUlsAsync(IReadOnlyCollection<int> codigosUls)
        {
            if (codigosUls is not { Count: > 0 })
                return [];

            var orgSpec = new OrganismosByCodigoULSNovaSpec(codigosUls);
            List<Organismo> organismos = (await _repository.GetListAsync<Organismo, Guid>(orgSpec).ConfigureAwait(false)).ToList();
            Dictionary<int, string?> porCodigoUls = [];
            foreach (Organismo o in organismos.Where(x => x.CodigoULSNova.HasValue))
            {
                int c = o.CodigoULSNova!.Value;
                if (!porCodigoUls.TryGetValue(c, out string? sigla) || string.IsNullOrWhiteSpace(sigla))
                {
                    string? abv = string.IsNullOrWhiteSpace(o.Abreviatura) ? null : o.Abreviatura.Trim();
                    porCodigoUls[c] = abv;
                }
            }

            return porCodigoUls;
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
    }
}