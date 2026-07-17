namespace CliCloud.Application.Services.Prescricao.InfarmedApiClient.DTOs;

public class InfarmedAutocompleteApiResponse
{
    public string Modo { get; set; } = "autocomplete";
    public List<MedicamentoAutocompleteItemDto> Items { get; set; } = [];
}