using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService.DTOs
{
    public class CreateUtentePatologiaComparticipacaoRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public int CodigoComparticipacao { get; set; }
        public string? Designacao { get; set; }
    }


    public class CreateUtentePatologiaComparticipacaoValidator : AbstractValidator<CreateUtentePatologiaComparticipacaoRequest>
    {
        public CreateUtentePatologiaComparticipacaoValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.CodigoComparticipacao).GreaterThan(0);
            _ = RuleFor(x => x.Designacao).MaximumLength(200);
        }
    }
}