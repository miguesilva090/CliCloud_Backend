using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

public class PassarParaAtivoRequest : IDto
{
    public Guid LoteDirectId { get; set; }
    public int NovoMes { get; set; }
    
    public int NovoAno { get; set; }
}

public class PassarParaAtivoResultDTO : IDto 
{
    public int CredenciaisActualizadas { get; set; }
    public int CodigoOrganismo { get; set; }
    public int MesOrigem { get; set; }
    public int AnoOrigem { get; set; }
    public int MesNovo { get; set; }
    public int AnoNovo { get; set; }
}