using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Application.Services.Documentos.DocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.DocumentoService.Filters;
using CliCloud.Application.Services.Documentos.DocumentoService.Specifications;
using CliCloud.Application.Services.Consultas.ConsultaService.Specifications;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Application.Services.Core.SmsService;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Application.Common.Mailer;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;
using System.Text;

namespace CliCloud.Application.Services.Documentos.DocumentoService
{
    public class DocumentoService : IDocumentoService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IServicoSms _servicoSms;
        private readonly ICurrentClinicaService _currentClinicaService;
        private readonly IMailService _mailService;

        public DocumentoService(IRepositoryAsync repository, IMapper mapper, IServicoSms servicoSms, ICurrentClinicaService currentClinicaService, IMailService mailService)
        {
            _repository = repository;
            _mapper = mapper;
            _servicoSms = servicoSms;
            _currentClinicaService = currentClinicaService;
            _mailService = mailService;
        }

        // get full List
        public async Task<Response<IEnumerable<DocumentoDTO>>> GetDocumentoAsync(string keyword = "")
        {
            DocumentoSearchList specification = new(keyword, GetCurrentClinicaId());
            IEnumerable<DocumentoDTO> list = await _repository.GetListAsync<Documento, DocumentoDTO, Guid>(specification);
            return ResponseFactory.Success<IEnumerable<DocumentoDTO>>(list);
        }

        // get lightweight list 
        public async Task<Response<IEnumerable<DocumentoLightDTO>>> GetDocumentoLightAsync(string keyword = "")
        {
            DocumentoSearchList specification = new(keyword, GetCurrentClinicaId());
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
            DocumentoSearchTable specification = new(filter.Filters ?? [], GetCurrentClinicaId(), dynamicOrder);
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
                DocumentoSearchTable specification = new(tableFilters, GetCurrentClinicaId(), dynamicOrder);
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
                if (!TryGetClinicaId(out Guid clinicaId, out Response<DocumentoDTO>? clinicaError))
                    return clinicaError!;

                IEnumerable<DocumentoDTO> results = await _repository.GetListAsync<Documento, DocumentoDTO, Guid>(
                    new DocumentoByIdClinicaSpec(id, clinicaId));
                DocumentoDTO? dto = results.FirstOrDefault();
                if (dto == null)
                    return ResponseFactory.Fail<DocumentoDTO>("Documento não encontrado");

                return ResponseFactory.Success(dto);
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
                if (!TryGetClinicaId(out Guid clinicaId, out Response<DocumentoDTO>? clinicaError))
                    return clinicaError!;

                DocumentoMatchTipoNumero specification = new(tipoDocumentoId, numeroDocumento, clinicaId);
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
            if (!TryGetClinicaId(out Guid clinicaId, out Response<Guid>? clinicaError))
                return clinicaError!;

            // Verificar unicidade do número de documento por tipo
            if(!Guid.TryParse(request.TipoDocumentoId, out Guid tipoDocumentoId))
            {
                return ResponseFactory.Fail<Guid>("TipoDocumentoId inválido");
            }

            DocumentoMatchTipoNumero specification = new(tipoDocumentoId, request.NumeroDocumento, clinicaId);
            bool DocumentoExists = await _repository.ExistsAsync<Documento, Guid>(specification);
            if (DocumentoExists)
            {
                return ResponseFactory.Fail<Guid>("Já existe um Documento com este TipoDocumentoId e NumeroDocumento");
            }

            Documento newDocumento = _mapper.Map(request, new Documento());
            newDocumento.ClinicaId = clinicaId;
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
            if (!TryGetClinicaId(out Guid clinicaId, out Response<Guid>? clinicaError))
                return clinicaError!;

            Documento? DocumentoInDb = (
                await _repository.GetListAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(id, clinicaId))
            ).FirstOrDefault();
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
                DocumentoMatchTipoNumero specification = new(tipoDocumentoId, request.NumeroDocumento, clinicaId);
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
                if (!TryGetClinicaId(out Guid clinicaId, out Response<Guid>? clinicaError))
                    return clinicaError!;

