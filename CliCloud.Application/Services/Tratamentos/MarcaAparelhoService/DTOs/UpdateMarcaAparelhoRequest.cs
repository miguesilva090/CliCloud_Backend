using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.DTOs
{
    public class UpdateMarcaAparelhoRequest : IDto
    {
        public string Designacao { get; set; }
    }

    public class UpdateMarcaAparelhoValidator : AbstractValidator<UpdateMarcaAparelhoRequest>
    {
        public UpdateMarcaAparelhoValidator()
        {
            _ = RuleFor(x => x.Designacao)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Designação é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}

