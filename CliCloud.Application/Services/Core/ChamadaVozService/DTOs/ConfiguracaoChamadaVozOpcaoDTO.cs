using System.Collections.Generic;

namespace CliCloud.Application.Services.Core.ChamadaVozService.DTOs
{
  public class ConfiguracaoChamadaVozOpcaoDTO
  {
    public string Tipo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Ordem { get; set; }
  }

  public class ConfiguracaoChamadaVozOpcoesDTO
  {
    public IEnumerable<ConfiguracaoChamadaVozOpcaoDTO> Idiomas { get; set; } = [];
    public IEnumerable<ConfiguracaoChamadaVozOpcaoDTO> Variacoes { get; set; } = [];
  }
}
