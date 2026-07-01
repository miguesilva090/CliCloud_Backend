using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Filters;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Specifications;
using CliCloud.Application.Services.Faturacao.WebserviceAdseService.Specifications;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService;

public sealed class AdseComunicacaoService(
    IRepositoryAsync repository,
    ICurrentClinicaService currentClinica,
    IAdseComunicacaoListReader listReader,
    IAdsePdfStorage pdfStorage,
    IAdseSoapClient soapClient
) : IAdseComunicacaoService
{
    public async Task<Response<AdseComunicacaoPaginatedDTO>> GetPaginatedAsync(
        string modulo, AdseComunicacaoTableFilter filter, CancellationToken ct = default)
    {
        if (!AdseComunicacaoModulo.TryParse(modulo, out string tipo))
            return ResponseFactory.Fail<AdseComunicacaoPaginatedDTO>("Módulo ADSE inválido.");

        Guid? clinicaId = await ObterClinicaIdAsync().ConfigureAwait(false);
        if (clinicaId is null)
            return ResponseFactory.Fail<AdseComunicacaoPaginatedDTO>("Clínica atual inválida.");

        WebserviceAdse? config = await ObterConfigAsync(clinicaId.Value).ConfigureAwait(false);
        if (config is null)
            return ResponseFactory.Fail<AdseComunicacaoPaginatedDTO>("Configure o organismo ADSE em Configurações.");

        AdseComunicacaoPaginatedDTO page = await listReader
            .ObterPaginadoAsync(clinicaId.Value, config.OrganismoId, tipo, filter, ct)
            .ConfigureAwait(false);

        return ResponseFactory.Success(page);
    }

    public async Task<Response<IReadOnlyList<AdsePreFaturaDTO>>> ListarPreFaturasAbertasAsync(
        string tipoPreFatura, CancellationToken ct = default)
    {
        Guid? clinicaId = await ObterClinicaIdAsync().ConfigureAwait(false);
        if (clinicaId is null)
            return ResponseFactory.Fail<IReadOnlyList<AdsePreFaturaDTO>>("Clínica atual inválida.");

        IEnumerable<AdsePreFatura> rows = await repository
            .GetListAsync<AdsePreFatura, Guid>(new AdsePreFaturasAbertasSpec(clinicaId.Value, tipoPreFatura))
            .ConfigureAwait(false);

        return ResponseFactory.Success(MapPreFaturas(rows));
    }

    public async Task<Response<IReadOnlyList<AdsePreFaturaDTO>>> ListarPreFaturasPorEstadoAsync(
        string tipoPreFatura, int estado, CancellationToken ct = default)
    {
        Guid? clinicaId = await ObterClinicaIdAsync().ConfigureAwait(false);
        if (clinicaId is null)
            return ResponseFactory.Fail<IReadOnlyList<AdsePreFaturaDTO>>("Clínica atual inválida.");

        IEnumerable<AdsePreFatura> rows = await repository
            .GetListAsync<AdsePreFatura, Guid>(new AdsePreFaturasPorEstadoSpec(clinicaId.Value, tipoPreFatura, estado))
            .ConfigureAwait(false);

        return ResponseFactory.Success(MapPreFaturas(rows));
    }

    public async Task<Response<Guid>> CriarPreFaturaAsync(
        CriarAdsePreFaturaRequest request, CancellationToken ct = default)
    {
        Guid? clinicaId = await ObterClinicaIdAsync().ConfigureAwait(false);
        if (clinicaId is null)
            return ResponseFactory.Fail<Guid>("Clínica atual inválida.");

        IEnumerable<AdsePreFatura> existentes = await repository
            .GetListAsync<AdsePreFatura, Guid>(new AdsePreFaturasPorClinicaTipoSpec(clinicaId.Value, request.TipoPreFatura))
            .ConfigureAwait(false);

        int proximaOrdem = existentes.Any() ? existentes.Max(x => x.NumOrdem) + 1 : 1;

        AdsePreFatura entity = new()
        {
            ClinicaId = clinicaId.Value,
            TipoPreFatura = request.TipoPreFatura,
            NumOrdem = proximaOrdem,
            Estado = AdseEstados.PreFaturaCriada,
            DataAbertura = DateTime.UtcNow,
        };

        _ = await repository.CreateAsync<AdsePreFatura, Guid>(entity).ConfigureAwait(false);
        _ = await repository.SaveChangesAsync().ConfigureAwait(false);
        return ResponseFactory.Success(entity.Id);
    }

    public async Task<Response<bool>> ApagarPreFaturaAsync(Guid id, CancellationToken ct = default)
    {
        AdsePreFatura? entity = await repository.GetByIdAsync<AdsePreFatura, Guid>(id).ConfigureAwait(false);
        if (entity is null || entity.DeletedOn is not null)
            return ResponseFactory.Fail<bool>("Pré-fatura não encontrada.");
        if (entity.Estado >= AdseEstados.PreFaturaFechada)
            return ResponseFactory.Fail<bool>("Não é possível apagar pré-fatura fechada.");

        await repository.RemoveAsync<AdsePreFatura, Guid>(entity).ConfigureAwait(false);
        _ = await repository.SaveChangesAsync().ConfigureAwait(false);
        return ResponseFactory.Success(true);
    }

    public async Task<Response<Guid>> RegistarPdfAsync(
        AdseUploadPdfRequest request, string tipoPreFatura, CancellationToken ct = default)
    {
        Guid? clinicaId = await ObterClinicaIdAsync().ConfigureAwait(false);
        if (clinicaId is null) return ResponseFactory.Fail<Guid>("Clínica atual inválida.");

        WebserviceAdse? config = await ObterConfigAsync(clinicaId.Value).ConfigureAwait(false);
        if (config is null) return ResponseFactory.Fail<Guid>("Configure o webservice ADSE.");

        byte[] bytes;
        try { bytes = Convert.FromBase64String(request.ConteudoBase64); }
        catch { return ResponseFactory.Fail<Guid>("PDF inválido."); }

        string nome = string.IsNullOrWhiteSpace(request.NomeFicheiro)
            ? $"{request.DocumentoId:N}.pdf"
            : request.NomeFicheiro.Trim();

        string ficheiro = await pdfStorage
            .GuardarAsync(clinicaId.Value, config.PastaPdfAdse, nome, bytes, ct)
            .ConfigureAwait(false);

        AdseCoPagamento? cop = (await repository
            .GetListAsync<AdseCoPagamento, Guid>(new AdseCoPagamentoPorDocumentoSpec(request.DocumentoId))
            .ConfigureAwait(false)).FirstOrDefault();

        if (cop is null)
        {
            cop = new AdseCoPagamento
            {
                ClinicaId = clinicaId.Value,
                DocumentoId = request.DocumentoId,
                OrigemClinicaId = request.OrigemClinicaId,
                TipoPreFatura = tipoPreFatura,
                Estado = AdseEstados.CoPagamentoPorComunicarComPdf,
            };
            _ = await repository.CreateAsync<AdseCoPagamento, Guid>(cop).ConfigureAwait(false);
        }
        else if (cop.Estado == AdseEstados.CoPagamentoPorComunicarSemPdf)
        {
            cop.Estado = AdseEstados.CoPagamentoPorComunicarComPdf;
        }

        if (request.RelatorioMedico) cop.PdfRelatorioFicheiro = ficheiro;
        else cop.PdfFicheiro = ficheiro;

        if (cop.Id != Guid.Empty)
            _ = await repository.UpdateAsync<AdseCoPagamento, Guid>(cop).ConfigureAwait(false);

        _ = await repository.SaveChangesAsync().ConfigureAwait(false);
        return ResponseFactory.Success(cop.Id);
    }

    public async Task<Response<string>> ComunicarDocumentosAsync(
        AdseComunicarDocumentosRequest request, CancellationToken ct = default)
    {
        Guid? clinicaId = await ObterClinicaIdAsync().ConfigureAwait(false);
        if (clinicaId is null) return ResponseFactory.Fail<string>("Clínica atual inválida.");

        WebserviceAdse? config = await ObterConfigAsync(clinicaId.Value).ConfigureAwait(false);
        if (config is null) return ResponseFactory.Fail<string>("Configure o organismo ADSE.");

        string codigoPf = AdseEstados.CodigoPreFatura(request.TipoPreFatura, request.NumOrdemPreFatura);
        List<string> erros = [];

        foreach (AdseComunicarLinhaRequest linha in request.Linhas)
        {
            AdseCoPagamento? cop = (await repository
                .GetListAsync<AdseCoPagamento, Guid>(new AdseCoPagamentoPorDocumentoSpec(linha.DocumentoId))
                .ConfigureAwait(false)).FirstOrDefault();

            string? validacao = ValidarOperacao(request.Operacao, cop);
            if (validacao is not null) { erros.Add(validacao); continue; }

            AdseComunicacaoLinhaDTO dto = new()
            {
                OrigemClinicaId = linha.OrigemClinicaId,
                DocumentoId = linha.DocumentoId,
                NumeroFatura = linha.NumeroFatura,
            };

            byte[]? pdf = cop?.PdfFicheiro is not null
                ? await pdfStorage.LerAsync(clinicaId.Value, config.PastaPdfAdse, cop.PdfFicheiro, ct) : null;
            byte[]? pdfRel = cop?.PdfRelatorioFicheiro is not null
                ? await pdfStorage.LerAsync(clinicaId.Value, config.PastaPdfAdse, cop.PdfRelatorioFicheiro, ct) : null;

            string? erroWs = await soapClient
                .ExecutarDocumentoAsync(config, request.Operacao, dto, codigoPf, pdf, pdfRel, ct)
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(erroWs))
            {
                if (cop is not null) { cop.Erros = erroWs; _ = await repository.UpdateAsync<AdseCoPagamento, Guid>(cop).ConfigureAwait(false); }
                erros.Add(erroWs);
                continue;
            }

            if (cop is not null)
            {
                AplicarSucesso(cop, request.Operacao, request.TipoPreFatura, request.NumOrdemPreFatura);
                cop.Erros = null;
                _ = await repository.UpdateAsync<AdseCoPagamento, Guid>(cop).ConfigureAwait(false);
            }
        }

        _ = await repository.SaveChangesAsync().ConfigureAwait(false);
        return erros.Count == 0
            ? ResponseFactory.Success("Comunicação concluída.")
            : ResponseFactory.Fail<string>(string.Join(Environment.NewLine, erros));
    }

    private static void AplicarSucesso(AdseCoPagamento cop, int operacao, string tipo, int numOrdem)
    {
        if (operacao == AdseEstados.OperacaoComunicar)
        {
            cop.Estado = AdseEstados.CoPagamentoComunicado;
            cop.DataComunicacao = DateTime.UtcNow;
            cop.TipoPreFatura = tipo;
            cop.NumOrdemPreFatura = numOrdem;
        }
        else if (operacao == AdseEstados.OperacaoEliminar)
        {
            cop.Estado = AdseEstados.CoPagamentoPorComunicarComPdf;
            cop.DataComunicacao = null;
            cop.NumOrdemPreFatura = null;
        }
    }

    private static string? ValidarOperacao(int operacao, AdseCoPagamento? cop)
    {
        if (operacao is AdseEstados.OperacaoValidar or AdseEstados.OperacaoComunicar)
        {
            if (cop is null || string.IsNullOrWhiteSpace(cop.PdfFicheiro)) return "PDF obrigatório.";
            if (cop.Estado is AdseEstados.CoPagamentoComunicado or AdseEstados.CoPagamentoFechado)
                return "Documento já comunicado ou fechado.";
        }
        if (operacao == AdseEstados.OperacaoEliminar && cop?.Estado != AdseEstados.CoPagamentoComunicado)
            return "Só é possível eliminar documentos comunicados.";
        return null;
    }

    private async Task<Guid?> ObterClinicaIdAsync()
    {
        await currentClinica.SetClinicaAsync().ConfigureAwait(false);
        return Guid.TryParse(currentClinica.ClinicaId, out Guid id) ? id : null;
    }

    private async Task<WebserviceAdse?> ObterConfigAsync(Guid clinicaId)
    {
        IEnumerable<WebserviceAdse> rows = await repository
            .GetListAsync<WebserviceAdse, Guid>(new WebserviceAdsePorClinicaSpec(clinicaId))
            .ConfigureAwait(false);
        return rows.FirstOrDefault();
    }

    private static IReadOnlyList<AdsePreFaturaDTO> MapPreFaturas(IEnumerable<AdsePreFatura> rows) =>
        rows.Select(x => new AdsePreFaturaDTO
        {
            Id = x.Id,
            TipoPreFatura = x.TipoPreFatura,
            NumOrdem = x.NumOrdem,
            Codigo = AdseEstados.CodigoPreFatura(x.TipoPreFatura, x.NumOrdem),
            Estado = x.Estado,
            EstadoDescricao = AdseEstados.DescricaoPreFatura(x.Estado),
            DataAbertura = x.DataAbertura,
            DataFecho = x.DataFecho,
            ValorTotal = x.ValorTotal,
            NumDocumentos = x.NumDocumentos,
            ReferenciaSerie = x.ReferenciaSerie,
            ReferenciaNumeroDocumento = x.ReferenciaNumeroDocumento,
            ReferenciaData = x.ReferenciaData,
            PdfFicheiro = x.PdfFicheiro,
            NumeroFaturaReferencia = FormatarFaturaReferencia(x.ReferenciaSerie, x.ReferenciaNumeroDocumento),
        }).ToList();

    private static string FormatarFaturaReferencia(string? serie, int? numero) =>
        numero is not int n
            ? string.Empty
            : !string.IsNullOrWhiteSpace(serie)
                ? $"{serie.Trim()}/{n}"
                : n.ToString();
}
