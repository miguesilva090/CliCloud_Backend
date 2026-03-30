using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.DTOs
{
    public class FichaClinicaSecaoConteudoDTO : IDto
    {
        public Guid Id { get; set; }
        
        public Guid UtenteId { get; set; }
         
        public Guid CampoId { get; set; }

        public string CampoNome { get; set; } = string.Empty;
        
        public Guid SeparadorId { get; set; }

        public string SeparadorNome { get; set; } = string.Empty;

        public string Texto { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public DateTime? LastModifiedOn { get; set; }
    }
}

