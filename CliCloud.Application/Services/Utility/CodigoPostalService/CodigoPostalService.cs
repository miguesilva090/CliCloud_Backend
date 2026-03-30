using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.CodigoPostalService.DTOs;
using CliCloud.Application.Services.Utility.CodigoPostalService.Filters;
using CliCloud.Application.Services.Utility.CodigoPostalService.Specifications;
using Microsoft.EntityFrameworkCore;

// After creating this service:
// -- 1. Create a CodigoPostal domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<CodigoPostal> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a CodigoPostal api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Utility.CodigoPostalService
{
    public class CodigoPostalService(IRepositoryAsync repository, IMapper mapper) : ICodigoPostalService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper; 

        // get full List
        public async Task<Response<IEnumerable<CodigoPostalDTO>>> GetCodigoPostalAsync(string keyword = "")
        {
            CodigoPostalSearchList specification = new(keyword); // ardalis specification
            IEnumerable<CodigoPostalDTO> list = await _repository.GetListAsync<CodigoPostal, CodigoPostalDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<CodigoPostalDTO>>(list);
        }

        //get lightweight list
        public async Task<Response<IEnumerable<CodigoPostalLightDTO>>> GetCodigoPostalLightAsync(string keyword = "")
        {
            CodigoPostalSearchList specification = new(keyword); // ardalis specification
            IEnumerable<CodigoPostalLightDTO> list = await _repository.GetListAsync<CodigoPostal, CodigoPostalLightDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<CodigoPostalLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<CodigoPostalTableDTO>> GetCodigoPostalPaginatedAsync(CodigoPostalTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            CodigoPostalSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<CodigoPostalTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<CodigoPostal, CodigoPostalTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        //get all CodigoPostals (non-paginated)
        public async Task<Response<IEnumerable<CodigoPostalTableDTO>>> GetAllCodigoPostalAsync(CodigoPostalAllFilter filter)
        {
            try
            {
                filter ??= new CodigoPostalAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                CodigoPostalSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<CodigoPostalTableDTO> list = await _repository.GetListAsync<CodigoPostal, CodigoPostalTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<CodigoPostalTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<CodigoPostalTableDTO>>(ex.Message);
            }
        }


        // get single CodigoPostal by Id 
        public async Task<Response<CodigoPostalDTO>> GetCodigoPostalAsync(Guid id)
        {
            try
            {
                CodigoPostalDTO dto = await _repository.GetByIdAsync<CodigoPostal, CodigoPostalDTO, Guid>(id);
                return ResponseFactory.Success<CodigoPostalDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<CodigoPostalDTO>(ex.Message);
            }
        }

        // create new CodigoPostal
        public async Task<Response<Guid>> CreateCodigoPostalAsync(CreateCodigoPostalRequest request)
        {
            CodigoPostalMatchCodigo specification = new(request.Codigo); // ardalis specification 
            bool CodigoPostalExists = await _repository.ExistsAsync<CodigoPostal, Guid>(specification);
            if (CodigoPostalExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um código postal com o código fornecido.");
            }

            CodigoPostal newCodigoPostal = _mapper.Map(request, new CodigoPostal()); // map dto to domain entity

            try
            {
                CodigoPostal response = await _repository.CreateAsync<CodigoPostal, Guid>(newCodigoPostal); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update CodigoPostal
        public async Task<Response<Guid>> UpdateCodigoPostalAsync(UpdateCodigoPostalRequest request, Guid id)
        {
            CodigoPostal CodigoPostalInDb = await _repository.GetByIdAsync<CodigoPostal, Guid>(id); // get existing entity
            if (CodigoPostalInDb == null)
            {
                return ResponseFactory.Fail<Guid>("O codigo postal não foi encontrado.");
            }

            CodigoPostal updatedCodigoPostal = _mapper.Map(request, CodigoPostalInDb); // map dto to domain entity

            try
            {
                CodigoPostal response = await _repository.UpdateAsync<CodigoPostal, Guid>(updatedCodigoPostal);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete CodigoPostal
        public async Task<Response<Guid>> DeleteCodigoPostalAsync(Guid id)
        {
            try
            {
                CodigoPostal? CodigoPostal = await _repository.RemoveByIdAsync<CodigoPostal, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(CodigoPostal.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleCodigoPostalAsync(IEnumerable<Guid> ids)
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
                CodigoPostal? entity = await _repository.GetByIdAsync<CodigoPostal, Guid>(id);
                    if(entity == null)
                {
                  failedDeletions.Add($"Codigo postal com ID {id}.");
                  continue;
                }

                CodigoPostal? deletedEntity = await _repository.RemoveByIdAsync<CodigoPostal, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"Codigo postal com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Codigo postal com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} códigos postais.";
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

