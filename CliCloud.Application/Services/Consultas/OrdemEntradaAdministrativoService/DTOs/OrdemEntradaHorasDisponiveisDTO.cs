using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.DTOs;

public class OrdemEntradaHorasDisponiveisRequest : IDto
{
    public Guid MedicoId { get; set; }
    public Guid TipoConsultaId { get; set; }
    public DateTime Data { get; set; }
    public Guid? AdmissaoId { get; set; }
}

public class OrdemEntradaHorasDisponiveisDTO : IDto
{
    public bool HorarioFlexivel { get; set; }
    public TimeSpan? Intervalo { get; set; }
    public List<string> HorasPossiveis { get; set; } = [];
}
