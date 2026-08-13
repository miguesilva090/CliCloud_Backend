using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Prescricao.MedicacaoCronicaService.DTOs
{
    public class CreateMedicacaoCronicaRequest : IDto 
    {
        public Guid UtenteId { get; set; }
        public string Cnpem { get; set; } = string.Empty;
        public string? EmbId { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public string? Dosagem { get; set; }
        public string? DescricaoEmbalagem { get; set; }
        public string? FormaFarmaceutica { get; set; }
        public string? PrincipioAtivo { get; set; }
        public string? Posologia { get; set; }
        public int TipoLinha { get; set; } = 1;
    }

    public class CreateMedicacaoCronicaRequestValidator : 
        AbstractValidator<CreateMedicacaoCronicaRequest>
    {
        public CreateMedicacaoCronicaRequestValidator()
        { 
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Cnpem).NotEmpty().MaximumLength(50);
            _ = RuleFor(x => x.EmbId).MaximumLength(50);
            _ = RuleFor(x => x.Designacao).NotEmpty().MaximumLength(500);
            _ = RuleFor(x => x.Dosagem).MaximumLength(254);
            _ = RuleFor(x => x.DescricaoEmbalagem).MaximumLength(500);
            _ = RuleFor(x => x.FormaFarmaceutica).MaximumLength(200);
            _ = RuleFor(x => x.PrincipioAtivo).MaximumLength(500);
            _ = RuleFor(x => x.Posologia).MaximumLength(1000);
            _ = RuleFor(x => x.TipoLinha).Must( t => t is 1 or 3)
                .WithMessage("Não é permitido adicionar a medicação crónica para este tipo de Prescrição");
        }
    }
}