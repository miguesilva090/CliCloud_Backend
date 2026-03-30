using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.ModeloRelatorioAtestadoService.DTOs
{
    public class ModeloRelatorioAtestadoDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid? MedicoId { get; set; }
        public string Titulo { get; set; } = null!;
        public string TextoHtml { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }

    public class CreateModeloRelatorioAtestadoRequest : IDto
    {
        public string Titulo { get; set; } = null!;
        public string TextoHtml { get; set; } = null!;
        public Guid? MedicoId { get; set; }
    }

    public class UpdateModeloRelatorioAtestadoRequest : IDto
    {
        public string Titulo { get; set; } = null!;
        public string TextoHtml { get; set; } = null!;
    }

    public class CreateModeloRelatorioAtestadoValidator : AbstractValidator<CreateModeloRelatorioAtestadoRequest>
    {
        public CreateModeloRelatorioAtestadoValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().MaximumLength(200);
            RuleFor(x => x.TextoHtml).NotEmpty();
        }
    }

    public class UpdateModeloRelatorioAtestadoValidator : AbstractValidator<UpdateModeloRelatorioAtestadoRequest>
    {
        public UpdateModeloRelatorioAtestadoValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().MaximumLength(200);
            RuleFor(x => x.TextoHtml).NotEmpty();
        }
    }
}

