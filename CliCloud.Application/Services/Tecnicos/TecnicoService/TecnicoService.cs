using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Tecnicos.TecnicoService.DTOs;
using CliCloud.Application.Services.Tecnicos.TecnicoService.Filters;
using CliCloud.Application.Services.Tecnicos.TecnicoService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tecnicos.TecnicoService
{
    public class TecnicoService : ITecnicoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public TecnicoService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<TecnicoDTO>>> GetTecnicoAsync(string keyword = "")
        {
            TecnicoSearchList specification = new(keyword);
            IEnumerable<TecnicoDTO> list = await _repository.GetListAsync<Tecnico, TecnicoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TecnicoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<TecnicoLightDTO>>> GetTecnicoLightAsync(string keyword = "")
        {
            TecnicoSearchList specification = new(keyword);
            IEnumerable<TecnicoLightDTO> list = await _repository.GetListAsync<Tecnico, TecnicoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<TecnicoLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<TecnicoTableDTO>> GetTecnicoPaginatedAsync(TecnicoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            TecnicoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<TecnicoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Tecnico, TecnicoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all Tecnicos (non-paginated)
        public async Task<Response<IEnumerable<TecnicoTableDTO>>> GetAllTecnicoAsync(TecnicoAllFilter filter)
        {
            try
            {
                filter ??= new TecnicoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                TecnicoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<TecnicoTableDTO> list = await _repository.GetListAsync<Tecnico, TecnicoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<TecnicoTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<TecnicoTableDTO>>(ex.Message);
            }
        }

        // get single Tecnico by Id 
        public async Task<Response<TecnicoDTO>> GetTecnicoAsync(Guid id)
        {
            try
            {
                TecnicoDTO dto = await _repository.GetByIdAsync<Tecnico, TecnicoDTO, Guid>(id);
                return ResponseFactory.Success<TecnicoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<TecnicoDTO>(ex.Message);
            }
        }

        // get single Tecnico by NContrib
        public async Task<Response<TecnicoDTO>> GetTecnicoByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<TecnicoDTO>("Numero de Contribuinte não pode ser vazio");
                }

                TecnicoMatchNContrib specification = new(ncontrib);
                IEnumerable<TecnicoDTO> results = await _repository.GetListAsync<Tecnico, TecnicoDTO, Guid>(specification);

                TecnicoDTO? tecnico = results.FirstOrDefault();
                if(tecnico == null)
                {
                  return ResponseFactory.Fail<TecnicoDTO>("Não foi encontrado nenhum Tecnico com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<TecnicoDTO>(tecnico);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<TecnicoDTO>(ex.Message);
            }
        }

        // get multiple Tecnicos by Nome 
        public async Task<Response<IEnumerable<TecnicoDTO>>> GetTecnicoByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<TecnicoDTO>>("Nome não pode ser vazio");
            }

            TecnicoSearchByName specification = new(nome);
            IEnumerable<TecnicoDTO> results = await _repository.GetListAsync<Tecnico, TecnicoDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<TecnicoDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<TecnicoDTO>>(ex.Message);
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

        // create new Tecnico
        public async Task<Response<Guid>> CreateTecnicoAsync(CreateTecnicoRequest request)
        {
            TecnicoMatchName specification = new(request.Nome);
            bool TecnicoExists = await _repository.ExistsAsync<Tecnico, Guid>(specification);
            if (TecnicoExists)
            {
                return ResponseFactory.Fail<Guid>("Tecnico já existe");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Tecnico newTecnico = _mapper.Map(request, new Tecnico());
            newTecnico.TipoEntidade = EntidadeTipo.Tecnico;

            // Converter EspecialidadeId de string para Guid
            if (!string.IsNullOrWhiteSpace(request.EspecialidadeId) && Guid.TryParse(request.EspecialidadeId, out Guid especialidadeId))
            {
                newTecnico.EspecialidadeId = especialidadeId;
            }
            else
            {
                newTecnico.EspecialidadeId = null;
            }

            // Converter IdUtilizador de string para Guid
            if (!string.IsNullOrWhiteSpace(request.IdUtilizador) && Guid.TryParse(request.IdUtilizador, out Guid idUtilizador))
            {
                newTecnico.IdUtilizador = idUtilizador;
            }
            else
            {
                newTecnico.IdUtilizador = null;
            }

            try
            {
                Tecnico response = await _repository.CreateAsync<Tecnico, Guid>(newTecnico);
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
                    return ResponseFactory.Fail<Guid>($"Tecnico criado, mas falhou ao criar contactos: {errorMessage}");
                  }
                }
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Tecnico
        public async Task<Response<Guid>> UpdateTecnicoAsync(UpdateTecnicoRequest request, Guid id)
        {
            Tecnico? TecnicoInDb = await _repository.GetByIdAsync<Tecnico, Guid>(id);
            if (TecnicoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Tecnico não encontrado");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Tecnico updatedTecnico = _mapper.Map(request, TecnicoInDb);
            updatedTecnico.TipoEntidade = EntidadeTipo.Tecnico;

            // Converter EspecialidadeId de string para Guid
            if (!string.IsNullOrWhiteSpace(request.EspecialidadeId) && Guid.TryParse(request.EspecialidadeId, out Guid especialidadeId))
            {
                updatedTecnico.EspecialidadeId = especialidadeId;
            }
            else
            {
                updatedTecnico.EspecialidadeId = null;
            }

            // Converter IdUtilizador de string para Guid
            if (!string.IsNullOrWhiteSpace(request.IdUtilizador) && Guid.TryParse(request.IdUtilizador, out Guid idUtilizador))
            {
                updatedTecnico.IdUtilizador = idUtilizador;
            }
            else
            {
                updatedTecnico.IdUtilizador = null;
            }

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(TecnicoInDb.Id);
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
                      if (!existingByTipo.TryGetValue(tipoId, out var ex))
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

                Tecnico response;

                try
                {
                  response = await _repository.UpdateAsync<Tecnico, Guid>(updatedTecnico);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = TecnicoInDb;
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
                    return ResponseFactory.Fail<Guid>($"Tecnico atualizado, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Tecnico
        public async Task<Response<Guid>> DeleteTecnicoAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"Tecnico eliminado, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                Tecnico? Tecnico = await _repository.RemoveByIdAsync<Tecnico, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Tecnico.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Tecnicos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTecnicoAsync(IEnumerable<Guid> ids)
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
                Tecnico? entity = await _repository.GetByIdAsync<Tecnico, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"Tecnico com ID {id}.");
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
                  failedDeletions.Add($"Tecnico com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                Tecnico? deletedEntity = await _repository.RemoveByIdAsync<Tecnico, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);

                }
                else
                {
                  failedDeletions.Add($"Tecnico com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Tecnico com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} tecnicos.";
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
