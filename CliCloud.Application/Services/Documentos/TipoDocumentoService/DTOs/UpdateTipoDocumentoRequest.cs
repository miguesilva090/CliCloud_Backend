using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.DTOs
{
    public class UpdateTipoDocumentoRequest : IDto
    {
        public required string Descricao { get; set; }
        public required string Abreviatura { get; set; }
        public string? Natureza { get; set; }
        public int? TipoMovimento { get; set; }
        public int? CodigoTipoDocumentoSaft { get; set; }
        public string? NumeroSerie { get; set; }
        public string? TipoSerie { get; set; }
        public int? NumeroDocumento { get; set; }
        public int? NumVias { get; set; }
        public int? TemCabecalho { get; set; }
        public int? ImprimirEmtodasAsVias { get; set; }
        public int? PermiteMovimento { get; set; }
        public int? Config { get; set; }
        public int? AtualizaStock { get; set; }
        public bool Inactivo { get; set; }
        public bool MostraFaturacao { get; set; }
        public bool DescarregarTesouraria { get; set; }
        public bool Habilitado { get; set; }
        public string? Cae { get; set; }
        public string? CodigoATCUD { get; set; }
        public string? ATCUDEstado { get; set; }
        public DateTime? ATCUDData { get; set; }
        public string? ContabContaConsulta { get; set; }
        public string? ContabContaTratamento { get; set; }
        public string? ContabContaOutros { get; set; }
        public int? ContabTipoServicoConsulta { get; set; }
        public int? ContabTipoServicoTratamento { get; set; }
        public string? ContabTipoConta { get; set; }
        public string? ContabDiario { get; set; }
        public string? ContabSeccao { get; set; }
        public string? ContabSerieSeccao { get; set; }
        public string? ContabDimensaoCCDebito { get; set; }
        public string? ContabDimensaoCCCredito { get; set; }
        public string? ContabValorCCDebito { get; set; }
        public string? ContabValorCCCredito { get; set; }
        public int? PocalTipoDocumento { get; set; }
        public int? PocalGuiaFatAutartica { get; set; }
        public int? PocalCodigoServico { get; set; }
        public int? PocalTipoDocPocalEmitido { get; set; }
        public int? PocalTipoDocPocalCobrado { get; set; }
        public string? ReportPersonalizado { get; set; }
    }

    public class UpdateTipoDocumentoValidator : AbstractValidator<UpdateTipoDocumentoRequest>
    {
        public UpdateTipoDocumentoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 50 caracteres.");
            _ = RuleFor(x => x.Abreviatura)
                .NotEmpty()
                .MaximumLength(5)
                .WithMessage("Abreviatura é obrigatória e deve ter no máximo 5 caracteres.");
            _ = RuleFor(x => x.NumeroSerie)
                .NotEmpty()
                .MaximumLength(14)
                .WithMessage("NumeroSerie é obrigatório e deve ter no máximo 14 caracteres.");
        }
    }
}
