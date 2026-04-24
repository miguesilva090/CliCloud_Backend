#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico
{
    [Table("HabitosEVicios", Schema = "ProcessoClinico")]
    public class HabitosEVicios : AuditableEntityWithSoftDelete
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; } = null!;

        public bool ConsumoDeFrutas { get; set; }

        public bool ConsumoAgua { get; set; }
        public string? QuantidadeAgua { get; set; }

        public bool ConsumoPeixe { get; set; }

        public bool ConsumoCarne { get; set; }
        public int? TipoCarne { get; set; }

        public bool ConsumoVegetais { get; set; }

        public bool IngestaoLeite { get; set; }

        public bool ConsumoSalgados { get; set; }

        public bool ConsumoAcucarados { get; set; }

        public bool ConsumoBebidasAlcoolicas { get; set; }
        public string? BebidasAlcoolicas { get; set; }
        public string? QuantidadeAlcool { get; set; }
        public DateTime? AlcoolDesdeQuando { get; set; }

        public bool Fuma { get; set; }
        public string? QuantosFumaDia { get; set; }
        public DateTime? TabacoDesdeQuando { get; set; }

        public bool ConsumoDrogas { get; set; }
        public string? Drogas { get; set; }
        public DateTime? DrogasDesdeQuando { get; set; }

        public string? OutrosVicios { get; set; }
        public DateTime? OutrosViciosDesdeQuando { get; set; }

        public string? ObservacoesHabitosAlimentaresEVicios { get; set; }
        public string? ObservacoesHabitosMedicamentosExercicioFisico { get; set; }

        public bool TomaFarmacosPrescritos { get; set; }

        public bool TomaFarmacosSemReceita { get; set; }
        public string? FarmacosSemReceita { get; set; }

        public bool PraticaExercicioFisico { get; set; }
        public string? TipoExercicioFisico { get; set; }
        public string? FrequenciaExFisico { get; set; }

    }
}