using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.DTOs;

public class SeparadorPersonalizadoDTO : IDto
{
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public string NomeSeparador { get; set; } = string.Empty;
    public Guid FormularioId { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}
