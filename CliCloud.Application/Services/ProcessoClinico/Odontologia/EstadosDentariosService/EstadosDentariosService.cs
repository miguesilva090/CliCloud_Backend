using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.Filters;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.Specifications;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService
{
    public class EstadosDentariosService : IEstadosDentariosService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public EstadosDentariosService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<EstadosDentariosDTO>>> GetEstadosDentariosAsync(string keyword = "")
        {
            EstadosDentariosSearchList specification = new(keyword); // ardalis specification
            IEnumerable<EstadosDentariosDTO> list = await _repository.GetListAsync<EstadosDentarios, EstadosDentariosDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<EstadosDentariosDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<EstadosDentariosDTO>> GetEstadosDentariosPaginatedAsync(EstadosDentariosTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            EstadosDentariosSearchTable specification = new(filter?.Keyword, dynamicOrder); // ardalis specification
            PaginatedResponse<EstadosDentariosDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<EstadosDentarios, EstadosDentariosDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single EstadosDentarios by Id 
        public async Task<Response<EstadosDentariosDTO>> GetEstadosDentariosAsync(Guid id)
        {
            try
            {
                EstadosDentariosDTO dto = await _repository.GetByIdAsync<EstadosDentarios, EstadosDentariosDTO, Guid>(id);
                return ResponseFactory.Success<EstadosDentariosDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<EstadosDentariosDTO>(ex.Message);
            }
        }

        // create new EstadosDentarios
        public async Task<Response<Guid>> CreateEstadosDentariosAsync(CreateEstadosDentariosRequest request)
        {
            EstadosDentariosMatchCodigo specification = new(request.Codigo); // ardalis specification 
            bool estadosDentariosExists = await _repository.ExistsAsync<EstadosDentarios, Guid>(specification);
            if (estadosDentariosExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um estado dentário com este código.");
            }

            EstadosDentarios newEstadosDentarios = _mapper.Map(request, new EstadosDentarios()); // map dto to domain entity

            try
            {
                EstadosDentarios response = await _repository.CreateAsync<EstadosDentarios, Guid>(newEstadosDentarios); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update EstadosDentarios
        public async Task<Response<Guid>> UpdateEstadosDentariosAsync(UpdateEstadosDentariosRequest request, Guid id)
        {
            EstadosDentarios estadosDentariosInDb = await _repository.GetByIdAsync<EstadosDentarios, Guid>(id); // get existing entity
            if (estadosDentariosInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não encontrado");
            }

            EstadosDentarios updatedEstadosDentarios = _mapper.Map(request, estadosDentariosInDb); // map dto to domain entity

            try
            {
                EstadosDentarios response = await _repository.UpdateAsync<EstadosDentarios, Guid>(updatedEstadosDentarios);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete EstadosDentarios
        public async Task<Response<Guid>> DeleteEstadosDentariosAsync(Guid id)
        {
            try
            {
                EstadosDentarios? estadosDentarios = await _repository.RemoveByIdAsync<EstadosDentarios, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(estadosDentarios.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

