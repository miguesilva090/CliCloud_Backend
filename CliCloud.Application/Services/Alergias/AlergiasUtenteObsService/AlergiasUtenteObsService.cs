using System.Linq;
using AutoMapper;
using CliCloud.Application.Services.AlergiasUtenteObsService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Alergias;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.AlergiasUtenteObsService.Filters;
using CliCloud.Application.Services.AlergiasUtenteObsService.Specifications;

// After creating this service:
// -- 1. Create a AlergiasUtenteObs domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AlergiasUtenteObs> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AlergiasUtenteObs api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.AlergiasUtenteObsService
{
    public class AlergiasUtenteObsService : IAlergiasUtenteObsService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AlergiasUtenteObsService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<AlergiasUtenteObsDTO>>> GetAlergiasUtenteObsAsync(string keyword = "")
        {
            AlergiasUtenteObsSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AlergiasUtenteObsDTO> list = await _repository.GetListAsync<AlergiasUtenteObs, AlergiasUtenteObsDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success(list);
        }


        public async Task<PaginatedResponse<AlergiasUtenteObsDTO>> GetAlergiasUtenteObsPaginatedAsync(AlergiasUtenteObsTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
                filter.PageNumber = 1;

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            AlergiasUtenteObsSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<AlergiasUtenteObsDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AlergiasUtenteObs, AlergiasUtenteObsDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }


        // get single AlergiasUtenteObs by Id 
        public async Task<Response<AlergiasUtenteObsDTO>> GetAlergiasUtenteObsAsync(Guid id)
        {
            try
            {
                AlergiasUtenteObsDTO dto = await _repository.GetByIdAsync<AlergiasUtenteObs, AlergiasUtenteObsDTO, Guid>(id);
                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AlergiasUtenteObsDTO>(ex.Message);
            }
        }

        public async Task<Response<AlergiasUtenteObsDTO?>> GetByUtenteIdAsync(Guid utenteId)
        {
            try
            {
                AlergiasUtenteObsByUtenteId specification = new(utenteId);
                IEnumerable<AlergiasUtenteObsDTO> list =
                    await _repository.GetListAsync<AlergiasUtenteObs, AlergiasUtenteObsDTO, Guid>(specification);
                AlergiasUtenteObsDTO? dto = list.FirstOrDefault();
                return ResponseFactory.Success<AlergiasUtenteObsDTO?>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AlergiasUtenteObsDTO?>(ex.Message);
            }
        }

        // create new AlergiasUtenteObs
        public async Task<Response<Guid>> CreateAlergiasUtenteObsAsync(CreateAlergiasUtenteObsRequest request)
        {
            AlergiasUtenteObsByUtenteId specification = new(request.UtenteId);
            bool AlergiasUtenteObsExists = await _repository.ExistsAsync<AlergiasUtenteObs, Guid>(specification);
            if (AlergiasUtenteObsExists)
            {
                return ResponseFactory.Fail<Guid>("AlergiasUtenteObs already exists");
            }

            AlergiasUtenteObs newAlergiasUtenteObs = _mapper.Map(request, new AlergiasUtenteObs()); // map dto to domain entity

            try
            {
                AlergiasUtenteObs response = await _repository.CreateAsync<AlergiasUtenteObs, Guid>(newAlergiasUtenteObs); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AlergiasUtenteObs
        public async Task<Response<Guid>> UpdateAlergiasUtenteObsAsync(UpdateAlergiasUtenteObsRequest request, Guid id)
        {
            AlergiasUtenteObs AlergiasUtenteObsInDb = await _repository.GetByIdAsync<AlergiasUtenteObs, Guid>(id); // get existing entity
            if (AlergiasUtenteObsInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            AlergiasUtenteObs updatedAlergiasUtenteObs = _mapper.Map(request, AlergiasUtenteObsInDb); // map dto to domain entity

            try
            {
                AlergiasUtenteObs response = await _repository.UpdateAsync<AlergiasUtenteObs, Guid>(updatedAlergiasUtenteObs);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AlergiasUtenteObs
        public async Task<Response<Guid>> DeleteAlergiasUtenteObsAsync(Guid id)
        {
            try
            {
                AlergiasUtenteObs? AlergiasUtenteObs = await _repository.RemoveByIdAsync<AlergiasUtenteObs, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success(AlergiasUtenteObs.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

