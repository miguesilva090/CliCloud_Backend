using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Services.Tratamentos.PrioridadeService.DTOs;
using CliCloud.Application.Services.Tratamentos.PrioridadeService.Filters;
using CliCloud.Application.Services.Tratamentos.PrioridadeService.Specifications;

namespace CliCloud.Application.Services.Tratamentos.PrioridadeService
{
    public class PrioridadeService : IPrioridadeService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public PrioridadeService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<PrioridadeDTO>>> GetPrioridadeAsync(string keyword = "")
        {
            var spec = new PrioridadeSearchList(keyword);
            var list = await _repository.GetListAsync<Prioridade, PrioridadeDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<PrioridadeLightDTO>>> GetPrioridadeLightAsync(string keyword = "")
        {
            var spec = new PrioridadeSearchList(keyword);
            var list = await _repository.GetListAsync<Prioridade, PrioridadeLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<PrioridadeTableDTO>> GetPrioridadePaginatedAsync(PrioridadeTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new PrioridadeSearchTable(filter.Filters ?? [], order);
            return await _repository.GetPaginatedResultsAsync<Prioridade, PrioridadeTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<PrioridadeTableDTO>>> GetAllPrioridadeAsync(PrioridadeAllFilter filter)
        {
            try
            {
                filter ??= new PrioridadeAllFilter();
                var order = PrioridadeAllFilter.GetOrderByString();
                var filters = filter.Filters ?? new List<TableFilter>();
                var spec = new PrioridadeSearchTable(filters, order);
                var list = await _repository.GetListAsync<Prioridade, PrioridadeTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<PrioridadeTableDTO>>(ex.Message); }
        }

        public async Task<Response<PrioridadeDTO>> GetPrioridadeAsync(Guid id)
        {
            try
            {
                var dto = await _repository.GetByIdAsync<Prioridade, PrioridadeDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex) { return ResponseFactory.Fail<PrioridadeDTO>(ex.Message); }
        }

        public async Task<Response<Guid>> CreatePrioridadeAsync(CreatePrioridadeRequest request)
        {
            var specDesc = new PrioridadeMatchDescricao(request.Descricao ?? "");
            if (await _repository.ExistsAsync<Prioridade, Guid>(specDesc))
                return ResponseFactory.Fail<Guid>("Já existe uma prioridade com esta descrição.");

            var entity = _mapper.Map<Prioridade>(request);
            entity.Id = Guid.NewGuid();

            try
            {
                var created = await _repository.CreateAsync<Prioridade, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> UpdatePrioridadeAsync(UpdatePrioridadeRequest request, Guid id)
        {
            var existing = await _repository.GetByIdAsync<Prioridade, Guid>(id);
            if (existing == null) return ResponseFactory.Fail<Guid>("Prioridade não encontrada.");
            if (existing.Descricao != request.Descricao)
            {
                var spec = new PrioridadeMatchDescricao(request.Descricao ?? "");
                if (await _repository.ExistsAsync<Prioridade, Guid>(spec))
                    return ResponseFactory.Fail<Guid>("Já existe uma prioridade com esta descrição.");
            }
            _mapper.Map(request, existing);
            try
            {
                var updated = await _repository.UpdateAsync<Prioridade, Guid>(existing);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(updated.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> DeletePrioridadeAsync(Guid id)
        {
            try
            {
                var entity = await _repository.RemoveByIdAsync<Prioridade, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultiplePrioridadeAsync(IEnumerable<Guid> ids)
        {
            var list = ids.ToList();
            var ok = new List<Guid>();
            var fail = new List<string>();
            foreach (var id in list)
            {
                try
                {
                    var e = await _repository.GetByIdAsync<Prioridade, Guid>(id);
                    if (e == null) { fail.Add($"Prioridade {id} não encontrada."); continue; }
                    var removed = await _repository.RemoveByIdAsync<Prioridade, Guid>(id);
                    _ = await _repository.SaveChangesAsync();
                    ok.Add(removed.Id);
                }
                catch { fail.Add($"Prioridade {id}."); _repository.ClearChangeTracker(); }
            }
            if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
            if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
            return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
        }
    }
}
