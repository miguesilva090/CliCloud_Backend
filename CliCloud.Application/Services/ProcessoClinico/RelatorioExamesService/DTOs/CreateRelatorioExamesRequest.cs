using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.DTOs
{
    public class CreateRelatorioExamesRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public string? Texto { get; set; }
    }

    public class CreateRelatorioExamesValidator : AbstractValidator<CreateRelatorioExamesRequest>
    {
        public CreateRelatorioExamesValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
        }
    }
}
