namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;

public class JuntarFicheirosEletronicosRequest
{
    public Guid DocumentoId { get; set; }
    public string Sigla { get; set; } = "SAD/GNR";
    public List<FicheiroEletronicoAnexoDTO> Ficheiros { get; set; } = [];

}

public class FicheiroEletronicoAnexoDTO
{
    public string Nome { get; set; } = string.Empty;
    public string ConteudoBase64 { get; set; } = string.Empty;
}