#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Core
{
  [Table("Clinica", Schema = "Core")]
  public class Clinica : Entidade
  {
    [StringLength(100)]
    public string? NomeComercial { get; set; }

    [StringLength(40)]
    public string? Abreviatura { get; set; }
    public bool PorDefeito { get; set; }
    public string? Morada { get; set; }
    public string? CCPostal { get; set; }
    public string? Localidade { get; set; }
    public string? IndicativoTelefone { get; set; }
    public string? Telefone { get; set; }
    public string? Telemovel { get; set; }
    public string? Fax { get; set; }
    public string? Web { get; set; }
    public string? Sucursal { get; set; }
    public string? NIB { get; set; }
    public string? Atividade { get; set; }
    public string? Regcom { get; set; }
    public decimal? Capsocial { get; set; }
    public string? Cae { get; set; }
    public ZonaFiscal? ZonFisc { get; set; }
    public string? Tipo { get; set; }
    public string? Portaria { get; set; }
    public string? DespachoUcc { get; set; }
    public string? ObsNotaCredito { get; set; }
    public string? CMoeda { get; set; }

    public int? FaturaRecibo { get; set; }

    public bool? ImprimeTicket { get; set; }

    public bool? TemSaft { get; set; }

    public bool? Cab { get; set; }
    [StringLength(30)]
    public string? FaturacaoDocumentosImpressao { get; set; }

    public bool? EmailLink { get; set; }

    [StringLength(120)]
    public string? Linha1 { get; set; }
    [StringLength(120)]
    public string? Linha2 { get; set; }
    [StringLength(120)]
    public string? Linha3 { get; set; }
    [StringLength(120)]
    public string? Linha4 { get; set; }
    [StringLength(120)]
    public string? Linha5 { get; set; }
    [StringLength(120)]
    public string? Linha6 { get; set; }

    [StringLength(10)]
    public string? Regrafaturacao { get; set; }

    public string? MotivoIsencaoDefeito { get; set; }

    public int? ValorMaxFaturaSimpli { get; set; }

    public string? ArmazemHabitual { get; set; }

    [StringLength(200)]
    public string? AtUser { get; set; }

    [StringLength(200)]
    public string? AtPass { get; set; }

    public bool? Interrupcao { get; set; }

    [StringLength(5)]
    public string? HoraInicManha { get; set; }

    [StringLength(5)]
    public string? HoraFimManha { get; set; }

    [StringLength(5)]
    public string? HoraInicTarde { get; set; }

    [StringLength(5)]
    public string? HoraFimTarde { get; set; }

    public bool? FolgaSeg { get; set; }
    public bool? FolgaTer { get; set; }
    public bool? FolgaQua { get; set; }
    public bool? FolgaQui { get; set; }
    public bool? FolgaSex { get; set; }
    public bool? FolgaSab { get; set; }
    public bool? FolgaDom { get; set; }

    public int? Descarga { get; set; }
    public int? Ruptura { get; set; }
    public int? Valorart { get; set; }

    public int? Tiporecal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Valorecal { get; set; }

    public bool? ControlarPlafond { get; set; }
    public int? CtrlPlafond { get; set; }

    public bool? Validade { get; set; }
    public int? Diasvalid { get; set; }
    public bool? Ligacb { get; set; }

    [StringLength(30)]
    public string? EntidadeUtilizadora { get; set; }

    [StringLength(7)]
    public string? LocalPrescricao { get; set; }

    [StringLength(25)]
    public string? NomeEtiqueta { get; set; }

    [StringLength(9)]
    public string? CodSb { get; set; }

    public int? Netiquetas { get; set; }

    public int? CccCodLocalEmissao { get; set; }

    [StringLength(50)]
    public string? CccDescLocalEmissao { get; set; }

    [StringLength(10)]
    public string? Regiao { get; set; }

    public int? Cid { get; set; }

    public int? PortaLeitorCartoes { get; set; }
    public bool? StocksColunaStockReal { get; set; }

    public int? CalendarioMarcacoesRadio { get; set; }
    public bool? NovoEstadoPaginaAtendimento { get; set; }
    public bool? NovaPrescricao { get; set; }
    public bool? GestaoSalas { get; set; }

    public int? TipoAdmissPorDefeito { get; set; }

    [StringLength(250)]
    public string? DiretoriaDocumentos { get; set; }

    [StringLength(250)]
    public string? CaminhoSaft { get; set; }

    [StringLength(50)]
    public string? ExportContabilidadeFa { get; set; }

    [StringLength(30)]
    public string? ExportTipoContaFa { get; set; }

    public int? ExportPredUtenteFa { get; set; }

    [StringLength(50)]
    public string? ExportContabilidadeFr { get; set; }

    [StringLength(30)]
    public string? ExportTipoContaFr { get; set; }

    public int? ExportPredUtenteFr { get; set; }

    public string? LabelAuxiliares { get; set; }

    public int? EnvioEmail { get; set; }
    public bool? MovimentosInternos { get; set; }

    [StringLength(2000)]
    public string? MsgFaltaPagamento { get; set; }

    [StringLength(2000)]
    public string? MsgCredenciais { get; set; }

    public bool? KqueueAvisoAtraso { get; set; }

    public int? KqueueTempoAvisoAtraso { get; set; }

    [StringLength(2000)]
    public string? KqueueMensagemAvisoAtraso { get; set; }

    public string? RgpdDescritivo { get; set; }

    public string? RgpdConsentimento { get; set; }

    public string? RgpdMarketing { get; set; }

    [StringLength(500)]
    public string? EmailAssuntoConsultas { get; set; }

    public string? EmailConteudoConsultas { get; set; }

    [StringLength(500)]
    public string? EmailAssuntoTratamentos { get; set; }

    public string? EmailConteudoTratamentos { get; set; }

    [StringLength(500)]
    public string? EmailAssuntoExames { get; set; }

    public string? EmailConteudoExames { get; set; }

    [StringLength(500)]
    public string? EmailAssuntoRelatorios { get; set; }

    public string? EmailConteudoRelatorios { get; set; }
  }
}
