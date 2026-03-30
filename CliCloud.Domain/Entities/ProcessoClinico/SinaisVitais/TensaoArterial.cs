#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.SinaisVitais 
{
    [Table("TensaoArterial", Schema = "SinaisVitais")]
    public class TensaoArterial : AuditableEntity
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public DateTime Data { get; set; }

        public TimeSpan Hora { get; set; }

        public int TensaoSistolica { get; set; }

        public int TensaoDiastolica { get; set; }

        public string? Observacoes { get; set; }
    }
}