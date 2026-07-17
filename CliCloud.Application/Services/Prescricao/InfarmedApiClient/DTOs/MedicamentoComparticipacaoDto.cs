namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

public class MedicamentoComparticipacaoDto
{
    public int? RegimeExcecionalId { get; set; }
    public int? NormaRegExcId { get; set; }
    public string? TipoRegime { get; set; }
    public string? NormaRegimeExcecional { get; set; }
    public string? RegimeExcecional { get; set; }
    public string? NivelComparticipacao { get; set; }
    public decimal? PercentComparticipacao { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}