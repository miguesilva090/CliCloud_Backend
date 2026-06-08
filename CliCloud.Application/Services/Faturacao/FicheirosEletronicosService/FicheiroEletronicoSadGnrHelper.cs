using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Documentos;
using System.Text;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService;

internal static class FicheiroEletronicoSadGnrHelper
{
    private const string Moeda = "EUR";

    public static FicheiroEletronicoGeradoDTO Gerar(
        Documento documento,
        Clinica clinica,
        List<FicheiroEletronicoSadGnrLinhaDTO> linhas,
        DateTime dataSistema,
        DateTime? ultimaGeracao)
    {
        FicheiroEletronicoDataHelper.ValidarClinicaParaGeracao(clinica);

        if (linhas.Count == 0)
            throw new InvalidOperationException("A fatura não tem admissões associadas");

        var erros = new List<string>();
        string filial = FicheiroEletronicoFormatHelper.ObterFilial(clinica.Sucursal);
        string contribuinte = clinica.NumeroContribuinte!.Trim();
        // Legado: tfatura.TotalFatura + tfatura.RetencaoFonteValor (= TotalDocumento)
        decimal totalOrganismoDoc = documento.TotalDocumento ?? 0;

        decimal somaOrganismoUtentes = linhas
            .GroupBy(x => x.DocumentoUtenteId)
            .Select(g => g.First())
            .Sum(x => x.ValorTotalReciboUtente - x.ValorBeneficiarioReciboUtente);

        if (decimal.Round(somaOrganismoUtentes, 2) != decimal.Round(totalOrganismoDoc, 2))
            erros.Add("O valor da fatura à entidade não corresponde com o somatório do valor organismo das faturas aos utentes");

        string nome = $"{documento.Data:yyyyMM}ADMG{contribuinte}{filial}{documento.NumeroDocumento}";

        var conteudo = new List<string>();

        string h1 = "H1";

        h1 += dataSistema.ToString("yyyyMMdd");
        h1 += "0";
        h1 += ultimaGeracao?.ToString("yyyyMMdd") ?? "00000000";
        h1 += "0";
        h1 += contribuinte;
        h1 += filial;
        h1 += documento.NumeroDocumento;
        h1 += FicheiroEletronicoFormatHelper.Pad(documento.NumeroDocumento.ToString(), 50);
        h1 += (documento.Data ?? dataSistema).ToString("yyyyMMdd");
        h1 += FicheiroEletronicoFormatHelper.ValorCentimos(documento.TotalDocumento ?? 0);
        h1 += FicheiroEletronicoFormatHelper.ValorCentimos(documento.TotalDesconto ?? 0);
        h1 += Moeda;
        conteudo.Add(h1);
        foreach (var grp in linhas.GroupBy(x => x.DocumentoUtenteId))
        {
            FicheiroEletronicoSadGnrLinhaDTO first = grp.First();
            decimal valorOrganismoRecibo = decimal.Round(first.ValorTotalReciboUtente - first.ValorBeneficiarioReciboUtente, 2);
            if (valorOrganismoRecibo != grp.Sum(x => x.ValorOrganismoAtoMedico))
                erros.Add($"O valor organismo do recibo utente n.º {first.NumeroDocumentoUtente} não equivale ao somatório dos detalhes.");
            if (first.Beneficiario.Length > 10)
                throw new InvalidOperationException($"O beneficiário do recibo utente n.º {first.NumeroDocumentoUtente} tem mais de 10 caracteres.");
            string h2 = "H2";
            h2 += first.NumeroDocumentoUtente;
            h2 += FicheiroEletronicoFormatHelper.Pad(first.NumeroDocumentoUtente.ToString(), 50);
            h2 += first.DataUtente.ToString("yyyyMMdd");
            h2 += first.Beneficiario;
            h2 += FicheiroEletronicoFormatHelper.Pad(first.Beneficiario, 10);
            h2 += FicheiroEletronicoFormatHelper.ValorCentimos(first.ValorTotalReciboUtente);
            h2 += FicheiroEletronicoFormatHelper.ValorCentimos(first.ValorBeneficiarioReciboUtente);
            h2 += FicheiroEletronicoFormatHelper.ValorCentimos(valorOrganismoRecibo);
            h2 += Moeda;
            conteudo.Add(h2);
            var detalhes = grp.ToArray();
            foreach (FicheiroEletronicoSadGnrLinhaDTO det in detalhes)
            {
                if (det.CodigoServico.Length > 10)
                    throw new InvalidOperationException($"O código de serviço do recibo utente n.º {det.NumeroDocumentoUtente} tem mais de 10 caracteres.");
                string d2 = "D2ADS";
                d2 += det.CodigoServico;
                d2 += FicheiroEletronicoFormatHelper.Pad(det.CodigoServico, 10);
                d2 += det.DataAtoMedico.ToString("yyyyMMdd");
                d2 += FicheiroEletronicoFormatHelper.PadLeft(det.Quantidade.ToString(), 3);
                d2 += FicheiroEletronicoFormatHelper.ValorCentimos(det.ValorAtoMedico);
                d2 += FicheiroEletronicoFormatHelper.ValorCentimos(det.ValorBeneficiarioAtoMedico);
                d2 += FicheiroEletronicoFormatHelper.ValorCentimos(det.ValorOrganismoAtoMedico);
                d2 += Moeda;
                d2 += (det.Dente ?? string.Empty).PadLeft(2, '0');
                d2 += new string('0', 60);
                conteudo.Add(d2);
            }
            string t2 = "T2";
            t2 += FicheiroEletronicoFormatHelper.PadLeft((detalhes.Length + 2).ToString(), 5);
            conteudo.Add(t2);
        }
        string t1 = "T1";
        t1 += FicheiroEletronicoFormatHelper.PadLeft((conteudo.Count + 1).ToString(), 5);
        conteudo.Add(t1);
        return new FicheiroEletronicoGeradoDTO
        {
            Bytes = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, conteudo)),
            Nome = nome,
            Erros = erros,
        };
    }
}