using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;

public class ReferenciaMbTableDTO : IDto
{
    public Guid Id { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string? Mensagem { get; set; }

    public string? EntidadeMb { get; set; }
    public string? ReferenciaCodigo { get; set; }
    
    public decimal Valor { get; set; }
    public DateTime DataReferenciaGerada { get; set; }
    public DateTime? DataLimitePagamento { get; set; }
    public DateTime? DataPagamento { get; set; }

    public bool Liquidada { get; set; }
    public bool Anulada { get; set; }

    public string Servico { get; set; } = "Referência MB";
}