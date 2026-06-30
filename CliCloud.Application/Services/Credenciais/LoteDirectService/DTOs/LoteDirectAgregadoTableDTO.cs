using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

public class LoteDirectAgregadoTableDTO : IDto
{
    public Guid Id { get; set; }
    public int Indice { get; set; }
    public int NumeroLote { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int CodigoOrganismo { get; set; }
    public string? OrganismoSigla { get; set; }
    public int TipoLote { get; set; }
    public string? TipoLoteDesignacao { get; set; }
    public int TipoServico { get; set; }
    public DateTime DataLote { get; set; }
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
    public decimal ValorTaxa { get; set; }
    public int NumeroRequisicoes { get; set; }
}
