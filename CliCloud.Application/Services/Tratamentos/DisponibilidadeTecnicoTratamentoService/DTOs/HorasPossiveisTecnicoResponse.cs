using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.DTOs;

public class HorasPossiveisTecnicoResponse : IDto 
{
    public string Duracao { get; set; } = "00:15";
    public IReadOnlyList<string> Horas { get; set; } = [];
}