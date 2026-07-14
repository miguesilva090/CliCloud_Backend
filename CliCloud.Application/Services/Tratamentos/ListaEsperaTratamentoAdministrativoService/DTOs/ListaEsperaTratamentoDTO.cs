using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;

public class ListaEsperaTratamentoDTO : IDto
{
    public Guid Id { get; set; }
    public int CodigoLegado { get; set; }
    public int Ordem { get; set; }
    public DateTime DataEntrada { get; set; }
    public int? OrdemOrigem { get; set; }
    public bool Historico { get; set; }

    public Guid UtenteId { get; set; }
    public string? UtenteNome { get; set; }

    public Guid? MedicoId { get; set; }
    public string? MedicoNome { get; set; }

    public Guid? OrganismoId { get; set; }
    public string? OrganismoNome { get; set; }

    public Guid? PrioridadeId { get; set; }
    public string? PrioridadeDesignacao { get; set; }

    public Guid? EstadoListaEsperaId { get; set; }
    public string? EstadoDesignacao { get; set; }

    public Guid? LocalTratamentoId { get; set; }
    public string? LocalTratamentoDesignacao { get; set; }

    public Guid? PatologiaId { get; set; }
    public string? PatologiaDesignacao { get; set; }

    public Guid? SinistradoId { get; set; }
    public Guid? SeguradoraId { get; set; }

    public string? Designacao { get; set; }
    public int? NumSessoes { get; set; }
    public string? HoraDesejada { get; set; }
    public int? NFaltMax { get; set; }
    public int? NFaltComax { get; set; }
    public string? Credencial { get; set; }
    public DateTime? ValidadeCredencial { get; set; }
    public int? TaxaModeradora { get; set; }
    public string? Obs { get; set; }
    public string? TecObs { get; set; }
    public string? DuracaoTotal { get; set; }
    public bool CredencialExterna { get; set; }
    public IReadOnlyList<ListaEsperaTratamentoServicoDTO> Servicos { get; set; } = [];
}