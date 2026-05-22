using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;

public class ConverterListaEsperaMarcacaoResultDTO : IDto
{
  public Guid ListaEsperaId { get; set; }
  public Guid ConsultaMarcacaoId { get; set; }
}
