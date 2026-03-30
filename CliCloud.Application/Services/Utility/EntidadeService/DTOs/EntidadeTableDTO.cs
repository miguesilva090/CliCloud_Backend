using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.EntidadeService.DTOs
{
  public class EntidadeTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public int TipoEntidadeId { get; set; }
    public string? Email { get; set; }
    public string? NumeroContribuinte { get; set; }
    public Guid? RuaId { get; set; }
    public EntidadeTableRuaDTO? Rua { get; set; }
    public Guid? CodigoPostalId { get; set; }
    public EntidadeTableCodigoPostalDTO? CodigoPostal { get; set; }
    public Guid? FreguesiaId { get; set; }
    public EntidadeTableFreguesiaDTO? Freguesia { get; set; }
    public Guid? ConcelhoId { get; set; }
    public EntidadeTableConcelhoDTO? Concelho { get; set; }
    public Guid? DistritoId { get; set; }
    public EntidadeTableDistritoDTO? Distrito { get; set; }
    public Guid? PaisId { get; set; }
    public EntidadeTablePaisDTO? Pais { get; set; }
    public string? NumeroPorta { get; set; }
    public string? AndarRua { get; set; }
    public int? Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public int ContactoCount { get; set; }
  }

  public class EntidadeTableCodigoPostalDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Codigo { get; set; }
    public string? Localidade { get; set; }
  }

  public class EntidadeTableRuaDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid FreguesiaId { get; set; }
    public EntidadeTableFreguesiaDTO? Freguesia { get; set; }
  }

  public class EntidadeTableFreguesiaDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid ConcelhoId { get; set; }
    public EntidadeTableConcelhoDTO? Concelho { get; set; }
  }

  public class EntidadeTableConcelhoDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid DistritoId { get; set; }
    public EntidadeTableDistritoDTO? Distrito { get; set; }
  }

  public class EntidadeTableDistritoDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public Guid PaisId { get; set; }
    public EntidadeTablePaisDTO? Pais { get; set; }
  }

  public class EntidadeTablePaisDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? Codigo { get; set; }
  }
}