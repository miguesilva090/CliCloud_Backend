using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Tratamentos.MotivoAltaService.DTOs
{
    public class UpdateMotivoAltaRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateMotivoAltaValidator : AbstractValidator<UpdateMotivoAltaRequest>
    {
        public UpdateMotivoAltaValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty()
                .MaximumLength(100)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}

