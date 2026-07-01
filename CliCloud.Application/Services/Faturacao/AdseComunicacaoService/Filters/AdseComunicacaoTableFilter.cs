using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Filters;

public class AdseComunicacaoTableFilter : PaginationFilter
{
    public string Modulo { get; set; } = AdseComunicacaoModulo.Tratamentos;
    public DateTime? DataInicial { get; set; }
    public DateTime? DataFinal { get; set; }
    public int? EstadoComunicacao { get; set; }
    public Guid? UtenteId { get; set; }
    public bool Devolucoes { get; set; }
    public int? NumOrdemPreFatura { get; set; }
}
