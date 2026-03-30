using AutoMapper;
using CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;
using CliCloud.Application.Utility;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.Filters;
using CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.Specifications;

// After creating this service:
// -- 1. Create a NotasBodyChart domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<NotasBodyChart> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a NotasBodyChart api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService
{
    public class NotasBodyChartService : INotasBodyChartService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public NotasBodyChartService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper; 
        }

        // get full List
        public async Task<Response<IEnumerable<NotasBodyChartDTO>>> GetNotasBodyChartAsync(string keyword = "")
        {
            NotasBodyChartSearchList specification = new(keyword); // ardalis specification
            IEnumerable<NotasBodyChartDTO> list = await _repository.GetListAsync<NotaBodyChart, NotasBodyChartDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<NotasBodyChartDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<NotasBodyChartLightDTO>>> GetNotasBodyChartLightAsync(string keyword = "")
        {
            NotasBodyChartSearchList specification = new(keyword);
            IEnumerable<NotasBodyChartLightDTO> list = await _repository.GetListAsync<NotaBodyChart, NotasBodyChartLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<NotasBodyChartLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<NotasBodyChartTableDTO>> GetNotasBodyChartPaginatedAsync(NotasBodyChartTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            NotasBodyChartSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<NotasBodyChartTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<NotaBodyChart, NotasBodyChartTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all list
        public async Task<Response<IEnumerable<NotasBodyChartTableDTO>>> GetAllNotasBodyChartAsync(NotasBodyChartAllFilter filter)
        {
            try
            {
                filter ??= new NotasBodyChartAllFilter();
                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                NotasBodyChartSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<NotasBodyChartTableDTO> list = await _repository.GetListAsync<NotaBodyChart, NotasBodyChartTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<NotasBodyChartTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<NotasBodyChartTableDTO>>(ex.Message);
            }
        }


        // get single NotasBodyChart by Id 
        public async Task<Response<NotasBodyChartDTO>> GetNotasBodyChartAsync(Guid id)
        {
            try
            {
                NotasBodyChartDTO dto = await _repository.GetByIdAsync<NotaBodyChart, NotasBodyChartDTO, Guid>(id);
                return ResponseFactory.Success<NotasBodyChartDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<NotasBodyChartDTO>(ex.Message);
            }
        }

        // get single NotasBodyChart by Nome 
        public async Task<Response<NotasBodyChartDTO>> GetNotasBodyChartByNomeAsync(string nome)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(nome))
                {
                    return ResponseFactory.Fail<NotasBodyChartDTO>("Nome não pode ser vazio");
                }

                NotasBodyChartMatchName specification = new(nome);
                IEnumerable<NotasBodyChartDTO> results = await _repository.GetListAsync<NotaBodyChart, NotasBodyChartDTO, Guid>(specification);
                NotasBodyChartDTO? NotasBodyChart = results.FirstOrDefault();

                if(NotasBodyChart == null)
                {
                    return ResponseFactory.Fail<NotasBodyChartDTO>("Não foi encontrado nenhum NotasBodyChart com o Nome fornecido");
                }

                return ResponseFactory.Success<NotasBodyChartDTO>(NotasBodyChart);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<NotasBodyChartDTO>(ex.Message);
            }
        }

        // create new NotasBodyChart
        public async Task<Response<Guid>> CreateNotasBodyChartAsync(CreateNotasBodyChartRequest request)
        {
            NotasBodyChartMatchName specification = new(request.Nome); // ardalis specification 
            bool NotasBodyChartExists = await _repository.ExistsAsync<NotaBodyChart, Guid>(specification);
            if (NotasBodyChartExists)
            {
                return ResponseFactory.Fail<Guid>("NotasBodyChart já existe");
            }

            NotaBodyChart newNotasBodyChart = _mapper.Map(request, new NotaBodyChart()); // map dto to domain entity

            try
            {
                NotaBodyChart response = await _repository.CreateAsync<NotaBodyChart, Guid>(newNotasBodyChart); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update NotasBodyChart
        public async Task<Response<Guid>> UpdateNotasBodyChartAsync(UpdateNotasBodyChartRequest request, Guid id)
        {
            NotaBodyChart NotasBodyChartInDb = await _repository.GetByIdAsync<NotaBodyChart, Guid>(id); // get existing entity
            if (NotasBodyChartInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Not Found");
            }

            NotaBodyChart updatedNotasBodyChart = _mapper.Map(request, NotasBodyChartInDb); // map dto to domain entity

            try
            {
                NotaBodyChart response = await _repository.UpdateAsync<NotaBodyChart, Guid>(updatedNotasBodyChart);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete NotasBodyChart
        public async Task<Response<Guid>> DeleteNotasBodyChartAsync(Guid id)
        {
            try
            {
                NotaBodyChart? NotasBodyChart = await _repository.RemoveByIdAsync<NotaBodyChart, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(NotasBodyChart.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple NotasBodyChart 
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleNotasBodyChartAsync(IEnumerable<Guid> ids)
        {
            try
            {
                List<Guid> idsList = ids.ToList();
                List<Guid> successfullyDeletedIds = [];
                List<string> failedDeletions = [];

                foreach(Guid id in idsList)
                {
                    try
                    {
                        NotaBodyChart? entity = await _repository.GetByIdAsync<NotaBodyChart, Guid>(id);
                        if(entity == null)
                        {
                            failedDeletions.Add($"Notas Body Chart com ID {id} não encontradas");
                            continue;
                        }

                        NotaBodyChart? deletedEntity = await _repository.RemoveByIdAsync<NotaBodyChart, Guid>(id);
                        if(deletedEntity != null)
                        {
                            _ = await _repository.SaveChangesAsync();
                            successfullyDeletedIds.Add(id);
                        }
                        else
                        {
                            failedDeletions.Add($"Falha ao eliminar Notas Body Chart com ID {id}.");
                        }
                    }
                    catch
                    {
                        failedDeletions.Add($"Falha ao eliminar Notas Body Chart com ID {id}.");
                        _repository.ClearChangeTracker();
                    }
                }

                if(successfullyDeletedIds.Count == idsList.Count)
                {
                    return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                if(successfullyDeletedIds.Count > 0)
                {
                    return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucessso {successfullyDeletedIds.Count} de {idsList.Count} Notas Body Charts.");
                }

                return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

