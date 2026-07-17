namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

public class MedicamentoListagemResumoResultDto
{
    public IReadOnlyList<MedicamentoListagemResumoItemDto> Items { get; set; } = [];
    public int Page { get; set; }
    public int Tipo { get; set; }
    public int? TotalCount { get; set; }
    public int? TotalPages { get; set; }
}