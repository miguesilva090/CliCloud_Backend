using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Alergias.AlergiaService.DTOs
{
    public class UpdateAlergiaRequest : IDto
    {
        public string? Descricao { get; set; }
    }

    public class UpdateAlergiaValidator : AbstractValidator<UpdateAlergiaRequest>
    {
        public UpdateAlergiaValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty();
        }
    }
}

