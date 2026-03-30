using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.DTOs
{
    public class HabitosEViciosTableDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public bool ConsumoBebidasAlcoolicas { get; set; }
        public bool Fuma { get; set; }
        public bool ConsumoDrogas { get; set; }
        public bool PraticaExercicioFisico { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

