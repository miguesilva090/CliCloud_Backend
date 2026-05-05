using System.Text;
using System.Xml.Linq;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ConfigWebServiceService.Specifications;
using CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService.DTOs;
using CliCloud.Domain.Entities.Common.Configurations;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using System.Net.Http.Headers;

namespace CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService;

public class SpmsPrescricaoSoapService(IRepositoryAsync repository) : ISpmsPrescricaoSoapService
{
    private readonly IRepositoryAsync _repository = repository;
    private const string OperacaoSucesso = "100006010001";
    private const string ConsultaUtenteSoapAction = "\"http://xmlnssns.min-saude.pt/ConsultaUtente/process\"";

    public async Task<Response<ProxyTokenResultDTO>> ObterTokenCredAsync(Guid clinicaId, ObterTokenCredRequest request)
    {
        var cfg = await ObterConfigAsync(clinicaId);
        if (cfg is null) return ResponseFactory.Fail<ProxyTokenResultDTO>("Configuração de WebService não encontrada");

        var medico = await _repository.GetByIdAsync<Medico, Guid>(request.MedicoId);
        if(medico is null) return ResponseFactory.Fail<ProxyTokenResultDTO>("Médico não encontrado");
        var clinica = await _repository.GetByIdAsync<Clinica, Guid>(clinicaId);
        if (clinica is null) return ResponseFactory.Fail<ProxyTokenResultDTO>("Clínica não encontrada");
        if(string.IsNullOrWhiteSpace(medico.LoginPRVR)) return ResponseFactory.Fail<ProxyTokenResultDTO>("Médico sem login PRVR");
        if(string.IsNullOrWhiteSpace(medico.GrupoFuncional)) return ResponseFactory.Fail<ProxyTokenResultDTO>("Médico sem grupo funcional");
        if(string.IsNullOrWhiteSpace(clinica.LocalPrescricao)) return ResponseFactory.Fail<ProxyTokenResultDTO>("Clínica sem LocalPrescricao");
        if(string.IsNullOrWhiteSpace(cfg.ProxyAutenticacao)) return ResponseFactory.Fail<ProxyTokenResultDTO>("Proxy de autenticação em falta");
        if(string.IsNullOrWhiteSpace(cfg.LoginAutenticacao) || string.IsNullOrWhiteSpace(cfg.PasswordAutenticacao))
            return ResponseFactory.Fail<ProxyTokenResultDTO>("Login/Password de autenticação em falta");
        if(string.IsNullOrWhiteSpace(request.PasswordPrvr))
            return ResponseFactory.Fail<ProxyTokenResultDTO>("Password PRVR obrigatória");

        var opXml = ConstruirProxyAutenticacaoCredXml(medico, clinica, request.PasswordPrvr);
        return await ExecutarOperacaoProxyAutenticacaoAsync(cfg, opXml);
    }

    public async Task<Response<ProxyTokenResultDTO>> ObterTokenCcAsync(Guid clinicaId, ObterTokenAssinadoRequest request)
    {
        var cfg = await ObterConfigAsync(clinicaId);
        if (cfg is null) return ResponseFactory.Fail<ProxyTokenResultDTO>("Configuração de WebService não encontrada");
        var medico = await _repository.GetByIdAsync<Medico, Guid>(request.MedicoId);
        if (medico is null) return ResponseFactory.Fail<ProxyTokenResultDTO>("Médico não encontrado");
        if (string.IsNullOrWhiteSpace(medico.NumeroContribuinte))
            return ResponseFactory.Fail<ProxyTokenResultDTO>("Médico sem NIF");
        if (string.IsNullOrWhiteSpace(medico.GrupoFuncional))
            return ResponseFactory.Fail<ProxyTokenResultDTO>("Médico sem grupo funcional");
        if (string.IsNullOrWhiteSpace(request.DigestValue) || string.IsNullOrWhiteSpace(request.SignatureValue) ||
            string.IsNullOrWhiteSpace(request.Assinatura) || string.IsNullOrWhiteSpace(request.AssinaturaSubCa))
            return ResponseFactory.Fail<ProxyTokenResultDTO>("Assinatura digital incompleta");

        var opXml = ConstruirProxyAutenticacaoCcComXml("CC", medico, request);
        return await ExecutarOperacaoProxyAutenticacaoAsync(cfg, opXml);
    }

