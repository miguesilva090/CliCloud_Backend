using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Artigos.ViaAdministracaoService.DTOs
{
    public class CreateViaAdministracaoRequest : IDto
    {
        public string? Descricao { get; set; }
    }

    public class CreateViaAdministracaoValidator : AbstractValidator<CreateViaAdministracaoRequest>
    {
        public CreateViaAdministracaoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
