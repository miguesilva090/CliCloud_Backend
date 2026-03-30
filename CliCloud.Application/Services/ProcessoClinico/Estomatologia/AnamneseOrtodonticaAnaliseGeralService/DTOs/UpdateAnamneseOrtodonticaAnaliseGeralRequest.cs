using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.DTOs
{
    public class UpdateAnamneseOrtodonticaAnaliseGeralRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public string? SimetriaFacial { get; set; }
        public string? DesenvolvimentoMaxila { get; set; }
        public string? DesenvolvimentoMandibula { get; set; }
        public string? PerfilFacial { get; set; }
        public string? AlturaFacialInferior { get; set; }
        public int? TipoFacial { get; set; }
        public string? CaracteristicasLabios { get; set; }
        public string? RelacaoLabioDenteSuperior { get; set; }
        public string? RelacaoLabioDenteInferior { get; set; }
        public string? FreioLingual { get; set; }
        public string? FormaNariz { get; set; }
        public string? TecidosMolesIntrabucais { get; set; }
        public int? DistanciaIntercomissuralNasal { get; set; }
        public int? DistanciaIntercomissuralPupilar { get; set; }
        public int? Adenoides { get; set; }
        public int? Amigdalas { get; set; }
    }

    public class UpdateAnamneseOrtodonticaAnaliseGeralValidator : AbstractValidator<UpdateAnamneseOrtodonticaAnaliseGeralRequest>
    {
        public UpdateAnamneseOrtodonticaAnaliseGeralValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty().NotNull();
        }
    }
}