    public async Task<Response<ProxyTokenResultDTO>> ObterTokenComAsync(Guid clinicaId, ObterTokenAssinadoRequest request)
    {
        var cfg = await ObterConfigAsync(clinicaId);
        if (cfg is null) return ResponseFactory.Fail<ProxyTokenResultDTO>("Configuração de WebService não encontrada");
        var medico = await _repository.GetByIdAsync<Medico, Guid>(request.MedicoId);
        if (medico is null) return ResponseFactory.Fail<ProxyTokenResultDTO>("Médico não encontrado");
        if (string.IsNullOrWhiteSpace(medico.GrupoFuncional))
            return ResponseFactory.Fail<ProxyTokenResultDTO>("Médico sem grupo funcional");
        if (string.IsNullOrWhiteSpace(request.DigestValue) || string.IsNullOrWhiteSpace(request.SignatureValue) ||
            string.IsNullOrWhiteSpace(request.Assinatura) || string.IsNullOrWhiteSpace(request.AssinaturaSubCa))
            return ResponseFactory.Fail<ProxyTokenResultDTO>("Assinatura digital incompleta");

        var opXml = ConstruirProxyAutenticacaoCcComXml("COM", medico, request);
        return await ExecutarOperacaoProxyAutenticacaoAsync(cfg, opXml);
    }

    public async Task<Response<SpmsSoapOperationResultDTO>> ExecutarConsultaUtenteAsync(Guid clinicaId, ConsultaUtenteRequest request)
    {
        var cfg = await ObterConfigAsync(clinicaId);
        if (cfg is null) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("Configuração de WebService não encontrada");
        if (string.IsNullOrWhiteSpace(cfg.UrlRnu)) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("URL RNU em falta");
        if (string.IsNullOrWhiteSpace(request.CorpoXml)) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("CorpoXml é obrigatório");

        // RNU ConsultaUtente (2.00) usa credenciais ACSS.
        var login = !string.IsNullOrWhiteSpace(cfg.LoginAcss) ? cfg.LoginAcss : cfg.LoginAutenticacao;
        var password = !string.IsNullOrWhiteSpace(cfg.PasswordAcss) ? cfg.PasswordAcss : cfg.PasswordAutenticacao;
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("Login/Password ACSS em falta");

        var envelope = ConstruirEnvelopeSoapConsultaUtente(login, password, request.CorpoXml.Trim());
        var raw = await EnviarSoapAsync(cfg.UrlRnu, envelope, ConsultaUtenteSoapAction);
        if (!raw.ok) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>(raw.errorMessage ?? "Erro SOAP");

        var parse = ParseSoap(raw.xml ?? string.Empty);
        if (!parse.ok) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>(parse.errorMessage ?? "Erro SOAP");

        return ResponseFactory.Success(new SpmsSoapOperationResultDTO
        {
            Codigo = parse.codigo,
            Descricao = parse.descricao,
            Token = parse.token,
            RawXml = raw.xml ?? string.Empty
        });
    }

    public async Task<Response<SpmsSoapOperationResultDTO>> ExecutarRegistoPrescricaoAsync(Guid clinicaId, RegistoPrescricaoRequest request)
    {
        var cfg = await ObterConfigAsync(clinicaId);
        if (cfg is null) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("Configuração de WebService não encontrada");
        if (string.IsNullOrWhiteSpace(cfg.UrlAcss)) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("URL ACSS em falta");
        if (string.IsNullOrWhiteSpace(request.CorpoXml)) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("CorpoXml é obrigatório");
        var body = BuildOperationRequestXml(
            "reg",
            "http://xmlns.dmm.spms.pt/201207/RegistoPrescricaoMedicamentos",
            "RegistoPrescricaoMedicamentosProcessRequest",
            request.CodigoOperacao,
            request.EnviadoEmUtc,
            request.AtivadoEmUtc,
            request.ChavePedido,
            request.ChavePedidoRelacionado,
            request.CorpoXml
        );
        return await ExecutarOperacaoGenericaAsync(cfg, cfg.UrlAcss, body, "process");
    }

