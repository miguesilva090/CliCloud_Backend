using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Domain.Enums;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;

namespace CliCloud.Application.Services.Utility.EntidadePessoaService.DTOs
{
    public class CreateEntidadePessoaRequest : IDto
    {
        // Campos da entidade base Entidade
        public required string Nome { get; set; }
        public required int TipoEntidadeId { get; set; }
        public required string Email { get; set; }
        public required string NumeroContribuinte { get; set; }
        public required string RuaId { get; set; }
        public required string CodigoPostalId { get; set; }
        public required string FreguesiaId { get; set; }
        public required string ConcelhoId { get; set; }
        public required string DistritoId { get; set; }
        public required string PaisId { get; set; }
        public required string NumeroPorta { get; set; }
        public required string AndarRua { get; set; }
        public required string Observacoes { get; set; }
        public required int Status { get; set; }
        public string? UrlFoto { get; set; }
        public IEnumerable<CreateEntidadeContactoItemRequest>? EntidadeContactos { get; set; }

        // Campos específicos de EntidadePessoa
        public DateOnly? DataNascimento { get; set; }
        public string? SexoId { get; set; }
        public EstadoCivil? EstadoCivil { get; set; }
        public string? Nacionalidade { get; set; }
        public string? Naturalidade { get; set; }
        public string? NumeroCartaoIdentificacao { get; set; }
        public DateOnly? DataEmissaoCartaoIdentificacao { get; set; }
        public DateOnly? DataValidadeCartaoIdentificacao { get; set; }
        public string? Arquivo { get; set; }
        public string? Carteira { get; set; }
        public string? NomeUtilizador { get; set; }
        public string? UrlFotoAssinatura { get; set; }
        public string? NumeroIdentificacaoBancaria { get; set; }
    }

    public class CreateEntidadePessoaValidator : AbstractValidator<CreateEntidadePessoaRequest>
    {
        public CreateEntidadePessoaValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.TipoEntidadeId)
              .NotEmpty()
              .InclusiveBetween(1, 9)
              .WithMessage("TipoEntidadeId deve ser um valor válido e não estar vazio.");
            _ = RuleFor(x => x.Email).NotEmpty().EmailAddress();
            _ = RuleFor(x => x.NumeroContribuinte).NotEmpty();
            _ = RuleFor(x => x.RuaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("RuaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.CodigoPostalId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("CodigoPostalId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.FreguesiaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("FreguesiaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.ConcelhoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("ConcelhoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.DistritoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("DistritoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.PaisId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("PaisId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.NumeroPorta).NotEmpty();
            _ = RuleFor(x => x.AndarRua).NotEmpty();
            _ = RuleFor(x => x.Observacoes).NotEmpty();
            _ = RuleFor(x => x.Status).NotEmpty().InclusiveBetween(1, 3).WithMessage("Status deve ser um valor válido e não estar vazio.");
            _ = RuleFor(x => x.UrlFoto).Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("UrlFoto deve ser uma URL válida.");
            _ = RuleFor(x => x.EntidadeContactos).NotEmpty().WithMessage("EntidadeContactos deve ser um array não vazio.");
            _ = RuleForEach(x => x.EntidadeContactos).SetValidator(new CreateEntidadeContactoItemValidator()).When(x => x.EntidadeContactos != null);
        }
    }
}