                bool exists = await _repository.ExistsAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(id, clinicaId));
                if (!exists)
                    return ResponseFactory.Fail<Guid>("Documento não encontrado");

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
            if (!TryGetClinicaId(out Guid clinicaId, out Response<IEnumerable<Guid>>? clinicaError))
                return clinicaError!;

            List<Guid> idsList = ids.ToList();
            List<Guid> successfullyDeletedIds = [];
            List<string> failedDeletions = [];

            foreach(Guid id in idsList)
            {
              try
              {
                Documento? entity = (
                    await _repository.GetListAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(id, clinicaId))
                ).FirstOrDefault();
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

        public async Task<Response<DocumentoPrintDTO>> GetDocumentoPrintAsync(Guid id)
        {
            try
            {
                if (!TryGetClinicaId(out Guid clinicaId, out Response<DocumentoPrintDTO>? clinicaError))
                    return clinicaError!;

                Documento? documento = (
                    await _repository.GetListAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(id, clinicaId))
                ).FirstOrDefault();

                if (documento == null)
                    return ResponseFactory.Fail<DocumentoPrintDTO>("Documento não encontrado");

                if (documento.Anulado || !documento.EstaEmitido)
                    return ResponseFactory.Fail<DocumentoPrintDTO>("Documento inválido para impressão/reimpressão");

                DocumentoPrintDTO dto = new()
                {
                    Template = ResolvePrintTemplate(documento),
                    IsAnulado = documento.Anulado,
                    IsEmitido = documento.EstaEmitido,
                    TipoSerie = documento.TipoSerie ?? string.Empty,
                    NumeroExibicao = documento.NumeroExibicao ?? $"{documento.NumeroDocumento}"
                };

                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<DocumentoPrintDTO>(ex.Message);
            }
        }

        public async Task<Response<DocumentoPrintDTO>> GetDocumentoPrintOriginalAsync(Guid id)
        {
            try
            {
                if (!TryGetClinicaId(out Guid clinicaId, out Response<DocumentoPrintDTO>? clinicaError))
                    return clinicaError!;

                Documento? documento = (
                    await _repository.GetListAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(id, clinicaId))
                ).FirstOrDefault();

                if (documento == null)
                    return ResponseFactory.Fail<DocumentoPrintDTO>("Documento não encontrado");

                if (documento.Anulado || !documento.EstaEmitido)
                    return ResponseFactory.Fail<DocumentoPrintDTO>("Documento inválido para impressão original");

                if (!string.Equals(documento.TipoSerie, "N", StringComparison.OrdinalIgnoreCase))
                    return ResponseFactory.Fail<DocumentoPrintDTO>("Impressão original só disponível para série normal");

                // Paridade com legado: registar evento de reimpressão original.
                // Nota: sem tabela dedicada neste momento, guardamos no campo observações.
                string stamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string linhaHistorico = $"[REIMP_ORIGINAL] {stamp}";
                documento.Observacoes = string.IsNullOrWhiteSpace(documento.Observacoes)
                    ? linhaHistorico
                    : $"{documento.Observacoes}{Environment.NewLine}{linhaHistorico}";
                _ = await _repository.UpdateAsync<Documento, Guid>(documento);
                _ = await _repository.SaveChangesAsync();

                DocumentoPrintDTO dto = new()
                {
                    Template = ResolvePrintTemplate(documento),
                    IsAnulado = documento.Anulado,
                    IsEmitido = documento.EstaEmitido,
                    TipoSerie = documento.TipoSerie ?? string.Empty,
                    NumeroExibicao = documento.NumeroExibicao ?? $"{documento.NumeroDocumento}"
                };

                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<DocumentoPrintDTO>(ex.Message);
            }
        }

        public async Task<Response<bool>> EnviarDocumentoPorEmailAsync(Guid id, EnviarDocumentoEmailRequest request)
        {
            try
            {
                if (!TryGetClinicaId(out Guid clinicaId, out Response<bool>? clinicaError))
                    return clinicaError!;

                Documento? documento = (
                    await _repository.GetListAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(id, clinicaId))
                ).FirstOrDefault();

                if (documento == null)
                    return ResponseFactory.Fail<bool>("Documento não encontrado");

                if (documento.Anulado || !documento.EstaEmitido)
                    return ResponseFactory.Fail<bool>("Documento inválido para envio por email");

                string? emailDestino = ResolveEmailDestino(documento, request.DestinatarioOverride);
                if (string.IsNullOrWhiteSpace(emailDestino))
                    return ResponseFactory.Fail<bool>("Sem email de destino para este documento");

                string numeroExibicao = documento.NumeroExibicao ?? $"{documento.NumeroDocumento}";
                string assunto = !string.IsNullOrWhiteSpace(request.AssuntoOverride)
                    ? request.AssuntoOverride.Trim()
                    : $"Documento {numeroExibicao}";

                string corpo = !string.IsNullOrWhiteSpace(request.MensagemOverride)
                    ? request.MensagemOverride.Trim()
                    : BuildDefaultEmailBody(documento);

                await _mailService.SendAsync(new MailRequest
                {
                    To = emailDestino,
                    Subject = assunto,
                    Body = corpo
                });

                return ResponseFactory.Success(true);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<bool>(ex.Message);
            }
        }

        public async Task<Response<DocumentoDetalhesAdmissoesDTO>> GetDocumentoDetalhesAdmissoesAsync(Guid id)
        {
            try
            {
                if (!TryGetClinicaId(out Guid clinicaId, out Response<DocumentoDetalhesAdmissoesDTO>? clinicaError))
                    return clinicaError!;

                Documento? documento = (
                    await _repository.GetListAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(id, clinicaId))
                ).FirstOrDefault();

                if (documento == null)
                    return ResponseFactory.Fail<DocumentoDetalhesAdmissoesDTO>("Documento não encontrado");

                DocumentoDetalhesAdmissoesDTO dto = new()
                {
                    DocumentoId = documento.Id
                };

                if (documento.OrigemClinica != null)
                {
                    dto.Itens.Add(new DocumentoAdmissaoDetalheDTO
                    {
                        AdmissaoId = documento.OrigemClinica.AdmissaoId,
                        ConsultaId = documento.OrigemClinica.ConsultaId,
                        Origem = "DocumentoOrigemClinica",
                        Descricao =
                            $"Origem clínica: admissão {documento.OrigemClinica.AdmissaoId?.ToString() ?? "-"} / consulta {documento.OrigemClinica.ConsultaId?.ToString() ?? "-"}"
                    });
                }

                List<Domain.Entities.Consultas.ConsultaFaturacao> faturacoes = (
                    await _repository.GetListAsync<Domain.Entities.Consultas.ConsultaFaturacao, Guid>(
                        new ConsultaFaturacaoByDocumentoIdSpec(documento.Id))
                ).ToList();

                foreach (var fat in faturacoes)
                {
                    Guid? admissaoId = null;
                    if (fat.ConsultaId.HasValue)
                    {
                        var admissao = (
                            await _repository.GetListAsync<Domain.Entities.Consultas.Admissao, Guid>(
                                new AdmissaoByConsultaIdSpec(fat.ConsultaId.Value))
                        ).FirstOrDefault();
                        admissaoId = admissao?.Id;
                    }

                    dto.Itens.Add(new DocumentoAdmissaoDetalheDTO
                    {
                        AdmissaoId = admissaoId,
                        ConsultaId = fat.ConsultaId,
                        Origem = "ConsultaFaturacao",
                        Descricao =
                            $"Consulta faturação: admissão {admissaoId?.ToString() ?? "-"} / consulta {fat.ConsultaId?.ToString() ?? "-"}"
                    });
                }

                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<DocumentoDetalhesAdmissoesDTO>(ex.Message);
            }
        }

        public async Task<Response<DocumentoLiquidacaoContextoDTO>> GetDocumentoLiquidacaoContextoAsync(Guid id)
        {
            try
            {
                if (!TryGetClinicaId(out Guid clinicaId, out Response<DocumentoLiquidacaoContextoDTO>? clinicaError))
                    return clinicaError!;

                Documento? documento = (
                    await _repository.GetListAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(id, clinicaId))
                ).FirstOrDefault();

                if (documento == null)
                    return ResponseFactory.Fail<DocumentoLiquidacaoContextoDTO>("Documento não encontrado");

                if (documento.Anulado)
                    return ResponseFactory.Fail<DocumentoLiquidacaoContextoDTO>("Documento anulado não pode ser liquidado");

                if (!documento.EstaEmitido)
                    return ResponseFactory.Fail<DocumentoLiquidacaoContextoDTO>("Documento ainda não emitido");

                if (documento.Liquidado)
                    return ResponseFactory.Fail<DocumentoLiquidacaoContextoDTO>("Documento já liquidado");

                DocumentoLiquidacaoContextoDTO dto = new()
                {
                    DocumentoId = documento.Id,
                    JaLiquidado = documento.Liquidado,
                    IsUtente = documento.UtenteId.HasValue,
                    UtenteId = documento.UtenteId,
                    OrganismoId = documento.OrganismoId,
                    TotalLiquido = documento.TotalLiquido ?? 0m,
                    NumeroExibicao = documento.NumeroExibicao ?? $"{documento.NumeroDocumento}"
                };

                return ResponseFactory.Success(dto);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<DocumentoLiquidacaoContextoDTO>(ex.Message);
            }
        }

        public async Task<Response<Guid>> LiquidarDocumentoAsync(Guid id)
        {
            try
            {
                if (!TryGetClinicaId(out Guid clinicaId, out Response<Guid>? clinicaError))
                    return clinicaError!;

                Documento? documento = (
                    await _repository.GetListAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(id, clinicaId))
                ).FirstOrDefault();

                if (documento == null)
                    return ResponseFactory.Fail<Guid>("Documento não encontrado");

                if (documento.Anulado)
                    return ResponseFactory.Fail<Guid>("Documento anulado não pode ser liquidado");

                if (!documento.EstaEmitido)
                    return ResponseFactory.Fail<Guid>("Documento ainda não emitido");

                if (documento.Liquidado)
                    return ResponseFactory.Fail<Guid>("Documento já liquidado");

                TipoDocumento? tipoRecibo = (
                    await _repository.GetListAsync<TipoDocumento, Guid>(new TipoDocumentoReciboByClinicaSpec(clinicaId))
                ).FirstOrDefault();
                if (tipoRecibo == null)
                    return ResponseFactory.Fail<Guid>("Tipo de documento de recibo não configurado na clínica");

                Documento? ultimoRecibo = (
                    await _repository.GetListAsync<Documento, Guid>(
                        new DocumentoUltimoNumeroByTipoAnoSpec(clinicaId, tipoRecibo.Id, documento.AnoFiscal)
                    )
                ).FirstOrDefault();

                int numeroRecibo = (ultimoRecibo?.NumeroDocumento ?? 0) + 1;
                string numeroExibicao = string.IsNullOrWhiteSpace(tipoRecibo.Abreviatura)
                    ? $"{numeroRecibo}"
                    : $"{tipoRecibo.Abreviatura}-{numeroRecibo}";

                Recibo recibo = new()
                {
                    ClinicaId = documento.ClinicaId,
                    AnoFiscal = documento.AnoFiscal,
                    TipoDocumentoId = tipoRecibo.Id,
                    NumeroDocumento = numeroRecibo,
                    NumeroExibicao = numeroExibicao,
                    Data = DateTime.Today,
                    DataSistemaRegisto = DateTime.Now,

                    UtenteId = documento.UtenteId,
                    OrganismoId = documento.OrganismoId,
                    FuncionarioId = documento.FuncionarioId,
                    NomeCliente = documento.NomeCliente,
                    MoradaCliente = documento.MoradaCliente,
                    CodigoPostalId = documento.CodigoPostalId,
                    LocalidadeCliente = documento.LocalidadeCliente,
                    NumeroContribuinteCliente = documento.NumeroContribuinteCliente,

                    TotalBruto = documento.TotalBruto,
                    TotalDocumento = documento.TotalDocumento,
                    TotalIva = documento.TotalIva,
                    TotalDesconto = documento.TotalDesconto,
                    TotalLiquido = documento.TotalLiquido,
                    DescontoCliente = documento.DescontoCliente,
                    DescontoPagamento = documento.DescontoPagamento,
                    Outros = documento.Outros,
                    PrecoUnitarioMercadorias = documento.PrecoUnitarioMercadorias,

                    CondicaoPagamento = documento.CondicaoPagamento,
                    TipoModoPagamento = documento.TipoModoPagamento,
                    MoedaId = documento.MoedaId,
                    TaxaCambio = documento.TaxaCambio,
                    TipoCambio = documento.TipoCambio,
                    DataVencimentoPagamento = documento.DataVencimentoPagamento,
                    BancoId = documento.BancoId,

                    EstadoDocumento = EstadoDocumento.Emitido,
                    Estado = (int)EstadoDocumento.Emitido,
                    Liquidado = true,
                    Rectificado = false,
                    Exportado = false,
                    IsentoIva = documento.IsentoIva,
                    Anulado = false,
                    IvaCaixa = documento.IvaCaixa,
                    Emitido = 1,
                    EstaEmitido = true,
                    TipoSerie = "N",
                    ModuloOrigem = documento.ModuloOrigem,
                    Origem = documento.Origem,

                    DocumentoOrigemId = documento.Id,
                    IdentificadorUnicoDocumentoOrigem = documento.NumeroExibicao,
                    DataDocumentoOrigem = documento.Data,
                    HashDocumentoOrigem = documento.GlobalHash,
                    Observacoes = $"Liquidação automática do documento {documento.NumeroExibicao ?? documento.NumeroDocumento.ToString()}"
                };

                Recibo createdRecibo = await _repository.CreateAsync<Recibo, Guid>(recibo);
                documento.Liquidado = true;
                _ = await _repository.UpdateAsync<Documento, Guid>(documento);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success(createdRecibo.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
            }
        }

        public async Task<Response<Guid>> AtualizarValidacaoTransporteAsync(
            Guid id,
            AtualizarValidacaoTransporteRequest request)
        {
            try
            {
                if (!TryGetClinicaId(out Guid clinicaId, out Response<Guid>? clinicaError))
                    return clinicaError!;

                Documento? documento = (
                    await _repository.GetListAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(id, clinicaId))
                ).FirstOrDefault();

                if (documento == null)
                    return ResponseFactory.Fail<Guid>("Documento não encontrado");

                if (documento.Anulado)
                    return ResponseFactory.Fail<Guid>("Documento anulado não permite validação de transporte");

                string abrev = documento.TipoDocumento?.Abreviatura?.Trim().ToUpperInvariant() ?? string.Empty;
                if (abrev != "GT" && abrev != "GR")
                    return ResponseFactory.Fail<Guid>("Validação de transporte só disponível para GT/GR");

                if (string.IsNullOrWhiteSpace(request.CodigoValidacaoTransporte))
                    return ResponseFactory.Fail<Guid>("Código de validação de transporte é obrigatório");

                documento.CodigoValidacaoTransporte = request.CodigoValidacaoTransporte.Trim();
                documento.DataTransporte = request.DataTransporte ?? documento.DataTransporte ?? DateTime.Today;
                documento.HoraTransporte = string.IsNullOrWhiteSpace(request.HoraTransporte)
                    ? documento.HoraTransporte
                    : request.HoraTransporte.Trim();

                _ = await _repository.UpdateAsync<Documento, Guid>(documento);
                _ = await _repository.SaveChangesAsync();

                return ResponseFactory.Success(documento.Id);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Fail<Guid>(ex.Message);
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

        private Guid? GetCurrentClinicaId()
        {
            return Guid.TryParse(_currentClinicaService.ClinicaId, out Guid clinicaId) ? clinicaId : null;
        }

        private static string ResolvePrintTemplate(Documento documento)
        {
            if (!string.IsNullOrWhiteSpace(documento.TipoDocumento?.ReportPersonalizado))
                return documento.TipoDocumento.ReportPersonalizado!.Trim();

            return "TFatura";
        }

        private static string? ResolveEmailDestino(Documento documento, string? destinatarioOverride)
        {
            if (!string.IsNullOrWhiteSpace(destinatarioOverride))
                return destinatarioOverride.Trim();

            if (!string.IsNullOrWhiteSpace(documento.Utente?.Email))
                return documento.Utente.Email!.Trim();

            if (!string.IsNullOrWhiteSpace(documento.Organismo?.Email))
                return documento.Organismo.Email!.Trim();

            return null;
        }

        private static string BuildDefaultEmailBody(Documento documento)
        {
            string numero = documento.NumeroExibicao ?? $"{documento.NumeroDocumento}";
            string cliente = string.IsNullOrWhiteSpace(documento.NomeCliente) ? "Cliente" : documento.NomeCliente!;
            string total = (documento.TotalLiquido ?? 0m).ToString("0.00");

            StringBuilder sb = new();
            _ = sb.AppendLine($"Exmo(a). {cliente},");
            _ = sb.AppendLine();
            _ = sb.AppendLine($"Segue o documento {numero}.");
            _ = sb.AppendLine($"Total líquido: {total}.");
            _ = sb.AppendLine();
            _ = sb.AppendLine("Cumprimentos.");

            return sb.ToString();
        }

        private bool TryGetClinicaId<T>(out Guid clinicaId, out Response<T>? error)
        {
            clinicaId = Guid.Empty;
            error = null;
            Guid? current = GetCurrentClinicaId();
            if (!current.HasValue || current.Value == Guid.Empty)
            {
                error = ResponseFactory.Fail<T>("Clínica atual inválida");
                return false;
            }

            clinicaId = current.Value;
            return true;
        }
    }
}
