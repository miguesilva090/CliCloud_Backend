namespace CliCloud.Application.Services.Core.ClinicaService.DTOs
{
  public class ConfiguracaoTratamentosDTO
  {
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }

    public string? TipoSrvTratamentos { get; set; }
    public string? AreaPrestacaoDefeitoAreaZ { get; set; }
    public bool? ControlarAparelhos { get; set; }

    public int? Segundos { get; set; }
    public int? FaltasMax { get; set; }
    public int? FaltasConsecutivasMax { get; set; }
    public decimal? Taxamoderadora { get; set; }
    public bool? CredencialExternaAdse { get; set; }

    public int? TipoPagamento { get; set; }
    public bool? AvisoInqueritoSessoesDiarias { get; set; }
  }
}

