using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;

public class ListaEsperaTratamentoProximoIdentificadorDTO : IDto
{
    public int CodigoListaEspera { get; set; }
    public int ProximaOrdem { get; set; }
}
