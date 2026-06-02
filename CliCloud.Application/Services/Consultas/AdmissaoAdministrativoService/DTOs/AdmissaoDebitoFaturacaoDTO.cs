using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class AdmissaoDebitoFaturacaoDTO : IDto
{
  public decimal Debito { get; set; }
  public bool PodeFaturar { get; set; }
  /// <summary>Serviços da admissão ainda não referenciados em documentos emitidos (não anulados).</summary>
  public int ServicosComDebito { get; set; }
  public int ServicosTotal { get; set; }
  public List<Guid> AdmissaoServicoIdsComDebito { get; set; } = [];
}
