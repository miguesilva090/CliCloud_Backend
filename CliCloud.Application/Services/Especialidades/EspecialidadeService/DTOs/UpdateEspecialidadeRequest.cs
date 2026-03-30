using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Especialidades.EspecialidadeService.DTOs
{
    public class UpdateEspecialidadeRequest : IDto
    {
        public required string Nome { get; set; }
        public string? CategoriaEspecialidadeId { get; set; }
        public bool Fisioterapia { get; set; }
        public bool Atendimento { get; set; }
        public bool Globalbooking { get; set; }
    }

    public class UpdateEspecialidadeValidator : AbstractValidator<UpdateEspecialidadeRequest>
    {
        public UpdateEspecialidadeValidator()
        {
            _ = RuleFor(x => x.Nome)
                .NotEmpty()
                .MaximumLength(30)
                .WithMessage("Nome é obrigatório e deve ter no máximo 30 caracteres.");
            _ = RuleFor(x => x.CategoriaEspecialidadeId)
                .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id))
                .WithMessage("CategoriaEspecialidadeId deve ser um GUID válido ou vazio.");
        }
    }
}
