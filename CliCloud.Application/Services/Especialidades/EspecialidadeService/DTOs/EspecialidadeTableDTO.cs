using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Especialidades.EspecialidadeService.DTOs
{
    public class EspecialidadeTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Guid? CategoriaEspecialidadeId { get; set; }
        public string? CategoriaEspecialidadeDescricao { get; set; }
        public bool Fisioterapia { get; set; }
        public bool Atendimento { get; set; }
        public bool Globalbooking { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
