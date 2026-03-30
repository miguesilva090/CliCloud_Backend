using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.DTOs
{
    public class UpdateAntecedentesPessoaisRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public Guid? DoencaId { get; set; }
        public string? NomeDoenca { get; set; }
        public int? Ano { get; set; }
        public int? Idade { get; set; }
        public DateTime? Data { get; set; }
    }

    public class UpdateAntecedentesPessoaisValidator : AbstractValidator<UpdateAntecedentesPessoaisRequest>
    {
        public UpdateAntecedentesPessoaisValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.DoencaId).NotEmpty();
            _ = RuleFor(x => x.NomeDoenca).NotEmpty();
            _ = RuleFor(x => x.Data).NotEmpty();
        }
    }
}