    public async Task<Response<SpmsSoapOperationResultDTO>> ExecutarRegistoPrescricaoRspAsync(Guid clinicaId, RegistoPrescricaoRspRequest request)
    {
        var cfg = await ObterConfigAsync(clinicaId);
        if (cfg is null) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("Configuração de WebService não encontrada");
        if (string.IsNullOrWhiteSpace(cfg.UrlAcssRsp)) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("URL ACSS RSP em falta");
        if (string.IsNullOrWhiteSpace(request.CorpoXml)) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("CorpoXml é obrigatório");
        var body = BuildOperationRequestXml(
            "rsp",
            "http://xmlns.dmm.spms.pt/201207/RegistoPrescricaoMedicamentosRSP",
            "RegistoPrescricaoMedicamentosRSPProcessRequest",
            request.CodigoOperacao,
            request.EnviadoEmUtc,
            request.AtivadoEmUtc,
            request.ChavePedido,
            request.ChavePedidoRelacionado,
            request.CorpoXml
        );
        return await ExecutarOperacaoGenericaAsync(cfg, cfg.UrlAcssRsp, body, "process");
    }

    private async Task<Response<ProxyTokenResultDTO>> ExecutarOperacaoProxyAutenticacaoAsync(ConfigWebService cfg, string operationBodyXml)
    {
        if (string.IsNullOrWhiteSpace(cfg.ProxyAutenticacao)) return ResponseFactory.Fail<ProxyTokenResultDTO>("Proxy de autenticação em falta");
        if (string.IsNullOrWhiteSpace(cfg.LoginAutenticacao) || string.IsNullOrWhiteSpace(cfg.PasswordAutenticacao))
            return ResponseFactory.Fail<ProxyTokenResultDTO>("Login/Password de autenticação em falta");

        var envelope = ConstruirEnvelopeSoap(cfg.LoginAutenticacao, cfg.PasswordAutenticacao, operationBodyXml);
        var raw = await EnviarSoapAsync(cfg.ProxyAutenticacao, envelope, "process");
        if (!raw.ok) return ResponseFactory.Fail<ProxyTokenResultDTO>(raw.errorMessage ?? "Erro SOAP");

        var parse = ParseSoap(raw.xml ?? string.Empty);
        if (!parse.ok) return ResponseFactory.Fail<ProxyTokenResultDTO>(parse.errorMessage ?? "Erro SOAP");
        if (!string.Equals(parse.codigo, OperacaoSucesso, StringComparison.Ordinal))
            return ResponseFactory.Fail<ProxyTokenResultDTO>($"Autenticação: {parse.descricao ?? "Operação inválida"}");

        return ResponseFactory.Success(new ProxyTokenResultDTO
        {
            Codigo = parse.codigo,
            Descricao = parse.descricao,
            Token = parse.token
        });
    }

    private async Task<Response<SpmsSoapOperationResultDTO>> ExecutarOperacaoGenericaAsync(
        ConfigWebService cfg,
        string endpoint,
        string operationBodyXml,
        string soapAction
    )
    {
        if (string.IsNullOrWhiteSpace(operationBodyXml))
            return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("Corpo XML da operação é obrigatório");
        if (string.IsNullOrWhiteSpace(cfg.LoginAutenticacao) || string.IsNullOrWhiteSpace(cfg.PasswordAutenticacao))
            return ResponseFactory.Fail<SpmsSoapOperationResultDTO>("Login/Password de autenticação em falta");

        var envelope = ConstruirEnvelopeSoap(cfg.LoginAutenticacao, cfg.PasswordAutenticacao, operationBodyXml.Trim());
        var raw = await EnviarSoapAsync(endpoint, envelope, soapAction);
        if (!raw.ok) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>(raw.errorMessage ?? "Erro SOAP");

        var parse = ParseSoap(raw.xml ?? string.Empty);
        if (!parse.ok) return ResponseFactory.Fail<SpmsSoapOperationResultDTO>(parse.errorMessage ?? "Erro SOAP");

        return ResponseFactory.Success(new SpmsSoapOperationResultDTO
        {
            Codigo = parse.codigo,
            Descricao = parse.descricao,
            Token = parse.token,
            RawXml = raw.xml ?? string.Empty
        });
    }

