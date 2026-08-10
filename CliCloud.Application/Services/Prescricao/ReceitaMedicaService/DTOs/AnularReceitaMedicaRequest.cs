using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Prescricao.ReceitaMedicaService.DTOs
{
    public class AnularReceitaMedicaRequest : IDto 
    {
        public string MotivoCodigo { get; set; } = string.Empty;
        public string MotivoDescricao { get; set; } = string.Empty;
    }

    public class AnularReceitaMedicaValidator : AbstractValidator<AnularReceitaMedicaRequest>
    {
        public AnularReceitaMedicaValidator()
        {
            RuleFor(x => x.MotivoCodigo).NotEmpty().WithMessage("Não existe motivo de anulação");
            RuleFor(x => x.MotivoDescricao).NotEmpty().MaximumLength(500);
        }
    }
}