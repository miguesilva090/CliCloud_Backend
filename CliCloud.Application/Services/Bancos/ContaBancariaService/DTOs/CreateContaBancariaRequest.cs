using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.DTOs
{
    public class CreateContaBancariaRequest : IDto
    {
        public required string Numero { get; set; }
        public required string TipoConta { get; set; }
        public Guid? BancoId { get; set; }
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
    }

    public class CreateContaBancariaValidator : AbstractValidator<CreateContaBancariaRequest>
    {
        public CreateContaBancariaValidator()
        {
            _ = RuleFor(x => x.Numero)
                .NotEmpty()
                .MaximumLength(24);
            _ = RuleFor(x => x.TipoConta)
                .NotEmpty()
                .MaximumLength(150);
            _ = RuleFor(x => x.NIB).MaximumLength(24);
            _ = RuleFor(x => x.IBAN).MaximumLength(34);
            _ = RuleFor(x => x.BIC).MaximumLength(11);
            _ = RuleFor(x => x.GestorConta).MaximumLength(24);
        }
    }
}