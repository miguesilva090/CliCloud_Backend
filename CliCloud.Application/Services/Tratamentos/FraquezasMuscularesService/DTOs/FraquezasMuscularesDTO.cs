using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs
{
    public class FraquezasMuscularesDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn {get;set;}
        public DateTime? LastModifiedOn {get;set;}
    }
}
