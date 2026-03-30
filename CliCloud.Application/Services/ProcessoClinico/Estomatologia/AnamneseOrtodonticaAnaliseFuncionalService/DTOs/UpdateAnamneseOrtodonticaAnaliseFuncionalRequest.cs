using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.DTOs
{
    public class UpdateAnamneseOrtodonticaAnaliseFuncionalRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public string? LabioSuperior { get; set; }
        public string? LabioSuperiorTonicidade { get; set; }
        public string? LabioInferior { get; set; }
        public string? LabioInferiorTonicidade { get; set; }
        public string? AspetoLabioSuperiorInferior { get; set; }
        public string? LinguaAspeto { get; set; }
        public string? LinguaTonicidade { get; set; }
        public string? LinguaPosicionamento { get; set; }
        public string? MusculaturaFacial { get; set; }
        public string? MusculaturaMentoniana { get; set; }
        public string? TipoRespiracao { get; set; }
        public string? Forracao { get; set; }
        public string? Mastigacao { get; set; }
        public string? MusculosMastigatorios { get; set; }
        public string? ComentariosAdicionais { get; set; }
    }

    public class UpdateAnamneseOrtodonticaAnaliseFuncionalValidator : AbstractValidator<UpdateAnamneseOrtodonticaAnaliseFuncionalRequest>
    {
        public UpdateAnamneseOrtodonticaAnaliseFuncionalValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty().NotNull();
        }
    }
}

