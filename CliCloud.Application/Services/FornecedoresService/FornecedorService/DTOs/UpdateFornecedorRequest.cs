using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.FornecedoresService.FornecedorService.DTOs
{
    public class UpdateFornecedorRequest : IDto
    {
        // Campos da entidade base Entidade
        public required string Nome { get; set; }
        public required int TipoEntidadeId { get; set; }
        public string? Email { get; set; }
        public required string NumeroContribuinte { get; set; }
        public required string RuaId { get; set; }
        public required string CodigoPostalId { get; set; }
        public required string FreguesiaId { get; set; }
        public required string ConcelhoId { get; set; }
        public required string DistritoId { get; set; }
        public required string PaisId { get; set; }
        public required string NumeroPorta { get; set; }
        public required string AndarRua { get; set; }
        public string? Observacoes { get; set; }
        public required int Status { get; set; }
        public string? UrlFoto { get; set; }
        public IEnumerable<UpsertEntidadeContactoItemRequest>? EntidadeContactos { get; set; }

        // Campos específicos de Fornecedor
        public string? InstituicaoFinanceiraId { get; set; }
        public string? NumeroConta { get; set; }
        public decimal? Plafond { get; set; }
        public Guid? CondicaoPagamentoId { get; set; }
        public decimal? Desconto { get; set; }
        public Moeda? Moeda { get; set; }
        public decimal? TotalDebito { get; set; }
        public OrigemFornecedor? Origem { get; set; }
        public TipoFornecedor? TipoFornecedor { get; set; }
        public Guid? ModoPagamentoId { get; set; }
        public string? NumeroNib { get; set; }
        public int? Aprovado { get; set; }
        public DateOnly? DataAprovacao { get; set; }
        public string? EnderecoWeb { get; set; }
        public int? DiasPrevEntrega { get; set; }
        public int? DiasEfectiEntrega { get; set; }
    }

    public class UpdateFornecedorValidator : AbstractValidator<UpdateFornecedorRequest>
    {
        public UpdateFornecedorValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.TipoEntidadeId)
              .NotEmpty()
              .Equal(8) // Fornecedor = 8
              .WithMessage("TipoEntidadeId deve ser 8 (Fornecedor).");
            _ = RuleFor(x => x.Email)
              .EmailAddress()
              .When(x => !string.IsNullOrWhiteSpace(x.Email));
            _ = RuleFor(x => x.NumeroContribuinte).NotEmpty();
            _ = RuleFor(x => x.RuaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("RuaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.CodigoPostalId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("CodigoPostalId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.FreguesiaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("FreguesiaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.ConcelhoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("ConcelhoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.DistritoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("DistritoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.PaisId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("PaisId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.NumeroPorta).NotEmpty();
            _ = RuleFor(x => x.AndarRua).NotEmpty();
            _ = RuleFor(x => x.Status).NotEmpty().InclusiveBetween(1, 3).WithMessage("Status deve ser um valor válido e não estar vazio.");
            _ = RuleFor(x => x.UrlFoto).Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("UrlFoto deve ser uma URL válida.");
            _ = RuleFor(x => x.EntidadeContactos).NotEmpty().WithMessage("EntidadeContactos deve ser um array não vazio.");
            _ = RuleForEach(x => x.EntidadeContactos).SetValidator(new UpsertEntidadeContactoItemValidator()).When(x => x.EntidadeContactos != null);
            _ = RuleFor(x => x.InstituicaoFinanceiraId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("InstituicaoFinanceiraId deve ser um GUID válido ou vazio.");
            _ = RuleFor(x => x.EnderecoWeb).Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("EnderecoWeb deve ser uma URL válida.");
        }
    }
}
