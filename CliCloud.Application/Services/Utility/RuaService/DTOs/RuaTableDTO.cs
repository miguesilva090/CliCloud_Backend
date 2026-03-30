using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.RuaService.DTOs
{
  public class RuaTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid FreguesiaId { get; set; }
    public RuaTableFreguesiaDTO? Freguesia { get; set; }
    public Guid CodigoPostalId { get; set; }
    public RuaTableCodigoPostalDTO? CodigoPostal { get; set; }
    public DateTime CreatedOn { get; set; }
  }

  public class RuaTableFreguesiaDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid? ConcelhoId { get; set; }
    public RuaTableConcelhoDTO? Concelho { get; set; }
  }

  public class RuaTableConcelhoDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid? DistritoId { get; set; }
    public RuaTableDistritoDTO? Distrito { get; set; }
  }

  public class RuaTableDistritoDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid? PaisId { get; set; }
    public RuaTablePaisDTO? Pais { get; set; }
  }

  public class RuaTablePaisDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? Codigo { get; set; }
  }

  public class RuaTableCodigoPostalDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Codigo { get; set; }
    public string? Localidade { get; set; }
  }
}