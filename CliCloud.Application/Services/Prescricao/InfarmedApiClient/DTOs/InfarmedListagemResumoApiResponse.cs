namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

internal sealed class InfarmedListagemResumoApiResponse
{
    public string Modo { get; set; } = "resumo";
    public List<MedicamentoListagemResumoItemDto> Items { get; set; } = [];
    public int Page { get; set; }
    public int Tipo { get; set; }
    public int? TotalCount { get; set; }
    public int? TotalPages { get; set; }
}