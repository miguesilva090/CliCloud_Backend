using System;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.TipoAdmissaoService.DTOs
{
  public class TipoAdmissaoDTO : IDto
  {
    public Guid Id { get; set; }

    public string Designacao { get; set; } = string.Empty;
  }
}

