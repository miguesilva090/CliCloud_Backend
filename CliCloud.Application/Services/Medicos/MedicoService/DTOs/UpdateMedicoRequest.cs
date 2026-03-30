using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Medicos.MedicoService.DTOs
{
    public class UpdateMedicoRequest : IDto
    {
        // Campos da entidade base Entidade
        public required string Nome { get; set; }
        public required int TipoEntidadeId { get; set; }
        public string? Email { get; set; }
        public string? NumeroContribuinte { get; set; }
        public string? RuaId { get; set; }
        public string? CodigoPostalId { get; set; }
        public string? FreguesiaId { get; set; }
        public string? ConcelhoId { get; set; }
        public string? DistritoId { get; set; }
        public string? PaisId { get; set; }
        public string? NumeroPorta { get; set; }
        public string? AndarRua { get; set; }
        public string? Observacoes { get; set; }
        public int? Status { get; set; }
        public string? UrlFoto { get; set; }
        public IEnumerable<UpsertEntidadeContactoItemRequest>? EntidadeContactos { get; set; }

        // Campos específicos de EntidadePessoa
        public DateOnly? DataNascimento { get; set; }
        public string? SexoId { get; set; }
        public string? EstadoCivilId { get; set; }
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

        // Campos específicos de Medico
        public bool Director { get; set; }
        public string? EspecialidadeId { get; set; }
        public double? Margem { get; set; }
        public string? LoginPRVR { get; set; }
        public bool ComunicacaoNif { get; set; }
        public bool? ComunicacaoNifAdse { get; set; }
        public string? GrupoFuncional { get; set; }
        public string? Letra { get; set; }
        public int CartaoCidadaoMedico { get; set; }
        public string? IdUtilizador { get; set; }
        public bool Globalbooking { get; set; }
    }

    public class UpdateMedicoValidator : AbstractValidator<UpdateMedicoRequest>
    {
        public UpdateMedicoValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.TipoEntidadeId)
              .NotEmpty()
              .InclusiveBetween(1, 11)
              .WithMessage("TipoEntidadeId deve ser um valor válido e não estar vazio.");
            _ = RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
            _ = RuleFor(x => x.RuaId).Must(id => string.IsNullOrWhiteSpace(id) || GSHelpers.BeValidGuid(id)).WithMessage("RuaId deve ser um GUID válido.");
            _ = RuleFor(x => x.CodigoPostalId).Must(id => string.IsNullOrWhiteSpace(id) || GSHelpers.BeValidGuid(id)).WithMessage("CodigoPostalId deve ser um GUID válido.");
            _ = RuleFor(x => x.FreguesiaId).Must(id => string.IsNullOrWhiteSpace(id) || GSHelpers.BeValidGuid(id)).WithMessage("FreguesiaId deve ser um GUID válido.");
            _ = RuleFor(x => x.ConcelhoId).Must(id => string.IsNullOrWhiteSpace(id) || GSHelpers.BeValidGuid(id)).WithMessage("ConcelhoId deve ser um GUID válido.");
            _ = RuleFor(x => x.DistritoId).Must(id => string.IsNullOrWhiteSpace(id) || GSHelpers.BeValidGuid(id)).WithMessage("DistritoId deve ser um GUID válido.");
            _ = RuleFor(x => x.PaisId).Must(id => string.IsNullOrWhiteSpace(id) || GSHelpers.BeValidGuid(id)).WithMessage("PaisId deve ser um GUID válido.");
            _ = RuleFor(x => x.Status).InclusiveBetween(1, 3).When(x => x.Status.HasValue).WithMessage("Status deve ser um valor válido.");
            _ = RuleFor(x => x.UrlFoto).Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("UrlFoto deve ser uma URL válida.");
            _ = RuleFor(x => x.EspecialidadeId).Must(id => string.IsNullOrWhiteSpace(id) || GSHelpers.BeValidGuid(id)).WithMessage("EspecialidadeId deve ser um GUID válido.");
            _ = RuleFor(x => x.IdUtilizador).Must(id => string.IsNullOrWhiteSpace(id) || GSHelpers.BeValidGuid(id)).WithMessage("IdUtilizador deve ser um GUID válido.");
            _ = RuleForEach(x => x.EntidadeContactos).SetValidator(new UpsertEntidadeContactoItemValidator()).When(x => x.EntidadeContactos != null);
        }
    }
}
