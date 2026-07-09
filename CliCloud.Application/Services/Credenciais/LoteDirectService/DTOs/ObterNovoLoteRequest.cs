using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

public sealed class ObterNovoLoteRequest : IDto
{
    public int CodigoOrganismo { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public int TipoLote { get; set; }
    public int TipoServico { get; set; }
}

public sealed class ObterNovoLoteResultDTO : IDto
{
    public int NovoIndice { get; set; }
    public int NovoLote { get; set; }

}