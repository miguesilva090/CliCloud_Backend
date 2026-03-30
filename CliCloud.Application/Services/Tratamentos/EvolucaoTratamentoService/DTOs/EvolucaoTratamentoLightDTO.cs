using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.DTOs
{
    public class EvolucaoTratamentoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid TratamentoId { get; set; }
        public Guid UtenteId { get; set; }

        public string? UtenteNome { get; set; }
        public string? TratamentoDesignacao { get; set; }

        public DateTime CreatedOn { get; set; }      
        public int? EscalaDorAlta { get; set; }          
        public DateTime? DataAlta { get; set; }          
        public string? ObjetivosAlcancados { get; set; }  
    }
}