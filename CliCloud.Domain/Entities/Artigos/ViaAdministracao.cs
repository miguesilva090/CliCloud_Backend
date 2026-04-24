#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Artigos
{
    [Table("ViaAdministracao", Schema = "Artigos")]
    public class ViaAdministracao : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Descricao { get; set; } = string.Empty;

        public ICollection<GrupoViasAdministracaoLinha> Grupos { get; set; } = [];
    }
}