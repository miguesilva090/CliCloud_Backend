using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.DTOs
{
    public class CategoriaEspecialidadeDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public int EspecialidadesCount { get; set; }
    }
}
