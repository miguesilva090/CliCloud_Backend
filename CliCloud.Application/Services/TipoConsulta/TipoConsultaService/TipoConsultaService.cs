using AutoMapper;
using CliCloud.Application.Services.TiposConsulta.TipoConsultaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.TiposConsulta.TipoConsultaService.Filters;
using CliCloud.Application.Services.TiposConsulta.TipoConsultaService.Specifications;

namespace CliCloud.Application.Services.TiposConsulta.TipoConsultaService
{
    public class TipoConsultaService : ITipoConsultaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public TipoConsultaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<TipoConsultaTableDTO>> GetTipoConsultaPaginatedAsync(TipoConsultaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            TipoConsultaSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<TipoConsultaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<TipoConsultaItem, TipoConsultaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        public async Task<Response<TipoConsultaDTO>> GetTipoConsultaAsync(Guid id)
        {
            try
            {
                TipoConsultaDTO dto = await _repository.GetByIdAsync<TipoConsultaItem, TipoConsultaDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<TipoConsultaDTO>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<TipoConsultaTableDTO>>> GetAllTipoConsultaAsync(TipoConsultaAllFilter? filter)
        {
            try
            {
                filter ??= new TipoConsultaAllFilter();
                var order = filter.GetOrderByString();
                var filters = filter.Filters ?? new List<TableFilter>();
                var spec = new TipoConsultaSearchTable(filters, order);
                var list = await _repository.GetListAsync<TipoConsultaItem, TipoConsultaTableDTO, Guid>(spec);
                return ResponseFactory.Success(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<TipoConsultaTableDTO>>(ex.Message);
            }
        }

        public async Task<Response<Guid>> UpdateTipoConsultaAsync(UpdateTipoConsultaRequest request, Guid id)
        {
            try
            {
                TipoConsultaItem entity = await _repository.GetByIdAsync<TipoConsultaItem, Guid>(id);
                _ = _mapper.Map(request, entity);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(entity.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> DeleteTipoConsultaAsync(Guid id)
        {
            try
            {
                _ = await _repository.RemoveByIdAsync<TipoConsultaItem, Guid>(id);
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success(id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}
