using AutoMapper;
using CliCloud.Application.Services.Utility.ConcelhoService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.ConcelhoService.Filters;
using CliCloud.Application.Services.Utility.ConcelhoService.Specifications;
using Microsoft.EntityFrameworkCore;

// After creating this service:
// -- 1. Create a Concelho domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<Concelho> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a Concelho api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Utility.ConcelhoService
{
    public class ConcelhoService(IRepositoryAsync repository, IMapper mapper) : IConcelhoService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper; 

        // get full List
        public async Task<Response<IEnumerable<ConcelhoDTO>>> GetConcelhoAsync(string keyword = "")
        {
            ConcelhoSearchList specification = new(keyword); // ardalis specification
            IEnumerable<ConcelhoDTO> list = await _repository.GetListAsync<Concelho, ConcelhoDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<ConcelhoDTO>>(list);
        }

        // get lightweight List
        public async Task<Response<IEnumerable<ConcelhoLightDTO>>> GetConcelhoLightAsync(string keyword = "")
        {
            ConcelhoSearchList specification = new(keyword); // ardalis specification
            IEnumerable<ConcelhoLightDTO> list = await _repository.GetListAsync<Concelho, ConcelhoLightDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<ConcelhoLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<ConcelhoTableDTO>> GetConcelhoPaginatedAsync(ConcelhoTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            ConcelhoSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<ConcelhoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Concelho, ConcelhoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        //Get all Concelhos (non-paginated)
        public async Task<Response<IEnumerable<ConcelhoTableDTO>>> GetAllConcelhoAsync(ConcelhoAllFilter filter)
        {
            try
            {
                filter ??= new ConcelhoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                ConcelhoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<ConcelhoTableDTO> list = await _repository.GetListAsync<Concelho, ConcelhoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<ConcelhoTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<ConcelhoTableDTO>>(ex.Message);
            }
        }


        // get single Concelho by Id 
        public async Task<Response<ConcelhoDTO>> GetConcelhoAsync(Guid id)
        {
            try
            {
                ConcelhoDTO dto = await _repository.GetByIdAsync<Concelho, ConcelhoDTO, Guid>(id);
                return ResponseFactory.Success<ConcelhoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<ConcelhoDTO>(ex.Message);
            }
        }

        // create new Concelho
        public async Task<Response<Guid>> CreateConcelhoAsync(CreateConcelhoRequest request)
        {
            try
            {
              _ = await _repository.GetByIdAsync<Distrito, Guid>(Guid.Parse(request.DistritoId));
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }

            ConcelhoMatchName specification = new(request.Nome); // ardalis specification 
            bool ConcelhoExists = await _repository.ExistsAsync<Concelho, Guid>(specification);
            if (ConcelhoExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um concelho com o nome fornecido.");
            }

            Concelho newConcelho = _mapper.Map(request, new Concelho()); // map dto to domain entity

            try
            {
                Concelho response = await _repository.CreateAsync<Concelho, Guid>(newConcelho); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Concelho
        public async Task<Response<Guid>> UpdateConcelhoAsync(UpdateConcelhoRequest request, Guid id)
        {
            Concelho ConcelhoInDb = await _repository.GetByIdAsync<Concelho, Guid>(id); // get existing entity
            if (ConcelhoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Concelho não encontrado.");
            }

            Concelho updatedConcelho = _mapper.Map(request, ConcelhoInDb); // map dto to domain entity

            try
            {
                Concelho response = await _repository.UpdateAsync<Concelho, Guid>(updatedConcelho);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Concelho
        public async Task<Response<Guid>> DeleteConcelhoAsync(Guid id)
        {
            try
            {
                Concelho? Concelho = await _repository.RemoveByIdAsync<Concelho, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Concelho.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }
        
        //delete multiple Concelhos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleConcelhoAsync(IEnumerable<Guid> ids)
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
                    Concelho? entity = await _repository.GetByIdAsync<Concelho, Guid>(id);
                    if(entity == null)
                    {
                      failedDeletions.Add($"Concelho com ID {id}.");
                      continue;
                    }

                    Concelho? deletedEntity = await _repository.RemoveByIdAsync<Concelho, Guid>(id);
                    if(deletedEntity != null)
                    {
                      _ = await _repository.SaveChangesAsync();
                      successfullyDeletedIds.Add(id);
                    }
                    else
                    {
                      failedDeletions.Add($"Concelho com ID {id}.");
                    }
                  }
                  catch(Exception)
                  {
                    failedDeletions.Add($"Concelho com ID {id}.");
                    _repository.ClearChangeTracker();
                  }
                }
                if(successfullyDeletedIds.Count == idsList.Count)
                {
                  return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if(successfullyDeletedIds.Count > 0)
                {
                  string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} concelhos.";
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

