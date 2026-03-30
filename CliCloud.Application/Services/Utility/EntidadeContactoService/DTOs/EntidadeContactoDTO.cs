using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs
{
    public class EntidadeContactoDTO : IDto
    {
        public Guid Id { get; set; }
        public int EntidadeContactoTipoId { get; set; }
        public Guid EntidadeId {get;set;}
        public string? Valor { get; set; }
        public bool Principal { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

