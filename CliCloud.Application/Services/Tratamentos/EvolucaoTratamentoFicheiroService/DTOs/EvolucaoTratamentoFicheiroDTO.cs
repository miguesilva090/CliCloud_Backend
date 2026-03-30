using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService.DTOs
{
  public class EvolucaoTratamentoFicheiroDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid EvolucaoTratamentoId { get; set; }

    public string Titulo { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string StoragePath { get; set; } = null!;
    public string? ContentType { get; set; }
    public long? TamanhoBytes { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}

