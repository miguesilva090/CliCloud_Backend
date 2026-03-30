using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.DTOs
{
    public class UpdateFichaClinicaSecaoConteudoRequest : IDto
    {
        public Guid Id { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    public class UpdateFichaClinicaSecaoConteudoValidator : AbstractValidator<UpdateFichaClinicaSecaoConteudoRequest>
    {
        public UpdateFichaClinicaSecaoConteudoValidator()
        {
            _ = RuleFor(x => x.Id).NotEmpty();
            _ = RuleFor(x => x.Texto).NotNull();
        }
    }
}

