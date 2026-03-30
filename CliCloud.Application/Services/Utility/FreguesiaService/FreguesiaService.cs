using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.FreguesiaService.DTOs;
using CliCloud.Application.Services.Utility.FreguesiaService.Filters;
using CliCloud.Application.Services.Utility.FreguesiaService.Specifications;
using Microsoft.EntityFrameworkCore;

// After creating this service:
// -- 1. Create a Freguesia domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<Freguesia> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a Freguesia api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Utility.FreguesiaService
{
    public class FreguesiaService(IRepositoryAsync repository, IMapper mapper) : IFreguesiaService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper; 

        // get full List
        public async Task<Response<IEnumerable<FreguesiaDTO>>> GetFreguesiaAsync(string keyword = "")
        {
            FreguesiaSearchList specification = new(keyword); // ardalis specification
            IEnumerable<FreguesiaDTO> list = await _repository.GetListAsync<Freguesia, FreguesiaDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<FreguesiaDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<FreguesiaLightDTO>>> GetFreguesiaLightAsync(string keyword = "")
        {
            FreguesiaSearchList specification = new(keyword);
            IEnumerable<FreguesiaLightDTO> list = await _repository.GetListAsync<Freguesia, FreguesiaLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<FreguesiaLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<FreguesiaTableDTO>> GetFreguesiaPaginatedAsync(FreguesiaTableFilter filter)
        {

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            FreguesiaSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<FreguesiaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Freguesia, FreguesiaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        //get all Freguesias (non-paginated)
        public async Task<Response<IEnumerable<FreguesiaTableDTO>>> GetAllFreguesiaAsync(FreguesiaAllFilter filter)
        {
            try
            {
                filter ??= new FreguesiaAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                FreguesiaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<FreguesiaTableDTO> list = await _repository.GetListAsync<Freguesia, FreguesiaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<FreguesiaTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<FreguesiaTableDTO>>(ex.Message);
            }
        }


        // get single Freguesia by Id 
        public async Task<Response<FreguesiaDTO>> GetFreguesiaAsync(Guid id)
        {
            try
            {
                FreguesiaDTO dto = await _repository.GetByIdAsync<Freguesia, FreguesiaDTO, Guid>(id);
                return ResponseFactory.Success<FreguesiaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FreguesiaDTO>(ex.Message);
            }
        }

        // create new Freguesia
        public async Task<Response<Guid>> CreateFreguesiaAsync(CreateFreguesiaRequest request)
        {
            FreguesiaMatchName specification = new(request.Nome); // ardalis specification 
            bool FreguesiaExists = await _repository.ExistsAsync<Freguesia, Guid>(specification);
            if (FreguesiaExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe uma freguesia com o nome fornecido.");
            }

            Freguesia newFreguesia = _mapper.Map(request, new Freguesia()); // map dto to domain entity

            try
            {
                Freguesia response = await _repository.CreateAsync<Freguesia, Guid>(newFreguesia); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Freguesia
        public async Task<Response<Guid>> UpdateFreguesiaAsync(UpdateFreguesiaRequest request, Guid id)
        {
            Freguesia FreguesiaInDb = await _repository.GetByIdAsync<Freguesia, Guid>(id); // get existing entity
            if (FreguesiaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Freguesia não encontrada.");
            }

            Freguesia updatedFreguesia = _mapper.Map(request, FreguesiaInDb); // map dto to domain entity

            try
            {
                Freguesia response = await _repository.UpdateAsync<Freguesia, Guid>(updatedFreguesia);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Freguesia
        public async Task<Response<Guid>> DeleteFreguesiaAsync(Guid id)
        {
            try
            {
                Freguesia? Freguesia = await _repository.RemoveByIdAsync<Freguesia, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Freguesia.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Freguesias
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleFreguesiaAsync(IEnumerable<Guid> ids)
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
                    Freguesia? entity = await _repository.GetByIdAsync<Freguesia, Guid>(id);
                    if(entity == null)
                    {
                      failedDeletions.Add($"Freguesia com ID {id}.");
                      continue;
                    }

                    Freguesia? deletedEntity = await _repository.RemoveByIdAsync<Freguesia, Guid>(id);
                    if(deletedEntity != null)
                    {
                      _ = await _repository.SaveChangesAsync();
                      successfullyDeletedIds.Add(id);
                    }
                    else
                    {
                      failedDeletions.Add($"Freguesia com ID {id}.");
                    }
                  }
                  catch(Exception)
                  {
                    failedDeletions.Add($"Freguesia com ID {id}.");
                    _repository.ClearChangeTracker();
                  }
                }

                if(successfullyDeletedIds.Count == idsList.Count)
                {
                  return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
                }
                else if(successfullyDeletedIds.Count > 0)
                {
                  string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} freguesias.";
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

