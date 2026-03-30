using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.Filters;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.Specifications;

// After creating this service:
// -- 1. Create a AnamneseOrtodonticaAnaliseFuncional domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AnamneseOrtodonticaAnaliseFuncional> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AnamneseOrtodonticaAnaliseFuncional api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService
{
    public class AnamneseOrtodonticaAnaliseFuncionalService : IAnamneseOrtodonticaAnaliseFuncionalService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AnamneseOrtodonticaAnaliseFuncionalService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        //get bu utente id 
        public async Task<Response<AnamneseOrtodonticaAnaliseFuncionalDTO?>> GetByUtenteAsync(Guid utenteId)
        {
            try
            {
                var specification = new AnamneseOrtodonticaAnaliseFuncionalByUtenteIdSpecification(utenteId);
                var list = await _repository.GetListAsync<AnamneseOrtodonticaAnaliseFuncional, AnamneseOrtodonticaAnaliseFuncionalDTO, Guid>(specification);
                var dto = list.FirstOrDefault();

                return ResponseFactory.Success<AnamneseOrtodonticaAnaliseFuncionalDTO?>(dto);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOrtodonticaAnaliseFuncionalDTO?>(ex.Message);
            }
        }

        // get full List
        public async Task<Response<IEnumerable<AnamneseOrtodonticaAnaliseFuncionalDTO>>> GetAnamneseOrtodonticaAnaliseFuncionalAsync(string keyword = "")
        {
            AnamneseOrtodonticaAnaliseFuncionalSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AnamneseOrtodonticaAnaliseFuncionalDTO> list = await _repository.GetListAsync<AnamneseOrtodonticaAnaliseFuncional, AnamneseOrtodonticaAnaliseFuncionalDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<AnamneseOrtodonticaAnaliseFuncionalDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<AnamneseOrtodonticaAnaliseFuncionalDTO>> GetAnamneseOrtodonticaAnaliseFuncionalPaginatedAsync(AnamneseOrtodonticaAnaliseFuncionalTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            AnamneseOrtodonticaAnaliseFuncionalSearchTable specification = new(filter.UtenteId, filter.Keyword, dynamicOrder); // ardalis specification
            PaginatedResponse<AnamneseOrtodonticaAnaliseFuncionalDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AnamneseOrtodonticaAnaliseFuncional, AnamneseOrtodonticaAnaliseFuncionalDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single AnamneseOrtodonticaAnaliseFuncional by Id 
        public async Task<Response<AnamneseOrtodonticaAnaliseFuncionalDTO>> GetAnamneseOrtodonticaAnaliseFuncionalAsync(Guid id)
        {
            try
            {
                AnamneseOrtodonticaAnaliseFuncionalDTO dto = await _repository.GetByIdAsync<AnamneseOrtodonticaAnaliseFuncional, AnamneseOrtodonticaAnaliseFuncionalDTO, Guid>(id);
                return ResponseFactory.Success<AnamneseOrtodonticaAnaliseFuncionalDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOrtodonticaAnaliseFuncionalDTO>(ex.Message);
            }
        }

        // create new AnamneseOrtodonticaAnaliseFuncional
        public async Task<Response<Guid>> CreateAnamneseOrtodonticaAnaliseFuncionalAsync(CreateAnamneseOrtodonticaAnaliseFuncionalRequest request)
        {
            var existsSpecification = new AnamneseOrtodonticaAnaliseFuncionalByUtenteIdSpecification(request.UtenteId);
            bool anameseOrtodonticaAnaliseFuncionalExists = await _repository.ExistsAsync<AnamneseOrtodonticaAnaliseFuncional, Guid>(existsSpecification);
            if (anameseOrtodonticaAnaliseFuncionalExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe anamnese ortodôntica - análise funcional para este utente.");
            }

            AnamneseOrtodonticaAnaliseFuncional newAnamneseOrtodonticaAnaliseFuncional = _mapper.Map(request, new AnamneseOrtodonticaAnaliseFuncional()); // map dto to domain entity

            try
            {
                AnamneseOrtodonticaAnaliseFuncional response = await _repository.CreateAsync<AnamneseOrtodonticaAnaliseFuncional, Guid>(newAnamneseOrtodonticaAnaliseFuncional); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AnamneseOrtodonticaAnaliseFuncional
        public async Task<Response<Guid>> UpdateAnamneseOrtodonticaAnaliseFuncionalAsync(UpdateAnamneseOrtodonticaAnaliseFuncionalRequest request, Guid id)
        {
            AnamneseOrtodonticaAnaliseFuncional AnamneseOrtodonticaAnaliseFuncionalInDb = await _repository.GetByIdAsync<AnamneseOrtodonticaAnaliseFuncional, Guid>(id); // get existing entity
            if (AnamneseOrtodonticaAnaliseFuncionalInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não encontrado");
            }

            AnamneseOrtodonticaAnaliseFuncional updatedAnamneseOrtodonticaAnaliseFuncional = _mapper.Map(request, AnamneseOrtodonticaAnaliseFuncionalInDb); // map dto to domain entity

            try
            {
                AnamneseOrtodonticaAnaliseFuncional response = await _repository.UpdateAsync<AnamneseOrtodonticaAnaliseFuncional, Guid>(updatedAnamneseOrtodonticaAnaliseFuncional);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AnamneseOrtodonticaAnaliseFuncional
        public async Task<Response<Guid>> DeleteAnamneseOrtodonticaAnaliseFuncionalAsync(Guid id)
        {
            try
            {
                AnamneseOrtodonticaAnaliseFuncional? AnamneseOrtodonticaAnaliseFuncional = await _repository.RemoveByIdAsync<AnamneseOrtodonticaAnaliseFuncional, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(AnamneseOrtodonticaAnaliseFuncional.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

