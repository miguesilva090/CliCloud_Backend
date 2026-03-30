using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.DTOs
{
    public class UpdateCategoriaEspecialidadeRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateCategoriaEspecialidadeValidator : AbstractValidator<UpdateCategoriaEspecialidadeRequest>
    {
        public UpdateCategoriaEspecialidadeValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
