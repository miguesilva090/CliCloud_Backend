using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.DTOs
{
    public class CreateAnamneseOdontopediatriaRequest : IDto
    {
        public Guid UtenteId { get; set; }

        public decimal? Peso { get; set; }
        public decimal? Altura { get; set; }
        public decimal? PesoPai { get; set; }
        public decimal? AlturaPai { get; set; }
        public decimal? PesoMae { get; set; }
        public decimal? AlturaMae { get; set; }

        public int? CaracteristicasGeraisDesenvolvimento { get; set; }
        public int? TipoAmamentacao { get; set; }
        public int? ObservacaoCardiaca { get; set; }
        public int? ObservacaoRespiracao { get; set; }
        public int? ObservacaoDiccao { get; set; }

        public string? ObservacoesAdicionais { get; set; }
    }

    public class CreateAnamneseOdontopediatriaValidator : AbstractValidator<CreateAnamneseOdontopediatriaRequest>
    {
        public CreateAnamneseOdontopediatriaValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty().NotNull();
        }
    }
}
