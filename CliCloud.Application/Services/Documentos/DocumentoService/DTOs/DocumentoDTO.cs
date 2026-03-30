using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Organismos.OrganismoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs
{
    public class DocumentoDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid TipoDocumentoId { get; set; }
        public TipoDocumentoDTO? TipoDocumento { get; set; }
        public int NumeroDocumento { get; set; }
        public DateTime? Data { get; set; }
        public Guid? UtenteId { get; set; }
        public UtenteDTO? Utente { get; set; }
        public Guid? OrganismoId { get; set; }
        public OrganismoDTO? Organismo { get; set; }
        public Guid? FuncionarioId { get; set; }
        // public FuncionarioDTO? Funcionario { get; set; } // TODO: Criar quando FuncionarioService estiver disponível
        public decimal? DescontoCliente { get; set; }
        public decimal? TotalDocumento { get; set; }
        public decimal? TotalIva { get; set; }
        public decimal? TotalDesconto { get; set; }
        public decimal? TotalLiquido { get; set; }
        public decimal? Outros { get; set; }
        public CondicaoPagamento? CondicaoPagamento { get; set; }
        public TipoModoPagamento? TipoModoPagamento { get; set; }
        public string? Observacoes { get; set; }
        public int? Estado { get; set; }
        public bool Liquidado { get; set; }
        public bool Rectificado { get; set; }
        public bool Exportado { get; set; }
        public bool IsentoIva { get; set; }
        public string? NomeCliente { get; set; }
        public string? MoradaCliente { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public string? LocalidadeCliente { get; set; }
        public string? NumeroContribuinteCliente { get; set; }
        public int? NumVias { get; set; }
        public int? Emitido { get; set; }
        public int? Origem { get; set; }
        public string? GlobalHash { get; set; }
        public int? VersaoChave { get; set; }
        public DateTime? DataSistemaRegisto { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
