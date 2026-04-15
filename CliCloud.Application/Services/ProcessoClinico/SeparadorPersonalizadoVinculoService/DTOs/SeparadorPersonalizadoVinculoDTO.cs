using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService.DTOs;

public class SeparadorPersonalizadoVinculoDTO : IDto
{
    public Guid Id { get; set; }
    public Guid SeparadorPersonalizadoId { get; set; }
    public TipoVinculoSeparador Tipo { get; set; }
    public Guid EntidadeId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}
