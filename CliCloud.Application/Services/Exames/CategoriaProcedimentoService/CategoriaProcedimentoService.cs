using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Application.Services.Exames.CategoriaProcedimentoService.DTOs;
using CliCloud.Application.Services.Exames.CategoriaProcedimentoService.Filters;
using CliCloud.Application.Services.Exames.CategoriaProcedimentoService.Specifications;

namespace CliCloud.Application.Services.Exames.CategoriaProcedimentoService
{
    public class CategoriaProcedimentoService : ICategoriaProcedimentoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public CategoriaProcedimentoService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<CategoriaProcedimentoDTO>>> GetCategoriaProcedimentoAsync(string keyword = "")
        {
            var spec = new CategoriaProcedimentoSearchList(keyword);
            var list = await _repository.GetListAsync<CategoriaProcedimento, CategoriaProcedimentoDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<CategoriaProcedimentoLightDTO>>> GetCategoriaProcedimentoLightAsync(string keyword = "")
        {
            var spec = new CategoriaProcedimentoSearchList(keyword);
            var list = await _repository.GetListAsync<CategoriaProcedimento, CategoriaProcedimentoLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<CategoriaProcedimentoTableDTO>> GetCategoriaProcedimentoPaginatedAsync(CategoriaProcedimentoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new CategoriaProcedimentoSearchTable(filter.Filters ?? [], order);
            return await _repository.GetPaginatedResultsAsync<CategoriaProcedimento, CategoriaProcedimentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<CategoriaProcedimentoTableDTO>>> GetAllCategoriaProcedimentoAsync(CategoriaProcedimentoAllFilter? filter)
        {
            try
            {
                filter ??= new CategoriaProcedimentoAllFilter();
                var order = filter.GetOrderByString();
                var spec = new CategoriaProcedimentoSearchTable(filter.Filters ?? [], order);
                var list = await _repository.GetListAsync<CategoriaProcedimento, CategoriaProcedimentoTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<CategoriaProcedimentoTableDTO>>(ex.Message); }
        }

        public async Task<Response<CategoriaProcedimentoDTO>> GetCategoriaProcedimentoAsync(Guid id)
        {
            try
            {
                var dto = await _repository.GetByIdAsync<CategoriaProcedimento, CategoriaProcedimentoDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex) { return ResponseFactory.Fail<CategoriaProcedimentoDTO>(ex.Message); }
        }

        public async Task<Response<Guid>> CreateCategoriaProcedimentoAsync(CreateCategoriaProcedimentoRequest request)
        {
            var entity = _mapper.Map<CategoriaProcedimento>(request);
            try
            {
                var created = await _repository.CreateAsync<CategoriaProcedimento, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> UpdateCategoriaProcedimentoAsync(UpdateCategoriaProcedimentoRequest request, Guid id)
        {
            var existing = await _repository.GetByIdAsync<CategoriaProcedimento, Guid>(id);
            _mapper.Map(request, existing);
            try
            {
                _ = await _repository.UpdateAsync<CategoriaProcedimento, Guid>(existing);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(existing.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> DeleteCategoriaProcedimentoAsync(Guid id)
        {
            try
            {
                var entity = await _repository.RemoveByIdAsync<CategoriaProcedimento, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleCategoriaProcedimentoAsync(IEnumerable<Guid> ids)
        {
            var ok = new List<Guid>();
            var fail = new List<string>();
            foreach (var id in ids.ToList())
            {
                try
                {
                    var e = await _repository.RemoveByIdAsync<CategoriaProcedimento, Guid>(id);
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
