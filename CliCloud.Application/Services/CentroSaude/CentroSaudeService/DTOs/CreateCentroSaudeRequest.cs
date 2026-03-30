using System.Linq;
using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;

namespace CliCloud.Application.Services.CentroSaude.CentroSaudeService.DTOs
{
    public class CreateCentroSaudeRequest : IDto
    {
        // Campos da entidade base Entidade
        public required string Nome { get; set; }
        public required int TipoEntidadeId { get; set; }
        public string? Email { get; set; }
        public string? NumeroContribuinte { get; set; }
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

        // Campos específicos de CentroSaude
        public string? CodigoLocalCS { get; set; }
    }

    public class CreateCentroSaudeValidator : AbstractValidator<CreateCentroSaudeRequest>
    {
        public CreateCentroSaudeValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.TipoEntidadeId)
              .NotEmpty()
              .Equal(7) // CentroSaude = 7
              .WithMessage("TipoEntidadeId deve ser 7 (CentroSaude).");
            // Email no legado não é exigido; no novo apenas valida formato quando for fornecido.
            _ = RuleFor(x => x.Email)
              .EmailAddress()
              .When(x => !string.IsNullOrWhiteSpace(x.Email));
            _ = RuleFor(x => x.RuaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("RuaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.CodigoPostalId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("CodigoPostalId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.FreguesiaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("FreguesiaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.ConcelhoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("ConcelhoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.DistritoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("DistritoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.PaisId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("PaisId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.NumeroPorta);
            _ = RuleFor(x => x.AndarRua);
            _ = RuleFor(x => x.Observacoes);
            _ = RuleFor(x => x.Status).NotEmpty().InclusiveBetween(1, 3).WithMessage("Status deve ser um valor válido e não estar vazio.");
            _ = RuleFor(x => x.UrlFoto).Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("UrlFoto deve ser uma URL válida.");
            _ = RuleForEach(x => x.EntidadeContactos).SetValidator(new CreateEntidadeContactoItemValidator()).When(x => x.EntidadeContactos != null && x.EntidadeContactos.Any());
        }
    }
}
