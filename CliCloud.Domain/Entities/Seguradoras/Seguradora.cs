#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Bancos;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Seguradoras
{
  /// <summary>
  /// Seguradora. Equivalente a SEGURADORA no legado.
  /// Responsabilidade: representar seguradoras (seguros), distinta de Organismo (pagador).
  /// </summary>
  [Table("Seguradora", Schema = "Seguradoras")]
  public class Seguradora : Entidade
  {
    [StringLength(15)]
    public string? Apolice { get; set; }

    public decimal? Avenca { get; set; }

    public DateOnly? DataInicioContrato { get; set; }

    public DateOnly? DataFimContrato { get; set; }

    public int? NumeroPagamentos { get; set; }

    public int? PrazoPagamento { get; set; }

    public decimal? Desconto { get; set; }

    public decimal? DescontoUtente { get; set; }

    public int? Faltas { get; set; }

    [StringLength(40)]
    public string? Contacto { get; set; }

    [StringLength(20)]
    public string? Categoria { get; set; }

    [StringLength(21)]
    public string? NumeroIdentificacaoBancaria { get; set; }

    public Guid? BancoId { get; set; }

    public Banco? Banco { get; set; }

    [StringLength(15)]
    public string? CodigoClinica { get; set; }

    [StringLength(50)]
    public string? Ars { get; set; }

    [StringLength(50)]
    public string? Subregiao { get; set; }

    [StringLength(50)]
    public string? Regiao { get; set; }

    [StringLength(40)]
    public string? Abreviatura { get; set; }
  }
}
