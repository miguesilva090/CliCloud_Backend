using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;

public class ConfigReferenciaMbDTO : IDto 
{
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }

    public decimal ValorMinimo { get; set; }
    public int PrazoPagamento { get; set; }
    
    public string? ServicoUrl { get; set; }
    public string? CodigoEntidade { get; set; }
    public string? SubEntidade { get; set; }
    public string? ChaveBackOffice { get; set; }
    public string? IfThenKey { get; set; }
}