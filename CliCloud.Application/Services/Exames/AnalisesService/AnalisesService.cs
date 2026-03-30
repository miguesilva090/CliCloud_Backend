using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Application.Services.Exames.AnalisesService.DTOs;
using CliCloud.Application.Services.Exames.AnalisesService.Filters;
using CliCloud.Application.Services.Exames.AnalisesService.Specifications;

namespace CliCloud.Application.Services.Exames.AnalisesService
{
    public class AnalisesService : IAnalisesService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AnalisesService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<AnaliseDTO>>> GetAnaliseAsync(string keyword = "")
        {
            var spec = new AnaliseSearchList(keyword);
            var list = await _repository.GetListAsync<Analises, AnaliseDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<AnaliseLightDTO>>> GetAnaliseLightAsync(string keyword = "")
        {
            var spec = new AnaliseSearchList(keyword);
            var list = await _repository.GetListAsync<Analises, AnaliseLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<AnaliseTableDTO>> GetAnalisePaginatedAsync(AnaliseTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new AnaliseSearchTable(filter.Filters ?? [], order);
            return await _repository.GetPaginatedResultsAsync<Analises, AnaliseTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<AnaliseTableDTO>>> GetAllAnaliseAsync(AnaliseAllFilter? filter)
        {
            try
            {
                filter ??= new AnaliseAllFilter();
                var order = filter.GetOrderByString();
                var spec = new AnaliseSearchTable(filter.Filters ?? [], order);
                var list = await _repository.GetListAsync<Analises, AnaliseTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<AnaliseTableDTO>>(ex.Message); }
        }

        public async Task<Response<AnaliseDTO>> GetAnaliseAsync(Guid id)
        {
            try
            {
                var dto = await _repository.GetByIdAsync<Analises, AnaliseDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex) { return ResponseFactory.Fail<AnaliseDTO>(ex.Message); }
        }

        public async Task<Response<Guid>> CreateAnaliseAsync(CreateAnaliseRequest request)
        {
            var entity = _mapper.Map<Analises>(request);
            try
            {
                var created = await _repository.CreateAsync<Analises, Guid>(entity);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> UpdateAnaliseAsync(UpdateAnaliseRequest request, Guid id)
        {
            var existing = await _repository.GetByIdAsync<Analises, Guid>(id);
            _mapper.Map(request, existing);
            try
            {
                _ = await _repository.UpdateAsync<Analises, Guid>(existing);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(existing.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> DeleteAnaliseAsync(Guid id)
        {
            try
            {
                var entity = await _repository.RemoveByIdAsync<Analises, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAnaliseAsync(IEnumerable<Guid> ids)
        {
            var ok = new List<Guid>();
            foreach (var id in ids.ToList())
            {
                try
                {
                    var e = await _repository.RemoveByIdAsync<Analises, Guid>(id);
                    if (e != null) { _ = await _repository.SaveChangesAsync(); ok.Add(id); }
                }
                catch { _repository.ClearChangeTracker(); }
            }
            if (ok.Count == ids.Count()) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
            if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {ids.Count()}.");
            return ResponseFactory.Fail<IEnumerable<Guid>>("Nenhum eliminado.");
        }
    }
}
