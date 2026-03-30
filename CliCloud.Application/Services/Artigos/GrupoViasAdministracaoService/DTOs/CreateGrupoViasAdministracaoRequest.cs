using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.DTOs
{
    public class CreateGrupoViasAdministracaoLinhaRequest
    {
        public Guid? ViaId { get; set; }
        public string? Descricao { get; set; }
        public decimal? Quantidade { get; set; }
    }

    public class CreateGrupoViasAdministracaoRequest : IDto
    {
        public string? Descricao { get; set; }
        public List<CreateGrupoViasAdministracaoLinhaRequest>? Linhas { get; set; }
    }

    public class CreateGrupoViasAdministracaoValidator : AbstractValidator<CreateGrupoViasAdministracaoRequest>
    {
        public CreateGrupoViasAdministracaoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
