using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.EntidadeService.DTOs;
using CliCloud.Application.Services.Bancos.BancoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Organismos.OrganismoService.DTOs
{
    public class OrganismoTableDTO : IDto
    {
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

        // Campos específicos de Organismo
        public string? NomeComercial { get; set; }
        public string? Abreviatura { get; set; }
        public Guid? BancoId { get; set; }
        public string? BancoNome { get; set; }
        public string? CodigoClinica { get; set; }
        public string? Categoria { get; set; }
        public string? Ars { get; set; }
        public bool LimitarConsultas { get; set; }
        public bool ContabilizarFaltas { get; set; }
        public bool TRUST { get; set; }
        public bool ADM { get; set; }
        public bool SADGNR { get; set; }
        public bool SADPSP { get; set; }
        public int? PrazoPagamento { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? DescontoUtente { get; set; }
        public bool Globalbooking { get; set; }
        public string? Contacto { get; set; }
    }
}
