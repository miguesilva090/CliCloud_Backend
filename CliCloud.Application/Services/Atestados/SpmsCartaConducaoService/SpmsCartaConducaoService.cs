using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using System.Globalization;
using CliCloud.Domain.Entities.Atestados;
using CliCloud.Domain.Entities.Common.Configurations;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utentes;


namespace CliCloud.Application.Services.Atestados.SpmsCartaConducaoService;

public class SpmsCartaConducaoService : ISpmsCartaConducaoService
{
    private static readonly HashSet<string> CategoriasGrupo2 = new(StringComparer.OrdinalIgnoreCase)
    {
        "C1", "C1E", "C", "CE", "D1", "D1E", "D", "DE"
    };


    public async Task<SpmsRegistoAtestadoResult> RegistarOnlineAsync(
        Atestado atestado, 
        Utente utente, 
        Medico medico, 
        Clinica clinica,
        ConfigCartaConducao config,
        IReadOnlyCollection<AtestadoCategoria> categorias,
        IReadOnlyCollection<AtestadoRestricao> restricoes, 
        IReadOnlyCollection<AtestadoRestricaoAnterior> restricoesAnteriores
    )
    {
        return await RegistarAsync(
            atestado,
            utente,
            medico,
            clinica,
            config,
            categorias,
            restricoes,
            restricoesAnteriores,
            useOfflineEndpoint: false
        );
    }

    public async Task<SpmsRegistoAtestadoResult> RegistarOfflineAsync(
        Atestado atestado,
        Utente utente,
        Medico medico,
        Clinica clinica,
        ConfigCartaConducao config,
        IReadOnlyCollection<AtestadoCategoria> categorias,
        IReadOnlyCollection<AtestadoRestricao> restricoes,
        IReadOnlyCollection<AtestadoRestricaoAnterior> restricoesAnteriores
    )
    {
        return await RegistarAsync(
            atestado,
            utente,
            medico,
            clinica,
            config,
            categorias,
            restricoes,
            restricoesAnteriores,
            useOfflineEndpoint: true
        );
    }

    private static async Task<SpmsRegistoAtestadoResult> RegistarAsync(
        Atestado atestado,
        Utente utente,
        Medico medico,
        Clinica clinica,
        ConfigCartaConducao config,
        IReadOnlyCollection<AtestadoCategoria> categorias,
        IReadOnlyCollection<AtestadoRestricao> restricoes,
        IReadOnlyCollection<AtestadoRestricaoAnterior> restricoesAnteriores,
        bool useOfflineEndpoint
    )
    {
        var targetUrl = useOfflineEndpoint ? config.UrlOffline : config.UrlOnline;
        var endpointName = useOfflineEndpoint ? "offline" : "online";

        if(string.IsNullOrWhiteSpace(targetUrl))
            return new SpmsRegistoAtestadoResult { Success = false, Message = $"Configuração inválida: URL {endpointName} em falta"};

        if(string.IsNullOrWhiteSpace(config.Utilizador) || string.IsNullOrWhiteSpace(config.Password))
            return new SpmsRegistoAtestadoResult { Success = false, Message = "Configuralção inválida: Utilizador/Password em falta"};

        var validacao = ValidatePayload(atestado, utente, medico, clinica, categorias);
        if (validacao.Count > 0)
            return new SpmsRegistoAtestadoResult { Success = false, Message = $"Payload inválido para SPMS: {string.Join(" | ", validacao)}" };

        if(string.IsNullOrWhiteSpace(atestado.NumeroSNS) && string.IsNullOrWhiteSpace(utente.NumeroUtente))
            return new SpmsRegistoAtestadoResult { Success = false, Message = "Utente sem número SNS (Numero de Utente)"};
        
        if(string.IsNullOrWhiteSpace(medico.Carteira) || medico.Carteira.Length < 2)
            return new SpmsRegistoAtestadoResult { Success = false, Message= "Médico sem cédula/carteira válida"};

        var endpoint = NormalizeWsdlToEndpoint(targetUrl);

        using var http = new HttpClient();
        var authBytes = Encoding.UTF8.GetBytes($"{config.Utilizador}:{config.Password}");
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

        var softwareCode = ResolveSoftwareCode(clinica.CodSb);
        var xml = BuildSoapEnvelope(atestado, utente, medico, clinica, config, categorias, restricoes, restricoesAnteriores, softwareCode);
        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Content = new StringContent(xml, Encoding.UTF8, "text/xml");
        _ = request.Headers.TryAddWithoutValidation("SOAPAction", "\"regista\"");

        var response = await http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        if(!response.IsSuccessStatusCode)
            return new SpmsRegistoAtestadoResult
            {
                Success = false,
                Message = BuildHttpErrorMessage((int)response.StatusCode, body)
            };
        
        return ParseResponse(body);
    }

