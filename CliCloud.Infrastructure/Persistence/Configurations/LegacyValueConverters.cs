using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CliCloud.Infrastructure.Persistence.Configurations;

/// <summary>
/// Conversores para colunas legadas SQL Server (bit) mapeadas como int/int? no modelo.
/// </summary>
public static class LegacyValueConverters
{
  public static readonly ValueConverter<int?, bool?> NullableIntFromBool = new(
    v => v.HasValue ? v.Value != 0 : null,
    v => v.HasValue ? (v.Value ? 1 : 0) : null);

  public static readonly ValueConverter<int, bool> IntFromBool = new(
    v => v != 0,
    v => v ? 1 : 0);
}
