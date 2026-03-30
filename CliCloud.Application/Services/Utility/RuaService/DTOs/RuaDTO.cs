using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.FreguesiaService.DTOs;
using CliCloud.Application.Services.Utility.CodigoPostalService.DTOs;

namespace CliCloud.Application.Services.Utility.RuaService.DTOs
{
    public class RuaDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public Guid? FreguesiaId { get; set; }
        public FreguesiaDTO? Freguesia { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public CodigoPostalDTO? CodigoPostal { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

