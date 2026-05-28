#nullable enable

using Ardalis.Specification;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Enums;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Consultas.ConsultaService.Specifications;
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;
using CliCloud.Application.Services.Documentos.DocumentoService.Specifications;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService;

public class DocumentoEmissaoService(
    IRepositoryAsync repository,
    ICurrentClinicaService currentClinicaService,
    ITransactionalExecutor transactionalExecutor
) : IDocumentoEmissaoService
{
    public async Task<Response<DocumentoEmissaoDTO>> EmitirDocumentoAsync(EmitirDocumentoRequest request)
    {
        try
        {
            return await transactionalExecutor.ExecuteAsync(async ct =>
            {
            if(request.Linhas == null || request.Linhas.Count == 0)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Documento deve conter pelo menos uma linha");
            if(request.Anulado)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Não é permitido emitir documento já anulado.");

            await currentClinicaService.SetClinicaAsync();
            if(!Guid.TryParse(currentClinicaService.ClinicaId, out Guid clinicaId) || clinicaId == Guid.Empty)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Clínica atual inválida");

            TipoDocumento? tipoDocumento = (
                await repository.GetListAsync<TipoDocumento, Guid>(new TipoDocumentoByIdClinicaSpec(request.TipoDocumentoId, clinicaId))
            ).FirstOrDefault();
            if (tipoDocumento == null)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Tipo de documento não encontrado na clínica atual");

            Clinica clinica = await repository.GetByIdAsync<Clinica, Guid>(clinicaId);

            DateTime dataDocumento = request.DataDocumento ?? DateTime.Today;
            if(request.DataVencimentoPagamento.HasValue && request.DataVencimentoPagamento.Value.Date < dataDocumento.Date)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Data de vencimento não pode ser inferior à data do documento.");

            if(dataDocumento.Year > 2022)
            {
                if(string.IsNullOrWhiteSpace(tipoDocumento.CodigoATCUD))
                    return ResponseFactory.Fail<DocumentoEmissaoDTO>("Tipo de documento não tem código ATCUD definido");

                if(!string.Equals(tipoDocumento.ATCUDEstado?.Trim(), "A" , StringComparison.OrdinalIgnoreCase))
                    return ResponseFactory.Fail<DocumentoEmissaoDTO>("ATCUD inválido/inativo para o tipo de Documento");
            }

            var specUltimo = new DocumentoUltimoNumeroSpec(clinicaId, request.TipoDocumentoId, request.AnoFiscal);
            Documento? ultimo = (await repository.GetListAsync<Documento, Guid>(specUltimo)).FirstOrDefault();

            int numeroDocumento = (ultimo?.NumeroDocumento ?? 0) + 1;
            string hashDocAnterior = ultimo?.GlobalHash ?? string.Empty;

            string serie = request.AnoFiscal < 2013 ? "1" : (tipoDocumento.NumeroSerie ?? string.Empty);
            if(request.AnoFiscal >= 2013 && string.IsNullOrWhiteSpace(serie))
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("O número de série do documento é obrigatório para SAFT");

            decimal precoUnitarioMercadorias = 0m;
            decimal totalDocumentoBase = 0m;
            decimal totalIva = 0m;
            decimal totalDescontosLinhas = 0m;

            List<DocumentoLinha> linhas = request.Linhas.Select((linhaReq, index) =>
            {
                int numeroLinha = linhaReq.NumeroLinha > 0 ? linhaReq.NumeroLinha : index + 1;

                decimal baseLinha = linhaReq.Quantidade * linhaReq.PrecoUnitario;
                precoUnitarioMercadorias += baseLinha;

                decimal descontoLinha = 0m;

                if(linhaReq.ValorDesconto.HasValue)
                {
                    descontoLinha = linhaReq.ValorDesconto.Value;
                }
                else if(linhaReq.PercentagemDesconto.HasValue)
                {
                    descontoLinha = Math.Round(baseLinha * (linhaReq.PercentagemDesconto.Value / 100m), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    descontoLinha =
                        (linhaReq.DescontoTipo1 ?? 0m) +
                        (linhaReq.DescontoTipo2 ?? 0m) +
                        (linhaReq.DescontoTipo3 ?? 0m);
                }

                decimal totalLinha = baseLinha - descontoLinha;
                totalDescontosLinhas += descontoLinha;

                decimal valorImposto = Math.Round(totalLinha * (linhaReq.TaxaIvaPercentagem / 100m), 2, MidpointRounding.AwayFromZero);
                totalIva += valorImposto;

                totalDocumentoBase += totalLinha;

                return new DocumentoLinha
                {
                    Id = Guid.NewGuid(),
                    NumeroLinha = numeroLinha,
                    CodigoArtigo = linhaReq.CodigoArtigo,
                    ServicoId = linhaReq.ServicoId,
                    AdmissaoServicoId = linhaReq.AdmissaoServicoId,
                    Descricao = linhaReq.Descricao,
                    Quantidade = linhaReq.Quantidade,
                    PrecoUnitario = linhaReq.PrecoUnitario,

                    PercentagemDesconto = linhaReq.PercentagemDesconto,
                    ValorDesconto = linhaReq.ValorDesconto,

                    DescontoTipo1 = linhaReq.DescontoTipo1,
                    DescontoTipo2 = linhaReq.DescontoTipo2,
                    DescontoTipo3 = linhaReq.DescontoTipo3,

                    TotalLinha = totalLinha,
                    TaxaIvaId = linhaReq.TaxaIvaId,
                    TaxaIvaPercentagem = linhaReq.TaxaIvaPercentagem,
                    ValorImposto = valorImposto,

                    ModuloOrigemLinha = request.ModuloOrigem
                };
            }).ToList();

            decimal descontoCliente = request.DescontoCliente ?? 0m;
            decimal descontoPagamento = request.DescontoPagamento ?? 0m;
            decimal outros = request.Outros ?? 0m;

            decimal totalDescontoHeader = totalDescontosLinhas + descontoCliente;
            decimal totalBruto = totalDocumentoBase + totalIva + outros;
            decimal totalLiquido = totalBruto - descontoPagamento;

            DateTime dataSistemaRegisto = DateTime.Now;
            dataSistemaRegisto = dataSistemaRegisto.AddTicks(-(dataSistemaRegisto.Ticks % TimeSpan.TicksPerSecond));

            string? codigoAtcud = null;
            if(dataDocumento.Year > 2022)
                codigoAtcud = tipoDocumento.CodigoATCUD?.Trim();

            string numeroExibicao = 
                !string.IsNullOrWhiteSpace(codigoAtcud)
                ? $"{codigoAtcud}-{numeroDocumento}"
                : $"{tipoDocumento.Abreviatura}-{numeroDocumento}";

            Documento documento = IsTipoRecibo(tipoDocumento) ? new Recibo() : new Documento();
            documento.Id = Guid.NewGuid();
            documento.ClinicaId = clinicaId;
            documento.AnoFiscal = request.AnoFiscal;
            documento.TipoDocumentoId = request.TipoDocumentoId;
            documento.NumeroDocumento = numeroDocumento;
            documento.NumeroExibicao = numeroExibicao;
            documento.Data = dataDocumento;
            documento.DataSistemaRegisto = dataSistemaRegisto;
            documento.UtenteId = request.UtenteId;
            documento.OrganismoId = request.OrganismoId;
            documento.FuncionarioId = request.FuncionarioId;
            documento.NomeCliente = request.NomeCliente;
            documento.MoradaCliente = request.MoradaCliente;
            documento.LocalidadeCliente = request.LocalidadeCliente;
            documento.NumeroContribuinteCliente = request.NumeroContribuinteCliente;
            documento.CodigoPostalId = request.CodigoPostalId;
            documento.TotalDocumento = totalDocumentoBase;
            documento.TotalIva = totalIva;
            documento.TotalDesconto = totalDescontoHeader;
            documento.TotalBruto = totalBruto;
            documento.TotalLiquido = totalLiquido;
            documento.DescontoCliente = descontoCliente;
            documento.DescontoPagamento = descontoPagamento;
            documento.Outros = outros;
            documento.PrecoUnitarioMercadorias = precoUnitarioMercadorias;
            documento.CondicaoPagamento = request.CondicaoPagamento;
            documento.TipoModoPagamento = request.TipoModoPagamento;
            documento.MoedaId = request.MoedaId;
            documento.BancoId = request.BancoId;
            documento.TaxaCambio = request.TaxaCambio;
            documento.TipoCambio = request.TipoCambio;
            documento.DataVencimentoPagamento = request.DataVencimentoPagamento ?? dataDocumento;
            documento.EstadoDocumento = EstadoDocumento.Emitido;
            documento.Estado = (int)EstadoDocumento.Emitido;
            documento.Liquidado = request.Liquidado;
            documento.Rectificado = request.Rectificado;
            documento.Exportado = false;
            documento.IsentoIva = request.IsentoIva;
            documento.Anulado = request.Anulado;
            documento.IvaCaixa = request.IvaCaixa;
            documento.Emitido = 1;
            documento.EstaEmitido = true;
            documento.Origem = request.ModuloOrigem != null ? (int)request.ModuloOrigem.Value : null;
            documento.ModuloOrigem = request.ModuloOrigem;
            documento.CaixaId = request.CaixaId;
            documento.Observacoes = request.Observacoes;
            documento.CodigoAtcud = codigoAtcud;
            documento.CodigoValidacaoTransporte = request.CodigoValidacaoTransporte;
            documento.DataTransporte = request.DataTransporte;
            documento.HoraTransporte = request.HoraTransporte;

            if(clinica.TemSaft == true)
            {
                int codigoTipoDocSaft = request.CodigoTipoDocSaft ?? tipoDocumento.TipoMovimento
                    ?? throw new InvalidOperationException("Não foi possível inferir o código de documento SAFT ");

                documento.VersaoChave = SaftHash.VersaoChave;
                documento.GlobalHash = SaftHash.GerarHash(
                    documento.Data!.Value,
                    documento.DataSistemaRegisto!.Value,
                    codigoTipoDocSaft,
                    serie,
                    numeroDocumento,
                    documento.TotalDocumento!.Value,
                    hashDocAnterior
                );
            }

            foreach (DocumentoLinha linha in linhas)
            {
                linha.DocumentoId = documento.Id;
            }

            await repository.CreateAsync<Documento, Guid>(documento);
            await repository.CreateRangeAsync<DocumentoLinha, Guid>(linhas);
            documento.Linhas = linhas;

            await repository.SaveChangesAsync();

            return ResponseFactory.Success(new DocumentoEmissaoDTO
            {
                Id = documento.Id,
                TipoDocumentoId = documento.TipoDocumentoId,
                AnoFiscal = documento.AnoFiscal,
                NumeroDocumento = documento.NumeroDocumento,
                NumeroExibicao = documento.NumeroExibicao,
                HashDocumento = documento.GlobalHash,
                VersaoChave = documento.VersaoChave,
            });
            });
        }
        catch(Exception ex)
        {
            return ResponseFactory.Fail<DocumentoEmissaoDTO>(ex.Message);
        }
    }

    public async Task<Response<DocumentoEmissaoDTO>> EmitirDocumentoDesdeAdmissaoAsync(Guid admissaoId, EmitirDocumentoDesdeAdmissaoRequest request)
    {
        try
        {
            return await transactionalExecutor.ExecuteAsync(async ct =>
            {
            List<Admissao> admissoes = (
                await repository.GetListAsync<Admissao, Guid>(new AdmissaoByIdWithServicosSpec(admissaoId))
            ).ToList();

            Admissao? admissao = admissoes.FirstOrDefault();
            if(admissao == null)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Admissão não encontrada");

            if(admissao.Servicos == null || admissao.Servicos.Count == 0)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Admissão não tem serviços para faturar");

            Consulta? consulta = (
                await repository.GetListAsync<Consulta, Guid>(new ConsultaPorAdmissaoSpec(admissao.Id))
            ).FirstOrDefault();

            string nomeCliente = 
                request.NomeCliente 
                ?? admissao.Utente?.Nome
                ?? admissao.Organismo?.Nome
                ?? "Cliente sem nome";

            string moradaCliente = 
                request.MoradaCliente
                ?? admissao.Utente?.Observacoes
                ?? "Morada não definida";

            string? localidadeCliente = 
                request.LocalidadeCliente
                ?? admissao.Utente?.CodigoPostal?.Localidade
                ?? null;

            string? nifCliente = 
                request.NumeroContribuinteCliente
                ?? admissao.Utente?.NumeroContribuinte
                ?? admissao.Organismo?.NumeroContribuinte;

            Guid? codigoPostalId = admissao.Utente?.CodigoPostalId;

            List<EmitirDocumentoLinhaRequest> linhas = admissao.Servicos
                .Select((s, index) =>
                {
                    decimal quantidade = s.Quantidade.GetValueOrDefault(1m);
                    if(quantidade <= 0) quantidade = 1m;

                    decimal preco = s.ValorServico
                        ?? s.ValorArtigo
                        ?? 0m;

                    string descricao = !string.IsNullOrWhiteSpace(s.NomeArtigo)
                        ? s.NomeArtigo
                        : "Serviço de admissão";

                    return new EmitirDocumentoLinhaRequest
                    {
                        NumeroLinha = index + 1,
                        CodigoArtigo = s.CodigoArtigo,
                        ServicoId = s.ServicoId,
                        AdmissaoServicoId = s.Id,
                        Descricao = descricao,
                        Quantidade = quantidade,
                        PrecoUnitario = preco,
                        TaxaIvaPercentagem = 0m
                    };
                }).ToList();

                var emitirRequest = new EmitirDocumentoRequest
                {
                    TipoDocumentoId = request.TipoDocumentoId,
                    AnoFiscal = request.AnoFiscal,
                    DataDocumento = request.DataDocumento ?? admissao.Data ?? DateTime.Today,

                    UtenteId = admissao.UtenteId,
                    OrganismoId = admissao.OrganismoId,
                    FuncionarioId = request.FuncionarioId ?? admissao.FuncionarioId,

                    NomeCliente = nomeCliente,
                    MoradaCliente = moradaCliente,
                    LocalidadeCliente = localidadeCliente,
                    NumeroContribuinteCliente = nifCliente,
                    CodigoPostalId = codigoPostalId,
                    
                    CondicaoPagamento = request.CondicaoPagamento,
                    TipoModoPagamento = request.TipoModoPagamento,
                    MoedaId = request.MoedaId ,
                    BancoId = request.BancoId,
                    DataVencimentoPagamento = request.DataVencimentoPagamento,

                    DescontoCliente = request.DescontoCliente,
                    DescontoPagamento = request.DescontoPagamento,
                    Outros = request.Outros,

                    IsentoIva = request.IsentoIva,
                    IvaCaixa = request.IvaCaixa,

                    ModuloOrigem = ModuloOrigemDocumento.Consultas,
                    CodigoTipoDocSaft = request.CodigoTipoDocSaft,

                    Linhas = linhas,
                };

                Response<DocumentoEmissaoDTO> emissao = await EmitirDocumentoAsync(emitirRequest);
                if(emissao.Status != ResponseStatus.Success || emissao.Data == null)
                    return emissao;

                Guid documentoId = emissao.Data.Id;
                bool faturado = request.Faturado ?? true;
                bool pago = request.Pago ?? false;

                DocumentoOrigemClinica origem = new()
                {
                    Id = Guid.NewGuid(),
                    DocumentoId = documentoId,
                    ModuloOrigem = ModuloOrigemDocumento.Consultas,
                    AdmissaoId = admissao.Id,
                    ConsultaId = consulta?.Id,
                };

                _ = await repository.CreateAsync<DocumentoOrigemClinica, Guid>(origem);

                admissao.Faturado = faturado;
                admissao.Pago = pago;

                _ = await repository.UpdateAsync<Admissao, Guid>(admissao);

                if(consulta != null)
                {
                    await AdmissaoFaturacaoPromocaoHelper.SincronizarComDocumentoAsync(
                        repository,
                        consulta.Id,
                        documentoId,
                        request.TipoDocumentoId,
                        pago, 
                        faturado
                    );
                }

                _ = await repository.SaveChangesAsync();
                return emissao;
            });
        }
        catch(Exception ex)
        {
            return ResponseFactory.Fail<DocumentoEmissaoDTO>(ex.Message);
        }
    }

    public async Task<Response<DocumentoEmissaoDTO>> EmitirDocumentoDesdeConsultaAsync(
        Guid consultaId,
        EmitirDocumentoDesdeConsultaRequest request
    )
    {
        try
        {
            return await transactionalExecutor.ExecuteAsync(async ct =>
            {
            List<Consulta> consultas = (
                await repository.GetListAsync<Consulta, Guid>(new ConsultaByIdWithServicosSpec(consultaId))
            ).ToList();

            Consulta? consulta = consultas.FirstOrDefault();
            if (consulta == null)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Consulta não encontrada.");

            if (consulta.Servicos == null || consulta.Servicos.Count == 0)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Consulta não tem serviços para faturar.");

            Admissao? admissao = (
                await repository.GetListAsync<Admissao, Guid>(new AdmissaoByConsultaIdSpec(consulta.Id))
            ).FirstOrDefault();

            string nomeCliente =
                request.NomeCliente
                ?? consulta.Utente?.Nome
                ?? consulta.Organismo?.Nome
                ?? "Cliente sem nome";

            string moradaCliente =
                request.MoradaCliente
                ?? consulta.Utente?.Observacoes
                ?? "Morada não definida";

            string? localidadeCliente =
                request.LocalidadeCliente
                ?? consulta.Utente?.CodigoPostal?.Localidade;

            string? nifCliente =
                request.NumeroContribuinteCliente
                ?? consulta.Utente?.NumeroContribuinte
                ?? consulta.Organismo?.NumeroContribuinte;

            Guid? codigoPostalId = consulta.Utente?.CodigoPostalId;

            List<EmitirDocumentoLinhaRequest> linhas = consulta.Servicos
                .Select((s, index) =>
                {
                    decimal quantidade = s.Quantidade.GetValueOrDefault(1m);
                    if (quantidade <= 0) quantidade = 1m;

                    decimal preco = s.ValorServico
                        ?? s.ValorArtigo
                        ?? 0m;

                    string descricao = !string.IsNullOrWhiteSpace(s.NomeArtigo)
                        ? s.NomeArtigo!
                        : "Serviço de consulta";

                    return new EmitirDocumentoLinhaRequest
                    {
                        NumeroLinha = index + 1,
                        CodigoArtigo = s.CodigoArtigo,
                        ServicoId = s.ServicoId,
                        Descricao = descricao,
                        Quantidade = quantidade,
                        PrecoUnitario = preco,
                        TaxaIvaPercentagem = 0m
                    };
                })
                .ToList();

            var emitirRequest = new EmitirDocumentoRequest
            {
                TipoDocumentoId = request.TipoDocumentoId,
                AnoFiscal = request.AnoFiscal,
                DataDocumento = request.DataDocumento ?? consulta.Data ?? DateTime.Today,

                UtenteId = consulta.UtenteId,
                OrganismoId = consulta.OrganismoId,
                FuncionarioId = request.FuncionarioId ?? consulta.FuncionarioId,

                NomeCliente = nomeCliente,
                MoradaCliente = moradaCliente,
                LocalidadeCliente = localidadeCliente,
                NumeroContribuinteCliente = nifCliente,
                CodigoPostalId = codigoPostalId,

                CondicaoPagamento = request.CondicaoPagamento,
                TipoModoPagamento = request.TipoModoPagamento,
                MoedaId = request.MoedaId,
                BancoId = request.BancoId,
                DataVencimentoPagamento = request.DataVencimentoPagamento,

                DescontoCliente = request.DescontoCliente,
                DescontoPagamento = request.DescontoPagamento,
                Outros = request.Outros,

                IsentoIva = request.IsentoIva,
                IvaCaixa = request.IvaCaixa,

                ModuloOrigem = ModuloOrigemDocumento.Consultas,
                CodigoTipoDocSaft = request.CodigoTipoDocSaft,
                Linhas = linhas
            };

            Response<DocumentoEmissaoDTO> emissao = await EmitirDocumentoAsync(emitirRequest);
            if (emissao.Status != ResponseStatus.Success || emissao.Data == null)
                return emissao;

            Guid documentoId = emissao.Data.Id;
            bool faturado = request.Faturado ?? true;
            bool pago = request.Pago ?? false;

            DocumentoOrigemClinica origem = new()
            {
                Id = Guid.NewGuid(),
                DocumentoId = documentoId,
                ModuloOrigem = ModuloOrigemDocumento.Consultas,
                AdmissaoId = admissao?.Id,
                ConsultaId = consulta.Id
            };

            _ = await repository.CreateAsync<DocumentoOrigemClinica, Guid>(origem);

            await AdmissaoFaturacaoPromocaoHelper.SincronizarComDocumentoAsync(
                repository,
                consulta.Id,
                documentoId,
                request.TipoDocumentoId,
                pago,
                faturado
            );

            if (admissao != null)
            {
                admissao.Faturado = faturado;
                admissao.Pago = pago;
                _ = await repository.UpdateAsync<Admissao, Guid>(admissao);
            }

            _ = await repository.SaveChangesAsync();
            return emissao;
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<DocumentoEmissaoDTO>(ex.Message);
        }
    }

    public async Task<Response<Guid>> AnularDocumentoAsync(Guid documentoId, AnularDocumentoRequest request)
    {
        try
        {
            return await transactionalExecutor.ExecuteAsync(async ct =>
            {
            await currentClinicaService.SetClinicaAsync();
            if(!Guid.TryParse(currentClinicaService.ClinicaId, out Guid clinicaId) || clinicaId == Guid.Empty)
                return ResponseFactory.Fail<Guid>("Clínica atual inválida");

            Documento? documento = (
                await repository.GetListAsync<Documento, Guid>(new DocumentoByIdClinicaSpec(documentoId, clinicaId))
            ).FirstOrDefault();
            if (documento == null)
                return ResponseFactory.Fail<Guid>("Documento não encontrado");

            if (documento.Anulado)
                return ResponseFactory.Fail<Guid>("Documento já se encontra anulado.");

            string motivo = request.MotivoAnulacao?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(motivo))
                return ResponseFactory.Fail<Guid>("Motivo de anulação é obrigatório.");
            if (request.DataAnulacao.HasValue && documento.Data.HasValue && request.DataAnulacao.Value.Date < documento.Data.Value.Date)
                return ResponseFactory.Fail<Guid>("Data de anulação não pode ser inferior à data do documento.");

            documento.Anulado = true;
            documento.EstadoDocumento = EstadoDocumento.Anulado;
            documento.Estado = (int)EstadoDocumento.Anulado;
            documento.MotivoAnulacao = motivo;
            documento.DataAnulacao = request.DataAnulacao ?? DateTime.Now;
            documento.EstaEmitido = false;
            documento.Emitido = 0;

            _ = await repository.UpdateAsync<Documento, Guid>(documento);

            if (request.ReverterEstadosClinicos)
            {
                List<ConsultaFaturacao> fatRows = (
                    await repository.GetListAsync<ConsultaFaturacao, Guid>(new ConsultaFaturacaoByDocumentoIdSpec(documentoId))
                ).ToList();

                foreach (ConsultaFaturacao fat in fatRows)
                {
                    fat.Faturado = false;
                    fat.Pago = false;
                    _ = await repository.UpdateAsync<ConsultaFaturacao, Guid>(fat);

                    if (fat.ConsultaId.HasValue)
                    {
                        Admissao? admissao = (
                            await repository.GetListAsync<Admissao, Guid>(new AdmissaoByConsultaIdSpec(fat.ConsultaId.Value))
                        ).FirstOrDefault();

                        if (admissao != null)
                        {
                            admissao.Faturado = false;
                            admissao.Pago = false;
                            _ = await repository.UpdateAsync<Admissao, Guid>(admissao);
                        }
                    }
                }
            }

            _ = await repository.SaveChangesAsync();
            return ResponseFactory.Success(documento.Id);
            });
        }
        catch (Exception ex)
        {
            return ResponseFactory.Fail<Guid>(ex.Message);
        }
    }

    private sealed class DocumentoUltimoNumeroSpec : Specification<Documento>
    {
        public DocumentoUltimoNumeroSpec(Guid clinicaId, Guid tipoDocumentoId, int anoFiscal)
        {
            Query.Where(d =>
                d.ClinicaId == clinicaId &&
                d.TipoDocumentoId == tipoDocumentoId &&
                d.AnoFiscal == anoFiscal &&
                d.Anulado == false
            );

            Query.OrderByDescending(d => d.NumeroDocumento);
            Query.Take(1);
        }
    }

    public async Task<Response<DocumentoEmissaoDTO>> CriarNotaCreditoAsync(CriarNotaCreditoRequest request)
    {
        try
        {
            return await transactionalExecutor.ExecuteAsync(async ct =>
            {
            await currentClinicaService.SetClinicaAsync();
            if(!Guid.TryParse(currentClinicaService.ClinicaId, out Guid clinicaId) || clinicaId == Guid.Empty)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Clínica atual inválida");

            Documento documentoOrigem = (
                await repository.GetListAsync<Documento, Guid>(new DocumentoByIdWithLinhasSpec(request.DocumentoOrigemId, clinicaId))
            ).FirstOrDefault() ?? throw new Exception("Documento origem não encontrado");

            if(documentoOrigem.Anulado)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Não é possível creditar um documento anulado");
            if(string.IsNullOrWhiteSpace(request.Motivo))
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Motivo da nota de crédito é obrigatório.");
            if(request.DataDocumento.HasValue && documentoOrigem.Data.HasValue && request.DataDocumento.Value.Date < documentoOrigem.Data.Value.Date)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Data da nota de crédito não pode ser inferior à data do documento origem.");

            if(documentoOrigem.Linhas == null || documentoOrigem.Linhas.Count == 0)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Documento de origem sem linhas para creditar");

            List<EmitirDocumentoLinhaRequest> linhasNc;

            if(request.CreditoTotal)
            {
                linhasNc = documentoOrigem.Linhas
                    .OrderBy(l => l.NumeroLinha)
                    .Select((l, idx) => new EmitirDocumentoLinhaRequest
                    {
                        NumeroLinha = idx + 1,
                        CodigoArtigo = l.CodigoArtigo,
                        ServicoId = l.ServicoId,
                        AdmissaoServicoId = l.AdmissaoServicoId,
                        Descricao = $"NC - {l.Descricao}",
                        Quantidade = l.Quantidade,
                        PrecoUnitario = l.PrecoUnitario,
                        PercentagemDesconto = l.PercentagemDesconto,
                        ValorDesconto = l.ValorDesconto,
                        DescontoTipo1 = l.DescontoTipo1,
                        DescontoTipo2 = l.DescontoTipo2,
                        DescontoTipo3 = l.DescontoTipo3,
                        TaxaIvaId = l.TaxaIvaId,
                        TaxaIvaPercentagem = l.TaxaIvaPercentagem,
                    }).ToList();
            }
            else
            {
                if(request.Linhas == null || request.Linhas.Count == 0)
                    return ResponseFactory.Fail<DocumentoEmissaoDTO>("Para crédito parcial deve indicar linhas");

                var origemDict = documentoOrigem.Linhas.ToDictionary(x => x.Id, x => x);

                linhasNc = request.Linhas
                    .Select((l, idx) =>
                    {
                        if(!origemDict.TryGetValue(l.DocumentoLinhaOrigemId, out DocumentoLinha? origem))
                            throw new InvalidOperationException($"Linha origem {l.DocumentoLinhaOrigemId} não encontrada no documento origem.");

                        decimal qtd = l.Quantidade;
                        if(qtd <= 0 || qtd > origem.Quantidade)
                            throw new InvalidOperationException("Quantidade de crédito inválida para uma das linhas.");

                        return new EmitirDocumentoLinhaRequest
                        {
                            NumeroLinha = idx + 1,
                            CodigoArtigo = origem.CodigoArtigo,
                            ServicoId = origem.ServicoId,
                            AdmissaoServicoId = origem.AdmissaoServicoId,
                            Descricao = $"NC - {origem.Descricao}",
                            Quantidade = qtd,
                            PrecoUnitario = l.PrecoUnitario ?? origem.PrecoUnitario,
                            TaxaIvaId = origem.TaxaIvaId,
                            TaxaIvaPercentagem = l.TaxaIvaPercentagem ?? origem.TaxaIvaPercentagem
                        };
                    })
                    .ToList();
            }

            var emitirReq = new EmitirDocumentoRequest
            {
                TipoDocumentoId = request.TipoDocumentoId,
                AnoFiscal = request.AnoFiscal,
                DataDocumento = request.DataDocumento ?? DateTime.Today,

                UtenteId = documentoOrigem.UtenteId,
                OrganismoId = documentoOrigem.OrganismoId,
                FuncionarioId = documentoOrigem.FuncionarioId,

                NomeCliente = documentoOrigem.NomeCliente ?? "Cliente",
                MoradaCliente = documentoOrigem.MoradaCliente ?? "Morada",
                LocalidadeCliente = documentoOrigem.LocalidadeCliente,
                NumeroContribuinteCliente = documentoOrigem.NumeroContribuinteCliente,
                CodigoPostalId = documentoOrigem.CodigoPostalId,

                CondicaoPagamento = documentoOrigem.CondicaoPagamento,
                TipoModoPagamento = documentoOrigem.TipoModoPagamento,
                MoedaId = documentoOrigem.MoedaId,
                BancoId = documentoOrigem.BancoId,
                TaxaCambio = documentoOrigem.TaxaCambio,
                TipoCambio = documentoOrigem.TipoCambio,
                DataVencimentoPagamento = documentoOrigem.DataVencimentoPagamento,

                Rectificado = true,
                Liquidado = false,
                Anulado = false,
                IsentoIva = documentoOrigem.IsentoIva,
                IvaCaixa = documentoOrigem.IvaCaixa,

                ModuloOrigem = documentoOrigem.ModuloOrigem,
                Linhas = linhasNc
            };

            Response<DocumentoEmissaoDTO> emissaoNc = await EmitirDocumentoAsync(emitirReq);
            if(emissaoNc.Status != ResponseStatus.Success || emissaoNc.Data == null)
                return emissaoNc;

            Documento nc = await repository.GetByIdAsync<Documento, Guid>(emissaoNc.Data.Id);
            nc.DocumentoOrigemId = documentoOrigem.Id;
            nc.DataDocumentoOrigem = documentoOrigem.Data;
            nc.HashDocumentoOrigem = documentoOrigem.GlobalHash;
            nc.IdentificadorUnicoDocumentoOrigem = documentoOrigem.NumeroExibicao;
            nc.Observacoes = string.IsNullOrWhiteSpace(nc.Observacoes)
                ? request.Motivo
                : $"{nc.Observacoes} | Motivo NC: {request.Motivo}";

            _ = await repository.UpdateAsync<Documento, Guid>(nc);

            if(request.ReverterEstadosClinicos)
            {
                List<ConsultaFaturacao> fatRows = (
                    await repository.GetListAsync<ConsultaFaturacao, Guid>(new ConsultaFaturacaoByDocumentoIdSpec(documentoOrigem.Id))
                ).ToList();

                foreach(ConsultaFaturacao fat in fatRows)
                {
                    fat.Faturado = false;
                    fat.Pago = false;
                    _ = await repository.UpdateAsync<ConsultaFaturacao, Guid>(fat);

                    if(fat.ConsultaId.HasValue)
                    {
                        Admissao? admissao = (
                            await repository.GetListAsync<Admissao, Guid>(new AdmissaoByConsultaIdSpec(fat.ConsultaId.Value))
                        ).FirstOrDefault();

                        if(admissao != null)
                        {
                            admissao.Faturado = false;
                            admissao.Pago = false;
                            _ = await repository.UpdateAsync<Admissao, Guid>(admissao);
                        }
                    }
                }
            }

            await repository.SaveChangesAsync();
            return emissaoNc;
            });
        }
        catch(Exception ex)
        {
            return ResponseFactory.Fail<DocumentoEmissaoDTO>(ex.Message);
        }
    }

    private static bool IsTipoRecibo(TipoDocumento tipoDocumento)
    {
        if(!string.IsNullOrWhiteSpace(tipoDocumento.Abreviatura) &&
           string.Equals(tipoDocumento.Abreviatura.Trim(), "RC", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return !string.IsNullOrWhiteSpace(tipoDocumento.Descricao) &&
               tipoDocumento.Descricao.Contains("recibo", StringComparison.OrdinalIgnoreCase);
    }
}