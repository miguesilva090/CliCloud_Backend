using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs
{
    public class UpdateTipoDeDorRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateTipoDeDorValidator : AbstractValidator<UpdateTipoDeDorRequest>
    {
        public UpdateTipoDeDorValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}

