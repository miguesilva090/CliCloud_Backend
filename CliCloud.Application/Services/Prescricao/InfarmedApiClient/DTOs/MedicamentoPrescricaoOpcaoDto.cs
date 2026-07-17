namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

public class MedicamentoPrescricaoOpcaoDto
{
    public string Cnpem { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Dosagem { get; set; }
    public string? Embalagem { get; set; }
    public bool Generico { get; set; }
    public decimal? Preco { get; set; }
    public decimal? PercentComparticipacao { get; set; }
    public bool Selecionado { get; set; }
}