using System.Collections.Generic;

namespace CliCloud.Application.Services.Core.VozService.DTOs
{
  public class ConfiguracaoVozOpcaoDTO
  {
    public string Tipo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Ordem { get; set; }
  }

  public class ConfiguracaoVozOpcoesDTO
  {
    public IEnumerable<ConfiguracaoVozOpcaoDTO> Idiomas { get; set; } = [];
    public IEnumerable<ConfiguracaoVozOpcaoDTO> Vozes { get; set; } = [];
  }
}
