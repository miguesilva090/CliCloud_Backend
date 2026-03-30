using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.EntidadesFinanceiras;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.DTOs;
using CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Filters;
using CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService
{
    public class EntidadeFinanceiraService : IEntidadeFinanceiraService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public EntidadeFinanceiraService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<EntidadeFinanceiraDTO>>> GetEntidadeFinanceiraAsync(string keyword = "")
        {
            EntidadeFinanceiraSearchList specification = new(keyword);
            IEnumerable<EntidadeFinanceiraDTO> list = await _repository.GetListAsync<EntidadeFinanceira, EntidadeFinanceiraDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<EntidadeFinanceiraDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<EntidadeFinanceiraLightDTO>>> GetEntidadeFinanceiraLightAsync(string keyword = "")
        {
            EntidadeFinanceiraSearchList specification = new(keyword);
            IEnumerable<EntidadeFinanceiraLightDTO> list = await _repository.GetListAsync<EntidadeFinanceira, EntidadeFinanceiraLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<EntidadeFinanceiraLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<EntidadeFinanceiraTableDTO>> GetEntidadeFinanceiraPaginatedAsync(EntidadeFinanceiraTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            EntidadeFinanceiraSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<EntidadeFinanceiraTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<EntidadeFinanceira, EntidadeFinanceiraTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all EntidadesFinanceiras (non-paginated)
        public async Task<Response<IEnumerable<EntidadeFinanceiraTableDTO>>> GetAllEntidadeFinanceiraAsync(EntidadeFinanceiraAllFilter filter)
        {
            try
            {
                filter ??= new EntidadeFinanceiraAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                EntidadeFinanceiraSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<EntidadeFinanceiraTableDTO> list = await _repository.GetListAsync<EntidadeFinanceira, EntidadeFinanceiraTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<EntidadeFinanceiraTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<EntidadeFinanceiraTableDTO>>(ex.Message);
            }
        }

        // get single EntidadeFinanceira by Id 
        public async Task<Response<EntidadeFinanceiraDTO>> GetEntidadeFinanceiraAsync(Guid id)
        {
            try
            {
                EntidadeFinanceiraDTO dto = await _repository.GetByIdAsync<EntidadeFinanceira, EntidadeFinanceiraDTO, Guid>(id);
                return ResponseFactory.Success<EntidadeFinanceiraDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<EntidadeFinanceiraDTO>(ex.Message);
            }
        }

        // get single EntidadeFinanceira by NContrib
        public async Task<Response<EntidadeFinanceiraDTO>> GetEntidadeFinanceiraByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<EntidadeFinanceiraDTO>("Numero de Contribuinte não pode ser vazio");
                }

                EntidadeFinanceiraMatchNContrib specification = new(ncontrib);
                IEnumerable<EntidadeFinanceiraDTO> results = await _repository.GetListAsync<EntidadeFinanceira, EntidadeFinanceiraDTO, Guid>(specification);

                EntidadeFinanceiraDTO? entidadeFinanceira = results.FirstOrDefault();
                if(entidadeFinanceira == null)
                {
                  return ResponseFactory.Fail<EntidadeFinanceiraDTO>("Não foi encontrada nenhuma EntidadeFinanceira com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<EntidadeFinanceiraDTO>(entidadeFinanceira);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<EntidadeFinanceiraDTO>(ex.Message);
            }
        }

        // get multiple EntidadesFinanceiras by Nome 
        public async Task<Response<IEnumerable<EntidadeFinanceiraDTO>>> GetEntidadeFinanceiraByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<EntidadeFinanceiraDTO>>("Nome não pode ser vazio");
            }

            EntidadeFinanceiraSearchByName specification = new(nome);
            IEnumerable<EntidadeFinanceiraDTO> results = await _repository.GetListAsync<EntidadeFinanceira, EntidadeFinanceiraDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<EntidadeFinanceiraDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<EntidadeFinanceiraDTO>>(ex.Message);
          }
        }

        //Helper method to convert full URL to partial URL (for storage)
        private static string? ConvertToPartialUrl(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return null;
            }

            if (imageUrl.StartsWith('/'))
            {
                return imageUrl;
            }

            if (imageUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                imageUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    Uri uri = new(imageUrl);
                    return uri.AbsolutePath;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return imageUrl;
        }

        // create new EntidadeFinanceira
        public async Task<Response<Guid>> CreateEntidadeFinanceiraAsync(CreateEntidadeFinanceiraRequest request)
        {
            EntidadeFinanceiraMatchNomePais specification = new(request.Nome, request.PaisPrefixo);
            bool EntidadeFinanceiraExists = await _repository.ExistsAsync<EntidadeFinanceira, Guid>(specification);
            if (EntidadeFinanceiraExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe uma EntidadeFinanceira com este Nome e PaisPrefixo");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            EntidadeFinanceira newEntidadeFinanceira = _mapper.Map(request, new EntidadeFinanceira());
            newEntidadeFinanceira.TipoEntidade = Domain.Enums.EntidadeTipo.EntidadeFinanceira;
            
            // Converter TipoEntidadeFinanceiraId de string para Guid
            if(Guid.TryParse(request.TipoEntidadeFinanceiraId, out Guid tipoEntidadeFinanceiraId))
            {
                newEntidadeFinanceira.TipoEntidadeFinanceiraId = tipoEntidadeFinanceiraId;
            }
            else
            {
                return ResponseFactory.Fail<Guid>("TipoEntidadeFinanceiraId inválido");
            }

            try
            {
                EntidadeFinanceira response = await _repository.CreateAsync<EntidadeFinanceira, Guid>(newEntidadeFinanceira);
                _ = await _repository.SaveChangesAsync();

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
                    return ResponseFactory.Fail<Guid>($"EntidadeFinanceira criada, mas falhou ao criar contactos: {errorMessage}");
                  }
                }
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update EntidadeFinanceira
        public async Task<Response<Guid>> UpdateEntidadeFinanceiraAsync(UpdateEntidadeFinanceiraRequest request, Guid id)
        {
            EntidadeFinanceira EntidadeFinanceiraInDb = await _repository.GetByIdAsync<EntidadeFinanceira, Guid>(id);
            if (EntidadeFinanceiraInDb == null)
            {
                return ResponseFactory.Fail<Guid>("EntidadeFinanceira não encontrada");
            }

            if (EntidadeFinanceiraInDb.Nome != request.Nome || EntidadeFinanceiraInDb.PaisPrefixo != request.PaisPrefixo)
            {
                EntidadeFinanceiraMatchNomePais specification = new(request.Nome, request.PaisPrefixo);
                bool nomePaisExists = await _repository.ExistsAsync<EntidadeFinanceira, Guid>(specification);
                if (nomePaisExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe uma EntidadeFinanceira com este Nome e PaisPrefixo");
                }
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            EntidadeFinanceira updatedEntidadeFinanceira = _mapper.Map(request, EntidadeFinanceiraInDb);
            updatedEntidadeFinanceira.TipoEntidade = Domain.Enums.EntidadeTipo.EntidadeFinanceira;
            
            // Converter TipoEntidadeFinanceiraId de string para Guid
            if(Guid.TryParse(request.TipoEntidadeFinanceiraId, out Guid tipoEntidadeFinanceiraId))
            {
                updatedEntidadeFinanceira.TipoEntidadeFinanceiraId = tipoEntidadeFinanceiraId;
            }
            else
            {
                return ResponseFactory.Fail<Guid>("TipoEntidadeFinanceiraId inválido");
            }

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(EntidadeFinanceiraInDb.Id);
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
                      if(!existingByTipo.TryGetValue(tipoId, out var ex))
                      {
                        hasContactChanges = true;
                        break;
                      }

                      if(!string.Equals(ex.Valor, req.Valor, StringComparison.Ordinal) || ex.Principal != req.Principal)
                      {
                        hasContactChanges = true;
                        break;
                      }
                    }
                  }
                }

                EntidadeFinanceira response;

                try
                {
                  response = await _repository.UpdateAsync<EntidadeFinanceira, Guid>(updatedEntidadeFinanceira);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = EntidadeFinanceiraInDb;
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
                    return ResponseFactory.Fail<Guid>($"EntidadeFinanceira atualizada, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete EntidadeFinanceira
        public async Task<Response<Guid>> DeleteEntidadeFinanceiraAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"EntidadeFinanceira eliminada, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                EntidadeFinanceira? EntidadeFinanceira = await _repository.RemoveByIdAsync<EntidadeFinanceira, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(EntidadeFinanceira.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple EntidadesFinanceiras
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleEntidadeFinanceiraAsync(IEnumerable<Guid> ids)
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
                EntidadeFinanceira? entity = await _repository.GetByIdAsync<EntidadeFinanceira, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"EntidadeFinanceira com ID {id} não encontrada.");
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
                  failedDeletions.Add($"EntidadeFinanceira com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                EntidadeFinanceira? deletedEntity = await _repository.RemoveByIdAsync<EntidadeFinanceira, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"EntidadeFinanceira com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"EntidadeFinanceira com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} entidades financeiras.";
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
