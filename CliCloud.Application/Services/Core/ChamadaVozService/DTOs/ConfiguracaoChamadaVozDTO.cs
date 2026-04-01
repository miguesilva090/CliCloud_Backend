using System;

namespace CliCloud.Application.Services.Core.ChamadaVozService.DTOs
{
  public class ConfiguracaoChamadaVozDTO
  {
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public bool Ativo { get; set; }
    public string? Url { get; set; }
    public string? Language { get; set; }
    public string? Tld { get; set; }
  }
}
