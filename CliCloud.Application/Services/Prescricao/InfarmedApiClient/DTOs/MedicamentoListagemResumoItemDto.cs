namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

public class MedicamentoListagemResumoItemDto
{
    public string ProdId { get; set; } = string.Empty;
    public string EmbId { get; set; } = string.Empty;
    public string Cnpem { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Dosagem { get; set; } 
    public string? Embalagem { get; set; }
    public string? NrRegisto { get; set; }
    public string? PrincipioActivo { get; set; }
    public string? FormaFarmaceutica { get; set; }
    public bool Generico { get; set; }


    public string NomeCompleto => 
        string.Join(" ", new[] { Nome, Dosagem }.Where(x => !string.IsNullOrWhiteSpace(x)));
}