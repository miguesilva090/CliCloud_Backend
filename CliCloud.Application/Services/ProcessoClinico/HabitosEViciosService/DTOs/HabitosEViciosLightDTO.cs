using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.DTOs
{
    public class HabitosEViciosLightDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public bool ConsumoDeFrutas { get; set; }
        public bool ConsumoAgua { get; set; }
        public bool ConsumoPeixe { get; set; }
        public bool ConsumoCarne { get; set; }
        public bool ConsumoVegetais { get; set; }
        public bool IngestaoLeite { get; set; }
        public bool ConsumoSalgados { get; set; }
        public bool ConsumoAcucarados { get; set; }
        public bool ConsumoBebidasAlcoolicas { get; set; }
        public bool Fuma { get; set; }
        public bool ConsumoDrogas { get; set; }
        public bool TomaFarmacosPrescritos { get; set; }
        public bool TomaFarmacosSemReceita { get; set; }
        public bool PraticaExercicioFisico { get; set; }
    }
}