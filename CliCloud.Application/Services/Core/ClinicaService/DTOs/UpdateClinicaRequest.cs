using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.ClinicaService.DTOs
{
  public class UpdateClinicaRequest : IDto
  {
    public required string Nome { get; set; }
    public string? NomeComercial { get; set; }
    public string? Abreviatura { get; set; }

    // ---- Identificação (tab_1_1) ----
    public string? Morada { get; set; }
    public string? CCPostal { get; set; }
    public string? Localidade { get; set; }
    public string? IndicativoTelefone { get; set; }
    public string? Telefone { get; set; }
    public string? Telemovel { get; set; }
    public string? Fax { get; set; }

    public string? Email { get; set; }
    public string? Web { get; set; }
    public string? Sucursal { get; set; }
    public string? NumeroContribuinte { get; set; }
    public string? NIB { get; set; }

    public string? Observacoes { get; set; }
    public string? UrlFoto { get; set; }

    // ---- Dados Fiscais (tab_1_2) ----
    public string? Atividade { get; set; }
    public string? Regcom { get; set; }
    public decimal? Capsocial { get; set; }
    public string? Cae { get; set; }
    public string? ZonFisc { get; set; }
    public string? Tipo { get; set; }
    public string? Portaria { get; set; }
    public string? DespachoUcc { get; set; }
    public string? ObsNotaCredito { get; set; }
    public string? CMoeda { get; set; }

    // ---- Faturação (tab_1_3 de ClinicasEdt.aspx) ----
    public int? FaturaRecibo { get; set; }
    public bool? ImprimeTicket { get; set; }
    public bool? TemSaft { get; set; }
    public bool? Cab { get; set; }
    public string? FaturacaoDocumentosImpressao { get; set; }
    public bool? EmailLink { get; set; }

    public string? Linha1 { get; set; }
    public string? Linha2 { get; set; }
    public string? Linha3 { get; set; }
    public string? Linha4 { get; set; }
    public string? Linha5 { get; set; }
    public string? Linha6 { get; set; }

    public string? Regrafaturacao { get; set; }
    public string? MotivoIsencaoDefeito { get; set; }
    public int? ValorMaxFaturaSimpli { get; set; }
    public string? ArmazemHabitual { get; set; }
    public string? AtUser { get; set; }
    public string? AtPass { get; set; }

    // ---- Horário de Funcionamento (legado: tab_1_5 de ClinicasEdt.aspx) ----
    public bool? Interrupcao { get; set; }
    public string? HoraInicManha { get; set; }
    public string? HoraFimManha { get; set; }
    public string? HoraInicTarde { get; set; }
    public string? HoraFimTarde { get; set; }
    public bool? FolgaSeg { get; set; }
    public bool? FolgaTer { get; set; }
    public bool? FolgaQua { get; set; }
    public bool? FolgaQui { get; set; }
    public bool? FolgaSex { get; set; }
    public bool? FolgaSab { get; set; }
    public bool? FolgaDom { get; set; }

    // ---- Outros Parâmetros (legado: tab_1_4 de ClinicasEdt.aspx) ----
    public int? Descarga { get; set; }
    public int? Ruptura { get; set; }
    public int? Valorart { get; set; }

    public int? Tiporecal { get; set; }
    public decimal? Valorecal { get; set; }

    public bool? ControlarPlafond { get; set; }
    public int? CtrlPlafond { get; set; }

    public bool? Validade { get; set; }
    public int? Diasvalid { get; set; }
    public bool? Ligacb { get; set; }

    public string? EntidadeUtilizadora { get; set; }
    public string? LocalPrescricao { get; set; }
    public string? NomeEtiqueta { get; set; }
    public string? CodSb { get; set; }
    public int? Netiquetas { get; set; }

    public int? CccCodLocalEmissao { get; set; }
    public string? CccDescLocalEmissao { get; set; }
    public string? Regiao { get; set; }
    public int? Cid { get; set; }

    public int? PortaLeitorCartoes { get; set; }
    public bool? StocksColunaStockReal { get; set; }

    public int? CalendarioMarcacoesRadio { get; set; }
    public bool? NovoEstadoPaginaAtendimento { get; set; }
    public bool? NovaPrescricao { get; set; }
    public bool? GestaoSalas { get; set; }

    public int? TipoAdmissPorDefeito { get; set; }
    public string? DiretoriaDocumentos { get; set; }
    public string? CaminhoSaft { get; set; }

    public string? ExportContabilidadeFa { get; set; }
    public string? ExportTipoContaFa { get; set; }
    public int? ExportPredUtenteFa { get; set; }
    public string? ExportContabilidadeFr { get; set; }
    public string? ExportTipoContaFr { get; set; }
    public int? ExportPredUtenteFr { get; set; }

    public string? LabelAuxiliares { get; set; }
    public int? EnvioEmail { get; set; }
    public bool? MovimentosInternos { get; set; }

    public string? MsgFaltaPagamento { get; set; }
    public string? MsgCredenciais { get; set; }
    public bool? KqueueAvisoAtraso { get; set; }
    public int? KqueueTempoAvisoAtraso { get; set; }
    public string? KqueueMensagemAvisoAtraso { get; set; }

    // ---- Consentimento RGPD (legado: separador RGPD) ----
    public string? RgpdDescritivo { get; set; }
    public string? RgpdConsentimento { get; set; }
    public string? RgpdMarketing { get; set; }

    public string? EmailAssuntoConsultas { get; set; }
    public string? EmailConteudoConsultas { get; set; }
    public string? EmailAssuntoTratamentos { get; set; }
    public string? EmailConteudoTratamentos { get; set; }
    public string? EmailAssuntoExames { get; set; }
    public string? EmailConteudoExames { get; set; }
    public string? EmailAssuntoRelatorios { get; set; }
    public string? EmailConteudoRelatorios { get; set; }

    // ---- Tratamentos (unificado no update da Clínica) ----
    // Flag para o backend saber se deve gravar a Configuração de Tratamentos
    // (mesmo que os valores venham como null para limpar campos).
    public bool? GravarConfiguracaoTratamentos { get; set; }

    public string? TipoSrvTratamentos { get; set; }
    public string? AreaPrestacaoDefeitoAreaZ { get; set; }
    public bool? ControlarAparelhos { get; set; }

    public int? Segundos { get; set; }
    public int? FaltasMax { get; set; }
    public int? FaltasConsecutivasMax { get; set; }
    public decimal? Taxamoderadora { get; set; }
    public bool? CredencialExternaAdse { get; set; }

    public int? TipoPagamento { get; set; }
    public bool? AvisoInqueritoSessoesDiarias { get; set; }
  }

  public class UpdateClinicaValidator : AbstractValidator<UpdateClinicaRequest>
  {
    public UpdateClinicaValidator()
    {
      _ = RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);

      // Em updates parciais (guardar por aba), não exigimos campos fora da aba ativa.
      _ = RuleFor(x => x.NomeComercial).MaximumLength(60);
      _ = RuleFor(x => x.NumeroContribuinte).MaximumLength(20);
      // (// retirar) campos antigos removidos: Idnum/Ano

      _ = RuleFor(x => x.CMoeda).MaximumLength(50);

      // Campos opcionais do legado (tab_1_1)
      _ = RuleFor(x => x.Abreviatura).MaximumLength(30);
      _ = RuleFor(x => x.Morada).MaximumLength(50);
      _ = RuleFor(x => x.CCPostal).MaximumLength(20);
      _ = RuleFor(x => x.Localidade).MaximumLength(50);
      _ = RuleFor(x => x.IndicativoTelefone).MaximumLength(10);
      _ = RuleFor(x => x.Telefone).MaximumLength(20);
      _ = RuleFor(x => x.Telemovel).MaximumLength(15);
      _ = RuleFor(x => x.Fax).MaximumLength(20);
      _ = RuleFor(x => x.Email).MaximumLength(255);
      _ = RuleFor(x => x.Web).MaximumLength(255);
      _ = RuleFor(x => x.Sucursal).MaximumLength(10);
      _ = RuleFor(x => x.NIB).MaximumLength(21);
      _ = RuleFor(x => x.Observacoes).MaximumLength(2000);
      _ = RuleFor(x => x.UrlFoto).MaximumLength(500);

      _ = RuleFor(x => x.Atividade).MaximumLength(50);
      _ = RuleFor(x => x.Regcom).MaximumLength(50);
      _ = RuleFor(x => x.Cae).MaximumLength(5);
      _ = RuleFor(x => x.ZonFisc).MaximumLength(50);
      _ = RuleFor(x => x.Tipo).MaximumLength(200);
      _ = RuleFor(x => x.Portaria).MaximumLength(254);
      _ = RuleFor(x => x.DespachoUcc).MaximumLength(250);
      _ = RuleFor(x => x.ObsNotaCredito).MaximumLength(250);

      // ---- Faturação (tab_1_3) - todos opcionais (sem NotEmpty), só limites ----
      _ = RuleFor(x => x.FaturaRecibo).GreaterThanOrEqualTo(0).When(x => x.FaturaRecibo.HasValue);
      // (// retirar) Recibo/NVias/Numdiasquota/Tiponotificacao removidos
      _ = RuleFor(x => x.ValorMaxFaturaSimpli).GreaterThanOrEqualTo(0).When(x => x.ValorMaxFaturaSimpli.HasValue);

      _ = RuleFor(x => x.FaturacaoDocumentosImpressao).MaximumLength(30);

      _ = RuleFor(x => x.Linha1).MaximumLength(120);
      _ = RuleFor(x => x.Linha2).MaximumLength(120);
      _ = RuleFor(x => x.Linha3).MaximumLength(120);
      _ = RuleFor(x => x.Linha4).MaximumLength(120);
      _ = RuleFor(x => x.Linha5).MaximumLength(120);
      _ = RuleFor(x => x.Linha6).MaximumLength(120);

      _ = RuleFor(x => x.Regrafaturacao).MaximumLength(10);
      _ = RuleFor(x => x.MotivoIsencaoDefeito).MaximumLength(50);

      // (// retirar) DiretoriaQRC removido
      _ = RuleFor(x => x.ArmazemHabitual).MaximumLength(50);
      _ = RuleFor(x => x.AtUser).MaximumLength(200);
      _ = RuleFor(x => x.AtPass).MaximumLength(200);

      // ---- Outros Parâmetros (tab_1_4) - todos opcionais (limites de texto) ----
      _ = RuleFor(x => x.DiretoriaDocumentos).MaximumLength(250);
      _ = RuleFor(x => x.CaminhoSaft).MaximumLength(250);

      _ = RuleFor(x => x.EntidadeUtilizadora).MaximumLength(30);
      _ = RuleFor(x => x.LocalPrescricao).MaximumLength(7);
      _ = RuleFor(x => x.NomeEtiqueta).MaximumLength(25);
      _ = RuleFor(x => x.CodSb).MaximumLength(9);
      _ = RuleFor(x => x.CccDescLocalEmissao).MaximumLength(50);
      _ = RuleFor(x => x.Regiao).MaximumLength(10);

      // ---- Tratamentos (unificado no update da Clínica) ----
      _ = RuleFor(x => x.TipoSrvTratamentos).MaximumLength(50);
      _ = RuleFor(x => x.AreaPrestacaoDefeitoAreaZ).MaximumLength(50);

      _ = RuleFor(x => x.Segundos)
        .GreaterThanOrEqualTo(0)
        .When(x => x.Segundos.HasValue);
      _ = RuleFor(x => x.FaltasMax)
        .GreaterThanOrEqualTo(0)
        .When(x => x.FaltasMax.HasValue);
      _ = RuleFor(x => x.FaltasConsecutivasMax)
        .GreaterThanOrEqualTo(0)
        .When(x => x.FaltasConsecutivasMax.HasValue);
      _ = RuleFor(x => x.Taxamoderadora)
        .GreaterThanOrEqualTo(0)
        .When(x => x.Taxamoderadora.HasValue);

      _ = RuleFor(x => x.TipoPagamento)
        .InclusiveBetween(0, 1)
        .When(x => x.TipoPagamento.HasValue);

      _ = RuleFor(x => x.ExportContabilidadeFa).MaximumLength(50);
      _ = RuleFor(x => x.ExportTipoContaFa).MaximumLength(30);
      _ = RuleFor(x => x.ExportContabilidadeFr).MaximumLength(50);
      _ = RuleFor(x => x.ExportTipoContaFr).MaximumLength(30);

      _ = RuleFor(x => x.LabelAuxiliares).MaximumLength(500);

      // ---- Horário de Funcionamento (tab_1_5) ----
      _ = RuleFor(x => x.HoraInicManha).MaximumLength(5);
      _ = RuleFor(x => x.HoraFimManha).MaximumLength(5);
      _ = RuleFor(x => x.HoraInicTarde).MaximumLength(5);
      _ = RuleFor(x => x.HoraFimTarde).MaximumLength(5);

      _ = RuleFor(x => x.MsgFaltaPagamento).MaximumLength(2000);
      _ = RuleFor(x => x.MsgCredenciais).MaximumLength(2000);
      _ = RuleFor(x => x.KqueueMensagemAvisoAtraso).MaximumLength(2000);

      _ = RuleFor(x => x.EmailAssuntoConsultas).MaximumLength(500);
      _ = RuleFor(x => x.EmailAssuntoTratamentos).MaximumLength(500);
      _ = RuleFor(x => x.EmailAssuntoExames).MaximumLength(500);
      _ = RuleFor(x => x.EmailAssuntoRelatorios).MaximumLength(500);
    }
  }
}
