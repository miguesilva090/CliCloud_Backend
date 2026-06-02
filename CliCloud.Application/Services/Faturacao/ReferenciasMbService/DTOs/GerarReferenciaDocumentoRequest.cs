using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;

/// <summary>1 = Referência Multibanco, 2 = Pedido MB Way (legado geraRefMB).</summary>
public class GerarReferenciaDocumentoRequest : IDto
{
    public Guid ClinicaId { get; set; }
    public Guid DocumentoId { get; set; }
    public Guid? UtenteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public string? NumeroExibicao { get; set; }
    public decimal Valor { get; set; }
    public int Modo { get; set; }
}
