using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.GrauAlergiaService.DTOs
{
    public class UpdateGrauAlergiaRequest : IDto
    {
        public string Descricao { get; set; } = string.Empty;
    }

    public class UpdateGrauAlergiaValidator : AbstractValidator<UpdateGrauAlergiaRequest>
    {
        public UpdateGrauAlergiaValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty();
        }
    }
}

