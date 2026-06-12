using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.DTOs
{
    public class ContaBancariaTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string TipoConta { get; set; } = string.Empty;
        public Guid? BancoId { get; set; }
        public string? BancoNome { get; set; }
        public DateTime? DataAbertura { get; set; }
        public string? NIB { get; set; }
        public string? IBAN { get; set; }
        public decimal? SaldoActual { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}