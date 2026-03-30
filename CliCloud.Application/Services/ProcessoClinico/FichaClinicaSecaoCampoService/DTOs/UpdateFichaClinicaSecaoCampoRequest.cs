using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.DTOs
{
    public class UpdateFichaClinicaSecaoCampoRequest : IDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string TipoCampo { get; set; } = "Texto";

        public int NumeroLinhas { get; set; } = 1;

        public int Ordem { get; set; }

        public bool Ativo { get; set; }
    }

    public class UpdateFichaClinicaSecaoCampoValidator : AbstractValidator<UpdateFichaClinicaSecaoCampoRequest>
    {
        public UpdateFichaClinicaSecaoCampoValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.TipoCampo).NotEmpty();
            _ = RuleFor(x => x.NumeroLinhas).GreaterThanOrEqualTo(1);
        }
    }
}

