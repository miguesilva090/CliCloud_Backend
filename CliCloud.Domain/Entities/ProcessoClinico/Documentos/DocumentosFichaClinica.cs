using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico.Documentos 
{
    [Table("DocumentosFichaClinica", Schema = "ProcessoClinico")]
    public class DocumentosFichaClinica : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public DocumentoFichaClinicaCategoria Categoria { get; set; }

        public DocumentoFichaClinicaTipo Tipo { get; set; }

        [Required]
        [MaxLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [MaxLength(260)]
        public string NomeFicheiro { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string CaminhoRelativo { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Terminacao { get; set; } = string.Empty;

        public bool IsVideo {get;set;}

        public Guid? UploadedByUserId { get; set; }
    }

    public enum DocumentoFichaClinicaCategoria 
    {
        Clinico = 0,
        Administrativo = 1,
    }

    public enum DocumentoFichaClinicaTipo
    {
        Documento = 0,
        Foto = 1,
        Video = 2,
    }
}