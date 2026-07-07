using FluentValidation;

namespace CliCloud.Application.Services.Utility.FundirUtentesService.DTOs;

public class FundirUtentesRequest
{
    public Guid UtenteOrigemId { get; set; }
    public Guid UtenteApagarId { get; set; }
}

public class FundirUtentesResponse
{
    public Guid UtenteOrigemId { get; set; }
    public Guid UtenteApagadoId { get; set; }
}

public class FundirUtentesValidator : AbstractValidator<FundirUtentesRequest>
{
    public FundirUtentesValidator()
    {
        RuleFor(x => x.UtenteOrigemId).NotEmpty();
        RuleFor(x => x.UtenteApagarId).NotEmpty();
        RuleFor(x => x)
            .Must(x => x.UtenteOrigemId != x.UtenteApagarId)
            .WithMessage("Está a inserir o mesmo Utente");
    }
}
