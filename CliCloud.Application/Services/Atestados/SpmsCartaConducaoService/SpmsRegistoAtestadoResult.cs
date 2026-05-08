namespace CliCloud.Application.Services.Atestados.SpmsCartaConducaoService;

public class SpmsRegistoAtestadoResult
{
    public bool Success { get; set; }
    public bool IsTransientFailure { get; set; }
    public string? NumeroAtestadoMedico { get; set; }
    public string? Message { get; set; }
}