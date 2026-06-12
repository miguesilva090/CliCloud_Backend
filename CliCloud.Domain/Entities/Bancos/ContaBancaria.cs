#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Bancos
{
    [Table("ContaBancaria", Schema = "Bancos")]
    public class ContaBancaria : AuditableEntityWithSoftDelete
    {
        [Required]
        [StringLength(24)]
        public string Numero { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string TipoConta { get; set; } = string.Empty;

        public Guid? BancoId { get; set; }
        public Banco? Banco { get; set; }

        public DateTime? DataAbertura { get; set; }

        [StringLength(24)]
        public string? NIB { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SaldoActual { get; set; }

        [StringLength(24)]
        public string? GestorConta { get; set; }

        public int? AlertaSaldo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorAlertaSaldo { get; set; }

        public string? OBS { get; set; }

        [StringLength(34)]
        public string? IBAN { get; set; }

        [StringLength(11)]
        public string? BIC { get; set; }

        public int? Ficheiro { get; set; }
    }
}
