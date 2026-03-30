using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Artigos.ViaAdministracaoService.DTOs
{
    public class UpdateViaAdministracaoRequest : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
    }

    public class UpdateViaAdministracaoValidator : AbstractValidator<UpdateViaAdministracaoRequest>
    {
        public UpdateViaAdministracaoValidator()
        {
            _ = RuleFor(x => x.Id).NotEmpty();
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
