using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;

public class AdseComunicacaoPaginatedDTO : IDto
{
    public List<AdseComunicacaoLinhaDTO> Linhas { get; set; } = [];
    public decimal TotalFaturaPagina { get; set; }
    public decimal TotalFatura { get; set; }
    public decimal TotalAdsePagina { get; set; }
    public decimal TotalAdse { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
