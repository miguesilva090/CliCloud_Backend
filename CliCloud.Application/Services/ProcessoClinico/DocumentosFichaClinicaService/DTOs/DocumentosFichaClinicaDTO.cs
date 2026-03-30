using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.DTOs
{
    public class DocumentosFichaClinicaDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        
        public string Categoria { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string NomeFicheiro { get; set; } = string.Empty;
        public string CaminhoRelativo { get; set; } = string.Empty;
        public string Terminacao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}

