#nullable enable

namespace CliCloud.Domain.Enums;

/// <summary>
/// Módulo de origem do documento fiscal (equivalente legado <c>TFaturaOrigem</c>).
/// </summary>
public enum ModuloOrigemDocumento
{
  Faturacao = 0,
  Tratamentos = 1,
  Consultas = 2,
  Exames = 3,
  CredenciaisSns = 4,
  HistorialUtente = 5,
  Modalidades = 6,
}
