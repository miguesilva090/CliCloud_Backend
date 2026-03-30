using AutoMapper;
using CliCloud.Application.Services.AlergiaUtenteService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Alergias;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.AlergiaUtenteService.Filters;
using CliCloud.Application.Services.AlergiaUtenteService.Specifications;

// After creating this service:
// -- 1. Create a AlergiaUtente domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AlergiaUtente> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AlergiaUtente api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.AlergiaUtenteService
{
    public class AlergiaUtenteService : IAlergiaUtenteService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AlergiaUtenteService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<AlergiaUtenteDTO>>> GetAlergiaUtenteAsync(string keyword = "")
        {
            AlergiaUtenteSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AlergiaUtenteDTO> list = await _repository.GetListAsync<AlergiaUtente, AlergiaUtenteDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success(list);
        }


        public async Task<PaginatedResponse<AlergiaUtenteDTO>> GetAlergiaUtentePaginatedAsync(AlergiaUtenteTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            AlergiaUtenteSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<AlergiaUtenteDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AlergiaUtente, AlergiaUtenteDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }


        // get single AlergiaUtente by Id 
        public async Task<Response<AlergiaUtenteDTO>> GetAlergiaUtenteAsync(Guid id)
        {
            try
            {
                AlergiaUtenteDTO dto = await _repository.GetByIdAsync<AlergiaUtente, AlergiaUtenteDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AlergiaUtenteDTO>(ex.Message);
            }
        }

        // create new AlergiaUtente
        public async Task<Response<Guid>> CreateAlergiaUtenteAsync(CreateAlergiaUtenteRequest request)
        {
            AlergiaUtenteByUtenteAlergia specification = new(request.UtenteId, request.AlergiaId);
            bool AlergiaUtenteExists = await _repository.ExistsAsync<AlergiaUtente, Guid>(specification);
            if (AlergiaUtenteExists)
            {
                return ResponseFactory.Fail<Guid>("AlergiaUtente already exists");
            }

            AlergiaUtente newAlergiaUtente = _mapper.Map(request, new AlergiaUtente()); // map dto to domain entity

            try
            {
                AlergiaUtente response = await _repository.CreateAsync<AlergiaUtente, Guid>(newAlergiaUtente); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AlergiaUtente
        public async Task<Response<Guid>> UpdateAlergiaUtenteAsync(UpdateAlergiaUtenteRequest request, Guid id)
        {
            AlergiaUtente AlergiaUtenteInDb = await _repository.GetByIdAsync<AlergiaUtente, Guid>(id); // get existing entity
            if (AlergiaUtenteInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            AlergiaUtente updatedAlergiaUtente = _mapper.Map(request, AlergiaUtenteInDb); // map dto to domain entity

            try
            {
                AlergiaUtente response = await _repository.UpdateAsync<AlergiaUtente, Guid>(updatedAlergiaUtente);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AlergiaUtente
        public async Task<Response<Guid>> DeleteAlergiaUtenteAsync(Guid id)
        {
            try
            {
                AlergiaUtente? AlergiaUtente = await _repository.RemoveByIdAsync<AlergiaUtente, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success(AlergiaUtente.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

