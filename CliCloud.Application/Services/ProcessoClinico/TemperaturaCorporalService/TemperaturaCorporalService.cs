using AutoMapper;
using CliCloud.Application.Services.TemperaturaCorporalService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.TemperaturaCorporalService.Filters;
using CliCloud.Application.Services.TemperaturaCorporalService.Specifications;

// After creating this service:
// -- 1. Create a TemperaturaCorporal domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<TemperaturaCorporal> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a TemperaturaCorporal api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.TemperaturaCorporalService
{
    public class TemperaturaCorporalService : ITemperaturaCorporalService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public TemperaturaCorporalService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<TemperaturaCorporalDTO>>> GetTemperaturaCorporalAsync(string keyword = "")
        {
            var specification = new TemperaturaCorporalSearchList(keyword);
            IEnumerable<TemperaturaCorporalDTO> list = await _repository.GetListAsync<TemperaturaCorporal, TemperaturaCorporalDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TemperaturaCorporalDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<TemperaturaCorporalDTO>> GetTemperaturaCorporalPaginatedAsync(TemperaturaCorporalTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            List<TableFilter> filters = filter.Filters ?? new List<TableFilter>();
            var specification = new TemperaturaCorporalSearchTable(filters, dynamicOrder);
            var pagedResponse = await _repository.GetPaginatedResultsAsync<TemperaturaCorporal, TemperaturaCorporalDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                specification
            );
            return pagedResponse;
        }


        // get single TemperaturaCorporal by Id 
        public async Task<Response<TemperaturaCorporalDTO>> GetTemperaturaCorporalAsync(Guid id)
        {
            try
            {
                TemperaturaCorporalDTO dto = await _repository.GetByIdAsync<TemperaturaCorporal, TemperaturaCorporalDTO, Guid>(id);
                return ResponseFactory.Success<TemperaturaCorporalDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<TemperaturaCorporalDTO>(ex.Message);
            }
        }

        // create new TemperaturaCorporal
        public async Task<Response<Guid>> CreateTemperaturaCorporalAsync(CreateTemperaturaCorporalRequest request)
        {
            var specification = new TemperaturaCorporalMatchName(request.UtenteId, request.Data, request.Hora);
            bool temperaturaCorporalExists = await _repository.ExistsAsync<TemperaturaCorporal, Guid>(specification);
            if (temperaturaCorporalExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um registo de temperatura corporal para o mesmo utente, data e hora.");
            }

            TemperaturaCorporal newTemperaturaCorporal = _mapper.Map(request, new TemperaturaCorporal());

            try
            {
                TemperaturaCorporal response = await _repository.CreateAsync<TemperaturaCorporal, Guid>(newTemperaturaCorporal);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update TemperaturaCorporal
        public async Task<Response<Guid>> UpdateTemperaturaCorporalAsync(UpdateTemperaturaCorporalRequest request, Guid id)
        {
            TemperaturaCorporal TemperaturaCorporalInDb = await _repository.GetByIdAsync<TemperaturaCorporal, Guid>(id); // get existing entity
            if (TemperaturaCorporalInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            TemperaturaCorporal updatedTemperaturaCorporal = _mapper.Map(request, TemperaturaCorporalInDb); // map dto to domain entity

            try
            {
                TemperaturaCorporal response = await _repository.UpdateAsync<TemperaturaCorporal, Guid>(updatedTemperaturaCorporal);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete TemperaturaCorporal
        public async Task<Response<Guid>> DeleteTemperaturaCorporalAsync(Guid id)
        {
            try
            {
                TemperaturaCorporal? temperaturaCorporal = await _repository.RemoveByIdAsync<TemperaturaCorporal, Guid>(id);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(temperaturaCorporal.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
    }
}

