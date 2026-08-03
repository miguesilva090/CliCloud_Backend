using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.DTOs
{
  public class UpdateServicoTratamentoRequest : IDto
  {
    public required string TratamentoId { get; set; }
    public string? ServicoId { get; set; }
    public string? Duracao { get; set; }
    public int? IDuraca { get; set; }
    public int? Ordem { get; set; }
    public int? UsaFisioter { get; set; }
    public int? UsaAuxiliar { get; set; }
    public int? UsaOutro { get; set; }
    public decimal? Preco { get; set; }
    public decimal? DescInst { get; set; }
    public decimal? ValorDesc { get; set; }
    public decimal? ValorUt { get; set; }
    public string? Obs { get; set; }
    public string? SessaoTratamentoId { get; set; }
  }

  public class UpdateServicoTratamentoValidator : AbstractValidator<UpdateServicoTratamentoRequest>
  {
    public UpdateServicoTratamentoValidator()
    {
      _ = RuleFor(x => x.TratamentoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("TratamentoId inválido.");
      _ = RuleFor(x => x.ServicoId)
        .NotEmpty().WithMessage("Seleccione o serviço.")
        .Must(id => !string.IsNullOrWhiteSpace(id) && GSHelpers.BeValidGuid(id!))
        .WithMessage("ServicoId inválido.");
      _ = RuleFor(x => x.SessaoTratamentoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("SessaoTratamentoId inválido.");
      _ = RuleFor(x => x.Duracao).MaximumLength(50);
      _ = RuleFor(x => x.Obs).MaximumLength(2000);
    }
  }
}

