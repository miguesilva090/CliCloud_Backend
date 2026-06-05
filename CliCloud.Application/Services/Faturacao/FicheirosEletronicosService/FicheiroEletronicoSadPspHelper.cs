using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Organismos;
using System.Globalization;
using System.Text;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService;

internal static class FicheiroEletronicoSadPspHelper
{
    public static FicheiroEletronicoGeradoDTO Gerar(
        Documento documento,
        Organismo organismo,
        List<FicheiroEletronicoSadPspLinhaDTO> linhas)
    {
        if (linhas.Count == 0)
            throw new InvalidOperationException("A fatura não tem admissões associadas.");

        string codigoEntidade = FicheiroEletronicoFormatHelper.ObterCodigoEntidadePsp(organismo.CodigoClinica);
        var erros = new List<string>();
        decimal totalOrganismoDoc = (documento.TotalLiquido ?? 0) + (documento.RetencaoValor ?? 0);

        if (totalOrganismoDoc != linhas.Sum(x => x.ValorAtoMedico))
            erros.Add("O valor da fatura à entidade não corresponde ao somatório dos serviços das admissões associadas.");

        string nome = $"PSP{codigoEntidade}_{documento.Data:yyMM}";
        var conteudo = new List<string>();

        string h1 = "1#";
        h1 += codigoEntidade + "#";
        h1 += documento.NumeroDocumento + "#";
        h1 += (documento.Data ?? DateTime.Today).ToString("dd/MM/yyyy") + "#";
        h1 += F(totalOrganismoDoc) + "#";
        conteudo.Add(h1);

        foreach (FicheiroEletronicoSadPspLinhaDTO linha in linhas)
        {
            if (linha.Beneficiario.Trim().Length > 12)
                throw new InvalidOperationException("O número do beneficiário tem mais do que 12 caracteres.");
            if (linha.CodigoServico.Trim().Length > 10)
                throw new InvalidOperationException("O código do serviço tem mais do que 10 caracteres.");

            string h2 = "2#";
            h2 += linha.Beneficiario + "#";
            h2 += linha.CodigoServico + "#";
            h2 += linha.Data.ToString("dd/MM/yyyy") + "#";
            h2 += linha.Data.ToString("dd/MM/yyyy") + "#";
            h2 += "1#";
            h2 += F(linha.ValorAtoMedico) + "#";
            h2 += "0#";
            conteudo.Add(h2);
        }

        return new FicheiroEletronicoGeradoDTO
        {
            Bytes = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, conteudo)),
            Nome = nome,
            Erros = erros,
        };
    }

    private static string F(decimal valor) =>
        decimal.Round(valor, 2).ToString(CultureInfo.InvariantCulture);
}