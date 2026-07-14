using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;

public class ListaEsperaTratamentoObservacoesDTO : IDto
{
    public string Observacoes { get; set; } = string.Empty;
}
