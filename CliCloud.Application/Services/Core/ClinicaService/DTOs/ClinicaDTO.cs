using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.ClinicaService.DTOs
{
  public class ClinicaDTO : IDto
  {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? NomeComercial { get; set; }
    public string? Abreviatura { get; set; }

    public bool PorDefeito { get; set; }

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
    public int? Recibo { get; set; }
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

    // ---- Configuração de Tratamentos (unificada na carga da Clínica) ----
    public ConfiguracaoTratamentosDTO? ConfiguracaoTratamentos { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}
