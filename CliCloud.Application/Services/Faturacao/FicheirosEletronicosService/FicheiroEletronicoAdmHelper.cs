using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Organismos;
using System.Globalization;
using System.Text;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService;

internal static class FicheiroEletronicoAdmHelper
{
    public static FicheiroEletronicoGeradoDTO Gerar(
        Documento documento,
        Clinica clinica,
        Organismo organismo,
        List<FicheiroEletronicoAdmLinhaDTO> linhas)
    {
        FicheiroEletronicoDataHelper.ValidarClinicaParaGeracao(clinica);

        if ((organismo.CodigoClinica ?? string.Empty).Trim().Length > 10)
            throw new InvalidOperationException("O organismo tem mais de 10 caracteres no código clínica.");

        if ((clinica.Sucursal ?? string.Empty).Trim().Length > 5)
            throw new InvalidOperationException("O código sucursal da clínica tem mais de 5 caracteres.");

        if (linhas.Count == 0)
            throw new InvalidOperationException("A fatura não tem admissões associadas.");

        var erros = new List<string>();
        decimal totalOrganismoDoc = (documento.TotalLiquido ?? 0) + (documento.RetencaoValor ?? 0);
        decimal valorTotalPvp = linhas.First().TotalFatura;
        decimal valorTotalBeneficiario = valorTotalPvp - (documento.TotalLiquido ?? 0) + (documento.RetencaoValor ?? 0);

        if (totalOrganismoDoc != linhas.Sum(x => x.ValorADM))
            erros.Add("O valor da fatura não corresponde ao somatório dos serviços das admissões associadas.");
        if (valorTotalPvp != linhas.Sum(x => x.ValorPVP))
            erros.Add("O valor total da fatura não corresponde ao somatório dos serviços das admissões associadas.");
        if (valorTotalBeneficiario != linhas.Sum(x => x.ValorBeneficiario))
            erros.Add("O valor que o beneficiário pagou não corresponde ao somatório dos serviços das admissões.");

        string nome = $"{clinica.NumeroContribuinte}_{documento.NumeroDocumento}_{documento.Data:yyyy-MM-dd}";
        var conteudo = new List<string>();

        string h1 = "1#";
        h1 += (organismo.CodigoClinica ?? string.Empty).Trim() + "#";
        h1 += (clinica.Sucursal ?? string.Empty).Trim() + "#";
        h1 += clinica.NumeroContribuinte + "#";
        h1 += documento.NumeroDocumento + "#";
        h1 += (documento.Data ?? DateTime.Today).ToString("yyyy-MM-dd") + "#";
        h1 += F(valorTotalPvp) + "#";
        h1 += F(totalOrganismoDoc) + "#";
        h1 += F(valorTotalBeneficiario) + "#";
        conteudo.Add(h1);

        foreach (FicheiroEletronicoAdmLinhaDTO linha in linhas)
        {
            string tratamento = linha.CodigoTratAdmiss?.ToString() ?? "0";

            if (linha.Beneficiario.Trim().Length > 15)
                throw new InvalidOperationException($"Beneficiário com mais de 15 caracteres. Tratamento/Admissão: {tratamento}");
            if (linha.CodigoServico.Trim().Length > 20)
                throw new InvalidOperationException($"Código serviço com mais de 20 caracteres. Tratamento/Admissão: {tratamento}");

            if (decimal.Round(linha.ValorADM, 2) != decimal.Round(linha.ValorPVP * linha.Comparticipacao / 100m, 2))
                erros.Add($"A comparticipação do {linha.CodigoServico} não corresponde ao cálculo dos valores. Tratamento: {tratamento}");

            string h2 = "2####";
            h2 += linha.Beneficiario + "#";
            h2 += linha.Data.ToString("yyyy-MM-dd") + "##";
            h2 += linha.CodigoServico.Trim() + "#";
            h2 += F(linha.Comparticipacao) + "#";
            h2 += linha.Quantidade + "#";
            h2 += F(linha.ValorPVP) + "#";
            h2 += "#";
            h2 += F(linha.ValorADM) + "#";
            h2 += F(linha.ValorBeneficiario) + "#";
            h2 += "#";
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