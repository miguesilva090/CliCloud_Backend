using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;

public class ListaEsperaTratamentoTableDTO : IDto
{
    public Guid Id { get; set; }
    public int Ordem { get; set; }
    public DateTime DataEntrada { get; set; }

    public Guid UtenteId { get; set; }
    public string? UtenteNome { get; set; }

    public string? Designacao { get; set; }
    public int? NumSessoes { get; set; }

    public string? PrioridadeDesignacao { get; set; }
    public string? EstadoDesignacao { get; set; }

    public string? Credencial { get; set; }
    public DateTime? ValidadeCredencial { get; set; }

    public string? LocalTratamentoDesignacao { get; set; }
    public string? OrganismoNome { get; set; }
}