using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Filters;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;

// After creating this service:
// -- 1. Create a Utente domain entity in CliCloud.Domain/Entities/Utentes
// -- 2. Add DbSet<Utente> to CliCloud.Infrastructure/Persistence/Contexts/ApplicationDbContext and create a new migration
// -- 3. Add mapping configuration for the new DTOs in CliCloud.Infrastructure/Mapper/MappingProfiles
// -- 4. Create a Utente api controller, you can use the command: dotnet new nano-controller -s (single name) -p (plural name) -ap (app name) -ui (spa/razor)
namespace CliCloud.Application.Services.Utentes.UtenteService
{
    public class UtenteService : IUtenteService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public UtenteService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<UtenteDTO>>> GetUtenteAsync(string keyword = "")
        {
            UtenteSearchList specification = new(keyword); // ardalis specification
            IEnumerable<UtenteDTO> list = await _repository.GetListAsync<Utente, UtenteDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<UtenteDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<UtenteLightDTO>>> GetUtenteLightAsync(string keyword = "")
        {
            UtenteSearchList specification = new(keyword); // ardalis specification
            IEnumerable<UtenteLightDTO> list = await _repository.GetListAsync<Utente, UtenteLightDTO, Guid>(specification); // full list, entity mapped to dto
            return ResponseFactory.Success<IEnumerable<UtenteLightDTO>>(list);
        }

        // get Tanstack Table paginated list (as seen in the React and Vue project tables)
        public async Task<PaginatedResponse<UtenteTableDTO>> GetUtentePaginatedAsync(UtenteTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0) // set to first page if any search filters are applied
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : ""; // possible dynamic ordering from datatable
            UtenteSearchTable specification = new(filter.Filters ?? [], dynamicOrder); // ardalis specification
            PaginatedResponse<UtenteTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Utente, UtenteTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;
        }

        // get all Utentes (non-paginated)
        public async Task<Response<IEnumerable<UtenteTableDTO>>> GetAllUtenteAsync(UtenteAllFilter filter)
        {
            try
            {
                filter ??= new UtenteAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                UtenteSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<UtenteTableDTO> list = await _repository.GetListAsync<Utente, UtenteTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<UtenteTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<UtenteTableDTO>>(ex.Message);
            }
        }

        // get single Utente by Id (carregar entidade com includes e mapear em memória para garantir Organismo/Empresa nas linhas)
        public async Task<Response<UtenteDTO>> GetUtenteAsync(Guid id)
        {
            try
            {
                UtenteByIdWithIncludes specification = new(id);
                Utente entity = await _repository.GetByIdAsync<Utente, Guid>(id, specification);
                UtenteDTO dto = _mapper.Map<Utente, UtenteDTO>(entity);
                return ResponseFactory.Success<UtenteDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<UtenteDTO>(ex.Message);
            }
        }

