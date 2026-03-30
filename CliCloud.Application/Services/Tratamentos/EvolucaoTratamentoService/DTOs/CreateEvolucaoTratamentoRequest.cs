using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.DTOs
{
    public class CreateEvolucaoTratamentoRequest : IDto
    {
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
    }

    public class CreateEvolucaoTratamentoValidator : AbstractValidator<CreateEvolucaoTratamentoRequest>
    {
        public CreateEvolucaoTratamentoValidator()
        {
            _ = RuleFor(x => x.TratamentoId).NotEmpty();
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            
        }
    }
}
