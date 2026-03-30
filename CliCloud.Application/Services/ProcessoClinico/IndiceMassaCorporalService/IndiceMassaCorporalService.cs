using AutoMapper;
using CliCloud.Application.Services.IndiceMassaCorporalService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.IndiceMassaCorporalService.Filters;
using CliCloud.Application.Services.IndiceMassaCorporalService.Specifications;

// After creating this service:
// -- 1. Create a IndiceMassaCorporal domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<IndiceMassaCorporal> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a IndiceMassaCorporal api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.IndiceMassaCorporalService
{
    public class IndiceMassaCorporalService : IIndiceMassaCorporalService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public IndiceMassaCorporalService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<IndiceMassaCorporalDTO>>> GetIndiceMassaCorporalAsync(string keyword = "")
        {
            var specification = new IndiceMassaCorporalSearchList(keyword);
            IEnumerable<IndiceMassaCorporalDTO> list = await _repository.GetListAsync<IndiceMassaCorporal, IndiceMassaCorporalDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<IndiceMassaCorporalDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<IndiceMassaCorporalDTO>> GetIndiceMassaCorporalPaginatedAsync(IndiceMassaCorporalTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            List<TableFilter> filters = filter.Filters ?? new List<TableFilter>();
            var specification = new IndiceMassaCorporalSearchTable(filters, dynamicOrder);
            var pagedResponse = await _repository.GetPaginatedResultsAsync<IndiceMassaCorporal, IndiceMassaCorporalDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                specification
            );
            return pagedResponse;
        }


        // get single IndiceMassaCorporal by Id 
        public async Task<Response<IndiceMassaCorporalDTO>> GetIndiceMassaCorporalAsync(Guid id)
        {
            try
            {
                IndiceMassaCorporalDTO dto = await _repository.GetByIdAsync<IndiceMassaCorporal, IndiceMassaCorporalDTO, Guid>(id);
                return ResponseFactory.Success<IndiceMassaCorporalDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IndiceMassaCorporalDTO>(ex.Message);
            }
        }

        // create new IndiceMassaCorporal
        public async Task<Response<Guid>> CreateIndiceMassaCorporalAsync(CreateIndiceMassaCorporalRequest request)
        {
            var specification = new IndiceMassaCorporalMatchName(request.UtenteId, request.Data, request.Hora);
            bool indiceMassaCorporalExists = await _repository.ExistsAsync<IndiceMassaCorporal, Guid>(specification);
            if (indiceMassaCorporalExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um registo de IMC para o mesmo utente, data e hora.");
            }

            IndiceMassaCorporal newIndiceMassaCorporal = _mapper.Map(request, new IndiceMassaCorporal());

            try
            {
                IndiceMassaCorporal response = await _repository.CreateAsync<IndiceMassaCorporal, Guid>(newIndiceMassaCorporal);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update IndiceMassaCorporal
        public async Task<Response<Guid>> UpdateIndiceMassaCorporalAsync(UpdateIndiceMassaCorporalRequest request, Guid id)
        {
            IndiceMassaCorporal IndiceMassaCorporalInDb = await _repository.GetByIdAsync<IndiceMassaCorporal, Guid>(id); // get existing entity
            if (IndiceMassaCorporalInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            IndiceMassaCorporal updatedIndiceMassaCorporal = _mapper.Map(request, IndiceMassaCorporalInDb); // map dto to domain entity

            try
            {
                IndiceMassaCorporal response = await _repository.UpdateAsync<IndiceMassaCorporal, Guid>(updatedIndiceMassaCorporal);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete IndiceMassaCorporal
        public async Task<Response<Guid>> DeleteIndiceMassaCorporalAsync(Guid id)
        {
            try
            {
                IndiceMassaCorporal? indiceMassaCorporal = await _repository.RemoveByIdAsync<IndiceMassaCorporal, Guid>(id);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(indiceMassaCorporal.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

