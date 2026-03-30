using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.DTOs
{
  public class ServicoTratamentoDTO : IDto
  {
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public Guid TratamentoId { get; set; }
    public Guid? ServicoId { get; set; }
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
    public Guid? SessaoTratamentoId { get; set; }
  }
}

