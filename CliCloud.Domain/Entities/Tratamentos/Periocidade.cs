#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tratamentos
{
    [Table("PeriocidadeTratamento", Schema = "Tratamentos")]
    public class PeriocidadeTratamento : AuditableEntity
    {
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}