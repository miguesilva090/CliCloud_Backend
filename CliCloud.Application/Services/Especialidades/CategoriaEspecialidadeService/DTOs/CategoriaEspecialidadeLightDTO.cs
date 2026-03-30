using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.DTOs
{
    public class CategoriaEspecialidadeLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
