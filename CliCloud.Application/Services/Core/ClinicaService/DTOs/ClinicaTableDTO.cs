using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.ClinicaService.DTOs
{
  public class ClinicaTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? NomeComercial { get; set; }
    public string? Abreviatura { get; set; }

    public bool PorDefeito { get; set; }

    public DateTime CreatedOn { get; set; }
  }
}