    private static string ConstruirProxyAutenticacaoCredXml(Medico medico, Clinica clinica, string passwordPrvr)
    {
        var now = DateTime.UtcNow.ToString("o");
        return $"""
            <prox:ProxyAutenticacaoProcessRequest xmlns:prox="http://xmlns.dmm.spms.pt/201207/ProxyAutenticacao" xmlns:int="http://xmlns.dmm.spms.pt/201207/Integration">
                <prox:Cabecalho>
                    <int:Operacao><int:Codigo>CRED</int:Codigo></int:Operacao>
                    <int:EnviadoEm>{now}</int:EnviadoEm>
                    <int:AtivadoEm>{now}</int:AtivadoEm>
                    <int:ChavePedido/>
                    <int:ChavePedidoRelacionado/>
                </prox:Cabecalho>
                <prox:Corpo>
                    <prox:ProxyAutenticacao>
                        <prox:Utilizador>{Escape(medico.LoginPRVR)}</prox:Utilizador>
                        <prox:Password>{Escape(passwordPrvr)}</prox:Password>
                        <prox:LocalPrescricao>{Escape(clinica.LocalPrescricao)}</prox:LocalPrescricao>
                        <prox:TipoOrdemProfissional>{Escape(medico.GrupoFuncional)}</prox:TipoOrdemProfissional>
                    </prox:ProxyAutenticacao>
                </prox:Corpo>
            </prox:ProxyAutenticacaoProcessRequest>
            """;
    }

    private static string ConstruirProxyAutenticacaoCcComXml(string operacao, Medico medico, ObterTokenAssinadoRequest req)
    {
        var now = DateTime.UtcNow.ToString("o");
        var nifTag = operacao == "CC" ? $"<prox:NIF>{Escape(medico.NumeroContribuinte)}</prox:NIF>" : string.Empty;
        return $"""
            <prox:ProxyAutenticacaoProcessRequest xmlns:prox="http://xmlns.dmm.spms.pt/201207/ProxyAutenticacao" xmlns:int="http://xmlns.dmm.spms.pt/201207/Integration" xmlns:xd="http://www.w3.org/2017/01/xmldsig#">
                <prox:Cabecalho>
                    <int:Operacao><int:Codigo>{operacao}</int:Codigo></int:Operacao>
                    <int:EnviadoEm>{now}</int:EnviadoEm>
                    <int:AtivadoEm>{now}</int:AtivadoEm>
                    <int:ChavePedido/>
                    <int:ChavePedidoRelacionado/>
                </prox:Cabecalho>
                <prox:Corpo>
                    <prox:ProxyAutenticacao>
                        {nifTag}
                        <prox:TipoOrdemProfissional>{Escape(medico.GrupoFuncional)}</prox:TipoOrdemProfissional>
                        <prox:LocalPrescricao />
                        <xd:Signature>
                            <xd:SignedInfo>
                                <xd:CanonicalizationMethod Algorithm="http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments" />
                                <xd:SignatureMethod Algorithm="http://www.w3.org/2000/09/xmldsig#rsa-sha1" />
                                <xd:Reference URI="">
                                    <xd:DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1" />
                                    <xd:DigestValue>{Escape(req.DigestValue)}</xd:DigestValue>
                                </xd:Reference>
                            </xd:SignedInfo>
                            <xd:SignatureValue>{Escape(req.SignatureValue)}</xd:SignatureValue>
                            <xd:KeyInfo>
                                <xd:X509Data>
                                    <xd:X509Certificate>{Escape(req.Assinatura)}</xd:X509Certificate>
                                    <xd:X509Certificate>{Escape(req.AssinaturaSubCa)}</xd:X509Certificate>
                                </xd:X509Data>
                            </xd:KeyInfo>
                        </xd:Signature>
                    </prox:ProxyAutenticacao>
                </prox:Corpo>
            </prox:ProxyAutenticacaoProcessRequest>
            """;
    }

    private static string ConstruirEnvelopeSoap(string loginAutenticacao, string passwordAutenticacao, string bodyXml)
    {
        return $"""
            <?xml version="1.0" encoding="utf-8"?>
            <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/">
                <soapenv:Header>
                    <wsse:Security xmlns:wsse="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
                        <wsse:UsernameToken xmlns:wsu="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd" wsu:Id="UsernameToken-1">
                            <wsse:Username>{Escape(loginAutenticacao)}</wsse:Username>
                            <wsse:Password>{Escape(passwordAutenticacao)}</wsse:Password>
                        </wsse:UsernameToken>
                    </wsse:Security>
                </soapenv:Header>
                <soapenv:Body>
                    {bodyXml}
                </soapenv:Body>
            </soapenv:Envelope>
            """;
    }

