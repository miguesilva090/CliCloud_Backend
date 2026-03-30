using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Doencas.DoencaService.DTOs
{
    /// <summary>Não utilizado – doenças são importadas via CliCloud.ICDImport da API WHO.</summary>
    public class CreateDoencaRequest : IDto
    {
        public string Title { get; set; } = string.Empty;
    }

    public class CreateDoencaValidator : AbstractValidator<CreateDoencaRequest>
    {
        public CreateDoencaValidator()
        {
            _ = RuleFor(x => x.Title).NotEmpty();
        }
    }
}
