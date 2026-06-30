using FluentValidation;

namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService.DTOs;

public class DeleteCredenciaisSnsRequest
{
    public IList<int> Indices { get; set; } = [];
}

public sealed class DeleteCredenciaisSnsRequestValidator : AbstractValidator<DeleteCredenciaisSnsRequest>
{
    public DeleteCredenciaisSnsRequestValidator()
    {
        RuleFor(x => x.Indices)
            .NotEmpty()
            .WithMessage("Nenhum lote selecionado.");

        RuleForEach(x => x.Indices)
            .GreaterThan(0)
            .WithMessage("Índice de lote inválido.");
    }
}
