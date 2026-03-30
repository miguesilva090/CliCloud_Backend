using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Funcionarios;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Funcionarios.FuncionarioService.DTOs;
using CliCloud.Application.Services.Funcionarios.FuncionarioService.Filters;
using CliCloud.Application.Services.Funcionarios.FuncionarioService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Funcionarios.FuncionarioService
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public FuncionarioService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<FuncionarioDTO>>> GetFuncionarioAsync(string keyword = "")
        {
            FuncionarioSearchList specification = new(keyword);
            IEnumerable<FuncionarioDTO> list = await _repository.GetListAsync<Funcionario, FuncionarioDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<FuncionarioDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<FuncionarioLightDTO>>> GetFuncionarioLightAsync(string keyword = "")
        {
            FuncionarioSearchList specification = new(keyword);
            IEnumerable<FuncionarioLightDTO> list = await _repository.GetListAsync<Funcionario, FuncionarioLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<FuncionarioLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<FuncionarioTableDTO>> GetFuncionarioPaginatedAsync(FuncionarioTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            FuncionarioSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<FuncionarioTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Funcionario, FuncionarioTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all Funcionarios (non-paginated)
        public async Task<Response<IEnumerable<FuncionarioTableDTO>>> GetAllFuncionarioAsync(FuncionarioAllFilter filter)
        {
            try
            {
                filter ??= new FuncionarioAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                FuncionarioSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<FuncionarioTableDTO> list = await _repository.GetListAsync<Funcionario, FuncionarioTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<FuncionarioTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<FuncionarioTableDTO>>(ex.Message);
            }
        }

        // get single Funcionario by Id (com Includes para Rua, CodigoPostal, EntidadeContactos)
        public async Task<Response<FuncionarioDTO>> GetFuncionarioAsync(Guid id)
        {
            try
            {
                FuncionarioByIdWithIncludes specification = new(id);
                FuncionarioDTO dto = await _repository.GetByIdAsync<Funcionario, FuncionarioDTO, Guid>(id, specification);
                return ResponseFactory.Success<FuncionarioDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FuncionarioDTO>(ex.Message);
            }
        }

        // get single Funcionario by NContrib
        public async Task<Response<FuncionarioDTO>> GetFuncionarioByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<FuncionarioDTO>("Numero de Contribuinte não pode ser vazio");
                }

                FuncionarioMatchNContrib specification = new(ncontrib);
                IEnumerable<FuncionarioDTO> results = await _repository.GetListAsync<Funcionario, FuncionarioDTO, Guid>(specification);

                FuncionarioDTO? funcionario = results.FirstOrDefault();
                if(funcionario == null)
                {
                  return ResponseFactory.Fail<FuncionarioDTO>("Não foi encontrado nenhum Funcionario com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<FuncionarioDTO>(funcionario);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<FuncionarioDTO>(ex.Message);
            }
        }

        // get multiple Funcionarios by Nome 
        public async Task<Response<IEnumerable<FuncionarioDTO>>> GetFuncionarioByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<FuncionarioDTO>>("Nome não pode ser vazio");
            }

            FuncionarioSearchByName specification = new(nome);
            IEnumerable<FuncionarioDTO> results = await _repository.GetListAsync<Funcionario, FuncionarioDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<FuncionarioDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<FuncionarioDTO>>(ex.Message);
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

        // create new Funcionario
        public async Task<Response<Guid>> CreateFuncionarioAsync(CreateFuncionarioRequest request)
        {
            FuncionarioMatchName specification = new(request.Nome);
            bool FuncionarioExists = await _repository.ExistsAsync<Funcionario, Guid>(specification);
            if (FuncionarioExists)
            {
                return ResponseFactory.Fail<Guid>("Funcionario já existe");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Funcionario newFuncionario = _mapper.Map(request, new Funcionario());
            newFuncionario.TipoEntidade = EntidadeTipo.Funcionario;

            try
            {
                Funcionario response = await _repository.CreateAsync<Funcionario, Guid>(newFuncionario);
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
                    return ResponseFactory.Fail<Guid>($"Funcionario criado, mas falhou ao criar contactos: {errorMessage}");
                  }
                }
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Funcionario
        public async Task<Response<Guid>> UpdateFuncionarioAsync(UpdateFuncionarioRequest request, Guid id)
        {
            Funcionario FuncionarioInDb = await _repository.GetByIdAsync<Funcionario, Guid>(id);
            if (FuncionarioInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Funcionario não encontrado");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Funcionario updatedFuncionario = _mapper.Map(request, FuncionarioInDb);
            updatedFuncionario.TipoEntidade = EntidadeTipo.Funcionario;

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(FuncionarioInDb.Id);
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

                Funcionario response;

                try
                {
                  response = await _repository.UpdateAsync<Funcionario, Guid>(updatedFuncionario);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = FuncionarioInDb;
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
                    return ResponseFactory.Fail<Guid>($"Funcionario atualizado, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Funcionario
        public async Task<Response<Guid>> DeleteFuncionarioAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"Funcionario eliminado, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                Funcionario? Funcionario = await _repository.RemoveByIdAsync<Funcionario, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Funcionario.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Funcionarios
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleFuncionarioAsync(IEnumerable<Guid> ids)
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
                Funcionario? entity = await _repository.GetByIdAsync<Funcionario, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"Funcionario com ID {id}.");
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
                  failedDeletions.Add($"Funcionario com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                Funcionario? deletedEntity = await _repository.RemoveByIdAsync<Funcionario, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);

                }
                else
                {
                  failedDeletions.Add($"Funcionario com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Funcionario com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} funcionarios.";
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
