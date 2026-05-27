#nullable enable

namespace CliCloud.Domain.Enums;

/// <summary>
/// Estado do ciclo de vida do documento fiscal.
/// </summary>
public enum EstadoDocumento
{
  Rascunho = 0,
  Emitido = 1,
  Anulado = 2,
  Rectificado = 3,
}
