using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService.DTOs
{
    public class ReplaceUtentePatologiaItemRequest
    {
        public int CodigoComparticipacao { get; set; }
        public string? Designacao { get; set; }
    }

    public class ReplaceUtentePatologiasComparticipacaoRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public List<ReplaceUtentePatologiaItemRequest> Items { get; set; } = new();
    }

    public class ReplaceUtentePatologiasComparticipacaoValidator
        : AbstractValidator<ReplaceUtentePatologiasComparticipacaoRequest>
    {
        public ReplaceUtentePatologiasComparticipacaoValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleForEach(x => x.Items).ChildRules(item =>
            {
                _ = item.RuleFor(i => i.CodigoComparticipacao).GreaterThan(0);
                _ = item.RuleFor(i => i.Designacao).MaximumLength(200);
            });
        }
    }
}
