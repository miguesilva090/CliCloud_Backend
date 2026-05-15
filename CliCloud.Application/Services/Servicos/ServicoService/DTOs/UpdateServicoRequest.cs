using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Servicos.ServicoService.DTOs
{
  public class UpdateServicoRequest : IDto
  {
    public required string Designacao { get; set; }
    public required Guid TipoServicoId { get; set; }
    public decimal? Preco { get; set; }
    public string? Duracao { get; set; }
    public Guid? TaxaIvaId { get; set; }
    public string? EAN { get; set; }
    public Guid? TipoAparelhoId { get; set; }
    public bool TratDentario { get; set; }
    public Guid? MotivoIsencaoId { get; set; }
    public int? CodigoMotivoIsencao { get; set; }
    public bool Inativo { get; set; }
  }

  public class UpdateServicoValidator : AbstractValidator<UpdateServicoRequest>
  {
    public UpdateServicoValidator()
    {
      _ = RuleFor(x => x.Designacao).NotEmpty().MaximumLength(250);
      _ = RuleFor(x => x.TipoServicoId).NotEmpty();
      _ = RuleFor(x => x.Duracao).MaximumLength(50);
      _ = RuleFor(x => x.EAN).MaximumLength(50);
    }
  }
}

