#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Documentos;

/// <summary>
/// Liga um documento fiscal à origem clínica (equivalente legado <c>Faturacao.FaturaAdmissao</c>).
/// </summary>
[Table("DocumentoOrigemClinica", Schema = "Documentos")]
public class DocumentoOrigemClinica : AuditableEntityWithSoftDelete
{
  public Guid DocumentoId { get; set; }
  public Documento? Documento { get; set; }

  public ModuloOrigemDocumento ModuloOrigem { get; set; }

  public Guid? AdmissaoId { get; set; }
  public Admissao? Admissao { get; set; }

  public Guid? ConsultaId { get; set; }
  public Consulta? Consulta { get; set; }

  /// <summary>Legado: NotaLiquid.</summary>
  public int? NotaLiquidacao { get; set; }

  /// <summary>Legado: Filtro (contexto de listagem/origem).</summary>
  public int? FiltroOrigem { get; set; }
}
