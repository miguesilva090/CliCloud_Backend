using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService.DTOs;

public class TratamentoMarcadosTableDTO : IDto
{
    public Guid Id { get; set; }
    public string? Designacao { get; set; }
    public string? NomePatologia { get; set; }

    public Guid? UtenteId { get; set; }
    public string? UtenteNome { get; set; }
    public string? NumeroUtente { get; set; }
    
    public Guid? OrganismoId { get; set; }
    public string? OrganismoNome { get; set; }

    public int Lotes { get; set; }
    public DateTime? DataFim { get; set; }
    public DateTime? DataInic {get; set;}

    public int Iniciado { get; set; }
    public int Suspenso { get; set; }
    public int Terminado { get; set; }
    public int Provisorio { get; set; }
    public decimal? Debito { get; set; }

    public Guid? LocalTratamentoId { get; set; }
    public string? LocalTratamentoNome { get; set; }
}