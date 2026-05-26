using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ConsultaService.DTOs;

public class IniciarAtendimentoConsultaRequest : IDto
{
  public Guid? ConsultaId { get; set; }
  public Guid? ConsultaMarcacaoId { get; set; }
  public Guid? AdmissaoId { get; set; }
}
