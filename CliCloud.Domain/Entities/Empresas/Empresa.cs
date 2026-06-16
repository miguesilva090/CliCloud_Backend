#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Bancos;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Pagamentos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Empresas
{
  [Table("Empresa", Schema = "Empresas")]
  public class Empresa : Entidade
  {
    // Campos financeiros básicos
    public int? PrazoPagamento { get; set; }
    public decimal? Desconto { get; set; }
    public decimal? DescontoUtente { get; set; }
    public Guid? CondicaoPagamentoId { get; set; }
    public CliCloud.Domain.Entities.Pagamentos.CondicaoPagamento? CondicaoPagamento { get; set; }
    public Guid? ModoPagamentoId { get; set; }
    public ModoPagamento? ModoPagamento { get; set; }

    public Guid? BancoId { get; set; }
    public Banco? Banco { get; set; }

    // Legado: CInstit (Organismo associado à Empresa)
    public Guid? OrganismoId { get; set; }
    public Organismo? Organismo { get; set; }

    [StringLength(21)]
    public string? NumeroIdentificacaoBancaria { get; set; }

    // Contrato
    [StringLength(15)]
    public string? Apolice { get; set; }
    public decimal? Avenca { get; set; }
    public DateOnly? DataInicioContrato { get; set; }
    public DateOnly? DataFimContrato { get; set; }
    public int? NumeroPagamentos { get; set; }

    // Informação adicional
    [StringLength(20)]
    public string? Categoria { get; set; }

    [StringLength(60)]
    public string? Actividade { get; set; }

    public int? Cae { get; set; }

    [StringLength(15)]
    public string? CodigoClinica { get; set; }

    public int? NumeroTrabalhadores { get; set; }
    public decimal? ValorTrabalhador { get; set; }

    public int? Rescindindo { get; set; }

    [StringLength(40)]
    public string? Contacto { get; set; }
  }
}

