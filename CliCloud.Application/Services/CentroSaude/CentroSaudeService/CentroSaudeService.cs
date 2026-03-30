using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CentroSaudeEntity = CliCloud.Domain.Entities.CentroSaude.CentroSaude;
using CliCloud.Application.Services.CentroSaude.CentroSaudeService.DTOs;
using CliCloud.Application.Services.CentroSaude.CentroSaudeService.Filters;
using CliCloud.Application.Services.CentroSaude.CentroSaudeService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;

namespace CliCloud.Application.Services.CentroSaude.CentroSaudeService
{
    public class CentroSaudeService : ICentroSaudeService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public CentroSaudeService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<CentroSaudeDTO>>> GetCentroSaudeAsync(string keyword = "")
        {
            CentroSaudeSearchList specification = new(keyword);
            IEnumerable<CentroSaudeDTO> list = await _repository.GetListAsync<CentroSaudeEntity, CentroSaudeDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<CentroSaudeDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<CentroSaudeLightDTO>>> GetCentroSaudeLightAsync(string keyword = "")
        {
            CentroSaudeSearchList specification = new(keyword);
            IEnumerable<CentroSaudeLightDTO> list = await _repository.GetListAsync<CentroSaudeEntity, CentroSaudeLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<CentroSaudeLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<CentroSaudeTableDTO>> GetCentroSaudePaginatedAsync(CentroSaudeTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            CentroSaudeSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<CentroSaudeTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<CentroSaudeEntity, CentroSaudeTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all CentroSaudes (non-paginated)
        public async Task<Response<IEnumerable<CentroSaudeTableDTO>>> GetAllCentroSaudeAsync(CentroSaudeAllFilter filter)
        {
            try
            {
                filter ??= new CentroSaudeAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                CentroSaudeSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<CentroSaudeTableDTO> list = await _repository.GetListAsync<CentroSaudeEntity, CentroSaudeTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<CentroSaudeTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<CentroSaudeTableDTO>>(ex.Message);
            }
        }

        // get single CentroSaude by Id 
        public async Task<Response<CentroSaudeDTO>> GetCentroSaudeAsync(Guid id)
        {
            try
            {
                CentroSaudeDTO dto = await _repository.GetByIdAsync<CentroSaudeEntity, CentroSaudeDTO, Guid>(id);
                return ResponseFactory.Success<CentroSaudeDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<CentroSaudeDTO>(ex.Message);
            }
        }

        // get single CentroSaude by NContrib
        public async Task<Response<CentroSaudeDTO>> GetCentroSaudeByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<CentroSaudeDTO>("Numero de Contribuinte não pode ser vazio");
                }

                CentroSaudeMatchNContrib specification = new(ncontrib);
                IEnumerable<CentroSaudeDTO> results = await _repository.GetListAsync<CentroSaudeEntity, CentroSaudeDTO, Guid>(specification);

                CentroSaudeDTO? centroSaude = results.FirstOrDefault();
                if(centroSaude == null)
                {
                  return ResponseFactory.Fail<CentroSaudeDTO>("Não foi encontrado nenhum CentroSaude com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<CentroSaudeDTO>(centroSaude);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<CentroSaudeDTO>(ex.Message);
            }
        }

        // get multiple CentroSaudes by Nome 
        public async Task<Response<IEnumerable<CentroSaudeDTO>>> GetCentroSaudeByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<CentroSaudeDTO>>("Nome não pode ser vazio");
            }

            CentroSaudeSearchByName specification = new(nome);
            IEnumerable<CentroSaudeDTO> results = await _repository.GetListAsync<CentroSaudeEntity, CentroSaudeDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<CentroSaudeDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<CentroSaudeDTO>>(ex.Message);
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

        // create new CentroSaude
        public async Task<Response<Guid>> CreateCentroSaudeAsync(CreateCentroSaudeRequest request)
        {
            CentroSaudeMatchName specification = new(request.Nome);
            bool CentroSaudeExists = await _repository.ExistsAsync<CentroSaudeEntity, Guid>(specification);
            if (CentroSaudeExists)
            {
                return ResponseFactory.Fail<Guid>("CentroSaude já existe");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            CentroSaudeEntity newCentroSaude = _mapper.Map(request, new CentroSaudeEntity());
            newCentroSaude.TipoEntidade = Domain.Enums.EntidadeTipo.CentroSaude;

            try
            {
                CentroSaudeEntity response = await _repository.CreateAsync<CentroSaudeEntity, Guid>(newCentroSaude);
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
                    return ResponseFactory.Fail<Guid>($"CentroSaude criado, mas falhou ao criar contactos: {errorMessage}");
                  }
                }
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update CentroSaude
        public async Task<Response<Guid>> UpdateCentroSaudeAsync(UpdateCentroSaudeRequest request, Guid id)
        {
            CentroSaudeEntity? CentroSaudeInDb = await _repository.GetByIdAsync<CentroSaudeEntity, Guid>(id);
            if (CentroSaudeInDb == null)
            {
                return ResponseFactory.Fail<Guid>("CentroSaude não encontrado");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            CentroSaudeEntity updatedCentroSaude = _mapper.Map(request, CentroSaudeInDb);
            updatedCentroSaude.TipoEntidade = Domain.Enums.EntidadeTipo.CentroSaude;

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(CentroSaudeInDb.Id);
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

                CentroSaudeEntity response;

                try
                {
                  response = await _repository.UpdateAsync<CentroSaudeEntity, Guid>(updatedCentroSaude);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = CentroSaudeInDb;
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
                    return ResponseFactory.Fail<Guid>($"CentroSaude atualizado, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete CentroSaude
        public async Task<Response<Guid>> DeleteCentroSaudeAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"CentroSaude eliminado, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                CentroSaudeEntity? CentroSaude = await _repository.RemoveByIdAsync<CentroSaudeEntity, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(CentroSaude.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple CentroSaudes
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleCentroSaudeAsync(IEnumerable<Guid> ids)
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
                CentroSaudeEntity? entity = await _repository.GetByIdAsync<CentroSaudeEntity, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"CentroSaude com ID {id} não encontrado.");
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
                  failedDeletions.Add($"CentroSaude com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                CentroSaudeEntity? deletedEntity = await _repository.RemoveByIdAsync<CentroSaudeEntity, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"CentroSaude com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"CentroSaude com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} centros de saúde.";
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
