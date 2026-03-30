using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs
{
    public class CreateEntidadeContactoRequest : CreateEntidadeContactoItemRequest
    {
        public required string EntidadeId { get; set; }
    }

    public class CreateEntidadeContactoValidator : AbstractValidator<CreateEntidadeContactoRequest>
    {
        public CreateEntidadeContactoValidator()
        {
            Include(new CreateEntidadeContactoItemValidator());
            _ = RuleFor(x => x.EntidadeId).NotEmpty();
        }
    }
}
