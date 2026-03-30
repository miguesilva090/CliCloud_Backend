using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.DTOs
{
    public class UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaRequest : IDto
    {
        public Guid UtenteId { get; set; }

        public int? RelacaoMolarDecidua { get; set; }
        public string? DentaduraMista { get; set; }
        public string? SequenciaEsfoliacao { get; set; }
        public string? SequenciaErupcao { get; set; }
        public string? EstagioCalcificacao { get; set; }
    }

    public class UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaValidator : AbstractValidator<UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaRequest>
    {
        public UpdateAnamneseOrtodonticaDenticaoDeciduaeMistaValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty().NotNull();
        }
    }
}

