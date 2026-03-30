using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.DTOs;
using CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.Filters;
using CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.Specifications;

namespace CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService
{
    public class EstadoListaEsperaService : IEstadoListaEsperaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public EstadoListaEsperaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<EstadoListaEsperaDTO>>> GetEstadoListaEsperaAsync(string keyword = "")
        {
            var spec = new EstadoListaEsperaSearchList(keyword);
            var list = await _repository.GetListAsync<EstadoListaEspera, EstadoListaEsperaDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<EstadoListaEsperaLightDTO>>> GetEstadoListaEsperaLightAsync(string keyword = "")
        {
            var spec = new EstadoListaEsperaSearchList(keyword);
            var list = await _repository.GetListAsync<EstadoListaEspera, EstadoListaEsperaLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<EstadoListaEsperaTableDTO>> GetEstadoListaEsperaPaginatedAsync(EstadoListaEsperaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new EstadoListaEsperaSearchTable(filter.Filters ?? [], order);
            return await _repository.GetPaginatedResultsAsync<EstadoListaEspera, EstadoListaEsperaTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<EstadoListaEsperaTableDTO>>> GetAllEstadoListaEsperaAsync(EstadoListaEsperaAllFilter filter)
        {
            try
            {
                filter ??= new EstadoListaEsperaAllFilter();
                var order = EstadoListaEsperaAllFilter.GetOrderByString();
                var filters = filter.Filters ?? new List<TableFilter>();
                var spec = new EstadoListaEsperaSearchTable(filters, order);
                var list = await _repository.GetListAsync<EstadoListaEspera, EstadoListaEsperaTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<EstadoListaEsperaTableDTO>>(ex.Message); }
        }

        public async Task<Response<EstadoListaEsperaDTO>> GetEstadoListaEsperaAsync(Guid id)
        {
            try
            {
                var dto = await _repository.GetByIdAsync<EstadoListaEspera, EstadoListaEsperaDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex) { return ResponseFactory.Fail<EstadoListaEsperaDTO>(ex.Message); }
        }

        public async Task<Response<Guid>> CreateEstadoListaEsperaAsync(CreateEstadoListaEsperaRequest request)
        {
            var specDesc = new EstadoListaEsperaMatchDescricao(request.Descricao ?? "");
            if (await _repository.ExistsAsync<EstadoListaEspera, Guid>(specDesc))
                return ResponseFactory.Fail<Guid>("Já existe um estado da lista de espera com esta descrição.");

            var entity = _mapper.Map<EstadoListaEspera>(request);
            entity.Id = Guid.NewGuid();

            try
            {
                var created = await _repository.CreateAsync<EstadoListaEspera, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> UpdateEstadoListaEsperaAsync(UpdateEstadoListaEsperaRequest request, Guid id)
        {
            var existing = await _repository.GetByIdAsync<EstadoListaEspera, Guid>(id);
            if (existing == null) return ResponseFactory.Fail<Guid>("Estado da lista de espera não encontrado.");
            if (existing.Descricao != request.Descricao)
            {
                var spec = new EstadoListaEsperaMatchDescricao(request.Descricao ?? "");
                if (await _repository.ExistsAsync<EstadoListaEspera, Guid>(spec))
                    return ResponseFactory.Fail<Guid>("Já existe um estado da lista de espera com esta descrição.");
            }
            _mapper.Map(request, existing);
            try
            {
                var updated = await _repository.UpdateAsync<EstadoListaEspera, Guid>(existing);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(updated.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> DeleteEstadoListaEsperaAsync(Guid id)
        {
            try
            {
                var entity = await _repository.RemoveByIdAsync<EstadoListaEspera, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleEstadoListaEsperaAsync(IEnumerable<Guid> ids)
        {
            var list = ids.ToList();
            var ok = new List<Guid>();
            var fail = new List<string>();
            foreach (var id in list)
            {
                try
                {
                    var e = await _repository.GetByIdAsync<EstadoListaEspera, Guid>(id);
                    if (e == null) { fail.Add($"Estado da lista de espera {id} não encontrado."); continue; }
                    var removed = await _repository.RemoveByIdAsync<EstadoListaEspera, Guid>(id);
                    _ = await _repository.SaveChangesAsync();
                    ok.Add(removed.Id);
                }
                catch { fail.Add($"Estado da lista de espera {id}."); _repository.ClearChangeTracker(); }
            }
            if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
            if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
            return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
        }
    }
}
