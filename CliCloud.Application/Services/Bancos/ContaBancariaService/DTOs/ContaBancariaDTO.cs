using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.DTOs
{
    public class ContaBancariaDTO : IDto 
    {
        public Guid Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string TipoConta { get; set; } = string.Empty;
        public Guid? BancoId { get; set; }
        public string? BancoNome { get; set; }
        public DateTime? DataAbertura { get; set; }
        public string? NIB { get; set; }
        public decimal? SaldoActual { get; set; }
        public string? GestorConta { get; set; }
        public int? AlertaSaldo { get; set; } 
        public decimal? ValorAlertaSaldo { get; set; } 
        public string? OBS { get; set; }
        public string? IBAN { get; set; }
        public string? BIC { get; set; }
        public int? Ficheiro { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}