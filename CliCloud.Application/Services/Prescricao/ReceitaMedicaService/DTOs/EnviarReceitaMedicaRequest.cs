using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Prescricao.ReceitaMedicaService.DTOs;

public class EnviarReceitaMedicaRequest : IDto 
{
    public string TokenPrescritor { get; set; } = string.Empty;
}

public class EnviarReceitaMedicaRequestValidator : AbstractValidator<EnviarReceitaMedicaRequest>
{
    public EnviarReceitaMedicaRequestValidator()
    {
        RuleFor(x => x.TokenPrescritor)
            .NotEmpty()
            .WithMessage("Autenticação do prescritor é obrigatória");
    }
}