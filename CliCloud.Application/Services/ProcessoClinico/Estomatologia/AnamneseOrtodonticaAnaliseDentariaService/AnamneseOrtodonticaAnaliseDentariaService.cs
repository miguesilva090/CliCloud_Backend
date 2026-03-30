using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.Filters;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.Specifications;

// After creating this service:
// -- 1. Create a AnamneseOrtodonticaAnaliseDentaria domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AnamneseOrtodonticaAnaliseDentaria> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AnamneseOrtodonticaAnaliseDentaria api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService
{
    public class AnamneseOrtodonticaAnaliseDentariaService : IAnamneseOrtodonticaAnaliseDentariaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AnamneseOrtodonticaAnaliseDentariaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get by utente id 
        public async Task<Response<AnamneseOrtodonticaAnaliseDentariaDTO?>> GetByUtenteAsync(Guid utenteId)
        {
            try
            {
                var specification = new AnamneseOrtodonticaAnaliseDentariaByUtenteIdSpecification(utenteId);
                var list = await _repository.GetListAsync<AnamneseOrtodonticaAnaliseDentaria, AnamneseOrtodonticaAnaliseDentariaDTO, Guid>(specification);
                var dto = list.FirstOrDefault();
                return ResponseFactory.Success<AnamneseOrtodonticaAnaliseDentariaDTO?>(dto);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOrtodonticaAnaliseDentariaDTO?>(ex.Message);
            }
        }

        // get full List
        public async Task<Response<IEnumerable<AnamneseOrtodonticaAnaliseDentariaDTO>>> GetAnamneseOrtodonticaAnaliseDentariaAsync(string keyword = "")
        {
            AnamneseOrtodonticaAnaliseDentariaSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AnamneseOrtodonticaAnaliseDentariaDTO> list = await _repository.GetListAsync<AnamneseOrtodonticaAnaliseDentaria, AnamneseOrtodonticaAnaliseDentariaDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<AnamneseOrtodonticaAnaliseDentariaDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<AnamneseOrtodonticaAnaliseDentariaDTO>> GetAnamneseOrtodonticaAnaliseDentariaPaginatedAsync(AnamneseOrtodonticaAnaliseDentariaTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            AnamneseOrtodonticaAnaliseDentariaSearchTable specification = new(filter.UtenteId, filter.Keyword, dynamicOrder); // ardalis specification
            PaginatedResponse<AnamneseOrtodonticaAnaliseDentariaDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AnamneseOrtodonticaAnaliseDentaria, AnamneseOrtodonticaAnaliseDentariaDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single AnamneseOrtodonticaAnaliseDentaria by Id 
        public async Task<Response<AnamneseOrtodonticaAnaliseDentariaDTO>> GetAnamneseOrtodonticaAnaliseDentariaAsync(Guid id)
        {
            try
            {
                AnamneseOrtodonticaAnaliseDentariaDTO dto = await _repository.GetByIdAsync<AnamneseOrtodonticaAnaliseDentaria, AnamneseOrtodonticaAnaliseDentariaDTO, Guid>(id);
                return ResponseFactory.Success<AnamneseOrtodonticaAnaliseDentariaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOrtodonticaAnaliseDentariaDTO>(ex.Message);
            }
        }

        // create new AnamneseOrtodonticaAnaliseDentaria
        public async Task<Response<Guid>> CreateAnamneseOrtodonticaAnaliseDentariaAsync(CreateAnamneseOrtodonticaAnaliseDentariaRequest request)
        {
            var existsSpecification = new AnamneseOrtodonticaAnaliseDentariaByUtenteIdSpecification(request.UtenteId);
            bool anameseOrtodonticaAnaliseDentariaExists =
                await _repository.ExistsAsync<AnamneseOrtodonticaAnaliseDentaria, Guid>(existsSpecification);
            if (anameseOrtodonticaAnaliseDentariaExists)
            {
                return ResponseFactory.Fail<Guid>(
                    "Já existe anamnese ortodôntica - análise dentária para este utente.");
            }

            AnamneseOrtodonticaAnaliseDentaria newAnamneseOrtodonticaAnaliseDentaria =
                _mapper.Map(request, new AnamneseOrtodonticaAnaliseDentaria()); // map dto to domain entity

            try
            {
                AnamneseOrtodonticaAnaliseDentaria response =
                    await _repository.CreateAsync<AnamneseOrtodonticaAnaliseDentaria, Guid>(
                        newAnamneseOrtodonticaAnaliseDentaria); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AnamneseOrtodonticaAnaliseDentaria
        public async Task<Response<Guid>> UpdateAnamneseOrtodonticaAnaliseDentariaAsync(UpdateAnamneseOrtodonticaAnaliseDentariaRequest request, Guid id)
        {
            AnamneseOrtodonticaAnaliseDentaria AnamneseOrtodonticaAnaliseDentariaInDb = await _repository.GetByIdAsync<AnamneseOrtodonticaAnaliseDentaria, Guid>(id); // get existing entity
            if (AnamneseOrtodonticaAnaliseDentariaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não encontrado");
            }

            AnamneseOrtodonticaAnaliseDentaria updatedAnamneseOrtodonticaAnaliseDentaria = _mapper.Map(request, AnamneseOrtodonticaAnaliseDentariaInDb); // map dto to domain entity

            try
            {
                AnamneseOrtodonticaAnaliseDentaria response = await _repository.UpdateAsync<AnamneseOrtodonticaAnaliseDentaria, Guid>(updatedAnamneseOrtodonticaAnaliseDentaria);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AnamneseOrtodonticaAnaliseDentaria
        public async Task<Response<Guid>> DeleteAnamneseOrtodonticaAnaliseDentariaAsync(Guid id)
        {
            try
            {
                AnamneseOrtodonticaAnaliseDentaria? AnamneseOrtodonticaAnaliseDentaria = await _repository.RemoveByIdAsync<AnamneseOrtodonticaAnaliseDentaria, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(AnamneseOrtodonticaAnaliseDentaria.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

