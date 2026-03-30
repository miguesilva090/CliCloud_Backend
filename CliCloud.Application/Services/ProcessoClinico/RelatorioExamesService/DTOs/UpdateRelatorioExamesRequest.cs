using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.DTOs
{
    public class UpdateRelatorioExamesRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public string? Texto { get; set; }
    }

    public class UpdateRelatorioExamesValidator : AbstractValidator<UpdateRelatorioExamesRequest>
    {
        public UpdateRelatorioExamesValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
        }
    }
}

