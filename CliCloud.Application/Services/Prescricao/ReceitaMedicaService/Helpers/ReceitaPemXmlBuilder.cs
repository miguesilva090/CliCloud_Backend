using System.Xml.Linq;
using CliCloud.Application.Services.Prescricao.ReceitaMedicaService.DTOs;
using CliCloud.Domain.Entities.Prescricao;

namespace CliCloud.Application.Services.Prescricao.ReceitaMedicaService.Helpers;

/// <summary>
/// Gera o corpo PEM/RSP alinhado ao legado
/// <c>RnuServiceRSP.GetXMLPrescricao</c> (CabecalhoReceita + Linhas).
/// O envelope SOAP/cabeçalho de operação é montado por <c>SpmsPrescricaoSoapService</c>.
/// </summary>
public static class ReceitaPemXmlBuilder
{
  private const string NsPem = "http://xmlns.dmm.spms.pt/201207/Receita";

  public static string Build(ReceitaMedica r) => BuildInternal(r, anular: false, motivo: null);

  public static string BuildAnulacao(ReceitaMedica r, AnularReceitaMedicaRequest motivo) =>
    BuildInternal(r, anular: true, motivo);

  private static string BuildInternal(
    ReceitaMedica r,
    bool anular,
    AnularReceitaMedicaRequest? motivo)
  {
    var utente = r.Utente;
    var medico = r.Medico;
    var clinica = r.Clinica;

    var sexo = "M";
    var sexoDesc = utente?.Sexo?.Descricao?.Trim().ToUpperInvariant() ?? "";
    if (sexoDesc.Contains("FEMIN"))
      sexo = "F";

    var numCedula = (medico?.Carteira ?? "0").Replace("M", "", StringComparison.OrdinalIgnoreCase).Trim();
    if (string.IsNullOrWhiteSpace(numCedula)) numCedula = "0";

    var localCodigo = !string.IsNullOrWhiteSpace(r.LocalPrescricao)
      ? r.LocalPrescricao!
      : (clinica?.LocalPrescricao ?? "");
    var localDesignacao = clinica?.NomeComercial ?? clinica?.Nome ?? "";

    var numeroBeneficiario = !string.IsNullOrWhiteSpace(r.NumeroBeneficiarioEfr)
      ? r.NumeroBeneficiarioEfr!
      : (utente?.NumeroBeneficiarioEfr ?? "");

    XNamespace ns = NsPem;

    var cabecalhoReceita = new XElement(ns + "CabecalhoReceita",
      new XElement(ns + "NumeroReceitaLocal", (r.NumeroReceitaLocal ?? "").Trim()),
      new XElement(ns + "TipoReceita", r.TipoReceita),
      new XElement(ns + "Renovavel", r.ReceitaRenovavel == 1 ? "S" : "N"),
      new XElement(ns + "NumeroVias", r.NumeroVias ?? 1),
      new XElement(ns + "Utente",
        new XElement(ns + "NomeCompleto", (utente?.Nome ?? "").Trim()),
        new XElement(ns + "Sexo", sexo),
        new XElement(ns + "NumeroSNS", utente?.NumeroUtente ?? ""),
        utente?.DataNascimento is { } dn
          ? new XElement(ns + "DataNascimento", dn.ToString("yyyy-MM-dd"))
          : null,
        new XElement(ns + "PaisNacionalidade", (utente?.Nacionalidade ?? "PT").Trim()),
        new XElement(ns + "Contacto", "")
      ),
      new XElement(ns + "SubsistemaResponsavel",
        new XElement(ns + "NumeroBeneficiario", numeroBeneficiario.Trim()),
        new XElement(ns + "Entidade",
          new XElement(ns + "Codigo", ""),
          new XElement(ns + "DominioEntidade", ""),
          new XElement(ns + "PaisEntidade", "PT")
        )
      ),
      new XElement(ns + "Profissional",
        new XElement(ns + "NumCedula", numCedula),
        new XElement(ns + "Ordem", (medico?.GrupoFuncional ?? "").Trim()),
        new XElement(ns + "Especialidade",
          new XElement(ns + "Codigo", ""),
          new XElement(ns + "Descricao", (medico?.Especialidade?.Nome ?? "").Trim())
        ),
        new XElement(ns + "Contacto", "")
      ),
      new XElement(ns + "LocalPrescricao",
        new XElement(ns + "Codigo", localCodigo.Trim()),
        new XElement(ns + "Designacao", localDesignacao.Trim())
      )
    );

    if (!string.IsNullOrWhiteSpace(r.NumeroReceita))
      cabecalhoReceita.Add(new XElement(ns + "NumeroReceita", r.NumeroReceita.Trim()));

    if (anular)
    {
      cabecalhoReceita.Add(new XElement(ns + "DataAnulacao",
        (r.DataAnulacao ?? DateTime.UtcNow).ToString("yyyy-MM-dd")));
      cabecalhoReceita.Add(new XElement(ns + "CodigoAnulacao", motivo?.MotivoCodigo ?? ""));
      cabecalhoReceita.Add(new XElement(ns + "DescricaoAnulacao", motivo?.MotivoDescricao ?? ""));
    }

    var linhas = new XElement(ns + "LinhasReceita",
      (r.Linhas ?? []).OrderBy(l => l.Ordem).Select((l, i) =>
      {
        var linhaEl = new XElement(ns + "LinhaReceita",
          new XElement(ns + "NumeroLinha", l.Ordem > 0 ? l.Ordem : i + 1),
          new XElement(ns + "TipoLinhaMedicamento", l.TipoLinha),
          new XElement(ns + "Quantidade", l.Quantidade),
          new XElement(ns + "LinhaRenovavel", "N"),
          new XElement(ns + "Medicamento",
            new XElement(ns + "CN_PEM", l.Cnpem ?? ""),
            new XElement(ns + "EmbId", l.EmbId ?? ""),
            new XElement(ns + "Descricao", l.Designacao),
            new XElement(ns + "DescricaoEmbalagem", l.DescricaoEmbalagem ?? "")
          ));

        // Paridade RnuServiceRSP: ListaPosologias estruturada + Instruções
        if (l.TipoLinha != 8
            && !string.IsNullOrWhiteSpace(l.PosologiaQuantidadeUnidade)
            && !string.IsNullOrWhiteSpace(l.PosologiaFrequenciaValor))
        {
          var listaPosologias = new XElement(ns + "ListaPosologias",
            new XElement(ns + "Instrucoes", l.PosologiaInstrucoes ?? l.Posologia ?? ""),
            new XElement(ns + "Posologia",
              new XElement(ns + "Intervalo", "1"),
              new XElement(ns + "Descanso", "N"),
              new XElement(ns + "Duracao",
                new XElement(ns + "Unidade", l.PosologiaDuracaoUnidade ?? ""),
                new XElement(ns + "Valor", l.PosologiaDuracaoValor ?? "")),
              new XElement(ns + "Tomas",
                new XElement(ns + "Toma",
                  new XElement(ns + "Toma", "1"),
                  new XElement(ns + "Frequencia",
                    new XElement(ns + "Unidade", l.PosologiaFrequenciaUnidade ?? ""),
                    new XElement(ns + "Valor", l.PosologiaFrequenciaValor ?? "")),
                  new XElement(ns + "Quantidade",
                    new XElement(ns + "Unidade", l.PosologiaQuantidadeUnidade ?? ""),
                    new XElement(ns + "Valor", l.PosologiaQuantidadeValor ?? ""))))));
          linhaEl.Add(listaPosologias);
        }
        else
        {
          linhaEl.Add(new XElement(ns + "Posologia", l.Posologia ?? ""));
        }

        if (!string.IsNullOrWhiteSpace(l.CodJustificacaoQuantidade))
        {
          linhaEl.Add(new XElement(ns + "JustificacaoQuantidade",
            new XElement(ns + "Codigo", l.CodJustificacaoQuantidade),
            new XElement(ns + "Descricao", l.JustificacaoQuantidade ?? "")));
        }

        return linhaEl;
      }));

    var doc = new XElement(ns + "Receita",
      cabecalhoReceita,
      linhas);

    return doc.ToString(SaveOptions.DisableFormatting);
  }
}
