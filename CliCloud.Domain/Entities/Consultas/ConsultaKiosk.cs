#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Domain.Entities.Consultas
{
    [Table("ConsultaKiosk", Schema = "Consultas")]
    public class ConsultaKiosk : AuditableEntity
    {
        public Guid? ConsultaId { get; set; }
        public Consulta? Consulta { get; set; }

        public Guid? UtenteId { get; set; }
        public Utente? Utente { get; set; }

        public DateTime? Data { get; set; }
        public TimeSpan? Hora { get; set; }

        public int? Ordem { get; set; }

        public string? Senha { get; set; }

        public string? IdentificadorQueue { get; set; }

    }
}