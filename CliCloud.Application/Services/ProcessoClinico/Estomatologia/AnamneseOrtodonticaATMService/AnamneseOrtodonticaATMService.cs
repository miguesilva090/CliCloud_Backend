using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.Filters;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.Specifications;

// After creating this service:
// -- 1. Create a AnamneseOrtodonticaATM domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AnamneseOrtodonticaATM> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AnamneseOrtodonticaATM api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService
{
    public class AnamneseOrtodonticaATMService : IAnamneseOrtodonticaATMService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AnamneseOrtodonticaATMService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get by utente id 
        public async Task<Response<AnamneseOrtodonticaATMDTO?>> GetByUtenteAsync(Guid utenteId)
        {
            try
            {
                var specification = new AnamneseOrtodonticaATMByUtenteIdSpecification(utenteId);
                var list = await _repository.GetListAsync<AnamneseOrtodonticaATM, AnamneseOrtodonticaATMDTO, Guid>(specification);
                var dto = list.FirstOrDefault();

                return ResponseFactory.Success<AnamneseOrtodonticaATMDTO?>(dto);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOrtodonticaATMDTO?>(ex.Message);
            }
        }

        // get full List
        public async Task<Response<IEnumerable<AnamneseOrtodonticaATMDTO>>> GetAnamneseOrtodonticaATMAsync(string keyword = "")
        {
            AnamneseOrtodonticaATMSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AnamneseOrtodonticaATMDTO> list = await _repository.GetListAsync<AnamneseOrtodonticaATM, AnamneseOrtodonticaATMDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<AnamneseOrtodonticaATMDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<AnamneseOrtodonticaATMDTO>> GetAnamneseOrtodonticaATMPaginatedAsync(AnamneseOrtodonticaATMTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            AnamneseOrtodonticaATMSearchTable specification = new(filter.UtenteId, filter.Keyword, dynamicOrder); // ardalis specification
            PaginatedResponse<AnamneseOrtodonticaATMDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AnamneseOrtodonticaATM, AnamneseOrtodonticaATMDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single AnamneseOrtodonticaATM by Id 
        public async Task<Response<AnamneseOrtodonticaATMDTO>> GetAnamneseOrtodonticaATMAsync(Guid id)
        {
            try
            {
                AnamneseOrtodonticaATMDTO dto = await _repository.GetByIdAsync<AnamneseOrtodonticaATM, AnamneseOrtodonticaATMDTO, Guid>(id);
                return ResponseFactory.Success<AnamneseOrtodonticaATMDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOrtodonticaATMDTO>(ex.Message);
            }
        }

        // create new AnamneseOrtodonticaATM
        public async Task<Response<Guid>> CreateAnamneseOrtodonticaATMAsync(CreateAnamneseOrtodonticaATMRequest request)
        {
            var existsSpecification = new AnamneseOrtodonticaATMByUtenteIdSpecification(request.UtenteId);
            bool anameseOrtodonticaATMExists = await _repository.ExistsAsync<AnamneseOrtodonticaATM, Guid>(existsSpecification);
            if (anameseOrtodonticaATMExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe anamnese ortodontica ATM para este utente.");
            }

            AnamneseOrtodonticaATM newAnamneseOrtodonticaATM = _mapper.Map(request, new AnamneseOrtodonticaATM()); // map dto to domain entity

            try
            {
                AnamneseOrtodonticaATM response = await _repository.CreateAsync<AnamneseOrtodonticaATM, Guid>(newAnamneseOrtodonticaATM); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AnamneseOrtodonticaATM
        public async Task<Response<Guid>> UpdateAnamneseOrtodonticaATMAsync(UpdateAnamneseOrtodonticaATMRequest request, Guid id)
        {
            AnamneseOrtodonticaATM AnamneseOrtodonticaATMInDb = await _repository.GetByIdAsync<AnamneseOrtodonticaATM, Guid>(id); // get existing entity
            if (AnamneseOrtodonticaATMInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não encontrado");
            }

            AnamneseOrtodonticaATM updatedAnamneseOrtodonticaATM = _mapper.Map(request, AnamneseOrtodonticaATMInDb); // map dto to domain entity

            try
            {
                AnamneseOrtodonticaATM response = await _repository.UpdateAsync<AnamneseOrtodonticaATM, Guid>(updatedAnamneseOrtodonticaATM);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AnamneseOrtodonticaATM
        public async Task<Response<Guid>> DeleteAnamneseOrtodonticaATMAsync(Guid id)
        {
            try
            {
                AnamneseOrtodonticaATM? AnamneseOrtodonticaATM = await _repository.RemoveByIdAsync<AnamneseOrtodonticaATM, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(AnamneseOrtodonticaATM.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

