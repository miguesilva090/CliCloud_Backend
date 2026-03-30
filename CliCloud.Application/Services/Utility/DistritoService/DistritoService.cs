using AutoMapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.DistritoService.DTOs;
using CliCloud.Application.Services.Utility.DistritoService.Filters;
using CliCloud.Application.Services.Utility.DistritoService.Specifications;
using Microsoft.EntityFrameworkCore;

// After creating this service:
// -- 1. Create a Distrito domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<Distrito> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a Distrito api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Utility.DistritoService
{
    public class DistritoService(IRepositoryAsync repository, IMapper mapper) : IDistritoService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper; 

        // get full List
        public async Task<Response<IEnumerable<DistritoDTO>>> GetDistritoAsync(string keyword = "")
        {
            DistritoSearchList specification = new(keyword); // ardalis specification
            IEnumerable<DistritoDTO> list = await _repository.GetListAsync<Distrito, DistritoDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<DistritoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<DistritoLightDTO>>> GetDistritoLightAsync(string keyword = "", Guid? paisId = null)
        {
            DistritoSearchList specification = new(keyword, paisId); // ardalis specification
            IEnumerable<DistritoLightDTO> list = await _repository.GetListAsync<Distrito, DistritoLightDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<DistritoLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<DistritoTableDTO>> GetDistritoPaginatedAsync(DistritoTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            DistritoSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<DistritoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Distrito, DistritoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all Distritos (non-paginated)
        public async Task<Response<IEnumerable<DistritoTableDTO>>> GetAllDistritoAsync(DistritoAllFilter filter)
        {
            try
            {
                filter ??= new DistritoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                DistritoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<DistritoTableDTO> list = await _repository.GetListAsync<Distrito, DistritoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<DistritoTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<DistritoTableDTO>>(ex.Message);
            }
        }


        // get single Distrito by Id 
        public async Task<Response<DistritoDTO>> GetDistritoAsync(Guid id)
        {
            try
            {
                DistritoDTO dto = await _repository.GetByIdAsync<Distrito, DistritoDTO, Guid>(id);
                return ResponseFactory.Success<DistritoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<DistritoDTO>(ex.Message);
            }
        }

        // create new Distrito
        public async Task<Response<Guid>> CreateDistritoAsync(CreateDistritoRequest request)
        {
            DistritoMatchName specification = new(request.Nome); // ardalis specification 
            bool DistritoExists = await _repository.ExistsAsync<Distrito, Guid>(specification);
            if (DistritoExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um distrito com o nome fornecido.");
            }

            Distrito newDistrito = _mapper.Map(request, new Distrito()); // map dto to domain entity

            try
            {
                Distrito response = await _repository.CreateAsync<Distrito, Guid>(newDistrito); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Distrito
        public async Task<Response<Guid>> UpdateDistritoAsync(UpdateDistritoRequest request, Guid id)
        {
            Distrito DistritoInDb = await _repository.GetByIdAsync<Distrito, Guid>(id); // get existing entity
            if (DistritoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Distrito não encontrado.");
            }

            Distrito updatedDistrito = _mapper.Map(request, DistritoInDb); // map dto to domain entity

            try
            {
                Distrito response = await _repository.UpdateAsync<Distrito, Guid>(updatedDistrito);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Distrito
        public async Task<Response<Guid>> DeleteDistritoAsync(Guid id)
        {
            try
            {
                Distrito? Distrito = await _repository.RemoveByIdAsync<Distrito, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Distrito.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Distritos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleDistritoAsync(IEnumerable<Guid> ids)
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
                    Distrito? entity = await _repository.GetByIdAsync<Distrito, Guid>(id);
                    if(entity == null)
                    {
                      failedDeletions.Add($"Distrito com ID {id}.");
                      continue;
                    }

                    Distrito? deletedEntity = await _repository.RemoveByIdAsync<Distrito, Guid>(id);
                    if(deletedEntity != null)
                    {
                      _ = await _repository.SaveChangesAsync();
                      successfullyDeletedIds.Add(id);
                    }
                    else
                    {
                      failedDeletions.Add($"Distrito com ID {id}.");
                    }
                  }
                  catch(Exception)
                  {
                    failedDeletions.Add($"Distrito com ID {id}.");
                    _repository.ClearChangeTracker();
                  }
                }

                if(successfullyDeletedIds.Count == idsList.Count)
                {
                  return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if(successfullyDeletedIds.Count > 0)
                {
                  string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} distritos.";
                  return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, message);
                }
                else
                {
                  return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ",failedDeletions));
                }
            }
            catch(Exception ex)
            {
              return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
            }
        }
    }
}

