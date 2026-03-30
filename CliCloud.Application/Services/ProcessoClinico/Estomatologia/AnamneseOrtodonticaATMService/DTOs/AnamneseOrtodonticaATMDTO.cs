using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.DTOs
{
    public class AnamneseOrtodonticaATMDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public string? Palpacao { get; set; }
        public string? RelacaoCentrica { get; set; }
        public string? LateralidadeEsquerda { get; set; }
        public string? LateralidadeDireita { get; set; }
        public string? Protrusao { get; set; }
        public string? MusculosMastigatorios { get; set; }
        public string? MusculosInfra { get; set; }
        public string? MusculosSupra { get; set; }
        public string? Obs { get; set; }
        public bool? ApertaOuRangeDentes { get; set; }
        public bool? MusculosMandibulaDoridosAoAcordar { get; set; }
        public bool? DorMandibulaOuvido { get; set; }
        public bool? NaoPodeAbrirFecharBoca { get; set; }
        public bool? DentesSensiveisDesgastados { get; set; }
        public bool? SofreuAlgumTraumatismo { get; set; }
        public bool? SenteBarulhoZumbido { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