    private static string ConstruirEnvelopeSoapConsultaUtente(string login, string password, string bodyXml)
    {
        return $"""
            <?xml version="1.0" encoding="utf-8"?>
            <soapenv:Envelope
              xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
              xmlns:wsse="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"
              xmlns:ws="http://xmlnssns.min-saude.pt/ConsultaUtenteWS">
              <soapenv:Header>
                <wsse:Security>
                  <wsse:UsernameToken>
                    <wsse:Username>{Escape(login)}</wsse:Username>
                    <wsse:Password>{Escape(password)}</wsse:Password>
                  </wsse:UsernameToken>
                </wsse:Security>
              </soapenv:Header>
              <soapenv:Body>
                {bodyXml}
              </soapenv:Body>
            </soapenv:Envelope>
            """;
    }

    private static async Task<(bool ok, string? xml, string? errorMessage)> EnviarSoapAsync(
        string endpoint,
        string xml,
        string soapAction
    )
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        using var req = new HttpRequestMessage(HttpMethod.Post, endpoint);
        req.Headers.TryAddWithoutValidation("SOAPAction", soapAction);
        req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
        req.Content = new StringContent(xml, Encoding.UTF8, "text/xml");

        var resp = await http.SendAsync(req);
        var respXml = await resp.Content.ReadAsStringAsync();

        if (!resp.IsSuccessStatusCode)
        {
            var fault = ExtractFaultStringFromSoapXml(respXml);
            var msg = !string.IsNullOrWhiteSpace(fault)
                ? $"HTTP {(int)resp.StatusCode}: {fault}"
                : $"HTTP {(int)resp.StatusCode}: erro SOAP";
            return (false, respXml, msg);
        }

        return (true, respXml, null);
    }

    private static (bool ok, string? codigo, string? descricao, string? token, string? errorMessage) ParseSoap(string xml)
    {
        try
        {
            var doc = XDocument.Parse(xml);
            var fault = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "faultstring")?.Value;
            if (!string.IsNullOrWhiteSpace(fault))
                return (false, null, null, null, fault);

            var codigo = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "Codigo")?.Value;
            var descricao = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "Descricao")?.Value;
            var token = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "Token")?.Value;
            return (true, codigo, descricao, token, null);
        }
        catch (Exception ex)
        {
            return (false, null, null, null, $"XML inválido: {ex.Message}");
        }
    }

    private static string? ExtractFaultStringFromSoapXml(string? xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            return null;
        try
        {
            var doc = XDocument.Parse(xml);
            return doc.Descendants()
                .FirstOrDefault(x => x.Name.LocalName.Equals("faultstring", StringComparison.OrdinalIgnoreCase))
                ?.Value
                ?.Trim();
        }
        catch
        {
            return null;
        }
    }

    private async Task<ConfigWebService?> ObterConfigAsync(Guid clinicaId)
    {
        return (await _repository.GetListAsync<ConfigWebService, Guid>(new ConfigWebServicePorClinicaSpec(clinicaId))).FirstOrDefault();
    }

    private static string BuildOperationRequestXml(
        string operationPrefix,
        string operationNamespace,
        string operationElementName,
        string codigoOperacao,
        DateTime? enviadoEmUtc,
        DateTime? ativadoEmUtc,
        string? chavePedido,
        string? chavePedidoRelacionado,
        string corpoXml
    )
    {
        var enviado = (enviadoEmUtc ?? DateTime.UtcNow).ToString("o");
        var ativado = (ativadoEmUtc ?? DateTime.UtcNow).ToString("o");
        var corpo = corpoXml?.Trim() ?? string.Empty;

        return $"""
            <{operationPrefix}:{operationElementName} xmlns:{operationPrefix}="{operationNamespace}" xmlns:int="http://xmlns.dmm.spms.pt/201207/Integration">
                <{operationPrefix}:Cabecalho>
                    <int:Operacao><int:Codigo>{Escape(codigoOperacao)}</int:Codigo></int:Operacao>
                    <int:EnviadoEm>{enviado}</int:EnviadoEm>
                    <int:AtivadoEm>{ativado}</int:AtivadoEm>
                    <int:ChavePedido>{Escape(chavePedido)}</int:ChavePedido>
                    <int:ChavePedidoRelacionado>{Escape(chavePedidoRelacionado)}</int:ChavePedidoRelacionado>
                </{operationPrefix}:Cabecalho>
                <{operationPrefix}:Corpo>
                    {corpo}
                </{operationPrefix}:Corpo>
            </{operationPrefix}:{operationElementName}>
            """;
    }

    private static string Escape(string? v) => System.Security.SecurityElement.Escape(v ?? string.Empty) ?? string.Empty;
}