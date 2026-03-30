using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.Filters;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.Specifications;

// After creating this service:
// -- 1. Create a AnamneseOrtodonticaDenticaoDeciduaeMista domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AnamneseOrtodonticaDenticaoDeciduaeMista> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AnamneseOrtodonticaDenticaoDeciduaeMista api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService
{
    public class AnamneseOrtodonticaDenticaoDeciduaeMistaService : IAnamneseOrtodonticaDenticaoDeciduaeMistaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AnamneseOrtodonticaDenticaoDeciduaeMistaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        public async Task<Response<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO?>> GetByUtenteAsync(Guid utenteId)
        {
            try
            {
                var specification = new AnamneseOrtodonticaDenticaoDeciduaeMistaByUtenteIdSpecification(utenteId);
                var list = await _repository.GetListAsync<AnamneseOrtodonticaDenticaoDeciduaeMista, AnamneseOrtodonticaDenticaoDeciduaeMistaDTO, Guid>(specification);
                var dto = list.FirstOrDefault();

                return ResponseFactory.Success<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO?>(dto);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO?>(ex.Message);
            }
        }

        // get full List
        public async Task<Response<IEnumerable<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>>> GetAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(string keyword = "")
        {
            AnamneseOrtodonticaDenticaoDeciduaeMistaSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO> list = await _repository.GetListAsync<AnamneseOrtodonticaDenticaoDeciduaeMista, AnamneseOrtodonticaDenticaoDeciduaeMistaDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>> GetAnamneseOrtodonticaDenticaoDeciduaeMistaPaginatedAsync(AnamneseOrtodonticaDenticaoDeciduaeMistaTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            AnamneseOrtodonticaDenticaoDeciduaeMistaSearchTable specification = new(filter.UtenteId, filter.Keyword, dynamicOrder); // ardalis specification
            PaginatedResponse<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AnamneseOrtodonticaDenticaoDeciduaeMista, AnamneseOrtodonticaDenticaoDeciduaeMistaDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single AnamneseOrtodonticaDenticaoDeciduaeMista by Id 
        public async Task<Response<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>> GetAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(Guid id)
        {
            try
            {
                AnamneseOrtodonticaDenticaoDeciduaeMistaDTO dto = await _repository.GetByIdAsync<AnamneseOrtodonticaDenticaoDeciduaeMista, AnamneseOrtodonticaDenticaoDeciduaeMistaDTO, Guid>(id);
                return ResponseFactory.Success<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOrtodonticaDenticaoDeciduaeMistaDTO>(ex.Message);
            }
        }

        // create new AnamneseOrtodonticaDenticaoDeciduaeMista
        public async Task<Response<Guid>> CreateAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(CreateAnamneseOrtodonticaDenticaoDeciduaeMistaRequest request)
        {
            AnamneseOrtodonticaDenticaoDeciduaeMistaByUtenteIdSpecification specification = new(request.UtenteId); // ardalis specification 
            bool anameseOrtodonticaDenticaoDeciduaeMistaExists = await _repository.ExistsAsync<AnamneseOrtodonticaDenticaoDeciduaeMista, Guid>(specification);
            if (anameseOrtodonticaDenticaoDeciduaeMistaExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe anamnese ortodontica dentição deciduae mista para este utente.");
            }

            AnamneseOrtodonticaDenticaoDeciduaeMista newAnamneseOrtodonticaDenticaoDeciduaeMista = _mapper.Map(request, new AnamneseOrtodonticaDenticaoDeciduaeMista()); // map dto to domain entity

            try
            {
                AnamneseOrtodonticaDenticaoDeciduaeMista response = await _repository.CreateAsync<AnamneseOrtodonticaDenticaoDeciduaeMista, Guid>(newAnamneseOrtodonticaDenticaoDeciduaeMista); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AnamneseOrtodonticaDenticaoDeciduaeMista
        public async Task<Response<Guid>> UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaRequest request, Guid id)
        {
            AnamneseOrtodonticaDenticaoDeciduaeMista AnamneseOrtodonticaDenticaoDeciduaeMistaInDb = await _repository.GetByIdAsync<AnamneseOrtodonticaDenticaoDeciduaeMista, Guid>(id); // get existing entity
            if (AnamneseOrtodonticaDenticaoDeciduaeMistaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            AnamneseOrtodonticaDenticaoDeciduaeMista updatedAnamneseOrtodonticaDenticaoDeciduaeMista = _mapper.Map(request, AnamneseOrtodonticaDenticaoDeciduaeMistaInDb); // map dto to domain entity

            try
            {
                AnamneseOrtodonticaDenticaoDeciduaeMista response = await _repository.UpdateAsync<AnamneseOrtodonticaDenticaoDeciduaeMista, Guid>(updatedAnamneseOrtodonticaDenticaoDeciduaeMista);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AnamneseOrtodonticaDenticaoDeciduaeMista
        public async Task<Response<Guid>> DeleteAnamneseOrtodonticaDenticaoDeciduaeMistaAsync(Guid id)
        {
            try
            {
                AnamneseOrtodonticaDenticaoDeciduaeMista? AnamneseOrtodonticaDenticaoDeciduaeMista = await _repository.RemoveByIdAsync<AnamneseOrtodonticaDenticaoDeciduaeMista, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(AnamneseOrtodonticaDenticaoDeciduaeMista.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

