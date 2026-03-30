using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.DTOs
{
    public class UpdateGrupoViasAdministracaoLinhaRequest
    {
        public Guid? ViaId { get; set; }
        public string? Descricao { get; set; }
        public decimal? Quantidade { get; set; }
    }

    public class UpdateGrupoViasAdministracaoRequest : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public List<UpdateGrupoViasAdministracaoLinhaRequest>? Linhas { get; set; }
    }

    public class UpdateGrupoViasAdministracaoValidator : AbstractValidator<UpdateGrupoViasAdministracaoRequest>
    {
        public UpdateGrupoViasAdministracaoValidator()
        {
            _ = RuleFor(x => x.Id).NotEmpty();
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
