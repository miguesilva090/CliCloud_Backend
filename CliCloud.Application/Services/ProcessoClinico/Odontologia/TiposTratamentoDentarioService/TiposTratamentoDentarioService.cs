using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.Filters;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.Specifications;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService
{
    public class TiposTratamentoDentarioService : ITiposTratamentoDentarioService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public TiposTratamentoDentarioService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<TiposTratamentoDentarioDTO>>> GetTiposTratamentoDentarioAsync(string keyword = "")
        {
            TiposTratamentoDentarioSearchList specification = new(keyword); // ardalis specification
            IEnumerable<TiposTratamentoDentarioDTO> list = await _repository.GetListAsync<TipoTratamentoDentario, TiposTratamentoDentarioDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<TiposTratamentoDentarioDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<TiposTratamentoDentarioDTO>> GetTiposTratamentoDentarioPaginatedAsync(TiposTratamentoDentarioTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = (filter.Sorting != null) ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            TiposTratamentoDentarioSearchTable specification = new(filter?.Keyword, dynamicOrder); // ardalis specification
            PaginatedResponse<TiposTratamentoDentarioDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<TipoTratamentoDentario, TiposTratamentoDentarioDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single TiposTratamentoDentario by Id 
        public async Task<Response<TiposTratamentoDentarioDTO>> GetTiposTratamentoDentarioAsync(Guid id)
        {
            try
            {
                TiposTratamentoDentarioDTO dto = await _repository.GetByIdAsync<TipoTratamentoDentario, TiposTratamentoDentarioDTO, Guid>(id);
                return ResponseFactory.Success<TiposTratamentoDentarioDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<TiposTratamentoDentarioDTO>(ex.Message);
            }
        }

        // create new TiposTratamentoDentario
        public async Task<Response<Guid>> CreateTiposTratamentoDentarioAsync(CreateTiposTratamentoDentarioRequest request)
        {
            TiposTratamentoDentarioMatchCodigo specification = new(request.Codigo); // ardalis specification 
            bool TiposTratamentoDentarioExists = await _repository.ExistsAsync<TipoTratamentoDentario, Guid>(specification);
            if (TiposTratamentoDentarioExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um tipo de tratamento dentário com este código.");
            }

            TipoTratamentoDentario newTiposTratamentoDentario = _mapper.Map(request, new TipoTratamentoDentario()); // map dto to domain entity

            try
            {
                TipoTratamentoDentario response = await _repository.CreateAsync<TipoTratamentoDentario, Guid>(newTiposTratamentoDentario); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update TiposTratamentoDentario
        public async Task<Response<Guid>> UpdateTiposTratamentoDentarioAsync(UpdateTiposTratamentoDentarioRequest request, Guid id)
        {
            TipoTratamentoDentario TiposTratamentoDentarioInDb = await _repository.GetByIdAsync<TipoTratamentoDentario, Guid>(id); // get existing entity
            if (TiposTratamentoDentarioInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não encontrado");
            }

            TipoTratamentoDentario updatedTiposTratamentoDentario = _mapper.Map(request, TiposTratamentoDentarioInDb); // map dto to domain entity

            try
            {
                TipoTratamentoDentario response = await _repository.UpdateAsync<TipoTratamentoDentario, Guid>(updatedTiposTratamentoDentario);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete TiposTratamentoDentario
        public async Task<Response<Guid>> DeleteTiposTratamentoDentarioAsync(Guid id)
        {
            try
            {
                TipoTratamentoDentario? TiposTratamentoDentario = await _repository.RemoveByIdAsync<TipoTratamentoDentario, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(TiposTratamentoDentario.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

