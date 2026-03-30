using System;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs
{
  public class MotivoConsultaDTO : IDto
  {
    public Guid Id { get; set; }

    public string Designacao { get; set; } = string.Empty;
  }
}
