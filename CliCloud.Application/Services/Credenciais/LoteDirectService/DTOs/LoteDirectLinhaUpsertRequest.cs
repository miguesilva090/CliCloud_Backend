namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

public class LoteDirectLinhaUpsertRequest 
{
    public Guid ServicoId { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorUtenteOriginal { get; set; }
    public decimal ValorInstituicaoOriginal { get; set; }
    public decimal ValorUtente { get; set; }
    public decimal ValorInstituicao { get; set; }

    public string? CodigoMcdt { get; set; }
}