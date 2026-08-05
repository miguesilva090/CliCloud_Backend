using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.DTOs;

public class UnidadesTempoTecnicoResponse : IDto
{
    public Guid TecnicoId { get; set; }
    public int MaxTratamentos { get; set; }
    public IReadOnlyList<int> UnidadesTempo { get; set; } = [];
}