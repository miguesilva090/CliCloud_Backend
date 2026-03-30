using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Bancos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Bancos.BancoService.DTOs;
using CliCloud.Application.Services.Bancos.BancoService.Filters;
using CliCloud.Application.Services.Bancos.BancoService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;

namespace CliCloud.Application.Services.Bancos.BancoService
{
    public class BancoService : IBancoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public BancoService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<BancoDTO>>> GetBancoAsync(string keyword = "")
        {
            BancoSearchList specification = new(keyword);
            IEnumerable<BancoDTO> list = await _repository.GetListAsync<Banco, BancoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<BancoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<BancoLightDTO>>> GetBancoLightAsync(string keyword = "")
        {
            BancoSearchList specification = new(keyword);
            IEnumerable<BancoLightDTO> list = await _repository.GetListAsync<Banco, BancoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<BancoLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<BancoTableDTO>> GetBancoPaginatedAsync(BancoTableFilter filter)
        {
            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            BancoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<BancoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Banco, BancoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all Bancos (non-paginated)
        public async Task<Response<IEnumerable<BancoTableDTO>>> GetAllBancoAsync(BancoAllFilter filter)
        {
            try
            {
                filter ??= new BancoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                BancoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<BancoTableDTO> list = await _repository.GetListAsync<Banco, BancoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<BancoTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<BancoTableDTO>>(ex.Message);
            }
        }

        // get single Banco by Id 
        public async Task<Response<BancoDTO>> GetBancoAsync(Guid id)
        {
            try
            {
                BancoDTO dto = await _repository.GetByIdAsync<Banco, BancoDTO, Guid>(id);
                return ResponseFactory.Success<BancoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<BancoDTO>(ex.Message);
            }
        }

        // get single Banco by NContrib
        public async Task<Response<BancoDTO>> GetBancoByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<BancoDTO>("Numero de Contribuinte não pode ser vazio");
                }

                BancoMatchNContrib specification = new(ncontrib);
                IEnumerable<BancoDTO> results = await _repository.GetListAsync<Banco, BancoDTO, Guid>(specification);

                BancoDTO? banco = results.FirstOrDefault();
                if(banco == null)
                {
                  return ResponseFactory.Fail<BancoDTO>("Não foi encontrado nenhum Banco com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<BancoDTO>(banco);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<BancoDTO>(ex.Message);
            }
        }

        // get multiple Bancos by Nome 
        public async Task<Response<IEnumerable<BancoDTO>>> GetBancoByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<BancoDTO>>("Nome não pode ser vazio");
            }

            BancoSearchByName specification = new(nome);
            IEnumerable<BancoDTO> results = await _repository.GetListAsync<Banco, BancoDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<BancoDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<BancoDTO>>(ex.Message);
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

        // create new Banco
        public async Task<Response<Guid>> CreateBancoAsync(CreateBancoRequest request)
        {
            BancoMatchName specification = new(request.Nome);
            bool BancoExists = await _repository.ExistsAsync<Banco, Guid>(specification);
            if (BancoExists)
            {
                return ResponseFactory.Fail<Guid>("Banco já existe");
            }

            if (request.UrlFoto != null)
            {
                request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Banco newBanco = _mapper.Map<Banco>(request);
            newBanco.TipoEntidade = Domain.Enums.EntidadeTipo.Banco;

            try
            {
                Banco response = await _repository.CreateAsync<Banco, Guid>(newBanco);
                _ = await _repository.SaveChangesAsync();

                if (request.EntidadeContactos != null && request.EntidadeContactos.Any())
                {
                    var contactoRequest = new CreateEntidadeContactoBulkRequest
                    {
                        EntidadeId = response.Id.ToString(),
                        Contactos = request.EntidadeContactos,
                    };

                    var contactsResult = await _entidadeContactoService.CreateEntidadeContactoBulkAsync(contactoRequest);
                    if (contactsResult.Status != ResponseStatus.Success)
                    {
                        string errorMessage = contactsResult.Messages.TryGetValue("$", out var messages) && messages.Count > 0 ? messages.First() : "Erro desconhecido ao criar contactos";
                        return ResponseFactory.Fail<Guid>($"Banco criado, mas falhou ao criar contactos: {errorMessage}");
                    }
                }
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Banco
        public async Task<Response<Guid>> UpdateBancoAsync(UpdateBancoRequest request, Guid id)
        {
            Banco BancoInDb = await _repository.GetByIdAsync<Banco, Guid>(id);
            if (BancoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Banco não encontrado");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Banco updatedBanco = _mapper.Map(request, BancoInDb);
            updatedBanco.TipoEntidade = Domain.Enums.EntidadeTipo.Banco;

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(BancoInDb.Id);
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

                Banco response;

                try
                {
                  response = await _repository.UpdateAsync<Banco, Guid>(updatedBanco);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = BancoInDb;
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
                    return ResponseFactory.Fail<Guid>($"Banco atualizado, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Banco
        public async Task<Response<Guid>> DeleteBancoAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"Banco eliminado, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                Banco? Banco = await _repository.RemoveByIdAsync<Banco, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Banco.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Bancos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleBancoAsync(IEnumerable<Guid> ids)
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
                Banco? entity = await _repository.GetByIdAsync<Banco, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"Banco com ID {id} não encontrado.");
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
                  failedDeletions.Add($"Banco com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                Banco? deletedEntity = await _repository.RemoveByIdAsync<Banco, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"Banco com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Banco com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} bancos.";
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
