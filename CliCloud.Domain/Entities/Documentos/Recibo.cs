#nullable enable

using System.ComponentModel.DataAnnotations.Schema;

namespace CliCloud.Domain.Entities.Documentos
{
  /// <summary>
  /// Recibo — documento de tipo recibo.
  /// Equivalente no legado: documento com tipo recibo (c_recibo ↔ n_doc).
  /// Herda toda a estrutura de Documento; sem campos adicionais.
  /// </summary>
  [Table("Recibo", Schema = "Documentos")]
  public class Recibo : Documento
  {
  }
}
