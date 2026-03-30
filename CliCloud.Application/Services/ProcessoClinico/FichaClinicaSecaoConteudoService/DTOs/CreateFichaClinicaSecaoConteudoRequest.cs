using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.DTOs
{
    public class CreateFichaClinicaSecaoConteudoRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public Guid CampoId { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    public class CreateFichaClinicaSecaoConteudoValidator : AbstractValidator<CreateFichaClinicaSecaoConteudoRequest>
    {
        public CreateFichaClinicaSecaoConteudoValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.CampoId).NotEmpty();
            _ = RuleFor(x => x.Texto).NotNull();
        }
    }
}
