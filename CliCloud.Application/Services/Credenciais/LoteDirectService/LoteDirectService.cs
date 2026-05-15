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
        ILoteDirectLinhasSyncRepository loteDirectLinhasSyncRepository
        ) : ILoteDirectService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILoteDirectCorrecaoLotesExecutor _correcaoLotesExecutor = correcaoLotesExecutor;
        private readonly ILoteDirectLinhasSyncRepository _loteDirectLinhasSyncRepository = loteDirectLinhasSyncRepository;

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
            if (dto.CodigoOrganismo is int cod)
            {
                Dictionary<int, string?> map = await ObterSiglasOrganismoPorCodigoUlsAsync([cod]).ConfigureAwait(false);
                if (map.TryGetValue(cod, out string? s) && !string.IsNullOrWhiteSpace(s))
                    dto.OrganismoSigla = s;
            }

            return ResponseFactory.Success(dto);
        }

        public async Task<Response<Guid>> CreateAsync(CreateLoteDirectRequest request)
        {
            if (request.UtenteId is null || request.UtenteId == Guid.Empty)
                return ResponseFactory.Fail<Guid>("Utente em falta.");

            if (string.IsNullOrWhiteSpace(request.Credencial))
                return ResponseFactory.Fail<Guid>("Nº credencial em falta.");

            if (request.Mes is < 1 or > 12)
                return ResponseFactory.Fail<Guid>("Mês inválido.");

            if (request.Ano is null or < 1900)
                return ResponseFactory.Fail<Guid>("Ano inválido.");

            var entity = _mapper.Map<LoteDirect>(request);
            entity.Id = Guid.NewGuid();
            await _repository.CreateAsync<LoteDirect, Guid>(entity);
            await SincronizarLinhasAsync(entity.Id, request.Linhas, request.Linhas789);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(entity.Id);
        }

        public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateLoteDirectRequest request)
        {
            if (request.UtenteId is null || request.UtenteId == Guid.Empty)
                return ResponseFactory.Fail<Guid>("Utente em falta.");

            if (string.IsNullOrWhiteSpace(request.Credencial))
                return ResponseFactory.Fail<Guid>("Nº credencial em falta.");

            if (request.Mes is < 1 or > 12)
                return ResponseFactory.Fail<Guid>("Mês inválido.");

            if (request.Ano is null or < 1900)
                return ResponseFactory.Fail<Guid>("Ano inválido.");

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

        public async Task<Response<int>> CorrigirLotesAsync(CorrigirLotesRequest request)
        {
            if (request.Mes is < 1 or > 12)
                return ResponseFactory.Fail<int>("Mês inválido.");

            if (request.Ano < 1900)
                return ResponseFactory.Fail<int>("Ano inválido.");

            try
            {
                await _correcaoLotesExecutor.ExecutarAsync(request.Ano, request.Mes);
                return ResponseFactory.Success(request.Ano);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<int>($"Não foi possível corrigir os lotes: {ex.Message}");
            }
        }

        public async Task<Response<IEnumerable<TipoLoteLightDTO>>> GetTiposLoteLightAsync()
        {
            var spec = new TipoLoteSearchList();
            var list = await _repository.GetListAsync<TipoLote, TipoLoteLightDTO, int>(spec);
            return ResponseFactory.Success(list);
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
    }
}