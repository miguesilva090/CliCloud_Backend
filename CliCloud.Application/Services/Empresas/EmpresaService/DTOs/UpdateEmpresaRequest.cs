using FluentValidation;
using System;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Empresas.EmpresaService.DTOs
{
    public class UpdateEmpresaRequest : IDto
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
        public string? Observacoes { get; set; }
        public required int Status { get; set; }
        public string? UrlFoto { get; set; }
        public IEnumerable<UpsertEntidadeContactoItemRequest>? EntidadeContactos { get; set; }

        // Campos específicos de Empresa
        public int? PrazoPagamento { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? DescontoUtente { get; set; }
        public CondicaoPagamento? CondicaoPagamento { get; set; }
        public TipoModoPagamento? TipoModoPagamento { get; set; }
        public string? BancoId { get; set; }
        public Guid? OrganismoId { get; set; }
        public string? NumeroIdentificacaoBancaria { get; set; }
        public string? Apolice { get; set; }
        public decimal? Avenca { get; set; }
        public DateOnly? DataInicioContrato { get; set; }
        public DateOnly? DataFimContrato { get; set; }
        public int? NumeroPagamentos { get; set; }
        public string? Categoria { get; set; }
        public string? Actividade { get; set; }
        public int? Cae { get; set; }
        public string? CodigoClinica { get; set; }
        public int? NumeroTrabalhadores { get; set; }
        public decimal? ValorTrabalhador { get; set; }
        public int? Rescindindo { get; set; }
        public string? Contacto { get; set; }
    }

    public class UpdateEmpresaValidator : AbstractValidator<UpdateEmpresaRequest>
    {
        public UpdateEmpresaValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.TipoEntidadeId)
              .NotEmpty()
              .Equal(9) // Empresa = 9
              .WithMessage("TipoEntidadeId deve ser 9 (Empresa).");
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
            _ = RuleFor(x => x.Status).NotEmpty().InclusiveBetween(1, 3).WithMessage("Status deve ser um valor válido e não estar vazio.");
            _ = RuleFor(x => x.UrlFoto).Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("UrlFoto deve ser uma URL válida.");
            _ = RuleFor(x => x.EntidadeContactos).NotEmpty().WithMessage("EntidadeContactos deve ser um array não vazio.");
            _ = RuleForEach(x => x.EntidadeContactos).SetValidator(new UpsertEntidadeContactoItemValidator()).When(x => x.EntidadeContactos != null);

            _ = RuleFor(x => x.BancoId)
                .Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id))
                .WithMessage("BancoId deve ser um GUID válido ou vazio.");

            _ = RuleFor(x => x.OrganismoId)
                .Must(id => !id.HasValue || id.Value != Guid.Empty);
        }
    }
}

