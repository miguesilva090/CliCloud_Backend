using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utentes.UtenteRnuService.DTOs;

public class ConsultarUtenteRnuRequest : IDto
{
    public string? NumeroSns { get; set; }

    public string? NumeroCartao { get; set; }

    public string? TipoCartao { get; set; }
}