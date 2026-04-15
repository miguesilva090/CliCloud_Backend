namespace CliCloud.Application.Services.ProcessoClinico.SeparadorService.DTOs;

public class SeparadorFichaClinicaDTO
{
    public Guid SeparadorId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public string Origem { get; set; } = string.Empty;
    public Guid? FormularioId { get; set; }
}
