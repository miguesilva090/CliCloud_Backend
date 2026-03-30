using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Seguradoras.SeguradoraService.DTOs
{
  public class SeguradoraDTO : IDto
  {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Apolice { get; set; }
    public decimal? Avenca { get; set; }
    public DateOnly? DataInicioContrato { get; set; }
    public DateOnly? DataFimContrato { get; set; }
    public string? Abreviatura { get; set; }
    public Guid? BancoId { get; set; }
    public string? NumeroIdentificacaoBancaria { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}
