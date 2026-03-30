using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Application.Services.Exames.AcordosService.DTOs;
using CliCloud.Application.Services.Exames.AcordosService.Filters;
using CliCloud.Application.Services.Exames.AcordosService.Specifications;

namespace CliCloud.Application.Services.Exames.AcordosService
{
    public class AcordosService : IAcordosService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AcordosService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<AcordosDTO>>> GetAcordosAsync(string keyword = "")
        {
            var spec = new AcordosSearchList(keyword);
            var list = await _repository.GetListAsync<Acordos, AcordosDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<Response<IEnumerable<AcordosLightDTO>>> GetAcordosLightAsync(string keyword = "")
        {
            var spec = new AcordosSearchList(keyword);
            var list = await _repository.GetListAsync<Acordos, AcordosLightDTO, Guid>(spec);
            return ResponseFactory.Success(list);
        }

        public async Task<PaginatedResponse<AcordosTableDTO>> GetAcordosPaginatedAsync(AcordosTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new AcordosSearchTable(filter.Filters ?? [], order);
            return await _repository.GetPaginatedResultsAsync<Acordos, AcordosTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<IEnumerable<AcordosTableDTO>>> GetAllAcordosAsync(AcordosAllFilter? filter)
        {
            try
            {
                filter ??= new AcordosAllFilter();
                var order = filter.GetOrderByString();
                var spec = new AcordosSearchTable(filter.Filters ?? [], order);
                var list = await _repository.GetListAsync<Acordos, AcordosTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<AcordosTableDTO>>(ex.Message); }
        }

        public async Task<Response<AcordosDTO>> GetAcordosAsync(Guid id)
        {
            try
            {
                var spec = new AcordosByIdWithIncludes(id);
                var entity = await _repository.GetByIdAsync<Acordos, Guid>(id, spec);
                var dto = _mapper.Map<AcordosDTO>(entity);
                return ResponseFactory.Success(dto);
            }
            catch (InvalidOperationException) { return ResponseFactory.Fail<AcordosDTO>("Acordo não encontrado."); }
            catch (Exception ex) { return ResponseFactory.Fail<AcordosDTO>(ex.Message); }
        }

        public async Task<Response<Guid>> CreateAcordosAsync(CreateAcordosRequest request)
        {
            var entity = _mapper.Map<Acordos>(request);
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            try
            {
                var created = await _repository.CreateAsync<Acordos, Guid>(entity);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(created.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> UpdateAcordosAsync(UpdateAcordosRequest request, Guid id)
        {
            Acordos existing;
            try
            {
                var spec = new AcordosByIdWithIncludes(id);
                existing = await _repository.GetByIdAsync<Acordos, Guid>(id, spec);
            }
            catch (InvalidOperationException)
            {
                return ResponseFactory.Fail<Guid>("Acordo não encontrado.");
            }
            _mapper.Map(request, existing);
            existing.LastModifiedOn = DateTime.UtcNow;
            try
            {
                _ = await _repository.UpdateAsync<Acordos, Guid>(existing);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success(existing.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<Guid>> DeleteAcordosAsync(Guid id)
        {
            try
            {
                var entity = await _repository.RemoveByIdAsync<Acordos, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(entity.Id);
            }
            catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAcordosAsync(IEnumerable<Guid> ids)
        {
            var ok = new List<Guid>();
            var fail = new List<string>();
            foreach (var id in ids.ToList())
            {
                try
                {
                    var e = await _repository.RemoveByIdAsync<Acordos, Guid>(id);
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