    private static string NormalizeWsdlToEndpoint(string url)
        => url.Replace("?wsdl", "", StringComparison.OrdinalIgnoreCase)
        .Replace(".wsdl", "", StringComparison.OrdinalIgnoreCase);

    private static string BuildSoapEnvelope(
        Atestado atestado, 
        Utente utente, 
        Medico medico,
        Clinica clinica,
        ConfigCartaConducao config, 
        IReadOnlyCollection<AtestadoCategoria> categorias,
        IReadOnlyCollection<AtestadoRestricao> restricoes,
        IReadOnlyCollection<AtestadoRestricaoAnterior> restricoesAnteriores,
        string softwareCode
    )
    {
        var sexo = ResolveSexo(utente);
        var cedula = ExtractDigits(medico.Carteira);
        var autoridade = config.AutoridadeSaudePublica == 1 ? "S" : "N";
        var dtNascimento = utente.DataNascimento?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty;
        var numSns = ExtractDigits(atestado.NumeroSNS ?? utente.NumeroUtente);
        var numNif = ExtractDigits(utente.NumeroContribuinte);
        var codigoPostal = NormalizeCodigoPostal(atestado.CodigoPostal?.Codigo ?? utente.CodigoPostal?.Codigo ?? utente.Rua?.CodigoPostal?.Codigo);
        var pais = ResolveCountryCode(utente.Pais?.Prefixo ?? utente.Pais?.Codigo ?? utente.Nacionalidade);
        var codLocalPrescricao = BuildCodLocalPrescricao(clinica);
        var localPrescricao = (clinica.CccDescLocalEmissao ?? clinica.NomeComercial ?? clinica.Nome).Trim();
        var (codDistrito, codConcelho, codFreguesia) = ResolveAdministrativeCodes(utente);

        var categoriasXml = string.Join("", categorias.Select( c => 
        {
            var codigoCategoria = c.CartaConducao?.CodigoCarta ?? string.Empty;
            var situacao = CategoriasGrupo2.Contains(codigoCategoria) 
                ? (c.AptoGrupo2 == 1 ? "A" : "I")
                : (c.Apto == 1 ? "A" : "I");

            var restrCat = restricoes
                .Where(r => r.CartaConducaoId == c.CartaConducaoId)
                .Select(r => $"<restricao><codigo>{MapRestricaoCodigo(r.CartaConducaoRestricao?.CodigoRestricao ?? 0)}</codigo><anotacao>{Escape(TrimToMax(r.Anotacoes, 10))}</anotacao></restricao>")
                .ToList();

            var restricoesCategoriaXml = restrCat.Count > 0
                ? $"<restricoesCategoria>{string.Join("", restrCat)}</restricoesCategoria>"
                : string.Empty;

            return $"<categoria><codigo>{Escape(codigoCategoria)}</codigo><situacaoAptidao>{situacao}</situacaoAptidao>{restricoesCategoriaXml}</categoria>";
        }));

        var restricoesAnterioresXml = string.Join("", restricoesAnteriores.Select( r => 
            $"<restricao><codigo>{MapRestricaoCodigo(r.CartaConducaoRestricao?.CodigoRestricao ?? 0)}</codigo><anotacao>{Escape(TrimToMax(r.Anotacoes, 10))}</anotacao></restricao>"
        ));

        return $"""
        <?xml version="1.0" encoding="UTF-8"?>
        <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                          xmlns:lib="http://lib.ccc.spms"
                          xmlns:wsse="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
            <soapenv:Header>
                <lib:Cliente>
                    <software>
                        <codigo>{softwareCode}</codigo>
                        <nome>CliCloud</nome>
                        <versao>1.0</versao>
                    </software>
                </lib:Cliente>
                <wsse:Security soapenv:mustUnderstand="1">
                    <wsse:UsernameToken>
                        <wsse:Username>{Escape(config.Utilizador)}</wsse:Username>
                        <wsse:Password>{Escape(config.Password)}</wsse:Password>
                    </wsse:UsernameToken>
                </wsse:Security>
            </soapenv:Header>
            <soapenv:Body>
                    <lib:AtestadoMedico>
                        <utente>
                            <numSNS>{Escape(numSns)}</numSNS>
                            <nomeCompleto>{Escape(utente.Nome)}</nomeCompleto>
                            <dtNascimento>{dtNascimento}</dtNascimento>
                            <sexo>{sexo}</sexo>
                            <naturalidade>
                                <pais>{Escape(pais)}</pais>
                                {BuildLocationCodesXml(codDistrito, codConcelho, codFreguesia)}
                                <paisNacionalidade>{Escape(pais)}</paisNacionalidade>
                            </naturalidade>
                            <numNIF>{Escape(numNif)}</numNIF>
                            <morada>
                                <rua>{Escape(TrimToMax(utente.Rua?.Nome ?? utente.Rua?.ToString() ?? string.Empty, 150))}</rua>
                                <codigoPostal>{Escape(codigoPostal)}</codigoPostal>
                                <localidade>
                                    <pais>{Escape(pais)}</pais>
                                    {BuildLocationCodesXml(codDistrito, codConcelho, codFreguesia)}
                                </localidade>
                            </morada>
                        </utente>
                        <prescricao>
                            <nomeMedico>{Escape(medico.Nome)}</nomeMedico>
                            <numCedulaMedico>{Escape(cedula)}</numCedulaMedico>
                            <codLocalPrescricao>{Escape(codLocalPrescricao)}</codLocalPrescricao>
                            <localPrescricao>{Escape(TrimToMax(localPrescricao, 500))}</localPrescricao>
                            <dtAtestadoMedico>{atestado.DataAtestado:yyyy-MM-dd}</dtAtestadoMedico>
                            <autoridadeSaudePublica>{autoridade}</autoridadeSaudePublica>
                        </prescricao>
                        <aptidao>
                            <restricoesAnteriores>
                                <possui>{(restricoesAnteriores.Count > 0 ? "S" : "N")}</possui>
                                {restricoesAnterioresXml}
                            </restricoesAnteriores>
                            <categorias>
                                {categoriasXml}
                            </categorias>
                        </aptidao>
                        <observacoes>{Escape(atestado.Observacoes)}</observacoes>
                    </lib:AtestadoMedico>
            </soapenv:Body>
        </soapenv:Envelope>
        """;
    }

