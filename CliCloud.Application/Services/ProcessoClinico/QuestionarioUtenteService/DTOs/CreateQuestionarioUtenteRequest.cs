using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.QuestionarioUtenteService.DTOs
{
  public class CreateQuestionarioUtenteRequest : IDto
  {
    public Guid UtenteId { get; set; }

    public bool EstaFazerTratamento { get; set; }
    public string? DuracaoTratamento { get; set; }
    public string? DescTratamento { get; set; }

    public bool ProblemasNeurologicos { get; set; }
    public bool ProblemasNeuromusculares { get; set; }
    public bool ProblemasRespiratorios { get; set; }
    public bool ProblemasArticularesOuReumatismo { get; set; }
    public bool TonturasEZumbidosNosOuvidos { get; set; }
    public bool ProblemasComportamento { get; set; }
    public bool ProblemasRenais { get; set; }
    public bool TonturasCardiovasculares { get; set; }
    public bool ProblemasDigestivos { get; set; }
    public bool ProblemasGastricos { get; set; }
    public bool FebreReumatica { get; set; }
    public bool ProblemasIntestinais { get; set; }
    public bool ProblemasUrinarios { get; set; }
    public bool ProblemasComAnestesia { get; set; }
    public bool ProblemasDeHomorragia { get; set; }
    public bool ProblemasFormigueiroDormencia { get; set; }
    public bool ProblemasCicratizacao { get; set; }
    public bool HepatiteSida { get; set; }
    public bool Gravidez { get; set; }
    public bool SedeEBocaSeca { get; set; }
    public bool UsaPacemaker { get; set; }

    public bool? DoencaAutoImune { get; set; }
    public bool? DoencaCronica { get; set; }
    public bool? DoencaGenetica { get; set; }

    public bool ProblemasDiabetes { get; set; }
    public int? TipoDiabetes { get; set; }

    public bool TensaoArterial { get; set; }
    public int? TipoTensaoArterial { get; set; }

    public bool Colestrol { get; set; }
    public int? TipoColestrol { get; set; }

    public string? Observacoes { get; set; }
  }
}

