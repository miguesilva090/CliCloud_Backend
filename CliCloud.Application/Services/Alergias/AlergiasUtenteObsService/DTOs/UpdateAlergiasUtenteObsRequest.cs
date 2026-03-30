using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.AlergiasUtenteObsService.DTOs
{
    public class UpdateAlergiasUtenteObsRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public string? Observacoes { get; set; }
        public string? InformacaoImportante { get; set; }
    }

    public class UpdateAlergiasUtenteObsValidator : AbstractValidator<UpdateAlergiasUtenteObsRequest>
    {
        public UpdateAlergiasUtenteObsValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
        }
    }
}
