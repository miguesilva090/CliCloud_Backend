using CliCloud.Application.Services.Faturacao.AdseComunicacaoService;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;
using CliCloud.Domain.Entities.Faturacao;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Xml.Linq;

namespace CliCloud.Infrastructure.Persistence.Faturacao;

public sealed class AdseSoapClient(IHttpClientFactory httpClientFactory) : IAdseSoapClient
{
    public async Task<string?> ExecutarDocumentoAsync(
        WebserviceAdse config, int operacaoLegado, AdseComunicacaoLinhaDTO linha,
        string codigoPreFatura, byte[]? pdf, byte[]? pdfRelatorio, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(config.UrlAdse))
            return "URL ADSE não configurada.";

        using HttpClient client = httpClientFactory.CreateClient(nameof(AdseSoapClient));
        using HttpRequestMessage req = new(HttpMethod.Post, config.UrlAdse.Trim());
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

        string user = string.IsNullOrWhiteSpace(config.UserAdse) ? string.Empty : config.UserAdse.Trim();
        string pass = string.IsNullOrWhiteSpace(config.PasswordAdse) ? string.Empty : config.PasswordAdse.Trim();
        string domain = string.IsNullOrWhiteSpace(config.DominioUserAdse) ? string.Empty : config.DominioUserAdse.Trim();
        string authUser = string.IsNullOrWhiteSpace(domain) ? user : $"{domain}\\{user}";
        if (!string.IsNullOrWhiteSpace(authUser))
        {
            string basic = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{authUser}:{pass}"));
            req.Headers.Authorization = new AuthenticationHeaderValue("Basic", basic);
        }

        string envelope = BuildEnvelope(config, operacaoLegado, linha, codigoPreFatura, pdf, pdfRelatorio);
        req.Content = new StringContent(envelope, Encoding.UTF8, "text/xml");

        HttpResponseMessage resp;
        try
        {
            resp = await client.SendAsync(req, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return $"Erro de comunicação SOAP ADSE: {ex.Message}";
        }

        string xml = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        if (!resp.IsSuccessStatusCode)
            return $"SOAP ADSE HTTP {(int)resp.StatusCode}: {TryExtractSoapFault(xml) ?? "sem detalhe"}";

        string? fault = TryExtractSoapFault(xml);
        return string.IsNullOrWhiteSpace(fault) ? null : fault;
    }

    private static string BuildEnvelope(
        WebserviceAdse config,
        int operacaoLegado,
        AdseComunicacaoLinhaDTO linha,
        string codigoPreFatura,
        byte[]? pdf,
        byte[]? pdfRelatorio)
    {
        string pdfBase64 = pdf is { Length: > 0 } ? Convert.ToBase64String(pdf) : string.Empty;
        string relBase64 = pdfRelatorio is { Length: > 0 } ? Convert.ToBase64String(pdfRelatorio) : string.Empty;
        string numeroFatura = SecurityElement.Escape(linha.NumeroFatura) ?? string.Empty;
        string preFatura = SecurityElement.Escape(codigoPreFatura) ?? string.Empty;
        string passLocal = SecurityElement.Escape(config.PasslocalAdse) ?? string.Empty;
        string nomeLocal = SecurityElement.Escape(config.NomelocalAdse ?? string.Empty) ?? string.Empty;

        return $"""
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
               xmlns:xsd="http://www.w3.org/2001/XMLSchema"
               xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <Documento xmlns="http://tempuri.org/">
      <numLocal>{config.NumlocalAdse}</numLocal>
      <passLocal>{passLocal}</passLocal>
      <nomeLocal>{nomeLocal}</nomeLocal>
      <codigoPreFatura>{preFatura}</codigoPreFatura>
      <numFactura>{numeroFatura}</numFactura>
      <idDocumento>{linha.DocumentoId}</idDocumento>
      <idOrigemClinica>{linha.OrigemClinicaId}</idOrigemClinica>
      <operacao>{operacaoLegado}</operacao>
      <pdfBase64>{pdfBase64}</pdfBase64>
      <pdfRelatorioBase64>{relBase64}</pdfRelatorioBase64>
    </Documento>
  </soap:Body>
</soap:Envelope>
""";
    }

    private static string? TryExtractSoapFault(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            return "Resposta SOAP vazia.";
        try
        {
            XDocument doc = XDocument.Parse(xml);
            XElement? fault = doc.Descendants().FirstOrDefault(x => x.Name.LocalName.Equals("faultstring", StringComparison.OrdinalIgnoreCase));
            if (fault is not null && !string.IsNullOrWhiteSpace(fault.Value))
                return fault.Value.Trim();

            XElement? result = doc.Descendants().FirstOrDefault(x => x.Name.LocalName.EndsWith("Result", StringComparison.OrdinalIgnoreCase));
            if (result is not null && !string.IsNullOrWhiteSpace(result.Value))
            {
                string value = result.Value.Trim();
                if (value.Equals("OK", StringComparison.OrdinalIgnoreCase) || value.Equals("true", StringComparison.OrdinalIgnoreCase))
                    return null;
                return value;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}
