using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ServicoSessaoService.DTOs
{
  public class ServicoSessaoDTO : IDto
  {
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public Guid SessaoTratamentoId { get; set; }
    public Guid? FisioterapeutaId { get; set; }
    public Guid? AuxiliarId { get; set; }
    public Guid? ServicoId { get; set; }

    public string? HoraInic { get; set; }
    public int? IHoraIni { get; set; }
    public string? HoraFim { get; set; }
    public int? IHoraFim { get; set; }

    public string? Duracao { get; set; }
    public int? IDuraca { get; set; }

    public int? Ordem { get; set; }
    public Guid? AparelhoId { get; set; }

    public decimal? Preco { get; set; }
    public decimal? DescInst { get; set; }
    public decimal? ValorDesc { get; set; }
    public decimal? ValorUt { get; set; }

    public string? Obs { get; set; }
  }
}

