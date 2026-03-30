using AutoMapper;
using CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Antecedentes;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.Filters;
using CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.Specifications;

// After creating this service:
// -- 1. Create a AntecedentesCirurgicos domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AntecedentesCirurgicos> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AntecedentesCirurgicos api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService
{
    public class AntecedentesCirurgicosService : IAntecedentesCirurgicosService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AntecedentesCirurgicosService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<AntecedentesCirurgicosDTO>>> GetAntecedentesCirurgicosAsync(string keyword = "")
        {
            AntecedentesCirurgicosSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AntecedentesCirurgicosDTO> list = await _repository.GetListAsync<AntecedentesCirurgicos, AntecedentesCirurgicosDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<AntecedentesCirurgicosDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<AntecedentesCirurgicosTableDTO>> GetAntecedentesCirurgicosPaginatedAsync(AntecedentesCirurgicosTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
            AntecedentesCirurgicosSearchTable specification = new(tableFilters, dynamicOrder); // ardalis specification
            PaginatedResponse<AntecedentesCirurgicosTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AntecedentesCirurgicos, AntecedentesCirurgicosTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single AntecedentesCirurgicos by Id 
        public async Task<Response<AntecedentesCirurgicosDTO>> GetAntecedentesCirurgicosAsync(Guid id)
        {
            try
            {
                AntecedentesCirurgicosDTO dto = await _repository.GetByIdAsync<AntecedentesCirurgicos, AntecedentesCirurgicosDTO, Guid>(id);
                return ResponseFactory.Success<AntecedentesCirurgicosDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AntecedentesCirurgicosDTO>(ex.Message);
            }
        }

        // create new AntecedentesCirurgicos
        public async Task<Response<Guid>> CreateAntecedentesCirurgicosAsync(CreateAntecedentesCirurgicosRequest request)
        {
            AntecedentesCirurgicos newAntecedentesCirurgicos = _mapper.Map(request, new AntecedentesCirurgicos()); // map dto to domain entity

            try
            {
                AntecedentesCirurgicos response = await _repository.CreateAsync<AntecedentesCirurgicos, Guid>(newAntecedentesCirurgicos); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AntecedentesCirurgicos
        public async Task<Response<Guid>> UpdateAntecedentesCirurgicosAsync(UpdateAntecedentesCirurgicosRequest request, Guid id)
        {
            AntecedentesCirurgicos AntecedentesCirurgicosInDb = await _repository.GetByIdAsync<AntecedentesCirurgicos, Guid>(id); // get existing entity
            if (AntecedentesCirurgicosInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            AntecedentesCirurgicos updatedAntecedentesCirurgicos = _mapper.Map(request, AntecedentesCirurgicosInDb); // map dto to domain entity

            try
            {
                AntecedentesCirurgicos response = await _repository.UpdateAsync<AntecedentesCirurgicos, Guid>(updatedAntecedentesCirurgicos);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AntecedentesCirurgicos
        public async Task<Response<Guid>> DeleteAntecedentesCirurgicosAsync(Guid id)
        {
            try
            {
                AntecedentesCirurgicos? AntecedentesCirurgicos = await _repository.RemoveByIdAsync<AntecedentesCirurgicos, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(AntecedentesCirurgicos.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

