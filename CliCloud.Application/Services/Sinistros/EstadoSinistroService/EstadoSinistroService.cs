using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Sinistros.EstadoSinistroService.DTOs;
using CliCloud.Application.Services.Sinistros.EstadoSinistroService.Filters;
using CliCloud.Application.Services.Sinistros.EstadoSinistroService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Sinistros;

namespace CliCloud.Application.Services.Sinistros.EstadoSinistroService
{
    public class EstadoSinistroService(IRepositoryAsync repository, IMapper mapper) : IEstadoSinistroService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper;

        public Task<PaginatedResponse<EstadoSinistroDTO>> GetPaginatedAsync(EstadoSinistroTableFilter filter)
        {
            if (filter.Filters?.Count > 0) filter.PageNumber = 1;
            var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            var spec = new EstadoSinistroSearchTable(filter.Filters ?? [], order);
            return _repository.GetPaginatedResultsAsync<EstadoSinistroItem, EstadoSinistroDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
        }

        public async Task<Response<EstadoSinistroDTO>> GetByIdAsync(Guid id)
        {
            var dto = await _repository.GetByIdAsync<EstadoSinistroItem, EstadoSinistroDTO, Guid>(id);
            return ResponseFactory.Success(dto);
        }

        public async Task<Response<Guid>> CreateAsync(CreateEstadoSinistroRequest request)
        {
            var entity = _mapper.Map<EstadoSinistroItem>(request);
            entity.Id = Guid.NewGuid();
            await _repository.CreateAsync<EstadoSinistroItem, Guid>(entity);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(entity.Id);
        }

        public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateEstadoSinistroRequest request)
        {
            var entity = await _repository.GetByIdAsync<EstadoSinistroItem, Guid>(id);
            _mapper.Map(request, entity);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(entity.Id);
        }

        public async Task<Response<Guid>> DeleteAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<EstadoSinistroItem, Guid>(id);
            await _repository.SaveChangesAsync();
            return ResponseFactory.Success(id);
        }
    }
}