using AutoMapper;
using CliCloud.Application.Services.Utility.RuaService.DTOs;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.RuaService.Filters;
using CliCloud.Application.Services.Utility.RuaService.Specifications;
using Microsoft.EntityFrameworkCore;

// After creating this service:
// -- 1. Create a Rua domain entity in CliCloud.Domain/Entities/Catalog
// -- 2. Add DbSet<Rua> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a Rua api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Utility.RuaService
{
    public class RuaService(IRepositoryAsync repository, IMapper mapper) : IRuaService
    {
        private readonly IRepositoryAsync _repository = repository;
        private readonly IMapper _mapper = mapper; 

        // get full List
        public async Task<Response<IEnumerable<RuaDTO>>> GetRuaAsync(string keyword = "")
        {
            RuaSearchList specification = new(keyword); // ardalis specification
            IEnumerable<RuaDTO> list = await _repository.GetListAsync<Rua, RuaDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<RuaDTO>>(list);
        }

        //get lightweight list
        public async Task<Response<IEnumerable<RuaLightDTO>>> GetRuaLightAsync(string keyword = "")
        {
            RuaSearchList specification = new(keyword);
            IEnumerable<RuaLightDTO> list = await _repository.GetListAsync<Rua, RuaLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<RuaLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<RuaTableDTO>> GetRuaPaginatedAsync(RuaTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            RuaSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<RuaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Rua, RuaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        //get all Ruas (non-paginated)
        public async Task<Response<IEnumerable<RuaTableDTO>>> GetAllRuaAsync(RuaAllFilter filter)
        {
            try
            {
                filter ??= new RuaAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                RuaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<RuaTableDTO> list = await _repository.GetListAsync<Rua, RuaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<RuaTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<RuaTableDTO>>(ex.Message);
            }
        }


        // get single Rua by Id 
        public async Task<Response<RuaDTO>> GetRuaAsync(Guid id)
        {
            try
            {
                RuaDTO dto = await _repository.GetByIdAsync<Rua, RuaDTO, Guid>(id);
                return ResponseFactory.Success<RuaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<RuaDTO>(ex.Message);
            }
        }

        // create new Rua
        public async Task<Response<Guid>> CreateRuaAsync(CreateRuaRequest request)
        {

          try
          {
            _ = await _repository.GetByIdAsync<Freguesia, Guid>(Guid.Parse(request.FreguesiaId));
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<Guid>(ex.Message);
          }

          try
          {
            _ = await _repository.GetByIdAsync<CodigoPostal, Guid>(Guid.Parse(request.CodigoPostalId));
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<Guid>(ex.Message);
          }

          RuaMatchNameAndLocation specification = new(request.Nome, Guid.Parse(request.FreguesiaId), Guid.Parse(request.CodigoPostalId));
          var existingList = await _repository.GetListAsync<Rua, Guid>(specification);
          var existingRua = existingList.FirstOrDefault();
          if (existingRua != null)
          {
            return ResponseFactory.Success<Guid>(existingRua.Id);
          }

          Rua newRua = _mapper.Map(request, new Rua());

          try
          {
              Rua response = await _repository.CreateAsync<Rua, Guid>(newRua); // create new entity 
              _ = await _repository.SaveChangesAsync(); // save changes to db
              return ResponseFactory.Success<Guid>(response.Id); // return id
          }
          catch (Exception ex)
          {
              return ResponseFactory.Fail<Guid>(ex.Message);
          }
        }

        // update Rua
        public async Task<Response<Guid>> UpdateRuaAsync(UpdateRuaRequest request, Guid id)
        {
            Rua RuaInDb = await _repository.GetByIdAsync<Rua, Guid>(id); // get existing entity
            if (RuaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Rua não encontrada");
            }

            try
            {
              _ = await _repository.GetByIdAsync<Freguesia, Guid>(Guid.Parse(request.FreguesiaId));
            }
            catch(Exception ex)
            {
              return ResponseFactory.Fail<Guid>(ex.Message);
            }

            try
            {
              _ = await _repository.GetByIdAsync<CodigoPostal, Guid>(Guid.Parse(request.CodigoPostalId));
            }
            catch(Exception ex)
            {
              return ResponseFactory.Fail<Guid>(ex.Message);
            }

            RuaMatchNameAndLocation specification = new(request.Nome, Guid.Parse(request.FreguesiaId), Guid.Parse(request.CodigoPostalId));
            bool RuaExists = await _repository.ExistsAsync<Rua, Guid>(specification);
            if (RuaExists && RuaInDb.Nome != request.Nome)
            {
              return ResponseFactory.Fail<Guid>("Já existe uma rua com o nome fornecido na mesma freguesia e código postal");
            }

            Rua updatedRua = _mapper.Map(request, RuaInDb); // map dto to domain entity

            try
            {
                Rua response = await _repository.UpdateAsync<Rua, Guid>(updatedRua);  // update entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Rua
        public async Task<Response<Guid>> DeleteRuaAsync(Guid id)
        {
            try
            {
                Rua? Rua = await _repository.RemoveByIdAsync<Rua, Guid>(id);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Rua.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Ruas 
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleRuasAsync( IEnumerable<Guid> ids)
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
               
               Rua? entity = await _repository.GetByIdAsync<Rua, Guid>(id);
               if(entity == null)
               {
                failedDeletions.Add($"Rua com ID {id}");
                continue;
               }

               Rua? deletedEntity = await _repository.RemoveByIdAsync<Rua, Guid>(id);
               if(deletedEntity != null)
               {
                _ = await _repository.SaveChangesAsync();
                successfullyDeletedIds.Add(id);
               }
               else
               {
                failedDeletions.Add($"Rua com ID {id}");
               }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Rua com ID {id}");

                _repository.ClearChangeTracker();
              }
            }

          if(successfullyDeletedIds.Count == idsList.Count)
          {
            return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
          }
          else if( successfullyDeletedIds.Count > 0)
          {
            string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} ruas.";
            return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, message);
          }
          else 
          {
            return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
          }
        }
        catch(Exception ex)
        {
          return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
        }
      }
    }
}

