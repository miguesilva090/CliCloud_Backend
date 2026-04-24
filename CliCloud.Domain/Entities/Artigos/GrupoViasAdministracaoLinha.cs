#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Artigos
{
    [Table("GrupoViasAdministracaoLinha", Schema = "Artigos")]
    public class GrupoViasAdministracaoLinha : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid GrupoId { get; set; }

        public GrupoViasAdministracao? Grupo { get; set; }

        public Guid? ViaId { get; set; }
        public ViaAdministracao? Via { get; set; }

        [Required]
        public int Linha { get; set; }

        public string? Descricao { get; set; }
        public decimal? Quantidade { get; set; }
    }
}