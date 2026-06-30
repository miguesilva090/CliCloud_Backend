using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

public class PassarParaHistoricoRequest : IDto
{
    public Guid LoteDirectId { get; set; }
}

public class PassarParaHistoricoResultDTO : IDto
{
    public int CredenciaisActualizadas { get; set; }
    public int CodigoOrganismo { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
}
