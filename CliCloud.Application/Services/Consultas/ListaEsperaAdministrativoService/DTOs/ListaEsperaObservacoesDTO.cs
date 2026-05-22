using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;

public class ListaEsperaObservacoesDTO : IDto 
{
    public string Observacoes { get; set; } = string.Empty;
}