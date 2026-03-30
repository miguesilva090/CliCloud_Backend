#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Bancos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Organismos
{
  [Table("Organismo", Schema = "Organismos")]
  public class Organismo : Entidade
  {
    
    [StringLength(100)]
    public string? NomeComercial { get; set; }
    [StringLength(40)]
    public string? Abreviatura { get; set; }
    
    // Campos financeiros
    public int? PrazoPagamento { get; set; }
    public decimal? Desconto { get; set; }
    public decimal? DescontoUtente { get; set; }
    public CondicaoPagamento? CondicaoPagamento { get; set; }
    public TipoModoPagamento? TipoModoPagamento { get; set; }
    public Guid? BancoId { get; set; }
    public Banco? Banco { get; set; }
    [StringLength(21)]
    public string? NumeroIdentificacaoBancaria { get; set; }
    
    // Campos de contrato
    [StringLength(15)]
    public string? Apolice { get; set; }
    public decimal? Avenca { get; set; }
    public DateOnly? DataInicioContrato { get; set; }
    public DateOnly? DataFimContrato { get; set; }
    public int? NumeroPagamentos { get; set; }
    
    // Campos específicos
    [StringLength(15)]
    public string? CodigoClinica { get; set; }
    public int? Faltas { get; set; }
    [StringLength(40)]
    public string? Contacto { get; set; }
    [StringLength(20)]
    public string? Categoria { get; set; }
    [StringLength(50)]
    public string? Ars { get; set; }
    [StringLength(50)]
    public string? Subregiao { get; set; }
    [StringLength(50)]
    public string? Regiao { get; set; }
    [StringLength(50)]
    public string? FraseADM { get; set; }
    public int? Bloqueio { get; set; }
    public bool LimitarConsultas { get; set; }
    public int? NumeroConsultas { get; set; }
    public bool ContabilizarFaltas { get; set; }
    
    // Campos de configuração
    public int? AssinarPagaDocumento { get; set; }
    public int? AdmissaoCC { get; set; }
    public int? FaturaCredencial { get; set; }
    public bool DiscriminaServicos { get; set; }
    [StringLength(100)]
    public string? DesignaTratamentos { get; set; }
    public bool ApresentarCredenciaisPrimeiraSessaoTratamento { get; set; }
    public bool ApresentarCredenciaisPrimeiraConsulta { get; set; }
    [StringLength(50)]
    public string? CodigoFaturacao { get; set; }
    public int? FiltroFaturacao { get; set; }
    [StringLength(50)]
    public string? CServicoFaturaResumo { get; set; }
    public int FaturarPorDatas { get; set; }
    
    // Campos booleanos específicos
    public bool TRUST { get; set; }
    public bool ADM { get; set; }
    public bool SADGNR { get; set; }
    public bool SADPSP { get; set; }
    public bool Globalbooking { get; set; }
    public bool AlterarPrecoTratamento { get; set; }
    
    // Campos contabilísticos (exportação de ficheiros de contabilidade)
    [StringLength(10)]
    public string? ContabContaFA { get; set; }
    [StringLength(10)]
    public string? ContabContaFR { get; set; }
    [StringLength(20)]
    public string? ContabTipoContaFA { get; set; }
    [StringLength(20)]
    public string? ContabTipoContaFR { get; set; }
    
    public int? CodigoULSNova { get; set; }
    public int? TratamentoCred { get; set; }
    public int Nacional { get; set; }
    public int? CodigoRegiaoAtestadoCC { get; set; }
  }
}
