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
using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Validators;
using CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.Specifications;
using CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Sinistros;
using CliCloud.Application.Services.Faturacao.ReferenciasMbService;
using CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;
using Microsoft.EntityFrameworkCore;
using CondicaoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.CondicaoPagamento;
using ModoPagamentoEntity = CliCloud.Domain.Entities.Pagamentos.ModoPagamento;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService;

public class DocumentoEmissaoService(
    IRepositoryAsync repository,
    ICurrentClinicaService currentClinicaService,
    ITransactionalExecutor transactionalExecutor,
    IReferenciasMbService referenciasMbService
) : IDocumentoEmissaoService
{
public async Task<Response<DocumentoEmissaoOpcoesPagamentoDTO>> GetOpcoesPagamentoAsync()
{
    await currentClinicaService.SetClinicaAsync();
    if (!Guid.TryParse(currentClinicaService.ClinicaId, out Guid clinicaId) || clinicaId == Guid.Empty)
        return ResponseFactory.Fail<DocumentoEmissaoOpcoesPagamentoDTO>("Clínica atual inválida.");

    var condicoes = (await repository.GetListAsync<CondicaoPagamentoEntity, Guid>(new CondicaoPagamentoByClinicaSpec(clinicaId)))
        .OrderBy(x => x.Descricao)
        .Select(x => new PagamentoOpcaoDTO
        {
            Valor = x.Codigo,
            Descricao = x.Descricao
        })
        .ToList();

    var modos = (await repository.GetListAsync<ModoPagamentoEntity, Guid>(new ModoPagamentoByClinicaSpec(clinicaId)))
        .Where(x => !x.Historico)
        .OrderBy(x => x.Descricao)
        .Select(x => new PagamentoOpcaoDTO
        {
            Valor = x.Codigo,
            Descricao = string.IsNullOrWhiteSpace(x.Abreviatura)
                ? x.Descricao
                : $"{x.Descricao} ({x.Abreviatura})"
        })
        .ToList();
    var tiposSerie = new List<OpcaoTextoDTO>
    {
        new() { Valor = "N", Descricao = "N — Normal" },
        new() { Valor = "D", Descricao = "D — Documento de conferência" },
        new() { Valor = "M", Descricao = "M — Manual" }
    };
    var impostosRetencao = new List<OpcaoTextoDTO>
    {
        new() { Valor = "IRS", Descricao = "IRS" },
        new() { Valor = "IRC", Descricao = "IRC" },
        new() { Valor = "IS", Descricao = "IS" }
    };
    var referenciasMb = new List<PagamentoOpcaoDTO>
    {
        new() { Valor = 0, Descricao = "Não gerar" },
        new() { Valor = 1, Descricao = "Referência Multibanco" },
        new() { Valor = 2, Descricao = "Pedido MB Way" }
    };

    DocumentoEmissaoOpcoesPagamentoDTO data = new()
    {
        CondicoesPagamento = condicoes,
        ModosPagamento = modos,
        TiposSerie = tiposSerie,
        ImpostosRetencao = impostosRetencao,
        ReferenciasMb = referenciasMb
    };

    return ResponseFactory.Success(data);
}

public async Task<Response<SinistradosInfoFaturacaoResponse>> SinistradosInfoFaturacaoAsync(
    SinistradosInfoFaturacaoRequest request)
{
    await currentClinicaService.SetClinicaAsync();
    if (!Guid.TryParse(currentClinicaService.ClinicaId, out Guid clinicaId) || clinicaId == Guid.Empty)
        return ResponseFactory.Fail<SinistradosInfoFaturacaoResponse>("Clínica atual inválida.");
    return await SinistradosInfoFaturacaoHelper.ObterAsync(repository, request);
}

public async Task<Response<FaturaGlobalObterResponse>> FaturaGlobalObterAsync(
    FaturaGlobalObterRequest request)
{
    await currentClinicaService.SetClinicaAsync();
    if (!Guid.TryParse(currentClinicaService.ClinicaId, out Guid clinicaId) || clinicaId == Guid.Empty)
        return ResponseFactory.Fail<FaturaGlobalObterResponse>("Clínica atual inválida.");
    return await FaturaGlobalObterHelper.ObterAsync(repository, request);
}

public async Task<Response<DocumentoEmissaoDTO>> EmitirDocumentoAsync(EmitirDocumentoRequest request)
{
    const int maxTentativas = 3;
    try
    {
        for (int tentativa = 1; tentativa <= maxTentativas; tentativa++)
        {
            try
            {
                return await transactionalExecutor.ExecuteAsync(async ct =>
                {
                    if (request.Linhas == null || request.Linhas.Count == 0)
                        return ResponseFactory.Fail<DocumentoEmissaoDTO>("Documento deve conter pelo menos uma linha");
                    if (request.Anulado)
                        return ResponseFactory.Fail<DocumentoEmissaoDTO>("Não é permitido emitir documento já anulado.");
                    await currentClinicaService.SetClinicaAsync();
                    if (!Guid.TryParse(currentClinicaService.ClinicaId, out Guid clinicaId) || clinicaId == Guid.Empty)
                        return ResponseFactory.Fail<DocumentoEmissaoDTO>("Clínica atual inválida");
                    TipoDocumento? tipoDocumento = (
                        await repository.GetListAsync<TipoDocumento, Guid>(new TipoDocumentoByIdClinicaSpec(request.TipoDocumentoId, clinicaId))
                    ).FirstOrDefault();
                    if (tipoDocumento == null)
                        return ResponseFactory.Fail<DocumentoEmissaoDTO>("Tipo de documento não encontrado na clínica atual");
                    Clinica clinica = await repository.GetByIdAsync<Clinica, Guid>(clinicaId);
                    int regraFaturacao = DocumentoEmissaoCalculoHelper.ParseRegraFaturacao(clinica.Regrafaturacao);
                    string? erroPerfil = DocumentoEmissaoPerfilValidator.Validar(tipoDocumento, request, regraFaturacao);
                    if (erroPerfil != null)
                        return ResponseFactory.Fail<DocumentoEmissaoDTO>(erroPerfil);
                    DateTime dataDocumento = request.DataDocumento ?? DateTime.Today;
                    if (request.DataVencimentoPagamento.HasValue && request.DataVencimentoPagamento.Value.Date < dataDocumento.Date)
                        return ResponseFactory.Fail<DocumentoEmissaoDTO>("Data de vencimento não pode ser inferior à data do documento.");
                    if (dataDocumento.Year > 2022)
                    {
                        if (string.IsNullOrWhiteSpace(tipoDocumento.CodigoATCUD))
                            return ResponseFactory.Fail<DocumentoEmissaoDTO>("Tipo de documento não tem código ATCUD definido");
                        if (!string.Equals(tipoDocumento.ATCUDEstado?.Trim(), "A", StringComparison.OrdinalIgnoreCase))
                            return ResponseFactory.Fail<DocumentoEmissaoDTO>("ATCUD inválido/inativo para o tipo de Documento");
                    }
                    var specUltimo = new DocumentoUltimoNumeroSpec(clinicaId, request.TipoDocumentoId, request.AnoFiscal);
                    Documento? ultimo = (await repository.GetListAsync<Documento, Guid>(specUltimo)).FirstOrDefault();
                    int numeroDocumento = (ultimo?.NumeroDocumento ?? 0) + 1;
                    string hashDocAnterior = ultimo?.GlobalHash ?? string.Empty;
                    string serie = request.AnoFiscal < 2013 ? "1" : (tipoDocumento.NumeroSerie ?? string.Empty);
                    if (request.AnoFiscal >= 2013 && string.IsNullOrWhiteSpace(serie))
                        return ResponseFactory.Fail<DocumentoEmissaoDTO>("O número de série do documento é obrigatório para SAFT");
                    decimal descontoCliente = request.DescontoCliente ?? 0m;
                    decimal descontoPagamento = request.DescontoPagamento ?? 0m;
                    decimal outros = request.Outros ?? 0m;
                    ModuloOrigemDocumento moduloOrigem = request.ModuloOrigem ?? ModuloOrigemDocumento.Faturacao;
                    decimal retencaoValor = 0m;
                    List<DocumentoEmissaoCalculoHelper.LinhaCalculoResult> linhasCalc = [];
                    List<DocumentoLinha> linhas = request.Linhas.Select((linhaReq, index) =>
                    {
                        int numeroLinha = linhaReq.NumeroLinha > 0 ? linhaReq.NumeroLinha : index + 1;
                        DocumentoEmissaoCalculoHelper.LinhaCalculoResult calc =
                            DocumentoEmissaoCalculoHelper.CalcularLinha(
                                linhaReq,
                                regraFaturacao,
                                descontoCliente,
                                descontoPagamento,
                                request.PercentagemDescontoGlobal,
                                request.IsentoIva
                            );
                        linhasCalc.Add(calc);
                        return new DocumentoLinha
                        {
                            Id = Guid.NewGuid(),
                            NumeroLinha = numeroLinha,
                            CodigoArtigo = DocumentoEmissaoSnapshotHelper.TruncateOptional(
                                linhaReq.CodigoArtigo,
                                DocumentoEmissaoSnapshotHelper.CodigoArtigoMax),
                            ServicoId = linhaReq.ServicoId,
                            AdmissaoServicoId = linhaReq.AdmissaoServicoId,
                            Descricao = DocumentoEmissaoSnapshotHelper.TruncateRequired(
                                linhaReq.Descricao,
                                DocumentoEmissaoSnapshotHelper.DescricaoLinhaMax,
                                "Linha"),
                            Quantidade = linhaReq.Quantidade,
                            PrecoUnitario = linhaReq.PrecoUnitario,
                            PercentagemDesconto = calc.PercentagemDescontoEfectiva,
                            ValorDesconto = calc.DescontoValor,
                            DescontoTipo1 = linhaReq.DescontoTipo1,
                            DescontoTipo2 = linhaReq.DescontoTipo2,
                            DescontoTipo3 = linhaReq.DescontoTipo3,
                            TotalLinha = DocumentoEmissaoCalculoHelper.ResolverTotalLinhaPersistencia(
                                calc,
                                regraFaturacao
                            ),
                            TaxaIvaId = linhaReq.TaxaIvaId,
                            MotivoIsencaoId = request.IsentoIva || linhaReq.TaxaIvaPercentagem == 0m
                                ? (linhaReq.MotivoIsencaoId ?? request.MotivoIsencaoId)
                                : null,
                            TaxaIvaPercentagem = request.IsentoIva ? 0m : linhaReq.TaxaIvaPercentagem,
                            ValorImposto = Math.Round(
                                calc.ValorIva,
                                2,
                                MidpointRounding.AwayFromZero
                            ),
                            ModuloOrigemLinha = moduloOrigem
                        };
                    }).ToList();
                    DocumentoEmissaoCalculoHelper.DocumentoTotaisCalculo totaisDoc =
                        DocumentoEmissaoCalculoHelper.CalcularTotaisDocumento(
                            linhasCalc,
                            regraFaturacao,
                            outros,
                            0m
                        );
                    decimal precoUnitarioMercadorias = totaisDoc.Mercadorias;
                    decimal totalDocumentoBase = totaisDoc.Total;
                    decimal totalIva = totaisDoc.Impostos;
                    decimal totalDescontoHeader = totaisDoc.Descontos;

                    string? erroDescontoOrganismo = await ValidarDescontosOrganismoEspecialAsync(
                        request,
                        totalDescontoHeader
                    );
                    if ( erroDescontoOrganismo != null ) 
                        return ResponseFactory.Fail<DocumentoEmissaoDTO>(erroDescontoOrganismo);

                    retencaoValor = DocumentoEmissaoCalculoHelper.ResolverRetencaoValor(
                        request.RetencaoAtiva,
                        request.RetencaoTaxa,
                        request.RetencaoValor,
                        totalDocumentoBase,
                        outros
                    );
                    totaisDoc = DocumentoEmissaoCalculoHelper.CalcularTotaisDocumento(
                        linhasCalc,
                        regraFaturacao,
                        outros,
                        retencaoValor
                    );
                    totalDocumentoBase = totaisDoc.Total;
                    decimal totalBruto = totaisDoc.Total + outros;
                    decimal totalLiquido = totaisDoc.APagar;
                    DateTime dataSistemaRegisto = DateTime.Now;
                    dataSistemaRegisto = dataSistemaRegisto.AddTicks(-(dataSistemaRegisto.Ticks % TimeSpan.TicksPerSecond));
                    string? codigoAtcud = null;
                    if (dataDocumento.Year > 2022)
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
                    documento.NomeCliente = DocumentoEmissaoSnapshotHelper.TruncateRequired(
                        request.NomeCliente,
                        DocumentoEmissaoSnapshotHelper.NomeClienteMax,
                        "Cliente sem nome");
                    documento.MoradaCliente = DocumentoEmissaoSnapshotHelper.TruncateRequired(
                        request.MoradaCliente,
                        DocumentoEmissaoSnapshotHelper.MoradaClienteMax,
                        "Morada não definida");
                    documento.LocalidadeCliente = DocumentoEmissaoSnapshotHelper.TruncateOptional(
                        request.LocalidadeCliente,
                        DocumentoEmissaoSnapshotHelper.LocalidadeClienteMax);
                    documento.NumeroContribuinteCliente = DocumentoEmissaoSnapshotHelper.TruncateOptional(
                        request.NumeroContribuinteCliente,
                        DocumentoEmissaoSnapshotHelper.NumeroContribuinteClienteMax);
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
                    documento.CondicaoPagamentoId = request.CondicaoPagamentoId;
                    documento.ModoPagamentoId = request.ModoPagamentoId;
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
                    documento.MotivoIsencaoId = request.IsentoIva ? request.MotivoIsencaoId : null;
                    documento.Beneficiario = DocumentoEmissaoSnapshotHelper.TruncateOptional(
                        request.Beneficiario,
                        DocumentoEmissaoSnapshotHelper.BeneficiarioMax);
                    documento.FaturaGlobalDataInicio = request.FaturaGlobalDataInicio;
                    documento.FaturaGlobalDataFim = request.FaturaGlobalDataFim;
                    documento.Anulado = request.Anulado;
                    documento.IvaCaixa = request.IvaCaixa;
                    documento.Emitido = 1;
                    documento.EstaEmitido = true;
                    documento.Origem = (int)moduloOrigem;
                    documento.ModuloOrigem = moduloOrigem;
                    documento.CaixaId = request.CaixaId;
                    documento.Observacoes = request.Observacoes;
                    documento.CodigoAtcud = codigoAtcud;
                    documento.CodigoValidacaoTransporte = request.CodigoValidacaoTransporte;
                    documento.DataTransporte = request.DataTransporte;
                    documento.HoraTransporte = request.HoraTransporte;
                    documento.RetencaoImposto = request.RetencaoAtiva ? request.RetencaoImposto?.Trim() : null;
                    documento.RetencaoTaxa = request.RetencaoAtiva ? request.RetencaoTaxa : null;
                    documento.RetencaoValor = request.RetencaoAtiva ? retencaoValor : null;
                    documento.RetencaoCodigoMotivo = request.RetencaoAtiva ? request.RetencaoCodigoMotivo : null;
                    documento.RetencaoMotivo = request.RetencaoAtiva ? request.RetencaoMotivo?.Trim() : null;
                    documento.DocumentoOrigemId = request.DocumentoOrigemId;
                    documento.IdentificadorUnicoDocumentoOrigem = request.IdentificadorUnicoDocumentoOrigem;
                    documento.DataDocumentoOrigem = request.DataDocumentoOrigem;
                    documento.TipoSerie = ResolveTipoSerieEmissao(request.TipoSerie, tipoDocumento.TipoSerie);
                    if (clinica.TemSaft == true)
                    {
                        int codigoTipoDocSaft = request.CodigoTipoDocSaft
                            ?? tipoDocumento.CodigoTipoDocumentoSaft
                            ?? throw new InvalidOperationException("Não foi possível inferir o código de documento SAFT");
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
                    IEnumerable<Guid> admissaoServicoIds = ExtrairAdmissaoServicoIds(request.Linhas);
                    await DocumentoEmissaoClinicaSyncHelper.SincronizarAposEmissaoAsync(
                        repository,
                        documento.Id,
                        request.TipoDocumentoId,
                        admissaoServicoIds,
                        moduloOrigem,
                        faturado: request.AdmissaoSyncFaturado ?? true,
                        pago: request.AdmissaoSyncPago ?? false);
                    await MarcarLinhasSinistradoFaturadasAsync(
                        request,
                        documento.NumeroExibicao,
                        documento.Data);
                    await repository.SaveChangesAsync();
                    string? refEntidade = null;
                    string? refCodigo = null;
                    bool refMbWay = false;
                    if (request.GerarReferenciaMb is 1 or 2)
                    {
                        Response<ReferenciaMbGeradaDTO> mb = await referenciasMbService.GerarParaDocumentoAsync(
                            new GerarReferenciaDocumentoRequest
                            {
                                ClinicaId = clinicaId,
                                DocumentoId = documento.Id,
                                UtenteId = documento.UtenteId,
                                ClienteNome = documento.NomeCliente ?? string.Empty,
                                NumeroExibicao = documento.NumeroExibicao,
                                Valor = documento.TotalLiquido ?? totalLiquido,
                                Modo = request.GerarReferenciaMb.Value,
                            });
                        if (mb.Status != ResponseStatus.Success || mb.Data == null)
                        {
                            string msg = mb.Messages.TryGetValue("$", out List<string>? errs) && errs.Count > 0
                                ? errs[0]
                                : "Não foi possível gerar a referência Multibanco.";
                            throw new InvalidOperationException(msg);
                        }
                        refEntidade = mb.Data.EntidadeMb;
                        refCodigo = mb.Data.ReferenciaCodigo;
                        refMbWay = mb.Data.MbWay;
                    }
                    return ResponseFactory.Success(new DocumentoEmissaoDTO
                    {
                        Id = documento.Id,
                        TipoDocumentoId = documento.TipoDocumentoId,
                        AnoFiscal = documento.AnoFiscal,
                        NumeroDocumento = documento.NumeroDocumento,
                        NumeroExibicao = documento.NumeroExibicao,
                        HashDocumento = documento.GlobalHash,
                        VersaoChave = documento.VersaoChave,
                        ReferenciaMbEntidade = refEntidade,
                        ReferenciaMbCodigo = refCodigo,
                        ReferenciaMbWay = refMbWay,
                    });
                });
            }
            catch (DbUpdateException dbEx) when (tentativa < maxTentativas && IsNumeroDocumentoCollision(dbEx))
            {
                repository.ClearChangeTracker();
                continue;
            }
        }
        return ResponseFactory.Fail<DocumentoEmissaoDTO>(
            "Não foi possível atribuir numeração ao documento após múltiplas tentativas. Tente novamente."
        );
    }
    catch (Exception ex)
    {
        return ResponseFactory.Fail<DocumentoEmissaoDTO>(ex.Message);
    }
}

public async Task<Response<DocumentoEmissaoDTO>> AtualizarDocumentoEmissaoAsync(
    Guid documentoId,
    EmitirDocumentoRequest request)
{
    try
    {
        return await transactionalExecutor.ExecuteAsync(async ct =>
        {
            if (request.Linhas == null || request.Linhas.Count == 0)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Documento deve conter pelo menos uma linha");
            if (request.Anulado)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Não é permitido gravar documento anulado.");

            await currentClinicaService.SetClinicaAsync();
            if (!Guid.TryParse(currentClinicaService.ClinicaId, out Guid clinicaId) || clinicaId == Guid.Empty)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Clínica atual inválida");

            Documento? documento = (
                await repository.GetListAsync<Documento, Guid>(
                    new DocumentoByIdWithLinhasSpec(documentoId, clinicaId))
            ).FirstOrDefault();
            if (documento == null)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Documento não encontrado");
            if (documento.Anulado)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Documento anulado não pode ser editado.");
            if (documento.TipoDocumentoId != request.TipoDocumentoId)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Não é permitido alterar o tipo de documento.");

            TipoDocumento? tipoDocumento = (
                await repository.GetListAsync<TipoDocumento, Guid>(
                    new TipoDocumentoByIdClinicaSpec(request.TipoDocumentoId, clinicaId))
            ).FirstOrDefault();
            if (tipoDocumento == null)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Tipo de documento não encontrado na clínica atual");

            Clinica clinica = await repository.GetByIdAsync<Clinica, Guid>(clinicaId);
            int regraFaturacao = DocumentoEmissaoCalculoHelper.ParseRegraFaturacao(clinica.Regrafaturacao);
            string? erroPerfil = DocumentoEmissaoPerfilValidator.Validar(tipoDocumento, request, regraFaturacao);
            if (erroPerfil != null)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>(erroPerfil);

            DateTime dataDocumento = request.DataDocumento ?? documento.Data ?? DateTime.Today;
            if (request.DataVencimentoPagamento.HasValue
                && request.DataVencimentoPagamento.Value.Date < dataDocumento.Date)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>(
                    "Data de vencimento não pode ser inferior à data do documento.");

            decimal descontoCliente = request.DescontoCliente ?? 0m;
            decimal descontoPagamento = request.DescontoPagamento ?? 0m;
            decimal outros = request.Outros ?? 0m;
            ModuloOrigemDocumento moduloOrigem =
                documento.ModuloOrigem ?? request.ModuloOrigem ?? ModuloOrigemDocumento.Faturacao;

            List<DocumentoEmissaoCalculoHelper.LinhaCalculoResult> linhasCalc = [];
            List<DocumentoLinha> linhas = request.Linhas.Select((linhaReq, index) =>
            {
                int numeroLinha = linhaReq.NumeroLinha > 0 ? linhaReq.NumeroLinha : index + 1;
                DocumentoEmissaoCalculoHelper.LinhaCalculoResult calc =
                    DocumentoEmissaoCalculoHelper.CalcularLinha(
                        linhaReq,
                        regraFaturacao,
                        descontoCliente,
                        descontoPagamento,
                        request.PercentagemDescontoGlobal,
                        request.IsentoIva);
                linhasCalc.Add(calc);
                return new DocumentoLinha
                {
                    Id = Guid.NewGuid(),
                    DocumentoId = documento.Id,
                    NumeroLinha = numeroLinha,
                    CodigoArtigo = DocumentoEmissaoSnapshotHelper.TruncateOptional(
                        linhaReq.CodigoArtigo,
                        DocumentoEmissaoSnapshotHelper.CodigoArtigoMax),
                    ServicoId = linhaReq.ServicoId,
                    AdmissaoServicoId = linhaReq.AdmissaoServicoId,
                    Descricao = DocumentoEmissaoSnapshotHelper.TruncateRequired(
                        linhaReq.Descricao,
                        DocumentoEmissaoSnapshotHelper.DescricaoLinhaMax,
                        "Linha"),
                    Quantidade = linhaReq.Quantidade,
                    PrecoUnitario = linhaReq.PrecoUnitario,
                    PercentagemDesconto = calc.PercentagemDescontoEfectiva,
                    ValorDesconto = calc.DescontoValor,
                    DescontoTipo1 = linhaReq.DescontoTipo1,
                    DescontoTipo2 = linhaReq.DescontoTipo2,
                    DescontoTipo3 = linhaReq.DescontoTipo3,
                    TotalLinha = DocumentoEmissaoCalculoHelper.ResolverTotalLinhaPersistencia(
                        calc,
                        regraFaturacao),
                    TaxaIvaId = linhaReq.TaxaIvaId,
                    MotivoIsencaoId = request.IsentoIva || linhaReq.TaxaIvaPercentagem == 0m
                        ? (linhaReq.MotivoIsencaoId ?? request.MotivoIsencaoId)
                        : null,
                    TaxaIvaPercentagem = request.IsentoIva ? 0m : linhaReq.TaxaIvaPercentagem,
                    ValorImposto = Math.Round(calc.ValorIva, 2, MidpointRounding.AwayFromZero),
                    ModuloOrigemLinha = moduloOrigem,
                };
            }).ToList();

            decimal retencaoValor = DocumentoEmissaoCalculoHelper.ResolverRetencaoValor(
                request.RetencaoAtiva,
                request.RetencaoTaxa,
                request.RetencaoValor,
                DocumentoEmissaoCalculoHelper.CalcularTotaisDocumento(linhasCalc, regraFaturacao, outros, 0m).Total,
                outros);
            DocumentoEmissaoCalculoHelper.DocumentoTotaisCalculo totaisDoc =
                DocumentoEmissaoCalculoHelper.CalcularTotaisDocumento(
                    linhasCalc,
                    regraFaturacao,
                    outros,
                    retencaoValor);

            string? erroDescontoOrganismo = await ValidarDescontosOrganismoEspecialAsync(
                request,
                totaisDoc.Descontos
            );
            
            if(erroDescontoOrganismo != null)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>(erroDescontoOrganismo);

            foreach (DocumentoLinha linhaAntiga in documento.Linhas.ToList())
                await repository.RemoveAsync<DocumentoLinha, Guid>(linhaAntiga);

            documento.Data = dataDocumento;
            documento.DataVencimentoPagamento = request.DataVencimentoPagamento ?? dataDocumento;
            documento.UtenteId = request.UtenteId;
            documento.OrganismoId = request.OrganismoId;
            documento.FuncionarioId = request.FuncionarioId;
            documento.NomeCliente = DocumentoEmissaoSnapshotHelper.TruncateRequired(
                request.NomeCliente,
                DocumentoEmissaoSnapshotHelper.NomeClienteMax,
                "Cliente sem nome");
            documento.MoradaCliente = DocumentoEmissaoSnapshotHelper.TruncateRequired(
                request.MoradaCliente,
                DocumentoEmissaoSnapshotHelper.MoradaClienteMax,
                "Morada não definida");
            documento.LocalidadeCliente = DocumentoEmissaoSnapshotHelper.TruncateOptional(
                request.LocalidadeCliente,
                DocumentoEmissaoSnapshotHelper.LocalidadeClienteMax);
            documento.NumeroContribuinteCliente = DocumentoEmissaoSnapshotHelper.TruncateOptional(
                request.NumeroContribuinteCliente,
                DocumentoEmissaoSnapshotHelper.NumeroContribuinteClienteMax);
            documento.CodigoPostalId = request.CodigoPostalId;
            documento.TotalDocumento = totaisDoc.Total;
            documento.TotalIva = totaisDoc.Impostos;
            documento.TotalDesconto = totaisDoc.Descontos;
            documento.TotalBruto = totaisDoc.Total + outros;
            documento.TotalLiquido = totaisDoc.APagar;
            documento.DescontoCliente = descontoCliente;
            documento.DescontoPagamento = descontoPagamento;
            documento.Outros = outros;
            documento.PrecoUnitarioMercadorias = totaisDoc.Mercadorias;
            documento.CondicaoPagamentoId = request.CondicaoPagamentoId;
            documento.ModoPagamentoId = request.ModoPagamentoId;
            documento.MoedaId = request.MoedaId;
            documento.BancoId = request.BancoId;
            documento.TaxaCambio = request.TaxaCambio;
            documento.TipoCambio = request.TipoCambio;
            documento.IsentoIva = request.IsentoIva;
            documento.MotivoIsencaoId = request.IsentoIva ? request.MotivoIsencaoId : null;
            documento.Beneficiario = DocumentoEmissaoSnapshotHelper.TruncateOptional(
                request.Beneficiario,
                DocumentoEmissaoSnapshotHelper.BeneficiarioMax);
            documento.FaturaGlobalDataInicio = request.FaturaGlobalDataInicio;
            documento.FaturaGlobalDataFim = request.FaturaGlobalDataFim;
            documento.IvaCaixa = request.IvaCaixa;
            documento.Observacoes = request.Observacoes;
            documento.CodigoValidacaoTransporte = request.CodigoValidacaoTransporte;
            documento.DataTransporte = request.DataTransporte;
            documento.HoraTransporte = request.HoraTransporte;
            documento.RetencaoImposto = request.RetencaoAtiva ? request.RetencaoImposto?.Trim() : null;
            documento.RetencaoTaxa = request.RetencaoAtiva ? request.RetencaoTaxa : null;
            documento.RetencaoValor = request.RetencaoAtiva ? retencaoValor : null;
            documento.RetencaoCodigoMotivo = request.RetencaoAtiva ? request.RetencaoCodigoMotivo : null;
            documento.RetencaoMotivo = request.RetencaoAtiva ? request.RetencaoMotivo?.Trim() : null;
            documento.DocumentoOrigemId = request.DocumentoOrigemId;
            documento.IdentificadorUnicoDocumentoOrigem = request.IdentificadorUnicoDocumentoOrigem;
            documento.DataDocumentoOrigem = request.DataDocumentoOrigem;
            documento.TipoSerie = ResolveTipoSerieEmissao(request.TipoSerie, tipoDocumento.TipoSerie);
            documento.Liquidado = request.Liquidado;
            documento.Rectificado = request.Rectificado;

            _ = await repository.UpdateAsync<Documento, Guid>(documento);
            await repository.CreateRangeAsync<DocumentoLinha, Guid>(linhas);
            documento.Linhas = linhas;
            await repository.SaveChangesAsync();

            IEnumerable<Guid> admissaoServicoIds = ExtrairAdmissaoServicoIds(request.Linhas);
            await DocumentoEmissaoClinicaSyncHelper.SincronizarAposEmissaoAsync(
                repository,
                documento.Id,
                request.TipoDocumentoId,
                admissaoServicoIds,
                moduloOrigem,
                faturado: true,
                pago: false);
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
    catch (Exception ex)
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

            TipoDocumento? tipoDocumento = await repository.GetByIdAsync<TipoDocumento, Guid>(request.TipoDocumentoId);
            if (tipoDocumento == null)
                return ResponseFactory.Fail<DocumentoEmissaoDTO>("Tipo de documento não encontrado");

            (bool pago, bool faturado) = DocumentoEmissaoAdmissaoFlagsHelper.ResolverFlags(
                tipoDocumento,
                request.Pago,
                request.Faturado);
            bool isFaturaRecibo = DocumentoEmissaoAdmissaoFlagsHelper.IsFaturaRecibo(tipoDocumento);

            Consulta? consulta = (
                await repository.GetListAsync<Consulta, Guid>(new ConsultaPorAdmissaoSpec(admissao.Id))
            ).FirstOrDefault();

            string nomeCliente = DocumentoEmissaoSnapshotHelper.ResolverNomeCliente(
                request.NomeCliente,
                admissao.Utente,
                admissao.Organismo);

            string moradaCliente = DocumentoEmissaoSnapshotHelper.ResolverMoradaCliente(
                request.MoradaCliente,
                admissao.Utente);

            string? localidadeCliente = DocumentoEmissaoSnapshotHelper.ResolverLocalidadeCliente(
                request.LocalidadeCliente,
                admissao.Utente);

            string? nifCliente = DocumentoEmissaoSnapshotHelper.ResolverNumeroContribuinteCliente(
                request.NumeroContribuinteCliente,
                admissao.Utente,
                admissao.Organismo);

            Guid? codigoPostalId = admissao.Utente?.CodigoPostalId;

            List<EmitirDocumentoLinhaRequest> linhas = admissao.Servicos
                .Select((s, index) =>
                {
                    decimal quantidade = s.Quantidade.GetValueOrDefault(1m);
                    if(quantidade <= 0) quantidade = 1m;

                    // Legado FR: TotalLinha = valor_ut; FA/outros: valor_servico.
                    decimal totalLinhaBase = isFaturaRecibo
                        ? (s.ValorUt ?? 0m)
                        : (s.ValorServico ?? s.ValorArtigo ?? 0m);
                    decimal precoEmissao = quantidade > 0
                        ? totalLinhaBase / quantidade
                        : totalLinhaBase;

                    string descricao = !string.IsNullOrWhiteSpace(s.NomeArtigo)
                        ? s.NomeArtigo
                        : "Serviço de admissão";

                    decimal taxaPct = request.IsentoIva ? 0m : (s.Servico?.TaxaIva?.Taxa ?? 0m);

                    return new EmitirDocumentoLinhaRequest
                    {
                        NumeroLinha = index + 1,
                        CodigoArtigo = s.CodigoArtigo,
                        ServicoId = s.ServicoId,
                        AdmissaoServicoId = s.Id,
                        Descricao = descricao,
                        Quantidade = quantidade,
                        PrecoUnitario = precoEmissao,
                        TaxaIvaId = s.Servico?.TaxaIvaId,
                        TaxaIvaPercentagem = taxaPct,
                        MotivoIsencaoId = request.IsentoIva || taxaPct == 0m
                            ? (s.Servico?.MotivoIsencaoId ?? request.MotivoIsencaoId)
                            : null,
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
                    
                    CondicaoPagamentoId = request.CondicaoPagamentoId,
                    ModoPagamentoId = request.ModoPagamentoId,
                    MoedaId = request.MoedaId ,
                    BancoId = request.BancoId,
                    DataVencimentoPagamento = request.DataVencimentoPagamento,

                    DescontoCliente = request.DescontoCliente,
                    DescontoPagamento = request.DescontoPagamento,
                    Outros = request.Outros,

                    IsentoIva = request.IsentoIva,
                    MotivoIsencaoId = request.IsentoIva ? request.MotivoIsencaoId : null,
                    IvaCaixa = request.IvaCaixa,

                    ModuloOrigem = ModuloOrigemDocumento.Consultas,
                    CodigoTipoDocSaft = request.CodigoTipoDocSaft,

                    Linhas = linhas,
                    AdmissaoSyncPago = pago,
                    AdmissaoSyncFaturado = faturado,
                };

                Response<DocumentoEmissaoDTO> emissao = await EmitirDocumentoAsync(emitirRequest);
                if(emissao.Status != ResponseStatus.Success || emissao.Data == null)
                    return emissao;

                admissao.Pago = pago;
                admissao.Faturado = faturado;
                _ = await repository.UpdateAsync<Admissao, Guid>(admissao);

                if (consulta != null)
                {
                    await AdmissaoFaturacaoPromocaoHelper.SincronizarComDocumentoAsync(
                        repository,
                        consulta.Id,
                        emissao.Data.Id,
                        request.TipoDocumentoId,
                        pago,
                        faturado);
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

            string nomeCliente = DocumentoEmissaoSnapshotHelper.ResolverNomeCliente(
                request.NomeCliente,
                consulta.Utente,
                consulta.Organismo);

            string moradaCliente = DocumentoEmissaoSnapshotHelper.ResolverMoradaCliente(
                request.MoradaCliente,
                consulta.Utente);

            string? localidadeCliente = DocumentoEmissaoSnapshotHelper.ResolverLocalidadeCliente(
                request.LocalidadeCliente,
                consulta.Utente);

            string? nifCliente = DocumentoEmissaoSnapshotHelper.ResolverNumeroContribuinteCliente(
                request.NumeroContribuinteCliente,
                consulta.Utente,
                consulta.Organismo);

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

                    decimal taxaPct = request.IsentoIva ? 0m : (s.Servico?.TaxaIva?.Taxa ?? 0m);

                    return new EmitirDocumentoLinhaRequest
                    {
                        NumeroLinha = index + 1,
                        CodigoArtigo = s.CodigoArtigo,
                        ServicoId = s.ServicoId,
                        Descricao = descricao,
                        Quantidade = quantidade,
                        PrecoUnitario = preco,
                        TaxaIvaId = s.Servico?.TaxaIvaId,
                        TaxaIvaPercentagem = taxaPct,
                        MotivoIsencaoId = request.IsentoIva || taxaPct == 0m
                            ? (s.Servico?.MotivoIsencaoId ?? request.MotivoIsencaoId)
                            : null,
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

                CondicaoPagamentoId = request.CondicaoPagamentoId,
                ModoPagamentoId = request.ModoPagamentoId,
                MoedaId = request.MoedaId,
                BancoId = request.BancoId,
                DataVencimentoPagamento = request.DataVencimentoPagamento,

                DescontoCliente = request.DescontoCliente,
                DescontoPagamento = request.DescontoPagamento,
                Outros = request.Outros,

                IsentoIva = request.IsentoIva,
                MotivoIsencaoId = request.IsentoIva ? request.MotivoIsencaoId : null,
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

            TipoDocumento? tipoDoc = (
                await repository.GetListAsync<TipoDocumento, Guid>(
                    new TipoDocumentoByIdClinicaSpec(documento.TipoDocumentoId, clinicaId))
            ).FirstOrDefault();

            string abrevTipo = tipoDoc?.Abreviatura?.Trim().ToUpperInvariant() ?? string.Empty;
            if (abrevTipo != "FP" && documento.Data.HasValue)
            {
                DateTime dataDoc = documento.Data.Value;
                DateTime agora = DateTime.Now;
                if (dataDoc.Month != agora.Month || dataDoc.Year != agora.Year)
                {
                    return ResponseFactory.Fail<Guid>(
                        "Só pode anular documentos do mês corrente (exceto FP).");
                }
            }

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
                d.AnoFiscal == anoFiscal
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
                        DescontoTipo1 = l.DescontoTipo1,
                        DescontoTipo2 = l.DescontoTipo2,
                        DescontoTipo3 = l.DescontoTipo3,
                        TaxaIvaId = l.TaxaIvaId,
                        MotivoIsencaoId = l.MotivoIsencaoId,
                        TaxaIvaPercentagem = documentoOrigem.IsentoIva ? 0m : l.TaxaIvaPercentagem,
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
                            PercentagemDesconto = origem.PercentagemDesconto,
                            DescontoTipo1 = origem.DescontoTipo1,
                            DescontoTipo2 = origem.DescontoTipo2,
                            DescontoTipo3 = origem.DescontoTipo3,
                            TaxaIvaId = origem.TaxaIvaId,
                            MotivoIsencaoId = origem.MotivoIsencaoId,
                            TaxaIvaPercentagem = documentoOrigem.IsentoIva
                                ? 0m
                                : (l.TaxaIvaPercentagem ?? origem.TaxaIvaPercentagem)
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

                CondicaoPagamentoId = documentoOrigem.CondicaoPagamentoId,
                ModoPagamentoId = documentoOrigem.ModoPagamentoId,
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
                DescontoCliente = documentoOrigem.DescontoCliente,
                DescontoPagamento = documentoOrigem.DescontoPagamento,
                MotivoIsencaoId = documentoOrigem.MotivoIsencaoId,

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

    private static string ResolveTipoSerieEmissao(string? requestTipoSerie, string? tipoDocumentoTipoSerie)
    {
        string? raw = string.IsNullOrWhiteSpace(requestTipoSerie)
            ? tipoDocumentoTipoSerie
            : requestTipoSerie.Trim();

        if (string.IsNullOrWhiteSpace(raw))
            return "N";

        return raw.Length > 1 ? raw[..1] : raw;
    }

    private async Task MarcarLinhasSinistradoFaturadasAsync(
        EmitirDocumentoRequest request,
        string? numeroExibicao,
        DateTime? dataDocumento)
    {
        if (!request.SinistradoId.HasValue)
            return;

        string numeroFatura = numeroExibicao ?? string.Empty;
        foreach (var linhaReq in request.Linhas)
        {
            if (!linhaReq.SinistradoLinhaServicoId.HasValue)
                continue;
            if (SinistradosInfoFaturacaoHelper.IsLinhaObservacao(linhaReq.SinistradoLinhaServicoId.Value))
                continue;

            var sl = await repository.GetByIdAsync<SinistradoLinhaServico, Guid>(
                linhaReq.SinistradoLinhaServicoId.Value);
            if (sl is null)
                continue;

            sl.NumeroTFatura = numeroFatura;
            sl.DataFatura = dataDocumento;
            await repository.UpdateAsync<SinistradoLinhaServico, Guid>(sl);
        }
    }

    private async Task<string?> ValidarDescontosOrganismoEspecialAsync(
        EmitirDocumentoRequest request,
        decimal totalDescontoDocumento
    )
    {
        if (!DocumentoEmissaoOrganismoDescontoValidator.EhFaturacaoAOrganismo(request))
            return null;

        Organismo organismo = await repository.GetByIdAsync<Organismo, Guid>(
            request.OrganismoId!.Value);
        
        return DocumentoEmissaoOrganismoDescontoValidator.Validar(
            organismo,
            request,
            totalDescontoDocumento
        );
    }

    private static bool IsNumeroDocumentoCollision(DbUpdateException ex)
    {
        Exception? sqlEx = ex.InnerException ?? ex.InnerException?.InnerException;
        int? sqlErrorNumber = GetSqlErrorNumber(sqlEx);

        if (sqlErrorNumber is 2601 or 2627)
        {
            string msg = sqlEx?.Message ?? ex.Message;
            return msg.Contains(
                    "IX_Documento_ClinicaId_TipoDocumentoId_AnoFiscal_NumeroDocumento",
                    StringComparison.OrdinalIgnoreCase
                )
                || (
                    msg.Contains("ClinicaId", StringComparison.OrdinalIgnoreCase)
                    && msg.Contains("TipoDocumentoId", StringComparison.OrdinalIgnoreCase)
                    && msg.Contains("AnoFiscal", StringComparison.OrdinalIgnoreCase)
                    && msg.Contains("NumeroDocumento", StringComparison.OrdinalIgnoreCase)
                );
        }

        return false;
    }

    private static int? GetSqlErrorNumber(Exception? ex)
    {
        if (ex == null)
            return null;

        var numberProp = ex.GetType().GetProperty("Number");
        if (numberProp?.PropertyType == typeof(int))
            return (int?)numberProp.GetValue(ex);

        return null;
    }

    private static IEnumerable<Guid> ExtrairAdmissaoServicoIds(
        IEnumerable<EmitirDocumentoLinhaRequest> linhas) =>
        linhas.SelectMany(l =>
        {
            if (l.AdmissaoServicosIds is { Count: > 0 } ids)
                return ids.Where(x => x != Guid.Empty);
            return l.AdmissaoServicoId is { } id && id != Guid.Empty ? [id] : [];
        });
}