using FluentValidation;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;

public class UpdateListaEsperaRequest : CreateListaEsperaRequest 
{
}

public class UpdateListaEsperaValidator : AbstractValidator<UpdateListaEsperaRequest>
{
    public UpdateListaEsperaValidator()
    {
        _ = RuleFor(x => x.UtenteId).NotEmpty();
        _ = RuleFor(x => x.EspecialidadeId).NotEmpty();
        _ = RuleFor(x => x.Data).NotEmpty();
        _ = RuleFor(x => x.Credencial).MaximumLength(100);
        _ = RuleFor(x => x.Obs).MaximumLength(4000);
        _ = RuleFor(x => x.HoraFim)
            .Must((dto, fim) => !fim.HasValue || !dto.HoraInicio.HasValue || fim > dto.HoraInicio)
            .WithMessage("A Hora de Fim deve ser superior à Hora de Início");
    }
}