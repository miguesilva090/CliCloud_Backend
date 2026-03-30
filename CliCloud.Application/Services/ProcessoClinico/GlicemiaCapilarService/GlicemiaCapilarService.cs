using AutoMapper;
using CliCloud.Application.Services.GlicemiaCapilarService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.GlicemiaCapilarService.Filters;
using CliCloud.Application.Services.GlicemiaCapilarService.Specifications;

// After creating this service:
// -- 1. Create a GlicemiaCapilar domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<GlicemiaCapilar> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a GlicemiaCapilar api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.GlicemiaCapilarService
{
    public class GlicemiaCapilarService : IGlicemiaCapilarService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public GlicemiaCapilarService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<GlicemiaCapilarDTO>>> GetGlicemiaCapilarAsync(string keyword = "")
        {
            var specification = new GlicemiaCapilarSearchList(keyword);
            IEnumerable<GlicemiaCapilarDTO> list = await _repository.GetListAsync<GlicemiaCapilar, GlicemiaCapilarDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<GlicemiaCapilarDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<GlicemiaCapilarDTO>> GetGlicemiaCapilarPaginatedAsync(GlicemiaCapilarTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            List<TableFilter> filters = filter.Filters ?? new List<TableFilter>();
            var specification = new GlicemiaCapilarSearchTable(filters, dynamicOrder);
            var pagedResponse = await _repository.GetPaginatedResultsAsync<GlicemiaCapilar, GlicemiaCapilarDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                specification
            );
            return pagedResponse;
        }


        // get single GlicemiaCapilar by Id 
        public async Task<Response<GlicemiaCapilarDTO>> GetGlicemiaCapilarAsync(Guid id)
        {
            try
            {
                GlicemiaCapilarDTO dto = await _repository.GetByIdAsync<GlicemiaCapilar, GlicemiaCapilarDTO, Guid>(id);
                return ResponseFactory.Success<GlicemiaCapilarDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<GlicemiaCapilarDTO>(ex.Message);
            }
        }

        // create new GlicemiaCapilar
        public async Task<Response<Guid>> CreateGlicemiaCapilarAsync(CreateGlicemiaCapilarRequest request)
        {
            var specification = new GlicemiaCapilarMatchName(request.UtenteId, request.Data, request.Hora);
            bool glicemiaCapilarExists = await _repository.ExistsAsync<GlicemiaCapilar, Guid>(specification);
            if (glicemiaCapilarExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um registo de glicemia capilar para o mesmo utente, data e hora.");
            }

            GlicemiaCapilar newGlicemiaCapilar = _mapper.Map(request, new GlicemiaCapilar());

            try
            {
                GlicemiaCapilar response = await _repository.CreateAsync<GlicemiaCapilar, Guid>(newGlicemiaCapilar);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update GlicemiaCapilar
        public async Task<Response<Guid>> UpdateGlicemiaCapilarAsync(UpdateGlicemiaCapilarRequest request, Guid id)
        {
            GlicemiaCapilar GlicemiaCapilarInDb = await _repository.GetByIdAsync<GlicemiaCapilar, Guid>(id); // get existing entity
            if (GlicemiaCapilarInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            GlicemiaCapilar updatedGlicemiaCapilar = _mapper.Map(request, GlicemiaCapilarInDb); // map dto to domain entity

            try
            {
                GlicemiaCapilar response = await _repository.UpdateAsync<GlicemiaCapilar, Guid>(updatedGlicemiaCapilar);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete GlicemiaCapilar
        public async Task<Response<Guid>> DeleteGlicemiaCapilarAsync(Guid id)
        {
            try
            {
                GlicemiaCapilar? glicemiaCapilar = await _repository.RemoveByIdAsync<GlicemiaCapilar, Guid>(id);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(glicemiaCapilar.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

