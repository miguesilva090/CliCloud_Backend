using System.Text.Json.Serialization;

namespace CliCloud.Application.Services.Utentes.UtenteService.DTOs
{
    /// <summary>Item para criar/atualizar uma linha do subsistema de saúde do utente.</summary>
    public class UpsertUtenteSubsistemaLinhaItemRequest
    {
        public string? OrganismoId { get; set; }
        public string? Designacao { get; set; }
        public string? NumeroBeneficiario { get; set; }
        public string? Sigla { get; set; }
        public string? NomeBeneficiario { get; set; }
        public DateOnly? DataCartao { get; set; }
        public string? NumeroApolice { get; set; }

        [JsonPropertyName("empresaId")]
        public string? EmpresaId { get; set; }
    }
}
