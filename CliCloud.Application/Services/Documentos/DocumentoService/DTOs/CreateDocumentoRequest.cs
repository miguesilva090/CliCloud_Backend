using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs
{
    public class CreateDocumentoRequest : IDto
    {
        public required string TipoDocumentoId { get; set; }
        public int NumeroDocumento { get; set; }
        public DateTime? Data { get; set; }
        public string? UtenteId { get; set; }
        public string? OrganismoId { get; set; }
        public string? FuncionarioId { get; set; }
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
        public string? CodigoPostalId { get; set; }
        public string? LocalidadeCliente { get; set; }
        public string? NumeroContribuinteCliente { get; set; }
        public int? NumVias { get; set; }
        public int? Emitido { get; set; }
        public int? Origem { get; set; }
        public string? GlobalHash { get; set; }
        public int? VersaoChave { get; set; }
        public DateTime? DataSistemaRegisto { get; set; }
    }

    public class CreateDocumentoValidator : AbstractValidator<CreateDocumentoRequest>
    {
        public CreateDocumentoValidator()
        {
            _ = RuleFor(x => x.TipoDocumentoId)
                .NotEmpty()
                .Must(GSHelpers.BeValidGuid)
                .WithMessage("TipoDocumentoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.NumeroDocumento)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("NumeroDocumento é obrigatório e deve ser maior que zero.");
            _ = RuleFor(x => x.UtenteId)
                .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id))
                .WithMessage("UtenteId deve ser um GUID válido ou vazio.");
            _ = RuleFor(x => x.OrganismoId)
                .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id))
                .WithMessage("OrganismoId deve ser um GUID válido ou vazio.");
            _ = RuleFor(x => x.FuncionarioId)
                .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id))
                .WithMessage("FuncionarioId deve ser um GUID válido ou vazio.");
            _ = RuleFor(x => x.CodigoPostalId)
                .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id))
                .WithMessage("CodigoPostalId deve ser um GUID válido ou vazio.");
        }
    }
}
