using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.Estomatologia;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.Filters;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.Specifications;

// After creating this service:
// -- 1. Create a AnamneseOdontopediatria domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<AnamneseOdontopediatria> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a AnamneseOdontopediatria api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService
{
    public class AnamneseOdontopediatriaService : IAnamneseOdontopediatriaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public AnamneseOdontopediatriaService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        //get by utente id 
        public async Task<Response<AnamneseOdontopediatriaDTO?>> GetByUtenteAsync(Guid utenteId)
        {
            try
            {
                var specification = new AnamneseOdontopediatriaByUtenteIdSpecification(utenteId);
                var list = await _repository.GetListAsync<AnamneseOdontopediatria, AnamneseOdontopediatriaDTO, Guid>(specification);
                var dto = list.FirstOrDefault();
                return ResponseFactory.Success<AnamneseOdontopediatriaDTO?>(dto);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOdontopediatriaDTO?>(ex.Message);
            }
        }

        // get full List
        public async Task<Response<IEnumerable<AnamneseOdontopediatriaDTO>>> GetAnamneseOdontopediatriaAsync(string keyword = "")
        {
            AnamneseOdontopediatriaSearchList specification = new(keyword); // ardalis specification
            IEnumerable<AnamneseOdontopediatriaDTO> list = await _repository.GetListAsync<AnamneseOdontopediatria, AnamneseOdontopediatriaDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<AnamneseOdontopediatriaDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<AnamneseOdontopediatriaDTO>> GetAnamneseOdontopediatriaPaginatedAsync(AnamneseOdontopediatriaTableFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword)) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            AnamneseOdontopediatriaSearchTable specification = new(filter.UtenteId, filter.Keyword, dynamicOrder); // ardalis specification
            PaginatedResponse<AnamneseOdontopediatriaDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<AnamneseOdontopediatria, AnamneseOdontopediatriaDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }


        // get single AnamneseOdontopediatria by Id 
        public async Task<Response<AnamneseOdontopediatriaDTO>> GetAnamneseOdontopediatriaAsync(Guid id)
        {
            try
            {
                AnamneseOdontopediatriaDTO dto = await _repository.GetByIdAsync<AnamneseOdontopediatria, AnamneseOdontopediatriaDTO, Guid>(id);
                return ResponseFactory.Success<AnamneseOdontopediatriaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<AnamneseOdontopediatriaDTO>(ex.Message);
            }
        }

        // create new AnamneseOdontopediatria
        public async Task<Response<Guid>> CreateAnamneseOdontopediatriaAsync(CreateAnamneseOdontopediatriaRequest request)
        {
            var existsSpecification = new AnamneseOdontopediatriaByUtenteIdSpecification(request.UtenteId);
            bool anameseOdontopediatriaExists = await _repository.ExistsAsync<AnamneseOdontopediatria, Guid>(existsSpecification);
            if (anameseOdontopediatriaExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe anamnese odontopediátrica para este utente.");
            }

            AnamneseOdontopediatria newAnamneseOdontopediatria = _mapper.Map(request, new AnamneseOdontopediatria()); // map dto to domain entity

            try
            {
                AnamneseOdontopediatria response = await _repository.CreateAsync<AnamneseOdontopediatria, Guid>(newAnamneseOdontopediatria); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update AnamneseOdontopediatria
        public async Task<Response<Guid>> UpdateAnamneseOdontopediatriaAsync(UpdateAnamneseOdontopediatriaRequest request, Guid id)
        {
            AnamneseOdontopediatria AnamneseOdontopediatriaInDb = await _repository.GetByIdAsync<AnamneseOdontopediatria, Guid>(id); // get existing entity
            if (AnamneseOdontopediatriaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            AnamneseOdontopediatria updatedAnamneseOdontopediatria = _mapper.Map(request, AnamneseOdontopediatriaInDb); // map dto to domain entity

            try
            {
                AnamneseOdontopediatria response = await _repository.UpdateAsync<AnamneseOdontopediatria, Guid>(updatedAnamneseOdontopediatria);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete AnamneseOdontopediatria
        public async Task<Response<Guid>> DeleteAnamneseOdontopediatriaAsync(Guid id)
        {
            try
            {
                AnamneseOdontopediatria? AnamneseOdontopediatria = await _repository.RemoveByIdAsync<AnamneseOdontopediatria, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(AnamneseOdontopediatria.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

