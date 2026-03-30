using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Medicos.MargemMedicoService.DTOs
{
    public class UpdateMargemMedicoRequest : IDto
    {
        public required string ServicoId { get; set; }
        public required string MedicoId { get; set; }
        public decimal? ValorMargem { get; set; }
        public decimal? PercentagemMargem { get; set; }
    }

    public class UpdateMargemMedicoValidator : AbstractValidator<UpdateMargemMedicoRequest>
    {
        public UpdateMargemMedicoValidator()
        {
            _ = RuleFor(x => x.ServicoId)
                .NotEmpty()
                .Must(GSHelpers.BeValidGuid)
                .WithMessage("ServicoId deve ser um GUID válido.");
            _ = RuleFor(x => x.MedicoId)
                .NotEmpty()
                .Must(GSHelpers.BeValidGuid)
                .WithMessage("MedicoId deve ser um GUID válido.");
        }
    }
}
