#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tratamentos
{
  [Table("EvolucaoTratamentoFicheiro", Schema = "Tratamentos")]
  public class EvolucaoTratamentoFicheiro : AuditableEntity
  {
    public Guid EvolucaoTratamentoId { get; set; }
    public EvolucaoTratamento EvolucaoTratamento { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    /// <summary>
    /// Nome original do ficheiro (para mostrar na UI).
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// Caminho ou chave de armazenamento (ex.: path em disco ou key em blob storage).
    /// </summary>
    public string StoragePath { get; set; } = null!;

    public string? ContentType { get; set; }
    public long? TamanhoBytes { get; set; }
  }
}

