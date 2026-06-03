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
        public int AnoFiscal { get; set; }
        public int NumeroDocumento { get; set; }
        public string? NumeroExibicao { get; set; }
        public DateTime? Data { get; set; }
        public Guid? UtenteId { get; set; }
        public UtenteDTO? Utente { get; set; }
        public Guid? OrganismoId { get; set; }
        public OrganismoDTO? Organismo { get; set; }
        public Guid? FuncionarioId { get; set; }
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
        public int? EstadoDocumento { get; set; }
        public string? EstadoDocumentoLabel { get; set; }
        public ModuloOrigemDocumento? ModuloOrigem { get; set; }
        public string? OrigemLabel { get; set; }
        public bool Liquidado { get; set; }
        public bool Rectificado { get; set; }
        public bool Exportado { get; set; }
        public bool IsentoIva { get; set; }
        public bool Anulado { get; set; }
        public bool EstaEmitido { get; set; }
        public string? NomeCliente { get; set; }
        public string? MoradaCliente { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public string? CodigoPostalCodigo { get; set; }
        public string? LocalidadeCliente { get; set; }
        public string? NumeroContribuinteCliente { get; set; }
        public string? Beneficiario { get; set; }
        public string? TipoSerie { get; set; }
        public bool IvaCaixa { get; set; }
        public Guid? MotivoIsencaoId { get; set; }
        public Guid? MoedaId { get; set; }
        public decimal? TaxaCambio { get; set; }
        public Guid? BancoId { get; set; }
        public DateTime? DataVencimentoPagamento { get; set; }
        public DateTime? FaturaGlobalDataInicio { get; set; }
        public DateTime? FaturaGlobalDataFim { get; set; }
        public string? RetencaoImposto { get; set; }
        public decimal? RetencaoTaxa { get; set; }
        public decimal? RetencaoValor { get; set; }
        public string? RetencaoMotivo { get; set; }
        public string? CodigoValidacaoTransporte { get; set; }
        public DateTime? DataTransporte { get; set; }
        public string? HoraTransporte { get; set; }
        public Guid? DocumentoOrigemId { get; set; }
        public string? IdentificadorUnicoDocumentoOrigem { get; set; }
        public DateTime? DataDocumentoOrigem { get; set; }
        public int? NumVias { get; set; }
        public int? Emitido { get; set; }
        public int? Origem { get; set; }
        public string? GlobalHash { get; set; }
        public int? VersaoChave { get; set; }
        public string? MotivoAnulacao { get; set; }
        public DateTime? DataAnulacao { get; set; }
        public DateTime? DataSistemaRegisto { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public List<DocumentoLinhaDTO> Linhas { get; set; } = [];
    }
}