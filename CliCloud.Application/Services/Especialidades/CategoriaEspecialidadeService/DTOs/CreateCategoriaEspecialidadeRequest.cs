using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.DTOs
{
    public class CreateCategoriaEspecialidadeRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateCategoriaEspecialidadeValidator : AbstractValidator<CreateCategoriaEspecialidadeRequest>
    {
        public CreateCategoriaEspecialidadeValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
