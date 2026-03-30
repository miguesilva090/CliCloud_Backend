using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.EntidadeService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeService.Filters;
using CliCloud.Application.Services.Utility.EntidadeService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;
using Microsoft.EntityFrameworkCore;

// After creating this service:
// -- 1. Create a Entidade domain entity in CliCloud.Domain/Entities/Utility
// -- 2. Add DbSet<Entidade> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a Entidade api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Utility.EntidadeService
{
    public class EntidadeService : IEntidadeService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper; 
        private readonly IEntidadeContactoService _entidadeContactoService;

        public EntidadeService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<EntidadeDTO>>> GetEntidadeAsync(string keyword = "")
        {
            EntidadeSearchList specification = new(keyword); // ardalis specification
            IEnumerable<EntidadeDTO> list = await _repository.GetListAsync<Entidade, EntidadeDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<EntidadeDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<EntidadeLightDTO>>> GetEntidadeLightAsync(string keyword = "")
        {
            EntidadeSearchList specification = new(keyword); // ardalis specification
            IEnumerable<EntidadeLightDTO> list = await _repository.GetListAsync<Entidade, EntidadeLightDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<EntidadeLightDTO>>(list);
        }


        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<EntidadeTableDTO>> GetEntidadePaginatedAsync(EntidadeTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            EntidadeSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<EntidadeTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Entidade, EntidadeTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all Entidades (non-paginated)
        public async Task<Response<IEnumerable<EntidadeTableDTO>>> GetAllEntidadeAsync(EntidadeAllFilter filter)
        {
            try
            {
                filter ??= new EntidadeAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                EntidadeSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<EntidadeTableDTO> list = await _repository.GetListAsync<Entidade, EntidadeTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<EntidadeTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<EntidadeTableDTO>>(ex.Message);
            }
        }


        // get single Entidade by Id 
        public async Task<Response<EntidadeDTO>> GetEntidadeAsync(Guid id)
        {
            try
            {
                EntidadeDTO dto = await _repository.GetByIdAsync<Entidade, EntidadeDTO, Guid>(id);
                return ResponseFactory.Success<EntidadeDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<EntidadeDTO>(ex.Message);
            }
        }

        // get single Entidade by NContrib
        public async Task<Response<EntidadeDTO>> GetEntidadeByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<EntidadeDTO>("Numero de Contribuinte não pode ser vazio");
                }

                EntidadeMatchNContrib specification = new(ncontrib);
                IEnumerable<EntidadeDTO> results = await _repository.GetListAsync<Entidade, EntidadeDTO, Guid>(specification);

                EntidadeDTO? entidade = results.FirstOrDefault();
                if(entidade == null)
                {
                  return ResponseFactory.Fail<EntidadeDTO>("Não foi encontrada nenhuma Entidade com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<EntidadeDTO>(entidade);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<EntidadeDTO>(ex.Message);
            }
        }

        // get multiple Entidades by Nome 
        public async Task<Response<IEnumerable<EntidadeDTO>>> GetEntidadeByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<EntidadeDTO>>("Nome não pode ser vazio");
            }

            EntidadeSearchByName specification = new(nome);
            IEnumerable<EntidadeDTO> results = await _repository.GetListAsync<Entidade, EntidadeDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<EntidadeDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<EntidadeDTO>>(ex.Message);
          }
        }

        //Helper method to convert full URL to partial URL (for storage)
        private static string? ConvertToPartialUrl(string? imageUrl)
        {
          if(string.IsNullOrWhiteSpace(imageUrl))
          {
            return null;
          }

          if(imageUrl.StartsWith('/'))
          {
            return imageUrl;
          }

          if(imageUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase) || imageUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
          {
            try
            {
              Uri uri = new(imageUrl);
              return uri.AbsolutePath;
            }
            catch(Exception)
            {
              return null;
            }
          }

          return imageUrl;
        }

        // create new Entidade
        public async Task<Response<Guid>> CreateEntidadeAsync(CreateEntidadeRequest request)
        {
            EntidadeMatchName specification = new(request.Nome); // ardalis specification 
            bool EntidadeExists = await _repository.ExistsAsync<Entidade, Guid>(specification);
            if (EntidadeExists)
            {
                return ResponseFactory.Fail<Guid>("Entidade já existe");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Entidade newEntidade = _mapper.Map(request, new Entidade()); // map dto to domain entity

            try
            {
                Entidade response = await _repository.CreateAsync<Entidade, Guid>(newEntidade); // create new entity 
                _ = await _repository.SaveChangesAsync(); // save changes to db

                if(request.EntidadeContactos != null && request.EntidadeContactos.Any())
                {
                  var contactoRequest = new CreateEntidadeContactoBulkRequest
                  {
                    EntidadeId = response.Id.ToString(),
                    Contactos = request.EntidadeContactos,
                  };

                  var contactsResult = await _entidadeContactoService.CreateEntidadeContactoBulkAsync(contactoRequest);
                  if(contactsResult.Status != ResponseStatus.Success)
                  {
                    string errorMessage = contactsResult.Messages.TryGetValue("$", out var messages) && messages.Count > 0 ? messages.First() : "Erro desconhecido ao criar contactos";
                    return ResponseFactory.Fail<Guid>($"Entidade criada , mas falhou ao criar contactos: {errorMessage}");
                  }
                }
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Entidade
        public async Task<Response<Guid>> UpdateEntidadeAsync(UpdateEntidadeRequest request, Guid id)
        {
            Entidade EntidadeInDb = await _repository.GetByIdAsync<Entidade, Guid>(id); // get existing entity
            if (EntidadeInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Entidade não encontrada");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Entidade updatedEntidade = _mapper.Map(request, EntidadeInDb); // map dto to domain entity

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(EntidadeInDb.Id);
                  IEnumerable<EntidadeContacto> existingContactos = await _repository.GetListAsync<EntidadeContacto, Guid>(contactosSpec);

                  var requestedByTipo = request.EntidadeContactos.ToDictionary(c => c.EntidadeContactoTipoId, c => c);

                  var existingByTipo = existingContactos.ToDictionary(c => c.EntidadeContactoTipoId, c => c);

                  if( requestedByTipo.Count != existingByTipo.Count || requestedByTipo.Keys.Except(existingByTipo.Keys).Any() || existingByTipo.Keys.Except(requestedByTipo.Keys).Any()
                  )
                  {
                    hasContactChanges = true;
                  }
                  else
                  {
                    foreach(var kvp in requestedByTipo)
                    {
                      var tipoId = kvp.Key;
                      var req = kvp.Value;
                      var ex = existingByTipo[tipoId];

                      if(!string.Equals(ex.Valor, req.Valor, StringComparison.Ordinal) || ex.Principal != req.Principal)
                      {
                        hasContactChanges = true;
                        break;
                      }
                    }
                  }
                }

                Entidade response;

                try
                {
                  response = await _repository.UpdateAsync<Entidade, Guid>(updatedEntidade);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = EntidadeInDb;
                }
                if(hasContactChanges)
                {
                  var contactoRequest = new UpsertEntidadeContactoBulkRequest
                  {
                    EntidadeId = response.Id.ToString(),
                    Contactos = request.EntidadeContactos!,
                  };

                  var contactsResult = await _entidadeContactoService.UpsertEntidadeContactoBulkAsync(contactoRequest);
                  if(contactsResult.Status != ResponseStatus.Success)
                  {
                    string errorMessage = 
                      contactsResult.Messages.TryGetValue("$", out var messages) && messages.Count > 0
                      ? messages.First()
                      : "Erro desconhecido ao atualizar contactos";
                    return ResponseFactory.Fail<Guid>($"Entidade atualizada, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Entidade
        public async Task<Response<Guid>> DeleteEntidadeAsync(Guid id)
        {
            try
            {
                EntidadeContactoSearchByEntidade specification = new(id);
                IEnumerable<EntidadeContacto> contacts = await _repository.GetListAsync<EntidadeContacto, Guid>(specification);

                foreach( var contacto in contacts)
                {
                  var deleteResult = await _entidadeContactoService.DeleteEntidadeContactoAsync(contacto.Id);
                  if(deleteResult.Status != ResponseStatus.Success)
                  {
                    string errorMessage = 
                      deleteResult.Messages.TryGetValue("$", out var messages) && messages.Count > 0
                      ? messages.First()
                      : "Erro desconhecido ao eliminar contactos";

                    return ResponseFactory.Fail<Guid>($"Entidade eliminada, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                Entidade? Entidade = await _repository.RemoveByIdAsync<Entidade, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Entidade.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Entidades
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleEntidadeAsync(IEnumerable<Guid> ids)
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
                Entidade? entity = await _repository.GetByIdAsync<Entidade, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"Entidade com ID {id}.");
                  continue;
                }

                EntidadeContactoSearchByEntidade specification = new(id);
                IEnumerable<EntidadeContacto> contacts = await _repository.GetListAsync<
                  EntidadeContacto, 
                  Guid
                >(specification);


                bool contactsDeletedSuccessfully = true;
                foreach( var contact in contacts)
                {
                  var deleteResult = await _entidadeContactoService.DeleteEntidadeContactoAsync(contact.Id);

                  if(deleteResult.Status != ResponseStatus.Success)
                  {
                    contactsDeletedSuccessfully = false;
                    break;
                  }
                }

                if(!contactsDeletedSuccessfully)
                {
                  failedDeletions.Add($"Entidade com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                Entidade? deletedEntity = await _repository.RemoveByIdAsync<Entidade, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);

                }
                else
                {
                  failedDeletions.Add($"Entidade com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Entidade com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} entidades.";
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
