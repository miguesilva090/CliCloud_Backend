using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Fornecedores;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.FornecedoresService.FornecedorService.DTOs;
using CliCloud.Application.Services.FornecedoresService.FornecedorService.Filters;
using CliCloud.Application.Services.FornecedoresService.FornecedorService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;
using CliCloud.Domain.Enums;

// After creating this service:
// -- 1. Create a Fornecedor domain entity in CliCloud.Domain/Entities/Fornecedores
// -- 2. Add DbSet<Fornecedor> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a Fornecedor api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.FornecedoresService.FornecedorService
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public FornecedorService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<FornecedorDTO>>> GetFornecedorAsync(string keyword = "")
        {
            FornecedorSearchList specification = new(keyword);
            IEnumerable<FornecedorDTO> list = await _repository.GetListAsync<Fornecedor, FornecedorDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<FornecedorDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<FornecedorLightDTO>>> GetFornecedorLightAsync(string keyword = "")
        {
            FornecedorSearchList specification = new(keyword);
            IEnumerable<FornecedorLightDTO> list = await _repository.GetListAsync<Fornecedor, FornecedorLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<FornecedorLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<FornecedorTableDTO>> GetFornecedorPaginatedAsync(FornecedorTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            FornecedorSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<FornecedorTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Fornecedor, FornecedorTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all Fornecedores (non-paginated)
        public async Task<Response<IEnumerable<FornecedorTableDTO>>> GetAllFornecedorAsync(FornecedorAllFilter filter)
        {
            try
            {
                filter ??= new FornecedorAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                FornecedorSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<FornecedorTableDTO> list = await _repository.GetListAsync<Fornecedor, FornecedorTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<FornecedorTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<FornecedorTableDTO>>(ex.Message);
            }
        }

        // get single Fornecedor by Id (com Includes para Rua, morada, EntidadeContactos)
        public async Task<Response<FornecedorDTO>> GetFornecedorAsync(Guid id)
        {
            try
            {
                FornecedorByIdWithIncludes specification = new(id);
                FornecedorDTO dto = await _repository.GetByIdAsync<Fornecedor, FornecedorDTO, Guid>(id, specification);
                return ResponseFactory.Success<FornecedorDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<FornecedorDTO>(ex.Message);
            }
        }

        // get single Fornecedor by NContrib
        public async Task<Response<FornecedorDTO>> GetFornecedorByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<FornecedorDTO>("Numero de Contribuinte não pode ser vazio");
                }

                FornecedorMatchNContrib specification = new(ncontrib);
                IEnumerable<FornecedorDTO> results = await _repository.GetListAsync<Fornecedor, FornecedorDTO, Guid>(specification);

                FornecedorDTO? fornecedor = results.FirstOrDefault();
                if(fornecedor == null)
                {
                  return ResponseFactory.Fail<FornecedorDTO>("Não foi encontrado nenhum Fornecedor com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<FornecedorDTO>(fornecedor);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<FornecedorDTO>(ex.Message);
            }
        }

        // get multiple Fornecedores by Nome 
        public async Task<Response<IEnumerable<FornecedorDTO>>> GetFornecedorByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<FornecedorDTO>>("Nome não pode ser vazio");
            }

            FornecedorSearchByName specification = new(nome);
            IEnumerable<FornecedorDTO> results = await _repository.GetListAsync<Fornecedor, FornecedorDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<FornecedorDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<FornecedorDTO>>(ex.Message);
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

        // create new Fornecedor
        public async Task<Response<Guid>> CreateFornecedorAsync(CreateFornecedorRequest request)
        {
            FornecedorMatchName specification = new(request.Nome);
            bool FornecedorExists = await _repository.ExistsAsync<Fornecedor, Guid>(specification);
            if (FornecedorExists)
            {
                return ResponseFactory.Fail<Guid>("Fornecedor já existe");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Fornecedor newFornecedor = _mapper.Map(request, new Fornecedor());
            newFornecedor.TipoEntidade = EntidadeTipo.Fornecedor;

            // Mapear InstituicaoFinanceiraId se fornecido
            if(!string.IsNullOrWhiteSpace(request.InstituicaoFinanceiraId) && Guid.TryParse(request.InstituicaoFinanceiraId, out Guid instituicaoFinanceiraGuid))
            {
              newFornecedor.InstituicaoFinanceiraId = instituicaoFinanceiraGuid;
            }

            try
            {
                Fornecedor response = await _repository.CreateAsync<Fornecedor, Guid>(newFornecedor);
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
                    return ResponseFactory.Fail<Guid>($"Fornecedor criado, mas falhou ao criar contactos: {errorMessage}");
                  }
                }
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Fornecedor
        public async Task<Response<Guid>> UpdateFornecedorAsync(UpdateFornecedorRequest request, Guid id)
        {
            Fornecedor FornecedorInDb = await _repository.GetByIdAsync<Fornecedor, Guid>(id);
            if (FornecedorInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Fornecedor não encontrado");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Fornecedor updatedFornecedor = _mapper.Map(request, FornecedorInDb);
            updatedFornecedor.TipoEntidade = EntidadeTipo.Fornecedor;

            // Mapear InstituicaoFinanceiraId se fornecido
            if(!string.IsNullOrWhiteSpace(request.InstituicaoFinanceiraId) && Guid.TryParse(request.InstituicaoFinanceiraId, out Guid instituicaoFinanceiraGuid))
            {
              updatedFornecedor.InstituicaoFinanceiraId = instituicaoFinanceiraGuid;
            }
            else if(string.IsNullOrWhiteSpace(request.InstituicaoFinanceiraId))
            {
              updatedFornecedor.InstituicaoFinanceiraId = null;
            }

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(FornecedorInDb.Id);
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

                Fornecedor response;

                try
                {
                  response = await _repository.UpdateAsync<Fornecedor, Guid>(updatedFornecedor);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = FornecedorInDb;
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
                    return ResponseFactory.Fail<Guid>($"Fornecedor atualizado, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Fornecedor
        public async Task<Response<Guid>> DeleteFornecedorAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"Fornecedor eliminado, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                Fornecedor? Fornecedor = await _repository.RemoveByIdAsync<Fornecedor, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Fornecedor.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Fornecedores
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleFornecedorAsync(IEnumerable<Guid> ids)
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
                Fornecedor? entity = await _repository.GetByIdAsync<Fornecedor, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"Fornecedor com ID {id} não encontrado.");
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
                  failedDeletions.Add($"Fornecedor com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                Fornecedor? deletedEntity = await _repository.RemoveByIdAsync<Fornecedor, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"Fornecedor com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Fornecedor com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} fornecedores.";
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
