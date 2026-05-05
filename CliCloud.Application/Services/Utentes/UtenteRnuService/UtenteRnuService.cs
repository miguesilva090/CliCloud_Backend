using System.Xml.Linq;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService;
using CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteRnuService.DTOs;

namespace CliCloud.Application.Services.Utentes.UtenteRnuService;

public class UtenteRnuService(ISpmsPrescricaoSoapService smpsSoapService) : IUtenteRnuService
{
    private readonly ISpmsPrescricaoSoapService _smpsSoapService = smpsSoapService;

    public async Task<Response<ConsultarUtenteRnuResponse>> ConsultarUtenteAsync(
        Guid clinicaId,
        ConsultarUtenteRnuRequest request
    )
    {
        var numeroSns = (request.NumeroSns ?? string.Empty).Trim();
        var numeroCartao = (request.NumeroCartao ?? string.Empty).Trim();
        var tipoCartao = (request.TipoCartao ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(numeroSns) && string.IsNullOrWhiteSpace(numeroCartao))
            return ResponseFactory.Fail<ConsultarUtenteRnuResponse>(
                "Indique o Número SNS ou o Número de Cartão"
            );

        var corpoXml = BuildConsultaUtenteCorpoXml(numeroSns, numeroCartao, tipoCartao);

        var soapRequest = new ConsultaUtenteRequest
        {
            CodigoOperacao = "CONS",
            CorpoXml = corpoXml
        };

        var soapResponse = await _smpsSoapService.ExecutarConsultaUtenteAsync(clinicaId, soapRequest);

        if (soapResponse.Status == ResponseStatus.Failure)
        {
            var msg = soapResponse.Messages.TryGetValue("$", out var list) && list.Count > 0 
                ? list[0]
                : "Falha ao consultar o RNU";
            return ResponseFactory.Fail<ConsultarUtenteRnuResponse>(msg);
        }

        if(soapResponse.Data is null)
            return ResponseFactory.Fail<ConsultarUtenteRnuResponse>("Resposta inválida do serviço RNU ");

        var dto = ParseRnuXmlToResponse(soapResponse.Data.RawXml);
        dto.RawXml = soapResponse.Data.RawXml;

        if(!string.Equals(dto.CodigoMensagem, "2200201001", StringComparison.Ordinal))
        {
            var errorMsg = !string.IsNullOrWhiteSpace(dto.DescricaoMensagem)
                ? dto.DescricaoMensagem
                : "O RNU devolveu um código de erro";
            return ResponseFactory.Fail<ConsultarUtenteRnuResponse>(errorMsg);
        }

        return ResponseFactory.Success(dto);
    }

    private static string BuildConsultaUtenteCorpoXml(string numeroSns, string numeroCartao, string tipoCartao)
    {
        if(!string.IsNullOrWhiteSpace(numeroSns))
        {
            return $"""
            <ws:ConsultaUtente_Input xmlns:ws="http://xmlnssns.min-saude.pt/ConsultaUtenteWS">
                <ws:NumeroSNS>{Escape(numeroSns)}</ws:NumeroSNS>
            </ws:ConsultaUtente_Input>
            """;
        }

        return $"""
            <ws:ConsultaUtente_Input xmlns:ws="http://xmlnssns.min-saude.pt/ConsultaUtenteWS">
                <ws:NumeroCartao>{Escape(numeroCartao)}</ws:NumeroCartao>
                <ws:TipoCartao>{Escape(tipoCartao)}</ws:TipoCartao>
            </ws:ConsultaUtente_Input>
            """;
    }

    private static ConsultarUtenteRnuResponse ParseRnuXmlToResponse(string rawXml)
    {
        var result = new ConsultarUtenteRnuResponse();

        if(string.IsNullOrWhiteSpace(rawXml))
            return result;
        
        try
        {
            var doc = XDocument.Parse(rawXml);

            result.CodigoMensagem = PickFirstValue(doc, "Codigo");
            result.DescricaoMensagem = PickFirstValue(doc, "Descricao");
            
            result.NumeroSns = PickFirstValue(doc, "NumeroSNS");
            result.NomeCompleto = PickFirstValue(doc, "NomeCompleto");
            result.NomesProprios = PickFirstValue(doc, "NomesProprios");
            result.Apelidos = PickFirstValue(doc, "Apelidos");
            result.Sexo = PickFirstValue(doc, "Sexo");
            result.PaisNacionalidade = PickFirstValue(doc, "PaisNacionalidade");

            var dataNascRaw = PickFirstValue(doc, "DataNascimento");
            if(DateTime.TryParse(dataNascRaw, out var dt))
                result.DataNascimento = dt;

            result.Obito = ParseBoolNS(PickFirstValue(doc, "Obito"));
            result.Duplicado = ParseBoolNS(PickFirstValue(doc, "Duplicado"));

            var entidades = doc 
                .Descendants()
                .Where(x => x.Name.LocalName.Equals("EntidadeResponsavel", StringComparison.OrdinalIgnoreCase));
            
            foreach(var ent in entidades)
            {
                var codigo = ent.Elements().FirstOrDefault(x => x.Name.LocalName == "Codigo")?.Value;
                var descricao = ent.Elements().FirstOrDefault(x => x.Name.LocalName == "Descricao")?.Value;

                result.EntidadesResponsaveis.Add(new EntidadeResponsavelRnuDTO
                {
                    Codigo = string.IsNullOrWhiteSpace(codigo) ? null : codigo,
                    Descricao = string.IsNullOrWhiteSpace(descricao) ? null : descricao,

                });
            }
        }

        catch
        {

        }

        return result;
    }

    private static bool? ParseBoolNS(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            return null;
        return value.Trim().ToUpperInvariant() switch
        {
            "S" => true,
            "N" => false,
            _ => null
        };
    }

    private static string PickFirstValue(XDocument doc, params string[] possibleNames)
    {
        foreach(var name in possibleNames)
        {
            var value = doc
                .Descendants()
                .FirstOrDefault(x => x.Name.LocalName.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?.Value;

            if(!string.IsNullOrWhiteSpace(value))
                return value.Trim();
        }

        return string.Empty;
    }

    private static string Escape(string value) => 
        System.Security.SecurityElement.Escape(value) ?? string.Empty;
}