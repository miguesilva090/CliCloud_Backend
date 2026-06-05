using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Organismos.OrganismoService.DTOs;
using CliCloud.Application.Services.Organismos.OrganismoService.Filters;
using CliCloud.Application.Services.Organismos.OrganismoService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;

namespace CliCloud.Application.Services.Organismos.OrganismoService
{
    public class OrganismoService : IOrganismoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public OrganismoService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<OrganismoDTO>>> GetOrganismoAsync(string keyword = "")
        {
            OrganismoSearchList specification = new(keyword);
            IEnumerable<OrganismoDTO> list = await _repository.GetListAsync<Organismo, OrganismoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<OrganismoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<OrganismoLightDTO>>> GetOrganismoLightAsync(string keyword = "", string? siglaFicheiro = null)
        {
            OrganismoSearchList specification = new(keyword, siglaFicheiro);
            IEnumerable<OrganismoLightDTO> list = await _repository.GetListAsync<Organismo, OrganismoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<OrganismoLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<OrganismoTableDTO>> GetOrganismoPaginatedAsync(OrganismoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            OrganismoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<OrganismoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Organismo, OrganismoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all Organismos (non-paginated)
        public async Task<Response<IEnumerable<OrganismoTableDTO>>> GetAllOrganismoAsync(OrganismoAllFilter filter)
        {
            try
            {
                filter ??= new OrganismoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                OrganismoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<OrganismoTableDTO> list = await _repository.GetListAsync<Organismo, OrganismoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<OrganismoTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<OrganismoTableDTO>>(ex.Message);
            }
        }

        // get single Organismo by Id (igual CentroSaude: GetByIdAsync sem specification)
        public async Task<Response<OrganismoDTO>> GetOrganismoAsync(Guid id)
        {
            try
            {
                OrganismoDTO dto = await _repository.GetByIdAsync<Organismo, OrganismoDTO, Guid>(id);
                return ResponseFactory.Success<OrganismoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<OrganismoDTO>(ex.Message);
            }
        }

        // get single Organismo by NContrib
        public async Task<Response<OrganismoDTO>> GetOrganismoByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<OrganismoDTO>("Numero de Contribuinte não pode ser vazio");
                }

                OrganismoMatchNContrib specification = new(ncontrib);
                IEnumerable<OrganismoDTO> results = await _repository.GetListAsync<Organismo, OrganismoDTO, Guid>(specification);

                OrganismoDTO? organismo = results.FirstOrDefault();
                if(organismo == null)
                {
                  return ResponseFactory.Fail<OrganismoDTO>("Não foi encontrado nenhum Organismo com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<OrganismoDTO>(organismo);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<OrganismoDTO>(ex.Message);
            }
        }

        // get multiple Organismos by Nome 
        public async Task<Response<IEnumerable<OrganismoDTO>>> GetOrganismoByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<OrganismoDTO>>("Nome não pode ser vazio");
            }

            OrganismoSearchByName specification = new(nome);
            IEnumerable<OrganismoDTO> results = await _repository.GetListAsync<Organismo, OrganismoDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<OrganismoDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<OrganismoDTO>>(ex.Message);
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

        // create new Organismo
        public async Task<Response<Guid>> CreateOrganismoAsync(CreateOrganismoRequest request)
        {
            OrganismoMatchName specification = new(request.Nome);
            bool OrganismoExists = await _repository.ExistsAsync<Organismo, Guid>(specification);
            if (OrganismoExists)
            {
                return ResponseFactory.Fail<Guid>("Organismo já existe");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Organismo newOrganismo = _mapper.Map(request, new Organismo());
            newOrganismo.TipoEntidade = Domain.Enums.EntidadeTipo.Organismo;
            
            // Converter BancoId de string para Guid
            if(!string.IsNullOrWhiteSpace(request.BancoId) && Guid.TryParse(request.BancoId, out Guid bancoId))
            {
                newOrganismo.BancoId = bancoId;
            }

            try
            {
                Organismo response = await _repository.CreateAsync<Organismo, Guid>(newOrganismo);
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
                    return ResponseFactory.Fail<Guid>($"Organismo criado, mas falhou ao criar contactos: {errorMessage}");
                  }
                }
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Organismo
        public async Task<Response<Guid>> UpdateOrganismoAsync(UpdateOrganismoRequest request, Guid id)
        {
            Organismo OrganismoInDb = await _repository.GetByIdAsync<Organismo, Guid>(id);
            if (OrganismoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Organismo não encontrado");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Organismo updatedOrganismo = _mapper.Map(request, OrganismoInDb);
            updatedOrganismo.TipoEntidade = Domain.Enums.EntidadeTipo.Organismo;
            
            // Converter BancoId de string para Guid
            if(!string.IsNullOrWhiteSpace(request.BancoId) && Guid.TryParse(request.BancoId, out Guid bancoId))
            {
                updatedOrganismo.BancoId = bancoId;
            }
            else
            {
                updatedOrganismo.BancoId = null;
            }

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(OrganismoInDb.Id);
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

                Organismo response;

                try
                {
                  response = await _repository.UpdateAsync<Organismo, Guid>(updatedOrganismo);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = OrganismoInDb;
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
                    return ResponseFactory.Fail<Guid>($"Organismo atualizado, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Organismo
        public async Task<Response<Guid>> DeleteOrganismoAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"Organismo eliminado, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                Organismo? Organismo = await _repository.RemoveByIdAsync<Organismo, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Organismo.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Organismos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleOrganismoAsync(IEnumerable<Guid> ids)
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
                Organismo? entity = await _repository.GetByIdAsync<Organismo, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"Organismo com ID {id} não encontrado.");
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
                  failedDeletions.Add($"Organismo com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                Organismo? deletedEntity = await _repository.RemoveByIdAsync<Organismo, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"Organismo com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Organismo com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} organismos.";
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
