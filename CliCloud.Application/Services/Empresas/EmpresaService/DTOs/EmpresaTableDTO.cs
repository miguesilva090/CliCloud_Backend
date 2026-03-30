using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.EntidadeService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Empresas.EmpresaService.DTOs
{
    public class EmpresaTableDTO : IDto
    {
        // Campos da entidade base Entidade
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public int TipoEntidadeId { get; set; }
        public string? Email { get; set; }
        public string? NumeroContribuinte { get; set; }
        public Guid? RuaId { get; set; }
        public EntidadeTableRuaDTO? Rua { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public EntidadeTableCodigoPostalDTO? CodigoPostal { get; set; }
        public Guid? FreguesiaId { get; set; }
        public EntidadeTableFreguesiaDTO? Freguesia { get; set; }
        public Guid? ConcelhoId { get; set; }
        public EntidadeTableConcelhoDTO? Concelho { get; set; }
        public Guid? DistritoId { get; set; }
        public EntidadeTableDistritoDTO? Distrito { get; set; }
        public Guid? PaisId { get; set; }
        public EntidadeTablePaisDTO? Pais { get; set; }
        public string? NumeroPorta { get; set; }
        public string? AndarRua { get; set; }
        public int? Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ContactoCount { get; set; }

        // Campos específicos de Empresa para listagem
        public string? Contacto { get; set; }
        public string? Fax { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? DescontoUtente { get; set; }
        public string? Categoria { get; set; }
        public int? NumeroTrabalhadores { get; set; }
        public decimal? ValorTrabalhador { get; set; }
        public int? Rescindindo { get; set; }
    }
}