    private static SpmsRegistoAtestadoResult ParseResponse(string xml)
    {
        try
        {
            var doc = XDocument.Parse(xml);
            var fault = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "faultstring")?.Value;
            if (!string.IsNullOrWhiteSpace(fault))
                return new SpmsRegistoAtestadoResult { Success = false, Message = fault };

            var mensagens = doc.Descendants()
                .Where(x => x.Name.LocalName is "mensagem" or "resultado")
                .ToList();

            var erros = mensagens 
                .Where(m => string.Equals(
                    m.Descendants().FirstOrDefault(x => x.Name.LocalName == "codigo")?.Value,
                    "S0004",
                    StringComparison.OrdinalIgnoreCase) == false)
                .Select(m => m.Descendants().FirstOrDefault(x => x.Name.LocalName is "texto" or "descricao")?.Value)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .ToList();

            if (erros.Count > 0)
                return new SpmsRegistoAtestadoResult { Success = false, Message = string.Join(" | ", erros) };

            var numero = doc.Descendants().FirstOrDefault( x => x.Name.LocalName == "numAtestadoMedico")?.Value;
            var chavePedidoId = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "chavePedidoId")?.Value;
            if (string.IsNullOrWhiteSpace(numero))
                numero = chavePedidoId;
            if(string.IsNullOrWhiteSpace(numero))
                return new SpmsRegistoAtestadoResult { Success = false, Message = "SPMS sem número de atestado no retorno"};
            