        // get single Utente by NContrib
        public async Task<Response<UtenteDTO>> GetUtenteByNContribAsync(string ncontrib)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(ncontrib))
                {
                  return ResponseFactory.Fail<UtenteDTO>("Numero de Contribuinte não pode ser vazio");
                }

                UtenteMatchNContrib specification = new(ncontrib);
                IEnumerable<UtenteDTO> results = await _repository.GetListAsync<Utente, UtenteDTO, Guid>(specification);

                UtenteDTO? utente = results.FirstOrDefault();
                if(utente == null)
                {
                  return ResponseFactory.Fail<UtenteDTO>("Não foi encontrado nenhum Utente com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<UtenteDTO>(utente);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<UtenteDTO>(ex.Message);
            }
        }

        // get multiple Utentes by Nome 
        public async Task<Response<IEnumerable<UtenteDTO>>> GetUtenteByNameAsync(string nome)
        {
          try
          {
            if(string.IsNullOrWhiteSpace(nome))
            {
              return ResponseFactory.Fail<IEnumerable<UtenteDTO>>("Nome não pode ser vazio");
            }

            UtenteSearchByName specification = new(nome);
            IEnumerable<UtenteDTO> results = await _repository.GetListAsync<Utente, UtenteDTO, Guid>(specification);

            return ResponseFactory.Success<IEnumerable<UtenteDTO>>(results);
          }
          catch(Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<UtenteDTO>>(ex.Message);
          }
        }

        // get single Utente by NumeroUtente
        public async Task<Response<UtenteDTO>> GetUtenteByNumeroUtenteAsync(string numeroUtente)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(numeroUtente))
                {
                  return ResponseFactory.Fail<UtenteDTO>("Numero de Utente não pode ser vazio");
                }

                UtenteMatchNumeroUtente specification = new(numeroUtente);
                IEnumerable<UtenteDTO> results = await _repository.GetListAsync<Utente, UtenteDTO, Guid>(specification);

                UtenteDTO? utente = results.FirstOrDefault();
                if(utente == null)
                {
                  return ResponseFactory.Fail<UtenteDTO>("Não foi encontrado nenhum Utente com o Numero de Utente fornecido");
                }

                return ResponseFactory.Success<UtenteDTO>(utente);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<UtenteDTO>(ex.Message);
            }
        }

        private static UtenteSubsistemaLinha MapSubsistemaLinhaRequestToEntity(UpsertUtenteSubsistemaLinhaItemRequest item, Guid utenteId)
        {
          return new UtenteSubsistemaLinha
          {
            UtenteId = utenteId,
            OrganismoId = Guid.TryParse(item.OrganismoId, out var oid) ? oid : null,
            Designacao = item.Designacao,
            NumeroBeneficiario = item.NumeroBeneficiario,
            Sigla = item.Sigla,
            NomeBeneficiario = item.NomeBeneficiario,
            DataCartao = item.DataCartao,
            NumeroApolice = item.NumeroApolice,
            EmpresaId = Guid.TryParse(item.EmpresaId, out var eid) ? eid : null,
          };
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

        // create new Utente
        public async Task<Response<Guid>> CreateUtenteAsync(CreateUtenteRequest request)
        {
            UtenteMatchName specification = new(request.Nome); // ardalis specification 
            bool UtenteExists = await _repository.ExistsAsync<Utente, Guid>(specification);
            if (UtenteExists)
            {
                return ResponseFactory.Fail<Guid>("Utente já existe");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Utente newUtente = _mapper.Map(request, new Utente()); // map dto to domain entity

            try
            {
                Utente response = await _repository.CreateAsync<Utente, Guid>(newUtente); // create new entity 
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
                    return ResponseFactory.Fail<Guid>($"Utente criado, mas falhou ao criar contactos: {errorMessage}");
                  }
                }

                // Subsistema de Saúde – comportamento alinhado com o legado:
                // - Se existir lista de linhas, usa-se tal e qual.
                // - Se NÃO houver linhas mas houver Empresa associada ao utente,
                //   cria-se automaticamente uma linha \"PARTICULAR\" ligada à Empresa.
                List<UpsertUtenteSubsistemaLinhaItemRequest> linhasRequest =
                  request.SubsistemaLinhas != null
                    ? request.SubsistemaLinhas.ToList()
                    : new List<UpsertUtenteSubsistemaLinhaItemRequest>();

                if (linhasRequest.Count == 0 && !string.IsNullOrWhiteSpace(request.EmpresaId))
                {
                  linhasRequest.Add(new UpsertUtenteSubsistemaLinhaItemRequest
                  {
                    OrganismoId = request.OrganismoId,
                    Designacao = "PARTICULAR",
                    NumeroBeneficiario = null,
                    Sigla = null,
                    NomeBeneficiario = null,
                    DataCartao = null,
                    NumeroApolice = null,
                    EmpresaId = request.EmpresaId,
                  });
                }

                if (linhasRequest.Count > 0)
                {
                  var linhas = linhasRequest
                    .Select(item => MapSubsistemaLinhaRequestToEntity(item, response.Id))
                    .ToList();
                  _ = await _repository.CreateRangeAsync<UtenteSubsistemaLinha, Guid>(linhas);
                  _ = await _repository.SaveChangesAsync();
                }

                return ResponseFactory.Success<Guid>(response.Id); // return id
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Utente
        public async Task<Response<Guid>> UpdateUtenteAsync(UpdateUtenteRequest request, Guid id)
        {
            Utente UtenteInDb = await _repository.GetByIdAsync<Utente, Guid>(id); // get existing entity
            if (UtenteInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Utente não encontrado");
            }

            if(request.UrlFoto != null)
            {
              request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Utente updatedUtente = _mapper.Map(request, UtenteInDb); // map dto to domain entity
            updatedUtente.TipoEntidade = EntidadeTipo.Utente; // garantir que nunca fica 0 na BD

            try
            {
                bool hasContactChanges = false;
                if( request.EntidadeContactos != null )
                {
                  EntidadeContactoSearchByEntidade contactosSpec = new(UtenteInDb.Id);
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

                Utente response;

                try
                {
                  response = await _repository.UpdateAsync<Utente, Guid>(updatedUtente);
                  _ = await _repository.SaveChangesAsync();
                }
                catch(Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                  response = UtenteInDb;
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
                    return ResponseFactory.Fail<Guid>($"Utente atualizado, mas falhou ao atualizar contactos: {errorMessage}");
                  }
                }

                // SubsistemaLinhas: null = não alterar linhas; lista (vazia ou não) = substituir todas as linhas por esta lista.
                // Alinhamento com legado:
                // - Se a lista vier vazia mas houver Empresa associada ao utente, cria-se automaticamente
                //   uma linha \"PARTICULAR\" ligada a essa Empresa.
                if (request.SubsistemaLinhas != null)
                {
                  List<UpsertUtenteSubsistemaLinhaItemRequest> linhasRequest = request.SubsistemaLinhas.ToList();

                  // Empresa efetiva do utente (pedido ou valor já existente)
                  string? effectiveEmpresaId = request.EmpresaId
                    ?? UtenteInDb.EmpresaId?.ToString();

                  if (linhasRequest.Count == 0 && !string.IsNullOrWhiteSpace(effectiveEmpresaId))
                  {
                    linhasRequest.Add(new UpsertUtenteSubsistemaLinhaItemRequest
                    {
                      OrganismoId = request.OrganismoId ?? UtenteInDb.OrganismoId?.ToString(),
                      Designacao = "PARTICULAR",
                      NumeroBeneficiario = null,
                      Sigla = null,
                      NomeBeneficiario = null,
                      DataCartao = null,
                      NumeroApolice = null,
                      EmpresaId = effectiveEmpresaId,
                    });
                  }

                  UtenteSubsistemaLinhaByUtenteId spec = new(response.Id);
                  IEnumerable<UtenteSubsistemaLinha> existingLinhas = await _repository.GetListAsync<UtenteSubsistemaLinha, Guid>(spec);
                  foreach (var linha in existingLinhas)
                    await _repository.RemoveAsync<UtenteSubsistemaLinha, Guid>(linha);

                  var newLinhas = linhasRequest
                    .Select(item => MapSubsistemaLinhaRequestToEntity(item, response.Id))
                    .ToList();

                  if (newLinhas.Count > 0)
                    _ = await _repository.CreateRangeAsync<UtenteSubsistemaLinha, Guid>(newLinhas);

                  _ = await _repository.SaveChangesAsync();
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                // Incluir inner exception para diagnóstico (ex.: erros do Entity Framework / BD)
                string message = ex.Message;
                Exception? inner = ex.InnerException;
                while (inner != null)
                {
                    message += " | " + inner.Message;
                    inner = inner.InnerException;
                }
                return ResponseFactory.Fail<Guid>(message);
            }
        }

        // delete Utente
        public async Task<Response<Guid>> DeleteUtenteAsync(Guid id)
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

                    return ResponseFactory.Fail<Guid>($"Utente eliminado, mas falhou ao eliminar contactos: {errorMessage}");
                  }
                }

                Utente? Utente = await _repository.RemoveByIdAsync<Utente, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(Utente.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Utentes
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleUtenteAsync(IEnumerable<Guid> ids)
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
                Utente? entity = await _repository.GetByIdAsync<Utente, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"Utente com ID {id}.");
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
                  failedDeletions.Add($"Utente com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                Utente? deletedEntity = await _repository.RemoveByIdAsync<Utente, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);

                }
                else
                {
                  failedDeletions.Add($"Utente com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Utente com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} utentes.";
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
