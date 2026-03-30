using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Utility.EntidadeService.DTOs;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Application.Utility;
using CliCloud.Domain.Enums;


namespace CliCloud.Application.Services.Utility.EntidadeService.DTOs
{
    public class UpdateEntidadeRequest : IDto
    {
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
        public IEnumerable<UpsertEntidadeContactoItemRequest>? EntidadeContactos { get; set; }
    }

    public class UpdateEntidadeValidator : AbstractValidator<UpdateEntidadeRequest>
    {
      public UpdateEntidadeValidator()
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
            _ = RuleForEach(x => x.EntidadeContactos).SetValidator(new UpsertEntidadeContactoItemValidator()).When(x => x.EntidadeContactos != null);
        }

        private static bool BeValidTipoEntidadeId(int tipoEntidadeId)
        {
            return Enum.IsDefined(typeof(EntidadeTipo), tipoEntidadeId) && tipoEntidadeId > 0 && tipoEntidadeId <= 9;
        }

    }
}
