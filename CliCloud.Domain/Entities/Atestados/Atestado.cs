#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Atestados
{
    [Table("Atestado", Schema = "Atestados")]
    public class Atestado : AuditableEntityWithSoftDelete
    {
        public Guid UtenteId { get; set; }
        public Utente? Utente { get; set; }

        public Guid MedicoId { get; set; }
        public Medico? Medico { get; set; }

        public Guid ClinicaId { get; set; }
        public Clinica? Clinica { get; set; }

        public Guid? CodigoPostalId { get; set; }
        public CodigoPostal? CodigoPostal { get; set; }

        // Dados Atestado 

        public DateTime DataAtestado { get; set; }
        public string? NumeroSPMS { get; set; }
        public int EstadoEnvio { get; set; }
        public DateTime? DataEnvio { get; set; }
        public string? MensagemErro { get; set; }
        public string? Observacoes { get; set; }
        public string? NumeroSNS { get; set; }

        public ICollection<AtestadoCategoria> Categorias { get; set; } = [];
        public ICollection<AtestadoRestricao> Restricoes { get; set; } = [];
        public ICollection<AtestadoRestricaoAnterior> RestricoesAnteriores { get; set; } = [];

    }
}