            return new SpmsRegistoAtestadoResult { Success = true, NumeroAtestadoMedico = numero};
        }
        catch ( Exception ex)
        {
            return new SpmsRegistoAtestadoResult {Success = false, Message = $"Resposta SPMS inválida: {ex.Message}"};
        }
    }

    private static string ResolveSexo(Utente utente)
    {
        var desc = utente.Sexo?.Descricao?.Trim().ToUpperInvariant();
        if (desc == "M" || desc == "MASCULINO") return "M";
        return "F";
    }

    private static string MapRestricaoCodigo(int codigoRestricao)
    {
        return codigoRestricao switch
        {
            501 => "50.a",
            502 => "50.b",
            503 => "50.c",
            504 => "50.d",
            505 => "50.e",
            506 => "50.f",
            507 => "50.g",
            _ => codigoRestricao.ToString(CultureInfo.InvariantCulture)
        };
    }

    private static List<string> ValidatePayload(
      Atestado atestado,
      Utente utente,
      Medico medico,
      Clinica clinica,
      IReadOnlyCollection<AtestadoCategoria> categorias)
    {
      var erros = new List<string>();
      if (categorias.Count == 0) erros.Add("categorias obrigatório");
      if (string.IsNullOrWhiteSpace(utente.Nome)) erros.Add("utente.nomeCompleto obrigatório");
      if (utente.DataNascimento == null) erros.Add("utente.dtNascimento obrigatório");

      var sns = ExtractDigits(atestado.NumeroSNS ?? utente.NumeroUtente);
      if (string.IsNullOrWhiteSpace(sns) || sns.Length > 9) erros.Add("utente.numSNS inválido");

      var nif = ExtractDigits(utente.NumeroContribuinte);
      if (string.IsNullOrWhiteSpace(nif) || nif.Length != 9) erros.Add("utente.numNIF inválido");

      var codigoPostal = NormalizeCodigoPostal(atestado.CodigoPostal?.Codigo ?? utente.CodigoPostal?.Codigo ?? utente.Rua?.CodigoPostal?.Codigo);
      if (string.IsNullOrWhiteSpace(codigoPostal)) erros.Add("utente.morada.codigoPostal obrigatório");

      var pais = ResolveCountryCode(utente.Pais?.Prefixo ?? utente.Pais?.Codigo ?? utente.Nacionalidade);
      if (string.IsNullOrWhiteSpace(pais)) erros.Add("utente.naturalidade.pais obrigatório");
      var (codDistrito, codConcelho, codFreguesia) = ResolveAdministrativeCodes(utente);
      if (string.IsNullOrWhiteSpace(codDistrito) || string.IsNullOrWhiteSpace(codConcelho) || string.IsNullOrWhiteSpace(codFreguesia))
        erros.Add("utente.localidade códigos administrativos em falta (distrito/concelho/freguesia)");

      var cedula = ExtractDigits(medico.Carteira);
      if (string.IsNullOrWhiteSpace(cedula)) erros.Add("prescricao.numCedulaMedico obrigatório");

      var codLocal = BuildCodLocalPrescricao(clinica);
      if (string.IsNullOrWhiteSpace(codLocal) || codLocal.Length != 7) erros.Add("prescricao.codLocalPrescricao inválido");

      var local = (clinica.CccDescLocalEmissao ?? clinica.NomeComercial ?? clinica.Nome).Trim();
      if (string.IsNullOrWhiteSpace(local)) erros.Add("prescricao.localPrescricao obrigatório");

      return erros;
    }

    private static string ExtractDigits(string? value)
      => new((value ?? string.Empty).Where(char.IsDigit).ToArray());

    private static string TrimToMax(string? value, int max)
    {
      var v = (value ?? string.Empty).Trim();
      if (v.Length <= max) return v;
      return v[..max];
    }

    private static string NormalizeCodigoPostal(string? value)
    {
      var v = (value ?? string.Empty).Trim();
      if (string.IsNullOrWhiteSpace(v)) return string.Empty;
      if (v.Length <= 8) return v;
      return v[..8];
    }

    private static string ResolveCountryCode(string? value)
    {
      var v = (value ?? string.Empty).Trim().ToUpperInvariant();
      if (string.IsNullOrWhiteSpace(v)) return "PT";

      // Utility.Pais may store phone prefix (e.g. +351); SPMS expects country code like PT.
      if (v == "+351" || v == "351") return "PT";

      var onlyDigits = new string(v.Where(char.IsDigit).ToArray());
      if (onlyDigits == "351") return "PT";

      var onlyLetters = new string(v.Where(char.IsLetter).ToArray());
      if (onlyLetters.Length >= 2) return onlyLetters[..2];

      return "PT";
    }

    private static string BuildCodLocalPrescricao(Clinica clinica)
    {
      var regiaoDigits = ExtractDigits(clinica.Regiao);
      if (string.IsNullOrWhiteSpace(regiaoDigits)) return string.Empty;
      var first = regiaoDigits[0];
      if (first < '1' || first > '7') return string.Empty;

      var local = (clinica.CccCodLocalEmissao ?? 0).ToString(CultureInfo.InvariantCulture).PadLeft(6, '0');
      if (local.Length > 6) local = local[^6..];
      return $"{first}{local}";
    }

    private static string ResolveSoftwareCode(string? codSb)
    {
      var digits = new string((codSb ?? string.Empty).Where(char.IsDigit).ToArray());
      if (string.IsNullOrWhiteSpace(digits)) return "001";
      if (digits.Length <= 3) return digits;
      return digits[^3..];
    }

    private static (string distrito, string concelho, string freguesia) ResolveAdministrativeCodes(Utente utente)
    {
      var naturalidade = (utente.Naturalidade ?? string.Empty).Trim().ToUpperInvariant();
      var chars = new string(naturalidade.Where(char.IsLetterOrDigit).ToArray());
      if (chars.Length >= 6)
      {
        var freguesia = chars[..6];
        var concelho = chars[..4];
        var distrito = chars[..2];
        if (IsDistritoCode(distrito) && IsConcelhoCode(concelho) && IsFreguesiaCode(freguesia))
          return (distrito, concelho, freguesia);
      }

      return (string.Empty, string.Empty, string.Empty);
    }

    private static bool IsDistritoCode(string value)
      => value.Length == 2 && value.All(char.IsDigit);

    private static bool IsConcelhoCode(string value)
      => value.Length == 4 && value[..2].All(char.IsDigit) && value[2..].All(char.IsLetterOrDigit);

    private static bool IsFreguesiaCode(string value)
      => value.Length == 6 && value[..2].All(char.IsDigit) && value[2..].All(char.IsLetterOrDigit);

    private static string BuildLocationCodesXml(string distrito, string concelho, string freguesia)
    {
      var parts = new List<string>();
      if (!string.IsNullOrWhiteSpace(distrito)) parts.Add($"<codDistrito>{Escape(distrito)}</codDistrito>");
      if (!string.IsNullOrWhiteSpace(concelho)) parts.Add($"<codConcelho>{Escape(concelho)}</codConcelho>");
      if (!string.IsNullOrWhiteSpace(freguesia)) parts.Add($"<codFreguesia>{Escape(freguesia)}</codFreguesia>");
      return string.Join(string.Empty, parts);
    }

    private static string BuildHttpErrorMessage(int statusCode, string body)
    {
      var fault = TryExtractFault(body);
      if (!string.IsNullOrWhiteSpace(fault) &&
          fault.Contains("Internal Error", StringComparison.OrdinalIgnoreCase))
      {
        return $"Falha HTTP SPMS {statusCode}: {fault}. Possível clínica não registada/credencial sem permissão no SPMS.";
      }

      return $"Falha HTTP SPMS {statusCode} : {body}";
    }

    private static string? TryExtractFault(string xml)
    {
      try
      {
        var doc = XDocument.Parse(xml);
        return doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "faultstring")?.Value;
      }
      catch
      {
        return null;
      }
    }

    private static string Escape(string? value)
        => System.Security.SecurityElement.Escape(value ?? string.Empty) ?? string.Empty;
}