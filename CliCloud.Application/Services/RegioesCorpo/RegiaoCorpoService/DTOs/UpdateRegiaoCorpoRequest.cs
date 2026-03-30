using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.DTOs
{
    public class UpdateRegiaoCorpoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateRegiaoCorpoValidator : AbstractValidator<UpdateRegiaoCorpoRequest>
    {
        public UpdateRegiaoCorpoValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty()
                .MaximumLength(100)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}

