using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs
{
    public class TipoDeDorDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}

