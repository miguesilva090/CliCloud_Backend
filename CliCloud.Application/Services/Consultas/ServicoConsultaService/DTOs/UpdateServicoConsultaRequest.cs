using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs
{
  public class UpdateServicoConsultaRequest : IDto
  {
    public required string ConsultaId { get; set; }
    public string? ServicoId { get; set; }
    public decimal? ValorServico { get; set; }

    public string? CodigoArtigo { get; set; }
    public string? NomeArtigo { get; set; }
    public decimal? ValorArtigo { get; set; }
    public decimal? Quantidade { get; set; }

    public decimal? MargemMed { get; set; }
    public decimal? MargemIns { get; set; }
    public decimal? RecMed { get; set; }
    public decimal? RecInst { get; set; }

    public decimal? DescInst { get; set; }
    public decimal? DescCli { get; set; }
    public decimal? ValorDesc { get; set; }

    public int? Ordem { get; set; }
    public string? Dente { get; set; }
    public string? ExameId { get; set; }
    public int Linha { get; set; }
    public string? NCheque { get; set; }
    public int? Electrocardiograma { get; set; }
    public decimal? ValorUt { get; set; }
  }

  public class UpdateServicoConsultaValidator : AbstractValidator<UpdateServicoConsultaRequest>
  {
    public UpdateServicoConsultaValidator()
    {
      _ = RuleFor(x => x.ConsultaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("ConsultaId inválido.");
      _ = RuleFor(x => x.ServicoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("ServicoId inválido.");
      _ = RuleFor(x => x.ExameId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("ExameId inválido.");
      _ = RuleFor(x => x.CodigoArtigo).MaximumLength(50);
      _ = RuleFor(x => x.NomeArtigo).MaximumLength(250);
      _ = RuleFor(x => x.Dente).MaximumLength(20);
      _ = RuleFor(x => x.NCheque).MaximumLength(50);
    }
  }
}

