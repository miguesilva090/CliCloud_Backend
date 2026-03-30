using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Medicos.MedicoService.DTOs;
using CliCloud.Application.Services.Medicos.MedicoService.Filters;
using CliCloud.Application.Services.Medicos.MedicoService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Medicos.MedicoService
{
    public class MedicoService : IMedicoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public MedicoService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<MedicoDTO>>> GetMedicoAsync(string keyword = "")
        {
            MedicoSearchList specification = new(keyword);
            IEnumerable<MedicoDTO> list = await _repository.GetListAsync<Medico, MedicoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MedicoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<MedicoLightDTO>>> GetMedicoLightAsync(string keyword = "")
        {
            MedicoSearchList specification = new(keyword);
            IEnumerable<MedicoLightDTO> list = await _repository.GetListAsync<Medico, MedicoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<MedicoLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<MedicoTableDTO>> GetMedicoPaginatedAsync(MedicoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            MedicoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<MedicoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Medico, MedicoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all Medicos (non-paginated)
        public async Task<Response<IEnumerable<MedicoTableDTO>>> GetAllMedicoAsync(MedicoAllFilter filter)
        {
            try
            {
                filter ??= new MedicoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                MedicoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<MedicoTableDTO> list = await _repository.GetListAsync<Medico, MedicoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<MedicoTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<MedicoTableDTO>>(ex.Message);
            }
        }

        // get single Medico by Id 
        public async Task<Response<MedicoDTO>> GetMedicoAsync(Guid id)
        {
            try
            {
                MedicoDTO dto = await _repository.GetByIdAsync<Medico, MedicoDTO, Guid>(id);
                return ResponseFactory.Success<MedicoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MedicoDTO>(ex.Message);
            }
        }

        // get single Medico by NContrib
        public async Task<Response<MedicoDTO>> GetMedicoByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<MedicoDTO>("Numero de Contribuinte não pode ser vazio");
                }

                MedicoMatchNContrib specification = new(ncontrib);
                IEnumerable<MedicoDTO> results = await _repository.GetListAsync<Medico, MedicoDTO, Guid>(specification);

                MedicoDTO? medico = results.FirstOrDefault();
                if(medico == null)
                {
                  return ResponseFactory.Fail<MedicoDTO>("Não foi encontrado nenhum Medico com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<MedicoDTO>(medico);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<MedicoDTO>(ex.Message);
            }
        }

        public async Task<Response<MedicoDTO?>> GetMedicoByIdUtilizadorAsync(Guid idUtilizador)
        {
            try
            {
                var spec = new MedicoByIdUtilizadorSpec(idUtilizador);
                var list = await _repository.GetListAsync<Medico, MedicoDTO, Guid>(spec);
                var medico = list.FirstOrDefault();
                return ResponseFactory.Success<MedicoDTO?>(medico);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<MedicoDTO?>(ex.Message);
            }
        }

        // get multiple Medicos by Nome 
        public async Task<Response<IEnumerable<MedicoDTO>>> GetMedicoByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<MedicoDTO>>("Nome não pode ser vazio");
            }

            MedicoSearchByName specification = new(nome);
            IEnumerable<MedicoDTO> results = await _repository.GetListAsync<Medico, MedicoDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<MedicoDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<MedicoDTO>>(ex.Message);
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

        // create new Medico
        public async Task<Response<Guid>> CreateMedicoAsync(CreateMedicoRequest request)
        {
            MedicoMatchName specification = new(request.Nome);
            bool MedicoExists = await _repository.ExistsAsync<Medico, Guid>(specification);
            if (MedicoExists)
            {
                return ResponseFactory.Fail<Guid>("Medico já existe");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Medico newMedico = _mapper.Map(request, new Medico());
            newMedico.TipoEntidade = EntidadeTipo.Medico;

            // Converter EspecialidadeId de string para Guid
            if (!string.IsNullOrWhiteSpace(request.EspecialidadeId) && Guid.TryParse(request.EspecialidadeId, out Guid especialidadeId))
            {
                newMedico.EspecialidadeId = especialidadeId;
            }
            else
            {
                newMedico.EspecialidadeId = null;
            }

            // Converter IdUtilizador de string para Guid
            if (!string.IsNullOrWhiteSpace(request.IdUtilizador) && Guid.TryParse(request.IdUtilizador, out Guid idUtilizador))
            {
                newMedico.IdUtilizador = idUtilizador;
            }
            else
            {
                newMedico.IdUtilizador = null;
            }

            try
            {
                Medico response = await _repository.CreateAsync<Medico, Guid>(newMedico);
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
                    return ResponseFactory.Fail<Guid>($"Medico criado, mas falhou ao criar contactos: {errorMessage}");
                  }
                }
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Medico
        public async Task<Response<Guid>> UpdateMedicoAsync(UpdateMedicoRequest request, Guid id)
        {
            Medico? MedicoInDb = await _repository.GetByIdAsync<Medico, Guid>(id);
            if (MedicoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Medico não encontrado");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Medico updatedMedico = _mapper.Map(request, MedicoInDb);
            updatedMedico.TipoEntidade = EntidadeTipo.Medico;

            // Converter EspecialidadeId de string para Guid
            if (!string.IsNullOrWhiteSpace(request.EspecialidadeId) && Guid.TryParse(request.EspecialidadeId, out Guid especialidadeId))
            {
                updatedMedico.EspecialidadeId = especialidadeId;
            }
            else
            {
                updatedMedico.EspecialidadeId = null;
            }

            // Converter IdUtilizador de string para Guid
            if (!string.IsNullOrWhiteSpace(request.IdUtilizador) && Guid.TryParse(request.IdUtilizador, out Guid idUtilizador))
            {
                updatedMedico.IdUtilizador = idUtilizador;
            }
            else
            {
                updatedMedico.IdUtilizador = null;
            }

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(MedicoInDb.Id);
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

                Medico response;

                try
                {
                  response = await _repository.UpdateAsync<Medico, Guid>(updatedMedico);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = MedicoInDb;
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
                    return ResponseFactory.Fail<Guid>($"Medico atualizado, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Medico
        public async Task<Response<Guid>> DeleteMedicoAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"Medico eliminado, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                Medico? Medico = await _repository.RemoveByIdAsync<Medico, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Medico.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Medicos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMedicoAsync(IEnumerable<Guid> ids)
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
                Medico? entity = await _repository.GetByIdAsync<Medico, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"Medico com ID {id}.");
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
                  failedDeletions.Add($"Medico com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                Medico? deletedEntity = await _repository.RemoveByIdAsync<Medico, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);

                }
                else
                {
                  failedDeletions.Add($"Medico com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Medico com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} medicos.";
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
