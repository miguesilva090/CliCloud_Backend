using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Application.Services.Documentos.DocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.DocumentoService.Filters;
using CliCloud.Application.Services.Documentos.DocumentoService.Specifications;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Application.Services.Core.SmsService;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Documentos.DocumentoService
{
    public class DocumentoService : IDocumentoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IServicoSms _servicoSms;

        public DocumentoService(IRepositoryAsync repository, IMapper mapper, IServicoSms servicoSms)
        {
            _repository = repository;
            _mapper = mapper;
            _servicoSms = servicoSms;
        }

        // get full List
        public async Task<Response<IEnumerable<DocumentoDTO>>> GetDocumentoAsync(string keyword = "")
        {
            DocumentoSearchList specification = new(keyword);
            IEnumerable<DocumentoDTO> list = await _repository.GetListAsync<Documento, DocumentoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<DocumentoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<DocumentoLightDTO>>> GetDocumentoLightAsync(string keyword = "")
        {
            DocumentoSearchList specification = new(keyword);
            IEnumerable<DocumentoLightDTO> list = await _repository.GetListAsync<Documento, DocumentoLightDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<DocumentoLightDTO>>(list);
        }

        // get Tanstack Table paginated list
        public async Task<PaginatedResponse<DocumentoTableDTO>> GetDocumentoPaginatedAsync(DocumentoTableFilter filter)
        {
            if (filter.Filters != null && filter.Filters.Count > 0)
            {
                filter.PageNumber = 1;
            }

            string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
            DocumentoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
            PaginatedResponse<DocumentoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Documento, DocumentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
            return pagedResponse;
        }

        // get all Documentos (non-paginated)
        public async Task<Response<IEnumerable<DocumentoTableDTO>>> GetAllDocumentoAsync(DocumentoAllFilter filter)
        {
            try
            {
                filter ??= new DocumentoAllFilter();

                string dynamicOrder = filter.GetOrderByString();
                List<TableFilter> tableFilters = filter.Filters ?? new List<TableFilter>();
                DocumentoSearchTable specification = new(tableFilters, dynamicOrder);
                IEnumerable<DocumentoTableDTO> list = await _repository.GetListAsync<Documento, DocumentoTableDTO, Guid>(specification);
                return ResponseFactory.Success<IEnumerable<DocumentoTableDTO>>(list);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<IEnumerable<DocumentoTableDTO>>(ex.Message);
            }
        }

        // get single Documento by Id 
        public async Task<Response<DocumentoDTO>> GetDocumentoAsync(Guid id)
        {
            try
            {
                DocumentoDTO dto = await _repository.GetByIdAsync<Documento, DocumentoDTO, Guid>(id);
                return ResponseFactory.Success<DocumentoDTO>(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<DocumentoDTO>(ex.Message);
            }
        }

        // get single Documento by TipoDocumentoId and NumeroDocumento
        public async Task<Response<DocumentoDTO>> GetDocumentoByTipoNumeroAsync(Guid tipoDocumentoId, int numeroDocumento)
        {
            try
            {
                DocumentoMatchTipoNumero specification = new(tipoDocumentoId, numeroDocumento);
                IEnumerable<DocumentoDTO> results = await _repository.GetListAsync<Documento, DocumentoDTO, Guid>(specification);

                DocumentoDTO? documento = results.FirstOrDefault();
                if(documento == null)
                {
                  return ResponseFactory.Fail<DocumentoDTO>("Não foi encontrado nenhum Documento com o TipoDocumentoId e NumeroDocumento fornecidos");
                }

                return ResponseFactory.Success<DocumentoDTO>(documento);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<DocumentoDTO>(ex.Message);
            }
        }

        // create new Documento
        public async Task<Response<Guid>> CreateDocumentoAsync(CreateDocumentoRequest request)
        {
            // Verificar unicidade do número de documento por tipo
            if(!Guid.TryParse(request.TipoDocumentoId, out Guid tipoDocumentoId))
            {
                return ResponseFactory.Fail<Guid>("TipoDocumentoId inválido");
            }

            DocumentoMatchTipoNumero specification = new(tipoDocumentoId, request.NumeroDocumento);
            bool DocumentoExists = await _repository.ExistsAsync<Documento, Guid>(specification);
            if (DocumentoExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um Documento com este TipoDocumentoId e NumeroDocumento");
            }

            Documento newDocumento = _mapper.Map(request, new Documento());
            newDocumento.TipoDocumentoId = tipoDocumentoId;
            
            // Converter IDs opcionais de string para Guid
            if(!string.IsNullOrWhiteSpace(request.UtenteId) && Guid.TryParse(request.UtenteId, out Guid utenteId))
            {
                newDocumento.UtenteId = utenteId;
            }
            if(!string.IsNullOrWhiteSpace(request.OrganismoId) && Guid.TryParse(request.OrganismoId, out Guid organismoId))
            {
                newDocumento.OrganismoId = organismoId;
            }
            if(!string.IsNullOrWhiteSpace(request.FuncionarioId) && Guid.TryParse(request.FuncionarioId, out Guid funcionarioId))
            {
                newDocumento.FuncionarioId = funcionarioId;
            }
            if(!string.IsNullOrWhiteSpace(request.CodigoPostalId) && Guid.TryParse(request.CodigoPostalId, out Guid codigoPostalId))
            {
                newDocumento.CodigoPostalId = codigoPostalId;
            }

            try
            {
                Documento response = await _repository.CreateAsync<Documento, Guid>(newDocumento);
                _ = await _repository.SaveChangesAsync();
                await TentarDispararSmsFaturacaoAsync(response);
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // update Documento
        public async Task<Response<Guid>> UpdateDocumentoAsync(UpdateDocumentoRequest request, Guid id)
        {
            Documento DocumentoInDb = await _repository.GetByIdAsync<Documento, Guid>(id);
            if (DocumentoInDb == null)
            {
                return ResponseFactory.Fail<Guid>("Documento não encontrado");
            }

            // Verificar se o número de documento já existe em outro registro do mesmo tipo
            if(!Guid.TryParse(request.TipoDocumentoId, out Guid tipoDocumentoId))
            {
                return ResponseFactory.Fail<Guid>("TipoDocumentoId inválido");
            }

            if (DocumentoInDb.TipoDocumentoId != tipoDocumentoId || DocumentoInDb.NumeroDocumento != request.NumeroDocumento)
            {
                DocumentoMatchTipoNumero specification = new(tipoDocumentoId, request.NumeroDocumento);
                bool TipoNumeroExists = await _repository.ExistsAsync<Documento, Guid>(specification);
                if (TipoNumeroExists)
                {
                    return ResponseFactory.Fail<Guid>("Já existe um Documento com este TipoDocumentoId e NumeroDocumento");
                }
            }

            Documento updatedDocumento = _mapper.Map(request, DocumentoInDb);
            updatedDocumento.TipoDocumentoId = tipoDocumentoId;
            
            // Converter IDs opcionais de string para Guid
            if(!string.IsNullOrWhiteSpace(request.UtenteId) && Guid.TryParse(request.UtenteId, out Guid utenteId))
            {
                updatedDocumento.UtenteId = utenteId;
            }
            else
            {
                updatedDocumento.UtenteId = null;
            }
            if(!string.IsNullOrWhiteSpace(request.OrganismoId) && Guid.TryParse(request.OrganismoId, out Guid organismoId))
            {
                updatedDocumento.OrganismoId = organismoId;
            }
            else
            {
                updatedDocumento.OrganismoId = null;
            }
            if(!string.IsNullOrWhiteSpace(request.FuncionarioId) && Guid.TryParse(request.FuncionarioId, out Guid funcionarioId))
            {
                updatedDocumento.FuncionarioId = funcionarioId;
            }
            else
            {
                updatedDocumento.FuncionarioId = null;
            }
            if(!string.IsNullOrWhiteSpace(request.CodigoPostalId) && Guid.TryParse(request.CodigoPostalId, out Guid codigoPostalId))
            {
                updatedDocumento.CodigoPostalId = codigoPostalId;
            }
            else
            {
                updatedDocumento.CodigoPostalId = null;
            }

            try
            {
                Documento response = await _repository.UpdateAsync<Documento, Guid>(updatedDocumento);
                _ = await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(response.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete Documento
        public async Task<Response<Guid>> DeleteDocumentoAsync(Guid id)
        {
            try
            {
                Documento? Documento = await _repository.RemoveByIdAsync<Documento, Guid>(id);
                if (Documento == null)
                {
                    return ResponseFactory.Fail<Guid>("Documento não encontrado");
                }
                await _repository.SaveChangesAsync();
                return ResponseFactory.Success<Guid>(Documento.Id);
            }
            catch(Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        // delete multiple Documentos
        public async Task<Response<IEnumerable<Guid>>> DeleteMultipleDocumentoAsync(IEnumerable<Guid> ids)
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
                Documento? entity = await _repository.GetByIdAsync<Documento, Guid>(id);
                if(entity == null)
                {
                  failedDeletions.Add($"Documento com ID {id} não encontrado.");
                  continue;
                }

                Documento? deletedEntity = await _repository.RemoveByIdAsync<Documento, Guid>(id);
                if(deletedEntity != null)
                {
                  _ = await _repository.SaveChangesAsync();
                  successfullyDeletedIds.Add(id);
                }
                else
                {
                  failedDeletions.Add($"Documento com ID {id}.");
                }
              }
              catch(Exception)
              {
                failedDeletions.Add($"Documento com ID {id}.");
                _repository.ClearChangeTracker();
              }
            }

            if(successfullyDeletedIds.Count == idsList.Count)
            {
              return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
            }
            else if(successfullyDeletedIds.Count > 0)
            {
              string message = $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} documentos.";
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

        private async Task TentarDispararSmsFaturacaoAsync(Documento documento)
        {
            try
            {
                var tipoDocumento = await _repository.GetByIdAsync<TipoDocumento, Guid>(documento.TipoDocumentoId);
                if (tipoDocumento is null || !tipoDocumento.MostraFaturacao) return;

                if (!documento.UtenteId.HasValue) return;
                var utente = await _repository.GetByIdAsync<Domain.Entities.Utentes.Utente, Guid>(documento.UtenteId.Value);
                if (utente is null) return;

                var contactos = await _repository.GetListAsync<EntidadeContacto, Guid>();
                var numero = contactos
                    .Where(x => x.EntidadeId == utente.Id && !string.IsNullOrWhiteSpace(x.Valor))
                    .OrderByDescending(x => x.Principal)
                    .Select(x => x.Valor!)
                    .FirstOrDefault();
                if (string.IsNullOrWhiteSpace(numero)) return;

                var clinica = (await _repository.GetListAsync<Clinica, Guid>(new ClinicaPorDefeitoSelected())).FirstOrDefault();
                if (clinica is null) return;

                var request = new EnviarSmsPorCodigoRequest
                {
                    CodigoConfiguracao = "8",
                    NumeroDestinatario = numero,
                    NomeUtente = utente.Nome ?? string.Empty,
                    Data = documento.Data,
                    Modulo = "Documento-Faturacao",
                };

                _ = await _servicoSms.EnviarSmsPorCodigoAsync(clinica.Id, request);
            }
            catch
            {
                // Não bloquear o fluxo de criação de documento por falha de SMS.
            }
        }
    }
}
