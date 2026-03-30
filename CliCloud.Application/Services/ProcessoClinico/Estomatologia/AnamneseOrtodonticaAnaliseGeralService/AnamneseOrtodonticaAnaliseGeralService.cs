using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.Filters;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.Specifications;

// After creating this service:
// -- 1. Create a AnamneseOrtodonticaAnaliseGeral domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AnamneseOrtodonticaAnaliseGeral> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AnamneseOrtodonticaAnaliseGeral api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService
{
    public class AnamneseOrtodonticaAnaliseGeralService : IAnamneseOrtodonticaAnaliseGeralService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AnamneseOrtodonticaAnaliseGeralService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get by utente id 
        public async Task<Response<AnamneseOrtodonticaAnaliseGeralDTO?>> GetByUtenteAsync(Guid utenteId)
        {
            try
            {
                var specification = new AnamneseOrtodonticaByUtenteIdSpecification(utenteId);
                var list = await _repository.GetListAsync<AnamneseOrtodonticaAnaliseGeral, AnamneseOrtodonticaAnaliseGeralDTO, Guid>(specification);
                var dto = list.FirstOrDefault();

                return ResponseFactory.Success<AnamneseOrtodonticaAnaliseGeralDTO?>(dto);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOrtodonticaAnaliseGeralDTO?>(ex.Message);
            }
        }

        // get full List
        public async Task<Response<IEnumerable<AnamneseOrtodonticaAnaliseGeralDTO>>> GetAnamneseOrtodonticaAnaliseGeralAsync(string keyword = "")
        {
            AnamneseOrtodonticaAnaliseGeralSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AnamneseOrtodonticaAnaliseGeralDTO> list = await _repository.GetListAsync<AnamneseOrtodonticaAnaliseGeral, AnamneseOrtodonticaAnaliseGeralDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<AnamneseOrtodonticaAnaliseGeralDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<AnamneseOrtodonticaAnaliseGeralDTO>> GetAnamneseOrtodonticaAnaliseGeralPaginatedAsync(AnamneseOrtodonticaAnaliseGeralTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            AnamneseOrtodonticaAnaliseGeralSearchTable specification = new(filter.UtenteId, filter.Keyword, dynamicOrder); // ardalis specification
            PaginatedResponse<AnamneseOrtodonticaAnaliseGeralDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AnamneseOrtodonticaAnaliseGeral, AnamneseOrtodonticaAnaliseGeralDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single AnamneseOrtodonticaAnaliseGeral by Id 
        public async Task<Response<AnamneseOrtodonticaAnaliseGeralDTO>> GetAnamneseOrtodonticaAnaliseGeralAsync(Guid id)
        {
            try
            {
                AnamneseOrtodonticaAnaliseGeralDTO dto = await _repository.GetByIdAsync<AnamneseOrtodonticaAnaliseGeral, AnamneseOrtodonticaAnaliseGeralDTO, Guid>(id);
                return ResponseFactory.Success<AnamneseOrtodonticaAnaliseGeralDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOrtodonticaAnaliseGeralDTO>(ex.Message);
            }
        }

        // create new AnamneseOrtodonticaAnaliseGeral
        public async Task<Response<Guid>> CreateAnamneseOrtodonticaAnaliseGeralAsync(CreateAnamneseOrtodonticaAnaliseGeralRequest request)
        {
            var existsSpecification = new AnamneseOrtodonticaByUtenteIdSpecification(request.UtenteId);
            bool anameseOrtodonticaAnaliseGeralExists = await _repository.ExistsAsync<AnamneseOrtodonticaAnaliseGeral, Guid>(existsSpecification);
            if (anameseOrtodonticaAnaliseGeralExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe anamnese ortodontica analisé geral para este utente.");
            }

            AnamneseOrtodonticaAnaliseGeral newAnamneseOrtodonticaAnaliseGeral = _mapper.Map(request, new AnamneseOrtodonticaAnaliseGeral()); // map dto to domain entity

            try
            {
                AnamneseOrtodonticaAnaliseGeral response = await _repository.CreateAsync<AnamneseOrtodonticaAnaliseGeral, Guid>(newAnamneseOrtodonticaAnaliseGeral); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AnamneseOrtodonticaAnaliseGeral
        public async Task<Response<Guid>> UpdateAnamneseOrtodonticaAnaliseGeralAsync(UpdateAnamneseOrtodonticaAnaliseGeralRequest request, Guid id)
        {
            AnamneseOrtodonticaAnaliseGeral AnamneseOrtodonticaAnaliseGeralInDb = await _repository.GetByIdAsync<AnamneseOrtodonticaAnaliseGeral, Guid>(id); // get existing entity
            if (AnamneseOrtodonticaAnaliseGeralInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Não encontrado");
            }

            AnamneseOrtodonticaAnaliseGeral updatedAnamneseOrtodonticaAnaliseGeral = _mapper.Map(request, AnamneseOrtodonticaAnaliseGeralInDb); // map dto to domain entity

            try
            {
                AnamneseOrtodonticaAnaliseGeral response = await _repository.UpdateAsync<AnamneseOrtodonticaAnaliseGeral, Guid>(updatedAnamneseOrtodonticaAnaliseGeral);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AnamneseOrtodonticaAnaliseGeral
        public async Task<Response<Guid>> DeleteAnamneseOrtodonticaAnaliseGeralAsync(Guid id)
        {
            try
            {
                AnamneseOrtodonticaAnaliseGeral? AnamneseOrtodonticaAnaliseGeral = await _repository.RemoveByIdAsync<AnamneseOrtodonticaAnaliseGeral, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(AnamneseOrtodonticaAnaliseGeral.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

