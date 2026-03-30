using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.DTOs
{
    public class EvolucaoTratamentoDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid TratamentoId { get; set; }
        public Guid UtenteId { get; set; }

        public bool? PacienteInformadoInicial { get; set; }
        public bool? PacienteMotivadoInicial { get; set; }
        public bool? PacienteColaboranteInicial { get; set; }
        public string? ObservacoesAvaliacaoInicial { get; set; }
        public string? AvaliacaoSubjetivaInicial { get; set; }

        public int? TipoInicioDorInicial { get; set; }
        public int? ValorDorInicial { get; set; }
        public string? TipoDorInicial { get; set; }

        public string? ExameFisicoRegiaoInicial { get; set; }
        public string? PatologiaInicial { get; set; }

        public bool? EdemaInicial { get; set; }
        public int? TipoEdemaInicial { get; set; }
        public string? RegiaoEdemaInicialId { get; set; }
        public string? RegiaoEdemaInicialDescricao { get; set; }
        public string? ObservacoesEdemaInicial { get; set; }

        public bool? ElasticidadeInicial { get; set; }
        public string? ObservacoesElasticidadeInicial { get; set; }

        public bool? ParestesiasInicial { get; set; }
        public string? ZonaParestesiasInicialId { get; set; }
        public string? ZonaParestesiasInicialDescricao { get; set; }

        public bool? DorIrradiadaInicial { get; set; }
        public string? ZonaDorIrradiadaInicialId { get; set; }
        public string? ZonaDorIrradiadaInicialDescricao { get; set; }

        public bool? CicatrizInicial { get; set; }
        public string? ZonaCicatrizInicialId { get; set; }
        public string? ZonaCicatrizInicialDescricao { get; set; }

        public bool? FraquezaMuscularInicial { get; set; }
        public string? ZonaFraquezaInicialId { get; set; }
        public string? ZonaFraquezaInicialDescricao { get; set; }

        public int? MarchaAutonomaInicial { get; set; }
        public string? ObservacoesMarchaAutonomaInicial { get; set; }

        public string? GoniometriaInicial { get; set; }
        public string? TesteMuscularInicial { get; set; }
        public string? AutonomiaInicial { get; set; }

        public string? ObjetivosEspecificosInicial { get; set; }
        public string? SessoesPropostasInicial { get; set; }
        public string? TempoNecessarioInicial { get; set; }

        // Avaliação Final
        public bool? PacienteInformadoFinal { get; set; }
        public bool? PacienteMotivadoFinal { get; set; }
        public bool? PacienteColaboranteFinal { get; set; }
        public string? ObservacoesAvaliacaoFinal { get; set; }
        public string? AvaliacaoSubjetivaFinal { get; set; }

        public int? TipoInicioDorFinal { get; set; }
        public int? ValorDorFinal { get; set; }
        public string? TipoDorFinal { get; set; }

        public string? ExameFisicoRegiaoFinalId { get; set; }
        public string? PatologiaFinalId { get; set; }

        public bool? EdemaFinal { get; set; }
        public int? TipoEdemaFinal { get; set; }
        public string? RegiaoEdemaFinalId { get; set; }
        public string? RegiaoEdemaFinalDescricao { get; set; }
        public string? ObservacoesEdemaFinal { get; set; }

        public bool? ElasticidadeFinal { get; set; }
        public string? ObservacoesElasticidadeFinal { get; set; }

        public bool? ParestesiasFinal { get; set; }
        public string? ZonaParestesiasFinalId { get; set; }
        public string? ZonaParestesiasFinalDescricao { get; set; }

        public bool? DorIrradiadaFinal { get; set; }
        public string? ZonaDorIrradiadaFinalId { get; set; }
        public string? ZonaDorIrradiadaFinalDescricao { get; set; }

        public bool? CicatrizFinal { get; set; }
        public string? ZonaCicatrizFinalId { get; set; }
        public string? ZonaCicatrizFinalDescricao { get; set; }

        public bool? FraquezaMuscularFinal { get; set; }
        public string? ZonaFraquezaFinalId { get; set; }
        public string? ZonaFraquezaFinalDescricao { get; set; }

        public int? MarchaAutonomaFinal { get; set; }
        public string? ObservacoesMarchaAutonomaFinal { get; set; }

        public string? GoniometriaFinal { get; set; }
        public string? TesteMuscularFinal { get; set; }
        public string? AutonomiaFinal { get; set; }

        public string? ObjetivosAlcancados { get; set; }
        public string? NovosObjetivos { get; set; }
        public string? SessoesPropostasFinal { get; set; }
        public string? TempoNecessarioFinal { get; set; }

        // Alta
        public DateTime? DataAlta { get; set; }
        public Guid? MotivoAltaId { get; set; }
        public int? EscalaDorAlta { get; set; }
        public string? IndicacoesParaUtenteAlta { get; set; }
        public string? ObservacaoClinica { get; set; }
    }
}