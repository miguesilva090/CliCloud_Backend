using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.DTOs
{
    public class FichaClinicaSecaoTemplateDTO : IDto
    {
        public Guid Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public int Ordem { get; set; }

        public bool Ativo { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public DateTime? LastModifiedOn { get; set; }
    }
}

