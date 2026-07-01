using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.WebserviceAdseService.DTOs;

public class WebserviceAdseDTO : IDto 
{
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public Guid OrganismoId { get; set; }
    public Guid ClinicaFisioterapiaId { get; set; }
    public string UrlAdse { get; set; } = string.Empty;
    public string DominioUserAdse { get; set; } = string.Empty;
    public string UserAdse { get; set; } = string.Empty;
    public string? PasswordAdse { get; set; }
    public string? PasslocalAdse { get; set; }
    public int NumlocalAdse { get; set; } 
    public string? NomelocalAdse { get; set; }
    public string PastaPdfAdse { get; set; } = string.Empty;
}