#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core.Sms
{
    [Table("ConfiguracaoSmsAutomaticaMedico", Schema = "Core")]
    public class ConfiguracaoSmsAutomaticaMedico : AuditableEntityWithSoftDelete
    {
        public Guid ClinicaId { get; set; }
        public Clinica Clinica { get; set; } = null!;

        [StringLength(20)]
        public string CodigoConfiguracao { get; set; } = "1";

        [StringLength(50)]
        public string CodigoMedico { get; set; } = string.Empty;
    }
}