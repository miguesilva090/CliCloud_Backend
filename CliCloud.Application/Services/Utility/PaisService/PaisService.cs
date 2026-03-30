using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.PaisService.DTOs;
using CliCloud.Application.Services.Utility.PaisService.Filters;
using CliCloud.Application.Services.Utility.PaisService.Specifications;
using Microsoft.EntityFrameworkCore;

// After creating this service:
// -- 1. Create a Pais domain entity in CliCloud.Domain/Entities/Utility
// -- 2. Add DbSet<Pais> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a Pais api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Utility.PaisService
{
    public class PaisService(IRepositoryAsync repository, IMapper mapper) : IPaisService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper; 

        // get full List
        public async Task<Response<IEnumerable<PaisDTO>>> GetPaisAsync(string keyword = "")
        {
            PaisSearchList specification = new(keyword); // ardalis specification
            IEnumerable<PaisDTO> list = await _repository.GetListAsync<Pais, PaisDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<PaisDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<PaisLightDTO>>> GetPaisLightAsync(string keyword = "")
        {
          PaisSearchList specification = new(keyword);
          IEnumerable<PaisLightDTO> list = await _repository.GetListAsync<Pais, PaisLightDTO, Guid>(specification);
          return ResponseFactory.Success<IEnumerable<PaisLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<PaisTableDTO>> GetPaisPaginatedAsync(PaisTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            PaisSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<PaisTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Pais, PaisTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        //get all Paises (non-paginated)
        public async Task<Response<IEnumerable<PaisTableDTO>>> GetAllPaisAsync(PaisAllFilter filter)
        {
          try
          {
            filter ??= new PaisAllFilter();

            string dynamicOrder =filter.GetOrderByString();
            List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
            PaisSearchTable specification = new(tableFilters, dynamicOrder);
            IEnumerable<PaisTableDTO> list = await _repository.GetListAsync<Pais, PaisTableDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<PaisTableDTO>>(list);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<PaisTableDTO>>(ex.Message);
          }
        }


        // get single Pais by Id 
        public async Task<Response<PaisDTO>> GetPaisAsync(Guid id)
        {
            try
            {
                PaisDTO dto = await _repository.GetByIdAsync<Pais, PaisDTO, Guid>(id);
                return ResponseFactory.Success<PaisDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<PaisDTO>(ex.Message);
            }
        }

        // create new Pais
        public async Task<Response<Guid>> CreatePaisAsync(CreatePaisRequest request)
        {
            PaisMatchName specification = new(request.Nome); // ardalis specification 
            bool PaisExists = await _repository.ExistsAsync<Pais, Guid>(specification);
            if (PaisExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um país com o nome fornecido.");
            }

            Pais newPais = _mapper.Map(request, new Pais()); // map dto to domain entity

            try
            {
                Pais response = await _repository.CreateAsync<Pais, Guid>(newPais); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Pais
        public async Task<Response<Guid>> UpdatePaisAsync(UpdatePaisRequest request, Guid id)
        {
            Pais PaisInDb = await _repository.GetByIdAsync<Pais, Guid>(id); // get existing entity
            if (PaisInDb == null)
            {
                return ResponseFactory.Fail<Guid>("País não encontrado.");
            }

            Pais updatedPais = _mapper.Map(request, PaisInDb); // map dto to domain entity

            try
            {
                Pais response = await _repository.UpdateAsync<Pais, Guid>(updatedPais);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            } 
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Pais
        public async Task<Response<Guid>> DeletePaisAsync(Guid id)
        {
            try
            {
                Pais? Pais = await _repository.RemoveByIdAsync<Pais, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Pais.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultiplePaisAsync(IEnumerable<Guid> ids)
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
                Pais? entity = await _repository.GetByIdAsync<Pais, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"País com ID {id}.");
                  continue;
                }

                Pais? deletedEntity = await _repository.RemoveByIdAsync<Pais, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"País com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"País com ID {id}.");

                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} países.";
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

