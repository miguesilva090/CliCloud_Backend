using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService.DTOs;

public class SeparadorVinculoDTO : IDto
{
    public Guid Id { get; set; }
    public Guid SeparadorId { get; set; }
    public TipoVinculoSeparador Tipo { get; set; }
    public Guid EntidadeId { get; set; }
    public DateTime CreatedOn { get; set; }
}
