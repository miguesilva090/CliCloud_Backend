using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Especialidades.EspecialidadeService.DTOs
{
    public class EspecialidadeLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Guid? CategoriaEspecialidadeId { get; set; }
        public string? CategoriaEspecialidadeDescricao { get; set; }
    }
}
