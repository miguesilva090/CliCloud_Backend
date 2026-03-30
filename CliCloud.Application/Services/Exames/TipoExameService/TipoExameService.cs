using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Application.Services.Exames.TipoExameService.DTOs;
using CliCloud.Application.Services.Exames.TipoExameService.Filters;
using CliCloud.Application.Services.Exames.TipoExameService.Specifications;

namespace CliCloud.Application.Services.Exames.TipoExameService
{
    public class TipoExameService : ITipoExameService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public TipoExameService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<TipoExameDTO>>> GetTipoExameAsync(string keyword = "")
        {
            var spec = new TipoExameSearchList(keyword);
            var list = await _repository.GetListAsync<TipoExame, TipoExameDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<TipoExameLightDTO>>> GetTipoExameLightAsync(string keyword = "")
        {
            var spec = new TipoExameSearchList(keyword);
            var list = await _repository.GetListAsync<TipoExame, TipoExameLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<TipoExameTableDTO>> GetTipoExamePaginatedAsync(TipoExameTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new TipoExameSearchTable(filter.Filters ?? [], order);
            return await _repository.GetPaginatedResultsAsync<TipoExame, TipoExameTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<TipoExameTableDTO>>> GetAllTipoExameAsync(TipoExameAllFilter? filter)
        {
            try
            {
                filter ??= new TipoExameAllFilter();
                var order = filter.GetOrderByString();
                var spec = new TipoExameSearchTable(filter.Filters ?? [], order);
                var list = await _repository.GetListAsync<TipoExame, TipoExameTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<TipoExameTableDTO>>(ex.Message); }
        }

        public async Task<Response<TipoExameDTO>> GetTipoExameAsync(Guid id)
        {
            try
            {
                var spec = new TipoExameByIdWithGrupoAnaliseLinhas(id);
                var entity = await _repository.GetByIdAsync<TipoExame, Guid>(id, spec);
                var dto = _mapper.Map<TipoExameDTO>(entity);
                return ResponseFactory.Success(dto);
            }
            catch (InvalidOperationException) { return ResponseFactory.Fail<TipoExameDTO>("Tipo de exame não encontrado."); }
            catch (Exception ex) { return ResponseFactory.Fail<TipoExameDTO>(ex.Message); }
        }

        public async Task<Response<Guid>> CreateTipoExameAsync(CreateTipoExameRequest request)
        {
            var entity = _mapper.Map<TipoExame>(request);
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            try
            {
                var created = await _repository.CreateAsync<TipoExame, Guid>(entity);
                foreach (var req in request.GrupoAnaliseLinhas ?? [])
                {
                    created.GrupoAnaliseLinhas.Add(new GrupoAnaliseLinha
                    {
                        TipoExameId = created.Id,
                        AnaliseId = req.AnaliseId,
                        Ordem = req.Ordem,
                        Descricao = req.Descricao,
                        UnidadeMedida = req.UnidadeMedida,
                        ValoresReferencia = req.ValoresReferencia
                    });
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> UpdateTipoExameAsync(UpdateTipoExameRequest request, Guid id)
        {
            TipoExame existing;
            try
            {
                // Carregar o TipoExame com as linhas de grupo de análises para podermos sincronizar adições/remoções/edições
                var spec = new TipoExameByIdWithGrupoAnaliseLinhas(id);
                existing = await _repository.GetByIdAsync<TipoExame, Guid>(id, spec);
            }
            catch (InvalidOperationException)
            {
                return ResponseFactory.Fail<Guid>("Tipo de exame não encontrado.");
            }
            // Atualizar apenas os campos básicos do TipoExame (o mapeamento ignora GrupoAnaliseLinhas)
            _mapper.Map(request, existing);
            existing.LastModifiedOn = DateTime.UtcNow;

            // --- Sincronizar GrupoAnaliseLinhas ---
            var linhasExistentes = existing.GrupoAnaliseLinhas.ToList();
            var linhasRequest = request.GrupoAnaliseLinhas ?? [];

            // Índice de linhas existentes por Id
            var existentesPorId = linhasExistentes
                .Where(l => l.Id != Guid.Empty)
                .ToDictionary(l => l.Id, l => l);

            // Ids que vêm do request (linhas que devem continuar a existir)
            var idsRequest = new HashSet<Guid>(
                linhasRequest
                    .Where(r => r.Id.HasValue && r.Id.Value != Guid.Empty)
                    .Select(r => r.Id!.Value)
            );

            // Remover linhas que existem na BD mas não vêm no request
            foreach (var linha in linhasExistentes)
            {
                if (!idsRequest.Contains(linha.Id))
                {
                    await _repository.RemoveAsync<GrupoAnaliseLinha, Guid>(linha);
                }
            }

            // Atualizar ou adicionar linhas do request
            foreach (var reqLinha in linhasRequest)
            {
                if (reqLinha.Id.HasValue &&
                    reqLinha.Id.Value != Guid.Empty &&
                    existentesPorId.TryGetValue(reqLinha.Id.Value, out var existente))
                {
                    // Atualizar linha existente
                    existente.AnaliseId = reqLinha.AnaliseId;
                    existente.Ordem = reqLinha.Ordem;
                    existente.Descricao = reqLinha.Descricao;
                    existente.UnidadeMedida = reqLinha.UnidadeMedida;
                    existente.ValoresReferencia = reqLinha.ValoresReferencia;
                }
                else
                {
                    // Nova linha
                    existing.GrupoAnaliseLinhas.Add(new GrupoAnaliseLinha
                    {
                        TipoExameId = existing.Id,
                        AnaliseId = reqLinha.AnaliseId,
                        Ordem = reqLinha.Ordem,
                        Descricao = reqLinha.Descricao,
                        UnidadeMedida = reqLinha.UnidadeMedida,
                        ValoresReferencia = reqLinha.ValoresReferencia
                    });
                }
            }

            try
            {
                _ = await _repository.UpdateAsync<TipoExame, Guid>(existing);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(existing.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> DeleteTipoExameAsync(Guid id)
        {
            try
            {
                var entity = await _repository.RemoveByIdAsync<TipoExame, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoExameAsync(IEnumerable<Guid> ids)
        {
            var ok = new List<Guid>();
            var fail = new List<string>();
            foreach (var id in ids.ToList())
            {
                try
                {
                    var e = await _repository.RemoveByIdAsync<TipoExame, Guid>(id);
                    if (e != null) { _ = await _repository.SaveChangesAsync(); ok.Add(id); }
                    else fail.Add(id.ToString());
                }
                catch { fail.Add(id.ToString()); _repository.ClearChangeTracker(); }
            }
            if (ok.Count == ids.Count()) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
            if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {ids.Count()}.");
            return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
        }
    }
}
