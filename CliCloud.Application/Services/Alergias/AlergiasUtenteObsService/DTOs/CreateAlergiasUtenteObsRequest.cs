using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.AlergiasUtenteObsService.DTOs
{
    public class CreateAlergiasUtenteObsRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public string? Observacoes { get; set; }
        public string? InformacaoImportante { get; set; }
    }

    public class CreateAlergiasUtenteObsValidator : AbstractValidator<CreateAlergiasUtenteObsRequest>
    {
        public CreateAlergiasUtenteObsValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
        }
    }
}
