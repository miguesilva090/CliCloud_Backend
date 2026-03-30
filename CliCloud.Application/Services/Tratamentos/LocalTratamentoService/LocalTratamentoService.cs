using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Services.Tratamentos.LocalTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.LocalTratamentoService.Filters;
using CliCloud.Application.Services.Tratamentos.LocalTratamentoService.Specifications;

namespace CliCloud.Application.Services.Tratamentos.LocalTratamentoService
{
    public class LocalTratamentoService : ILocalTratamentoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public LocalTratamentoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<LocalTratamentoDTO>>> GetLocalTratamentoAsync(string keyword = "")
        {
            var spec = new LocalTratamentoSearchList(keyword);
            var list = await _repository.GetListAsync<LocalTratamento, LocalTratamentoDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<LocalTratamentoLightDTO>>> GetLocalTratamentoLightAsync(string keyword = "")
        {
            var spec = new LocalTratamentoSearchList(keyword);
            var list = await _repository.GetListAsync<LocalTratamento, LocalTratamentoLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<LocalTratamentoTableDTO>> GetLocalTratamentoPaginatedAsync(LocalTratamentoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new LocalTratamentoSearchTable(filter.Filters ?? [], order);
            return await _repository.GetPaginatedResultsAsync<LocalTratamento, LocalTratamentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<LocalTratamentoTableDTO>>> GetAllLocalTratamentoAsync(LocalTratamentoAllFilter filter)
        {
            try
            {
                filter ??= new LocalTratamentoAllFilter();
                var order = filter.GetOrderByString();
                var filters = filter.Filters ?? new List<TableFilter>();
                var spec = new LocalTratamentoSearchTable(filters, order);
                var list = await _repository.GetListAsync<LocalTratamento, LocalTratamentoTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<LocalTratamentoTableDTO>>(ex.Message); }
        }

        public async Task<Response<LocalTratamentoDTO>> GetLocalTratamentoAsync(Guid id)
        {
            try
            {
                var dto = await _repository.GetByIdAsync<LocalTratamento, LocalTratamentoDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex) { return ResponseFactory.Fail<LocalTratamentoDTO>(ex.Message); }
        }

        public async Task<Response<Guid>> CreateLocalTratamentoAsync(CreateLocalTratamentoRequest request)
        {
            var specDesignacao = new LocalTratamentoMatchDesignacao(request.Designacao);
            if (await _repository.ExistsAsync<LocalTratamento, Guid>(specDesignacao))
                return ResponseFactory.Fail<Guid>("Local de tratamento com esta designação já existe.");

            var entity = _mapper.Map<LocalTratamento>(request);
            entity.Id = Guid.NewGuid();

            try
            {
                var created = await _repository.CreateAsync<LocalTratamento, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> UpdateLocalTratamentoAsync(UpdateLocalTratamentoRequest request, Guid id)
        {
            var existing = await _repository.GetByIdAsync<LocalTratamento, Guid>(id);
            if (existing == null) return ResponseFactory.Fail<Guid>("Local de tratamento não encontrado.");
            if (existing.Designacao != request.Designacao)
            {
                var spec = new LocalTratamentoMatchDesignacao(request.Designacao);
                if (await _repository.ExistsAsync<LocalTratamento, Guid>(spec))
                    return ResponseFactory.Fail<Guid>("Já existe um local de tratamento com esta designação.");
            }
            _mapper.Map(request, existing);
            try
            {
                var updated = await _repository.UpdateAsync<LocalTratamento, Guid>(existing);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(updated.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> DeleteLocalTratamentoAsync(Guid id)
        {
            try
            {
                var entity = await _repository.RemoveByIdAsync<LocalTratamento, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleLocalTratamentoAsync(IEnumerable<Guid> ids)
        {
            var list = ids.ToList();
            var ok = new List<Guid>();
            var fail = new List<string>();
            foreach (var id in list)
            {
                try
                {
                    var e = await _repository.GetByIdAsync<LocalTratamento, Guid>(id);
                    if (e == null) { fail.Add($"Local de tratamento {id} não encontrado."); continue; }
                    var removed = await _repository.RemoveByIdAsync<LocalTratamento, Guid>(id);
                    _ = await _repository.SaveChangesAsync();
                    ok.Add(removed.Id);
                }
                catch { fail.Add($"Local de tratamento {id}."); _repository.ClearChangeTracker(); }
            }
            if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
            if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
            return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
        }
    }
}
