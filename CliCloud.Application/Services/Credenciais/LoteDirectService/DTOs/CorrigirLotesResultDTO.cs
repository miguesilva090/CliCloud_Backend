namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

public class CorrigirLotesResultDTO
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int CabecalhosProcessados { get; set; }
    public int AgregadosCriados { get; set; }
    public int DetalhesCriados { get; set; }
    public IList<string> Avisos { get; set; } = [];
}
