using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Tratamentos.GoniometriasService.DTOs
{
    public class UpdateGoniometriasRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateGoniometriasValidator : AbstractValidator<UpdateGoniometriasRequest>
    {
        public UpdateGoniometriasValidator()    
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}

