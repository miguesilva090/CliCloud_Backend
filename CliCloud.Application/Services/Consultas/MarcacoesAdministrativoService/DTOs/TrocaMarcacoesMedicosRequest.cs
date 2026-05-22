using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class TrocaMarcacoesMedicosRequest : IDto
{
    public Guid MedicoOrigemId { get; set; }
    public Guid MedicoDestinoId { get; set; }
    public DateTime DataOrigem { get; set; }
    public DateTime DataDestino { get; set; }
}

public class TrocaMarcacoesMedicosRequestValidator : AbstractValidator<TrocaMarcacoesMedicosRequest>
{
    public TrocaMarcacoesMedicosRequestValidator()
    {
        _ = RuleFor(x => x.MedicoOrigemId).NotEmpty();
        _ = RuleFor(x => x.MedicoDestinoId).NotEmpty();
        _ = RuleFor(x => x.DataOrigem).NotEmpty();
        _ = RuleFor(x => x.DataDestino).NotEmpty();

        _ = RuleFor(x => x)
            .Must(x => x.MedicoOrigemId != x.MedicoDestinoId)
            .WithMessage("O médico de origem e o médico de destino devem ser diferentes");
    }
}