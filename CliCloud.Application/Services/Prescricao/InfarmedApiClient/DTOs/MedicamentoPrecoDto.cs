namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

public class MedicamentoPrecoDto
{
    public string? TipoPreco { get; set; }
    public decimal? Preco { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public DateTime? DataFimEscoamento { get; set; }
    public bool Ativo { get; set; }
    public bool AtivoMesSeguinte { get; set; }
}