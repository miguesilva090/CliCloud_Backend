using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.DTOs
{
    public class UpdateModeloAparelhoRequest : IDto
    {
        public string Designacao { get; set; }
        public Guid MarcaAparelhoId { get; set; }
    }

    public class UpdateModeloAparelhoValidator : AbstractValidator<UpdateModeloAparelhoRequest>
    {
        public UpdateModeloAparelhoValidator()
        {
            _ = RuleFor(x => x.Designacao).NotEmpty()
            .MaximumLength(100)
            .WithMessage("Designação é obrigatória e deve ter no máximo 100 caracteres.");
            _ = RuleFor(x => x.MarcaAparelhoId)
            .NotEmpty()
            .WithMessage("MarcaAparelhoId é obrigatório.");
        }
    }
}

