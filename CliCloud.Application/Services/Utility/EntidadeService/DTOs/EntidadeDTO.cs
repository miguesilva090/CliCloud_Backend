using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Services.Utility.RuaService.DTOs;
using CliCloud.Application.Services.Utility.CodigoPostalService.DTOs;
using CliCloud.Application.Services.Utility.FreguesiaService.DTOs;
using CliCloud.Application.Services.Utility.ConcelhoService.DTOs;
using CliCloud.Application.Services.Utility.DistritoService.DTOs;
using CliCloud.Application.Services.Utility.PaisService.DTOs;

namespace CliCloud.Application.Services.Utility.EntidadeService.DTOs
{
    public class EntidadeDTO : IDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int TipoEntidadeId { get; set; }
        public string? Email { get; set; }
        public string? NumeroContribuinte { get; set; }
        public Guid? RuaId { get; set; }
        public RuaDTO? Rua { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public CodigoPostalDTO? CodigoPostal { get; set; }
        public Guid? FreguesiaId { get; set; }
        public FreguesiaDTO? Freguesia { get; set; }
        public Guid? ConcelhoId { get; set; }
        public ConcelhoDTO? Concelho { get; set; }
        public Guid? DistritoId { get; set; }
        public DistritoDTO? Distrito { get; set; }
        public Guid? PaisId { get; set; }
        public PaisDTO? Pais { get; set; }
        public string? NumeroPorta { get; set; }
        public string? AndarRua { get; set; }
        public string? Observacoes { get; set; }
        public int? Status { get; set; }
        public string? UrlFoto { get; set; }
        public DateTime CreatedOn { get; set; }
        public IEnumerable<EntidadeContactoDTO>? EntidadeContactos { get; set; }
    }
}

