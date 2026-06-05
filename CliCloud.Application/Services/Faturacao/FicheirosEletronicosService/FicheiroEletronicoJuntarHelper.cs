using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Organismos;
using System.Globalization;
using System.Text;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService;

internal static class FicheiroEletronicoJuntarHelper
{
    private const string Moeda = "EUR";

    public static FicheiroEletronicoGeradoDTO Juntar(
        string sigla,
        Documento documento,
        Clinica clinica,
        Organismo organismo,
        IReadOnlyList<FicheiroEletronicoAnexoDTO> ficheiros,
        DateTime dataSistema)
    {
        if (ficheiros.Count < 2)
            throw new InvalidOperationException("Deve selecionar pelo menos 2 ficheiros.");

        return sigla.Trim().ToUpperInvariant() switch
        {
            "SAD/GNR" or "SADGNR" => JuntarSadGnr(documento, clinica, ficheiros, dataSistema),
            "ADM" => JuntarAdm(documento, clinica, organismo, ficheiros, dataSistema),
            "SAD/PSP" or "SADPSP" => JuntarSadPsp(documento, organismo, ficheiros, dataSistema),
            _ => throw new InvalidOperationException($"Sigla inválida para juntar ficheiros: {sigla}"),
        };
    }

    private static FicheiroEletronicoGeradoDTO JuntarSadGnr(
        Documento documento,
        Clinica clinica,
        IReadOnlyList<FicheiroEletronicoAnexoDTO> ficheiros,
        DateTime dataSistema)
    {
        FicheiroEletronicoDataHelper.ValidarClinicaParaGeracao(clinica);

        string filial = FicheiroEletronicoFormatHelper.ObterFilial(clinica.Sucursal);
        string contribuinte = clinica.NumeroContribuinte!.Trim();
        string nome = $"{documento.Data:yyyyMM}ADMG{contribuinte}{filial}";

        var conteudo = new List<string>();
        string h1 = "H1";
        h1 += dataSistema.ToString("yyyyMMdd") + "0";
        h1 += dataSistema.ToString("yyyyMMdd") + "0";
        h1 += contribuinte + filial + documento.NumeroDocumento;
        h1 += FicheiroEletronicoFormatHelper.Pad(documento.NumeroDocumento.ToString(), 50);
        h1 += (documento.Data ?? dataSistema).ToString("yyyyMMdd");
        h1 += FicheiroEletronicoFormatHelper.ValorCentimos(documento.TotalDocumento ?? 0);
        h1 += FicheiroEletronicoFormatHelper.ValorCentimos(documento.TotalDesconto ?? 0);
        h1 += Moeda;
        conteudo.Add(h1);

        foreach (FicheiroEletronicoAnexoDTO ficheiro in ficheiros)
        {
            int count = 0;
            foreach (string line in LerLinhas(ficheiro))
            {
                if (line.StartsWith("H2") || line.StartsWith("D2") || line.StartsWith("T2"))
                {
                    count++;
                    conteudo.Add(line);
                }
            }

            if (count == 0)
                throw new InvalidOperationException("Por favor valide se os ficheiros anexados estão no formato correto.");
        }

        string t1 = "T1";
        t1 += FicheiroEletronicoFormatHelper.PadLeft((conteudo.Count + 1).ToString(), 5);
        conteudo.Add(t1);

        return new FicheiroEletronicoGeradoDTO
        {
            Bytes = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, conteudo)),
            Nome = nome,
            Erros = [],
        };
    }

    private static FicheiroEletronicoGeradoDTO JuntarAdm(
        Documento documento,
        Clinica clinica,
        Organismo organismo,
        IReadOnlyList<FicheiroEletronicoAnexoDTO> ficheiros,
        DateTime dataSistema)
    {
        var conteudo = new List<string>();
        decimal pvp = 0, fac = 0, ben = 0;

        foreach (FicheiroEletronicoAnexoDTO ficheiro in ficheiros)
        {
            int count = 0;
            foreach (string line in LerLinhas(ficheiro))
            {
                if (!line.StartsWith("2#")) continue;

                string[] values = line.Split('#');
                if (values.Length >= 14)
                {
                    pvp += ParseDecimal(values[10]);
                    fac += ParseDecimal(values[12]);
                    ben += ParseDecimal(values[13]);
                }

                count++;
                conteudo.Add(line);
            }

            if (count == 0)
                throw new InvalidOperationException("Por favor valide se os ficheiros anexados estão no formato correto.");
        }

        decimal totalOrganismoDoc = (documento.TotalLiquido ?? 0) + (documento.RetencaoValor ?? 0);
        if (totalOrganismoDoc != fac)
            throw new InvalidOperationException($"O valor da fatura ({totalOrganismoDoc}) não corresponde ao total dos ficheiros anexados ({fac}).");
        if (pvp != ben + fac)
            throw new InvalidOperationException($"O valor PVP ({pvp}) não corresponde a beneficiário + fatura ({ben + fac}).");

        string nome = $"{clinica.NumeroContribuinte}_{documento.NumeroDocumento}_{documento.Data:yyyy-MM-dd}";
        string h1 = "1#";
        h1 += (organismo.CodigoClinica ?? string.Empty).Trim() + "#";
        h1 += (clinica.Sucursal ?? string.Empty).Trim() + "#";
        h1 += clinica.NumeroContribuinte + "#";
        h1 += documento.NumeroDocumento + "#";
        h1 += (documento.Data ?? DateTime.Today).ToString("yyyy-MM-dd") + "#";
        h1 += F(pvp) + "#" + F(fac) + "#" + F(ben) + "#";
        conteudo.Insert(0, h1);

        return new FicheiroEletronicoGeradoDTO
        {
            Bytes = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, conteudo)),
            Nome = nome,
            Erros = [],
        };
    }

    private static FicheiroEletronicoGeradoDTO JuntarSadPsp(
        Documento documento,
        Organismo organismo,
        IReadOnlyList<FicheiroEletronicoAnexoDTO> ficheiros,
        DateTime dataSistema)
    {
        string codigoEntidade = FicheiroEletronicoFormatHelper.ObterCodigoEntidadePsp(organismo.CodigoClinica);
        var conteudo = new List<string>();
        decimal fac = 0;

        foreach (FicheiroEletronicoAnexoDTO ficheiro in ficheiros)
        {
            int count = 0;
            foreach (string line in LerLinhas(ficheiro))
            {
                if (!line.StartsWith("2#")) continue;

                string[] values = line.Split('#');
                if (values.Length >= 7)
                    fac += ParseDecimal(values[6]);

                count++;
                conteudo.Add(line);
            }

            if (count == 0)
                throw new InvalidOperationException("Por favor valide se os ficheiros anexados estão no formato correto.");
        }

        decimal totalOrganismoDoc = (documento.TotalLiquido ?? 0) + (documento.RetencaoValor ?? 0);
        if (totalOrganismoDoc != fac)
            throw new InvalidOperationException($"O valor da fatura ({totalOrganismoDoc}) não corresponde ao total dos ficheiros anexados ({fac}).");

        string nome = $"PSP{codigoEntidade}_{documento.Data:yyMM}";
        string h1 = "1#";
        h1 += codigoEntidade + "#";
        h1 += documento.NumeroDocumento + "#";
        h1 += (documento.Data ?? DateTime.Today).ToString("dd/MM/yyyy") + "#";
        h1 += F(totalOrganismoDoc) + "#";
        conteudo.Insert(0, h1);

        return new FicheiroEletronicoGeradoDTO
        {
            Bytes = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, conteudo)),
            Nome = nome,
            Erros = [],
        };
    }

    private static IEnumerable<string> LerLinhas(FicheiroEletronicoAnexoDTO ficheiro)
    {
        byte[] raw = Convert.FromBase64String(ficheiro.ConteudoBase64);
        string text = Encoding.UTF8.GetString(raw);
        return text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static decimal ParseDecimal(string value) =>
        decimal.Parse(value.Replace(".", ","), CultureInfo.GetCultureInfo("pt-PT"));

    private static string F(decimal valor) =>
        decimal.Round(valor, 2).ToString(CultureInfo.InvariantCulture);
}