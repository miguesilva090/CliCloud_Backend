using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.DTOs
{
    public class UpdateMotivosDesmarcacaoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateMotivosDesmarcacaoValidator : AbstractValidator<UpdateMotivosDesmarcacaoRequest>
    {
        public UpdateMotivosDesmarcacaoValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty()
                .MaximumLength(100)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}

