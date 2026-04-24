#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Doencas
{
  /// <summary>
  /// Entidade hierárquica do ICD-11 MMS (capítulos, blocos, categorias).
  /// Populada pelo CliCloud.ICDImport a partir da API WHO.
  /// Mapeada para a tabela Doencas.Doenca.
  /// </summary>
  [Table("Doenca", Schema = "Doencas")]
  public class Doenca : AuditableEntityWithSoftDelete
  {
    /// <summary>ID oficial WHO (extraído do @id ou URI).</summary>
    [Column(TypeName = "nvarchar(100)")]
    public string IcdId { get; set; } = string.Empty;

    /// <summary>Código ICD-11 (ex: 1A00). Vazio para chapter/block.</summary>
    [Column(TypeName = "nvarchar(20)")]
    public string? Code { get; set; }

    /// <summary>Título/descrição em português.</summary>
    [Column(TypeName = "nvarchar(500)")]
    public string Title { get; set; } = string.Empty;

    /// <summary>Tipo: chapter, block, category.</summary>
    [Column(TypeName = "nvarchar(20)")]
    public string ClassKind { get; set; } = string.Empty;

    /// <summary>Profundidade na árvore (1=capítulo, 2+=blocos/categorias).</summary>
    public int Level { get; set; }

    public Guid? ParentId { get; set; }
    public Doenca? Parent { get; set; }
    public ICollection<Doenca> Children { get; set; } = new List<Doenca>();
  }
}
