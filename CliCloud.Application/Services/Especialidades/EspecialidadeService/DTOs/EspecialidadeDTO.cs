using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.DTOs;

namespace CliCloud.Application.Services.Especialidades.EspecialidadeService.DTOs
{
    public class EspecialidadeDTO : IDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Guid? CategoriaEspecialidadeId { get; set; }
        public CategoriaEspecialidadeLightDTO? CategoriaEspecialidade { get; set; }
        public bool Fisioterapia { get; set; }
        public bool Atendimento { get; set; }
        public bool Globalbooking { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
