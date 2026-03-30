using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.Filters;
using CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.Specifications;

// After creating this service:
// -- 1. Create a TensaoArterial domain entity in CliCloud.Domain/Entities/ProcessoClinico/SinaisVitais
// -- 2. Add DbSet<TensaoArterial> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a TensaoArterial api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.ProcessoClinico.TensaoArterialService
{
    public class TensaoArterialService : ITensaoArterialService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public TensaoArterialService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<TensaoArterialDTO>>> GetTensaoArterialAsync(string keyword = "")
        {
            var specification = new TensaoArterialSearchList(keyword);
            var list = await _repository.GetListAsync<TensaoArterial, TensaoArterialDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TensaoArterialDTO>>(list);
        }

        // get lightweight list
        public async Task<Response<IEnumerable<TensaoArterialLightDTO>>> GetTensaoArterialLightAsync(string keyword = "")
        {
            var specification = new TensaoArterialSearchList(keyword);
            var list = await _repository.GetListAsync<TensaoArterial, TensaoArterialLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TensaoArterialLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<TensaoArterialDTO>> GetTensaoArterialPaginatedAsync(TensaoArterialTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            List<TableFilter> filters = filter.Filters ?? new List<TableFilter>();
            var specification = new TensaoArterialSearchTable(filters, dynamicOrder);
            var pagedResponse = await _repository.GetPaginatedResultsAsync<TensaoArterial, TensaoArterialDTO, Guid>(
                filter.PageNumber,
                filter.PageSize,
                specification
            );
            return pagedResponse;
        }

        // get all (non-paginated)
        public async Task<Response<IEnumerable<TensaoArterialTableDTO>>> GetAllTensaoArterialAsync(TensaoArterialAllFilter filter)
        {
            try
            {
                filter ??= new TensaoArterialAllFilter();
                var dynamicOrder = filter.GetOrderByString();
                var filters = filter.Filters ?? new List<TableFilter>();
                var specification = new TensaoArterialSearchTable(filters, dynamicOrder);
                var list = await _repository.GetListAsync<TensaoArterial, TensaoArterialTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<TensaoArterialTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<TensaoArterialTableDTO>>(ex.Message);
            }
        }


        // get single TensaoArterial by Id 
        public async Task<Response<TensaoArterialDTO>> GetTensaoArterialAsync(Guid id)
        {
            try
            {
                TensaoArterialDTO dto = await _repository.GetByIdAsync<TensaoArterial, TensaoArterialDTO, Guid>(id);
                return ResponseFactory.Success<TensaoArterialDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<TensaoArterialDTO>(ex.Message);
            }
        }

        // create new TensaoArterial
        public async Task<Response<Guid>> CreateTensaoArterialAsync(CreateTensaoArterialRequest request)
        {
            var specification = new TensaoArterialMatchName(request.UtenteId, request.Data, request.Hora);
            bool tensaoArterialExists = await _repository.ExistsAsync<TensaoArterial, Guid>(specification);
            if (tensaoArterialExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um registo de tensão arterial para o mesmo utente, data e hora.");
            }

            TensaoArterial newTensaoArterial = _mapper.Map(request, new TensaoArterial());

            try
            {
                TensaoArterial response = await _repository.CreateAsync<TensaoArterial, Guid>(newTensaoArterial);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update TensaoArterial
        public async Task<Response<Guid>> UpdateTensaoArterialAsync(UpdateTensaoArterialRequest request, Guid id)
        {
            TensaoArterial TensaoArterialInDb = await _repository.GetByIdAsync<TensaoArterial, Guid>(id); // get existing entity
            if (TensaoArterialInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            TensaoArterial updatedTensaoArterial = _mapper.Map(request, TensaoArterialInDb); // map dto to domain entity

            try
            {
                TensaoArterial response = await _repository.UpdateAsync<TensaoArterial, Guid>(updatedTensaoArterial);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete TensaoArterial
        public async Task<Response<Guid>> DeleteTensaoArterialAsync(Guid id)
        {
            try
            {
                TensaoArterial? TensaoArterial = await _repository.RemoveByIdAsync<TensaoArterial, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(TensaoArterial.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTensaoArterialAsync(IEnumerable<Guid> ids)
        {
            try
            {
                var idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach (var id in idsList)
                {
                    try
                    {
                        var deletedEntity = await _repository.RemoveByIdAsync<TensaoArterial, Guid>(id);
                        if (deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                            failedDeletions.Add($"TensaoArterial com ID {id}.");
                    }
                    catch (Exception)
                    {
                        failedDeletions.Add($"TensaoArterial com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if (successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if (successfullyDeletedIds.Count > 0)
                {
                    string message = $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} tensões arteriais.";
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, message);
                }
                else
                {
                    return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

