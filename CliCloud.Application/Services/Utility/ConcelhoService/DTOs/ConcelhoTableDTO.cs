using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.ConcelhoService.DTOs
{
  public class ConcelhoTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid? DistritoId { get; set; }
    public ConcelhoTableDistritoDTO? Distrito { get; set; }
    public DateTime CreatedOn { get; set; }
  }

  public class ConcelhoTableDistritoDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid? PaisId { get; set; }
    public ConcelhoTablePaisDTO? Pais { get; set; }
  }

  public class ConcelhoTablePaisDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Codigo { get; set; }
    public string? Nome { get; set; }
  }
}