#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Artigos
{
    [Table("GrupoViasAdministracao", Schema = "Artigos")]
    public class GrupoViasAdministracao : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Descricao { get; set; } = string.Empty;

        public ICollection<GrupoViasAdministracaoLinha> Vias { get; set; } = [];
    }
}