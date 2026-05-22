using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class SalaDisponivelDTO : IDto 
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int NumeroSala { get; set; }
    public Guid ClinicaId { get; set; }
    public bool Ativa { get; set; }
    public bool Disponivel { get; set; }
}