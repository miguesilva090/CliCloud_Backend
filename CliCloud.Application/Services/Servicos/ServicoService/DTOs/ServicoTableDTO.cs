using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.ServicoService.DTOs
{
  public class ServicoTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public Guid TipoServicoId { get; set; }
    public string? TipoServicoDescricao { get; set; }
    public decimal? Preco { get; set; }
    public bool TratDentario { get; set; }
    public bool Inativo { get; set; }
    public DateTime CreatedOn { get; set; }
  }
}

