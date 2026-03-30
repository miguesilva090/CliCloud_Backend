using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Empresas;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Empresas.EmpresaService.DTOs;
using CliCloud.Application.Services.Empresas.EmpresaService.Filters;
using CliCloud.Application.Services.Empresas.EmpresaService.Specifications;
using CliCloud.Application.Services.Utility.EntidadeContactoService;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.Specifications;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Empresas.EmpresaService
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IEntidadeContactoService _entidadeContactoService;

        public EmpresaService(IRepositoryAsync repository, IMapper mapper, IEntidadeContactoService entidadeContactoService)
        {
            _repository = repository;
            _mapper = mapper;
            _entidadeContactoService = entidadeContactoService;
        }

        // get full List
        public async Task<Response<IEnumerable<EmpresaDTO>>> GetEmpresaAsync(string keyword = "")
        {
            EmpresaSearchList specification = new(keyword);
            IEnumerable<EmpresaDTO> list = await _repository.GetListAsync<Empresa, EmpresaDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<EmpresaDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<EmpresaTableDTO>> GetEmpresaPaginatedAsync(EmpresaTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            EmpresaSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<EmpresaTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Empresa, EmpresaTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all Empresas (non-paginated)
        public async Task<Response<IEnumerable<EmpresaTableDTO>>> GetAllEmpresaAsync(EmpresaAllFilter filter)
        {
            try
            {
                filter ??= new EmpresaAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                EmpresaSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<EmpresaTableDTO> list = await _repository.GetListAsync<Empresa, EmpresaTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<EmpresaTableDTO>>(list);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<EmpresaTableDTO>>(ex.Message);
            }
        }

        // get single Empresa by Id
        public async Task<Response<EmpresaDTO>> GetEmpresaAsync(Guid id)
        {
            try
            {
                EmpresaByIdWithIncludes specification = new(id);
                EmpresaDTO dto = await _repository.GetByIdAsync<Empresa, EmpresaDTO, Guid>(id, specification);
                return ResponseFactory.Success<EmpresaDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<EmpresaDTO>(ex.Message);
            }
        }

        // get single Empresa by NContrib
        public async Task<Response<EmpresaDTO>> GetEmpresaByNContribAsync(string ncontrib)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ncontrib))
                {
                    return ResponseFactory.Fail<EmpresaDTO>("Numero de Contribuinte não pode ser vazio");
                }

                EmpresaMatchNContrib specification = new(ncontrib);
                IEnumerable<EmpresaDTO> results = await _repository.GetListAsync<Empresa, EmpresaDTO, Guid>(specification);

                EmpresaDTO? empresa = results.FirstOrDefault();
                if (empresa == null)
                {
                    return ResponseFactory.Fail<EmpresaDTO>("Não foi encontrada nenhuma Empresa com o Numero de Contribuinte fornecido");
                }

                return ResponseFactory.Success<EmpresaDTO>(empresa);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<EmpresaDTO>(ex.Message);
            }
        }

        // get multiple Empresas by Nome 
        public async Task<Response<IEnumerable<EmpresaDTO>>> GetEmpresaByNameAsync(string nome)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    return ResponseFactory.Fail<IEnumerable<EmpresaDTO>>("Nome não pode ser vazio");
                }

                EmpresaSearchByName specification = new(nome);
                IEnumerable<EmpresaDTO> results = await _repository.GetListAsync<Empresa, EmpresaDTO, Guid>(specification);

                return ResponseFactory.Success<IEnumerable<EmpresaDTO>>(results);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<EmpresaDTO>>(ex.Message);
            }
        }

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

            if (imageUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase) || imageUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
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

        // create new Empresa
        public async Task<Response<Guid>> CreateEmpresaAsync(CreateEmpresaRequest request)
        {
            EmpresaSearchByName specification = new(request.Nome);
            bool empresaExists = await _repository.ExistsAsync<Empresa, Guid>(specification);
            if (empresaExists)
            {
                return ResponseFactory.Fail<Guid>("Empresa já existe");
            }

            if (request.UrlFoto != null)
            {
                request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Empresa newEmpresa = _mapper.Map(request, new Empresa());
            newEmpresa.TipoEntidade = EntidadeTipo.Empresa;

            if (!string.IsNullOrWhiteSpace(request.BancoId) && Guid.TryParse(request.BancoId, out Guid bancoGuid))
            {
                newEmpresa.BancoId = bancoGuid;
            }

            try
            {
                Empresa response = await _repository.CreateAsync<Empresa, Guid>(newEmpresa);
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
                        return ResponseFactory.Fail<Guid>($"Empresa criada, mas falhou ao criar contactos: {errorMessage}");
                    }
                }
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Empresa
        public async Task<Response<Guid>> UpdateEmpresaAsync(UpdateEmpresaRequest request, Guid id)
        {
            Empresa empresaInDb = await _repository.GetByIdAsync<Empresa, Guid>(id);
            if (empresaInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Empresa não encontrada");
            }

            if (request.UrlFoto != null)
            {
                request.UrlFoto = ConvertToPartialUrl(request.UrlFoto);
            }

            Empresa updatedEmpresa = _mapper.Map(request, empresaInDb);
            updatedEmpresa.TipoEntidade = EntidadeTipo.Empresa;

            if (!string.IsNullOrWhiteSpace(request.BancoId) && Guid.TryParse(request.BancoId, out Guid bancoGuid))
            {
                updatedEmpresa.BancoId = bancoGuid;
            }
            else if (string.IsNullOrWhiteSpace(request.BancoId))
            {
                updatedEmpresa.BancoId = null;
            }

            try
            {
                bool hasContactChanges = false;
                if (request.EntidadeContactos != null)
                {
                    EntidadeContactoSearchByEntidade contactosSpec = new(empresaInDb.Id);
                    IEnumerable<EntidadeContacto> existingContactos = await _repository.GetListAsync<EntidadeContacto, Guid>(contactosSpec);

                    var requestedByTipo = request.EntidadeContactos.ToDictionary(c => c.EntidadeContactoTipoId, c => c);
                    var existingByTipo = existingContactos.ToDictionary(c => c.EntidadeContactoTipoId, c => c);

                    if (requestedByTipo.Count != existingByTipo.Count || requestedByTipo.Keys.Except(existingByTipo.Keys).Any() || existingByTipo.Keys.Except(requestedByTipo.Keys).Any())
                    {
                        hasContactChanges = true;
                    }
                    else
                    {
                        foreach (var kvp in requestedByTipo)
                        {
                            var tipoId = kvp.Key;
                            var req = kvp.Value;
                            if (!existingByTipo.TryGetValue(tipoId, out var ex))
                            {
                                hasContactChanges = true;
                                break;
                            }

                            if (!string.Equals(ex.Valor, req.Valor, StringComparison.Ordinal) || ex.Principal != req.Principal)
                            {
                                hasContactChanges = true;
                                break;
                            }
                        }
                    }
                }

                Empresa response;

                try
                {
                    response = await _repository.UpdateAsync<Empresa, Guid>(updatedEmpresa);
                    _ = await _repository.SaveChangesAsync();
                }
                catch (Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
                {
                    response = empresaInDb;
                }

                if (hasContactChanges)
                {
                    var contactoRequest = new UpsertEntidadeContactoBulkRequest
                    {
                        EntidadeId = response.Id.ToString(),
                        Contactos = request.EntidadeContactos!,
                    };

                    var contactsResult = await _entidadeContactoService.UpsertEntidadeContactoBulkAsync(contactoRequest);
                    if (contactsResult.Status != ResponseStatus.Success)
                    {
                        string errorMessage =
                          contactsResult.Messages.TryGetValue("$", out var messages) && messages.Count > 0
                          ? messages.First()
                          : "Erro desconhecido ao atualizar contactos";
                        return ResponseFactory.Fail<Guid>($"Empresa atualizada, mas falhou ao atualizar contactos: {errorMessage}");
                    }
                }

                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Empresa
        public async Task<Response<Guid>> DeleteEmpresaAsync(Guid id)
        {
            try
            {
                EntidadeContactoSearchByEntidade specification = new(id);
                IEnumerable<EntidadeContacto> contacts = await _repository.GetListAsync<EntidadeContacto, Guid>(specification);

                foreach (var contacto in contacts)
                {
                    var deleteResult = await _entidadeContactoService.DeleteEntidadeContactoAsync(contacto.Id);
                    if (deleteResult.Status != ResponseStatus.Success)
                    {
                        string errorMessage =
                          deleteResult.Messages.TryGetValue("$", out var messages) && messages.Count > 0
                          ? messages.First()
                          : "Erro desconhecido ao eliminar contactos";

                        return ResponseFactory.Fail<Guid>($"Empresa eliminada, mas falhou ao eliminar contactos: {errorMessage}");
                    }
                }

                Empresa? empresa = await _repository.RemoveByIdAsync<Empresa, Guid>(id);
                await _repository.SaveChangesAsync();

                return ResponseFactory.Success<Guid>(empresa.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Empresas
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleEmpresaAsync(IEnumerable<Guid> ids)
        {
          try
          {
            List<Guid> idsList = ids.ToList();
            List<Guid> successfullyDeletedIds = [];
            List<string> failedDeletions = [];

            foreach (Guid id in idsList)
            {
              try
              {
                Empresa? entity = await _repository.GetByIdAsync<Empresa, Guid>(id);
                if (entity == null)
                {
                  failedDeletions.Add($"Empresa com ID {id} não encontrada.");
                  continue;
                }

                EntidadeContactoSearchByEntidade specification = new(id);
                IEnumerable<EntidadeContacto> contacts = await _repository.GetListAsync<
                  EntidadeContacto,
                  Guid
                >(specification);

                bool contactsDeletedSuccessfully = true;
                foreach (var contact in contacts)
                {
                  var deleteResult = await _entidadeContactoService.DeleteEntidadeContactoAsync(contact.Id);

                  if (deleteResult.Status != ResponseStatus.Success)
                  {
                    contactsDeletedSuccessfully = false;
                    break;
                  }
                }

                if (!contactsDeletedSuccessfully)
                {
                  failedDeletions.Add($"Empresa com ID {id}.");
                  _repository.ClearChangeTracker();
                  continue;
                }

                Empresa? deletedEntity = await _repository.RemoveByIdAsync<Empresa, Guid>(id);
                if (deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"Empresa com ID {id}.");
                }
              }
              catch (Exception)
              {
                failedDeletions.Add($"Empresa com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if (successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if (successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminadas com sucesso {successfullyDeletedIds.Count} de {idsList.Count} empresas.";
              return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, message);
            }
            else
            {
              return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
            }
          }
          catch (Exception ex)
          {
            return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
          }
        }
    }
}

