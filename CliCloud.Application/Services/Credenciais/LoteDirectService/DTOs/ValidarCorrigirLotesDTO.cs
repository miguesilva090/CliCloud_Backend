namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

public class ValidarCorrigirLotesDTO
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int CabecalhosEncontrados { get; set; }
    public int CabecalhosSemOrganismo { get; set; }
    public int CabecalhosSemLinhasNemConsulta { get; set; }
    public int LinhasSemPreco { get; set; }
    public int AgregadosExistentes { get; set; }
    public bool PodeCorrigir { get; set; }
    public IList<string> Problemas { get; set; } = [];
    public IList<string> Avisos { get; set; } = [];
}
