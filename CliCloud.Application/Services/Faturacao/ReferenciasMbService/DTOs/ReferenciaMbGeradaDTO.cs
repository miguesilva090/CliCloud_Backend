using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;

public class ReferenciaMbGeradaDTO : IDto
{
    public Guid ReferenciaId { get; set; }
    public string? EntidadeMb { get; set; }
    public string? ReferenciaCodigo { get; set; }
    public string? RequestId { get; set; }
    public decimal Valor { get; set; }
    public DateTime? DataLimitePagamento { get; set; }
    public bool MbWay { get; set; }
}
