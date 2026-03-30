using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaDenticaoDeciduaeMistaService.DTOs
{
    public class AnamneseOrtodonticaDenticaoDeciduaeMistaDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }

        public int? RelacaoMolarDecidua { get; set; }
        public string? DentaduraMista { get; set; }
        public string? SequenciaEsfoliacao { get; set; }
        public string? SequenciaErupcao { get; set; }
        public string? EstagioCalcificacao { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

