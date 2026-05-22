using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class AssociarSalaMarcacaoRequest : IDto
{
    public Guid SalaId { get; set; }
}

public class AssociarSalaMarcacaoRequestValidator : AbstractValidator<AssociarSalaMarcacaoRequest>
{
    public AssociarSalaMarcacaoRequestValidator()
    {
        _ = RuleFor(x => x.SalaId).NotEmpty();
    }
}