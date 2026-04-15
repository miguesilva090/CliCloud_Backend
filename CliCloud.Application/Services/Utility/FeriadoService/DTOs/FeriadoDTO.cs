using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.FeriadoService.DTOs;

public class FeriadoDTO : IDto 
{
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public DateTime Data { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    
}