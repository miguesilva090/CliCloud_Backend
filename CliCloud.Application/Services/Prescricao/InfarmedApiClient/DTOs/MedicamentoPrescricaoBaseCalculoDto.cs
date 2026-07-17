namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

public class MedicamentoPrescricaoBaseCalculoDto
{
    public decimal? Pvp { get; set; }
    public decimal? PrecoReferencia { get; set; }
    public decimal? PvpNotificado { get; set; }
    public decimal? PvpMax100Re { get; set; }
    public decimal? PrecoUnitario { get; set; }
    public decimal? TaxaComparticipacao { get; set; }
    public string? GrupoHomogeneoCod { get; set; }
}