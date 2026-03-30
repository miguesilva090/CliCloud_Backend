using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.EntidadePessoaService.DTOs;
using CliCloud.Application.Services.Utility.EntidadePessoaService.Filters;
using CliCloud.Application.Services.Utility.EntidadePessoaService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;

// After creating this service:
// -- 1. Create a EntidadePessoa domain entity in CliCloud.Domain/Entities/Utility
// -- 2. Add DbSet<EntidadePessoa> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a EntidadePessoa api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Utility.EntidadePessoaService
{
    public class EntidadePessoaService : IEntidadePessoaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public EntidadePessoaService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<EntidadePessoaDTO>>> GetEntidadePessoaAsync(string keyword = "")
        {
            EntidadePessoaSearchList specification = new(keyword); // ardalis specification
            IEnumerable<EntidadePessoaDTO> list = await _repository.GetListAsync<EntidadePessoa, EntidadePessoaDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<EntidadePessoaDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<EntidadePessoaLightDTO>>> GetEntidadePessoaLightAsync(string keyword = "")
        {
            EntidadePessoaSearchList specification = new(keyword); // ardalis specification
            IEnumerable<EntidadePessoaLightDTO> list = await _repository.GetListAsync<EntidadePessoa, EntidadePessoaLightDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<EntidadePessoaLightDTO>>(list);
        }

        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<EntidadePessoaTableDTO>> GetEntidadePessoaPaginatedAsync(EntidadePessoaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            EntidadePessoaSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<EntidadePessoaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<EntidadePessoa, EntidadePessoaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all EntidadePessoas (non-paginated)
        public async Task<Response<IEnumerable<EntidadePessoaTableDTO>>> GetAllEntidadePessoaAsync(EntidadePessoaAllFilter filter)
        {
            try
            {
                filter ??= new EntidadePessoaAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                EntidadePessoaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<EntidadePessoaTableDTO> list = await _repository.GetListAsync<EntidadePessoa, EntidadePessoaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<EntidadePessoaTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<EntidadePessoaTableDTO>>(ex.Message);
            }
        }

        // get single EntidadePessoa by Id 
        public async Task<Response<EntidadePessoaDTO>> GetEntidadePessoaAsync(Guid id)
        {
            try
            {
                EntidadePessoaDTO dto = await _repository.GetByIdAsync<EntidadePessoa, EntidadePessoaDTO, Guid>(id);
                return ResponseFactory.Success<EntidadePessoaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<EntidadePessoaDTO>(ex.Message);
            }
        }

        // get single EntidadePessoa by NContrib
        public async Task<Response<EntidadePessoaDTO>> GetEntidadePessoaByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<EntidadePessoaDTO>("Numero de Contribuinte não pode ser vazio");
                }

                EntidadePessoaMatchNContrib specification = new(ncontrib);
                IEnumerable<EntidadePessoaDTO> results = await _repository.GetListAsync<EntidadePessoa, EntidadePessoaDTO, Guid>(specification);

                EntidadePessoaDTO? entidadePessoa = results.FirstOrDefault();
                if(entidadePessoa == null)
                {
                  return ResponseFactory.Fail<EntidadePessoaDTO>("Não foi encontrada nenhuma EntidadePessoa com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<EntidadePessoaDTO>(entidadePessoa);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<EntidadePessoaDTO>(ex.Message);
            }
        }

        // get multiple EntidadePessoas by Nome 
        public async Task<Response<IEnumerable<EntidadePessoaDTO>>> GetEntidadePessoaByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<EntidadePessoaDTO>>("Nome não pode ser vazio");
            }

            EntidadePessoaSearchByName specification = new(nome);
            IEnumerable<EntidadePessoaDTO> results = await _repository.GetListAsync<EntidadePessoa, EntidadePessoaDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<EntidadePessoaDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<EntidadePessoaDTO>>(ex.Message);
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

        // create new EntidadePessoa
        public async Task<Response<Guid>> CreateEntidadePessoaAsync(CreateEntidadePessoaRequest request)
        {
            EntidadePessoaMatchName specification = new(request.Nome); // ardalis specification 
            bool EntidadePessoaExists = await _repository.ExistsAsync<EntidadePessoa, Guid>(specification);
            if (EntidadePessoaExists)
            {
                return ResponseFactory.Fail<Guid>("EntidadePessoa já existe");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            if(request.UrlFotoAssinatura != null)
            {
              request.UrlFotoAssinatura = ConvertToPartialUrl(request.UrlFotoAssinatura);
            }

            EntidadePessoa newEntidadePessoa = _mapper.Map(request, new EntidadePessoa()); // map dto to domain entity

            try
            {
                EntidadePessoa response = await _repository.CreateAsync<EntidadePessoa, Guid>(newEntidadePessoa); // create new entity 
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
                    return ResponseFactory.Fail<Guid>($"EntidadePessoa criada, mas falhou ao criar contactos: {errorMessage}");
                  }
                }
                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update EntidadePessoa
        public async Task<Response<Guid>> UpdateEntidadePessoaAsync(UpdateEntidadePessoaRequest request, Guid id)
        {
            EntidadePessoa EntidadePessoaInDb = await _repository.GetByIdAsync<EntidadePessoa, Guid>(id); // get existing entity
            if (EntidadePessoaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("EntidadePessoa não encontrada");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            if(request.UrlFotoAssinatura != null)
            {
              request.UrlFotoAssinatura = ConvertToPartialUrl(request.UrlFotoAssinatura);
            }

            EntidadePessoa updatedEntidadePessoa = _mapper.Map(request, EntidadePessoaInDb); // map dto to domain entity

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(EntidadePessoaInDb.Id);
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

                EntidadePessoa response;

                try
                {
                  response = await _repository.UpdateAsync<EntidadePessoa, Guid>(updatedEntidadePessoa);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = EntidadePessoaInDb;
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
                    return ResponseFactory.Fail<Guid>($"EntidadePessoa atualizada, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete EntidadePessoa
        public async Task<Response<Guid>> DeleteEntidadePessoaAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"EntidadePessoa eliminada, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                EntidadePessoa? EntidadePessoa = await _repository.RemoveByIdAsync<EntidadePessoa, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(EntidadePessoa.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple EntidadePessoas
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleEntidadePessoaAsync(IEnumerable<Guid> ids)
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
                EntidadePessoa? entity = await _repository.GetByIdAsync<EntidadePessoa, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"EntidadePessoa com ID {id}.");
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
                  failedDeletions.Add($"EntidadePessoa com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                EntidadePessoa? deletedEntity = await _repository.RemoveByIdAsync<EntidadePessoa, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);

                }
                else
                {
                  failedDeletions.Add($"EntidadePessoa com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"EntidadePessoa com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} entidades pessoa.";
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

