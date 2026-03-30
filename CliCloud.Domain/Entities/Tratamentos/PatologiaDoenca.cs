#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Doencas;

namespace CliCloud.Domain.Entities.Tratamentos
{
  /// <summary>
  /// Associação N-N entre Patologia e Doença (ICD-11).
  /// No legado as doenças estão em texto (TRATPRED.doencas); esta tabela permite escolher doenças da BD.
  /// Chave primária composta (PatologiaId, DoencaId) configurada em PatologiaDoencaConfiguration.
  /// </summary>
  [Table("PatologiaDoenca", Schema = "Tratamentos")]
  public class PatologiaDoenca
  {
    public Guid PatologiaId { get; set; }
    public Patologia Patologia { get; set; } = null!;

    public Guid DoencaId { get; set; }
    public Doenca Doenca { get; set; } = null!;
  }
}
