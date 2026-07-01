namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;

public class AdseComunicarDocumentosRequest
{
    public string TipoPreFatura { get; set; } = string.Empty;
    public int NumOrdemPreFatura { get; set; }
    public int Operacao { get; set; }
    public bool Devolucoes { get; set; }
    public List<AdseComunicarLinhaRequest> Linhas { get; set; } = [];
}

public class AdseComunicarLinhaRequest
{
    public Guid OrigemClinicaId { get; set; }
    public Guid DocumentoId { get; set; }
    public string NumeroFatura { get; set; } = string.Empty;
}
