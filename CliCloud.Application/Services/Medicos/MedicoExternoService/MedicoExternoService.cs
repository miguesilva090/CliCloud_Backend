using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Medicos.MedicoExternoService.DTOs;
using CliCloud.Application.Services.Medicos.MedicoExternoService.Filters;
using CliCloud.Application.Services.Medicos.MedicoExternoService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Medicos.MedicoExternoService
{
    public class MedicoExternoService : IMedicoExternoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public MedicoExternoService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<MedicoExternoDTO>>> GetMedicoExternoAsync(string keyword = "")
        {
            MedicoExternoSearchList specification = new(keyword);
            IEnumerable<MedicoExternoDTO> list = await _repository.GetListAsync<MedicoExterno, MedicoExternoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MedicoExternoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<MedicoExternoLightDTO>>> GetMedicoExternoLightAsync(string keyword = "")
        {
            MedicoExternoSearchList specification = new(keyword);
            IEnumerable<MedicoExternoLightDTO> list = await _repository.GetListAsync<MedicoExterno, MedicoExternoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MedicoExternoLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<MedicoExternoTableDTO>> GetMedicoExternoPaginatedAsync(MedicoExternoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            MedicoExternoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<MedicoExternoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<MedicoExterno, MedicoExternoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all MedicosExternos (non-paginated)
        public async Task<Response<IEnumerable<MedicoExternoTableDTO>>> GetAllMedicoExternoAsync(MedicoExternoAllFilter filter)
        {
            try
            {
                filter ??= new MedicoExternoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                MedicoExternoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<MedicoExternoTableDTO> list = await _repository.GetListAsync<MedicoExterno, MedicoExternoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<MedicoExternoTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<MedicoExternoTableDTO>>(ex.Message);
            }
        }

        // get single MedicoExterno by Id 
        public async Task<Response<MedicoExternoDTO>> GetMedicoExternoAsync(Guid id)
        {
            try
            {
                MedicoExternoDTO dto = await _repository.GetByIdAsync<MedicoExterno, MedicoExternoDTO, Guid>(id);
                return ResponseFactory.Success<MedicoExternoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MedicoExternoDTO>(ex.Message);
            }
        }

        // get single MedicoExterno by NContrib
        public async Task<Response<MedicoExternoDTO>> GetMedicoExternoByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<MedicoExternoDTO>("Numero de Contribuinte não pode ser vazio");
                }

                MedicoExternoMatchNContrib specification = new(ncontrib);
                IEnumerable<MedicoExternoDTO> results = await _repository.GetListAsync<MedicoExterno, MedicoExternoDTO, Guid>(specification);

                MedicoExternoDTO? medicoExterno = results.FirstOrDefault();
                if(medicoExterno == null)
                {
                  return ResponseFactory.Fail<MedicoExternoDTO>("Não foi encontrado nenhum MedicoExterno com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<MedicoExternoDTO>(medicoExterno);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<MedicoExternoDTO>(ex.Message);
            }
        }

        // get multiple MedicosExternos by Nome 
        public async Task<Response<IEnumerable<MedicoExternoDTO>>> GetMedicoExternoByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<MedicoExternoDTO>>("Nome não pode ser vazio");
            }

            MedicoExternoSearchByName specification = new(nome);
            IEnumerable<MedicoExternoDTO> results = await _repository.GetListAsync<MedicoExterno, MedicoExternoDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<MedicoExternoDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<MedicoExternoDTO>>(ex.Message);
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

        // create new MedicoExterno
        public async Task<Response<Guid>> CreateMedicoExternoAsync(CreateMedicoExternoRequest request)
        {
            MedicoExternoMatchName specification = new(request.Nome);
            bool MedicoExternoExists = await _repository.ExistsAsync<MedicoExterno, Guid>(specification);
            if (MedicoExternoExists)
            {
                return ResponseFactory.Fail<Guid>("MedicoExterno já existe");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            MedicoExterno newMedicoExterno = _mapper.Map(request, new MedicoExterno());
            newMedicoExterno.TipoEntidade = EntidadeTipo.MedicoExterno;

            try
            {
                MedicoExterno response = await _repository.CreateAsync<MedicoExterno, Guid>(newMedicoExterno);
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
                    return ResponseFactory.Fail<Guid>($"MedicoExterno criado, mas falhou ao criar contactos: {errorMessage}");
                  }
                }
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update MedicoExterno
        public async Task<Response<Guid>> UpdateMedicoExternoAsync(UpdateMedicoExternoRequest request, Guid id)
        {
            MedicoExterno? MedicoExternoInDb = await _repository.GetByIdAsync<MedicoExterno, Guid>(id);
            if (MedicoExternoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("MedicoExterno não encontrado");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            MedicoExterno updatedMedicoExterno = _mapper.Map(request, MedicoExternoInDb);
            updatedMedicoExterno.TipoEntidade = EntidadeTipo.MedicoExterno;

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(MedicoExternoInDb.Id);
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

                MedicoExterno response;

                try
                {
                  response = await _repository.UpdateAsync<MedicoExterno, Guid>(updatedMedicoExterno);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = MedicoExternoInDb;
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
                    return ResponseFactory.Fail<Guid>($"MedicoExterno atualizado, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete MedicoExterno
        public async Task<Response<Guid>> DeleteMedicoExternoAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"MedicoExterno eliminado, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                MedicoExterno? MedicoExterno = await _repository.RemoveByIdAsync<MedicoExterno, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(MedicoExterno.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple MedicosExternos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMedicoExternoAsync(IEnumerable<Guid> ids)
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
                MedicoExterno? entity = await _repository.GetByIdAsync<MedicoExterno, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"MedicoExterno com ID {id}.");
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
                  failedDeletions.Add($"MedicoExterno com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                MedicoExterno? deletedEntity = await _repository.RemoveByIdAsync<MedicoExterno, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);

                }
                else
                {
                  failedDeletions.Add($"MedicoExterno com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"MedicoExterno com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} medicos externos.";
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
