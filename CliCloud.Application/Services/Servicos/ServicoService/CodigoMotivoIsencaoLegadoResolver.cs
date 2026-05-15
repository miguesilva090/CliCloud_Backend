using System.Text.RegularExpressions;

namespace CliCloud.Application.Services.Servicos.ServicoService
{
  /// <summary>
  /// Converte o código textual de <see cref="CliCloud.Domain.Entities.TaxasIva.MotivoIsencao"/> para o inteiro legado
  /// usado em <see cref="CliCloud.Domain.Entities.Servicos.Servico.CodigoMotivoIsencao"/> (int32), alinhado ao front.
  /// </summary>
  public static class CodigoMotivoIsencaoLegadoResolver
  {
    public const int Int32Max = 2147483647;

    public static int? FromMotivoCodigo(string? codigoRaw)
    {
      if (string.IsNullOrWhiteSpace(codigoRaw)) return null;

      var t = codigoRaw.Trim();

      var mExact = Regex.Match(t, @"^M(\d+)$", RegexOptions.IgnoreCase);
      if (mExact.Success && int.TryParse(mExact.Groups[1].Value, out var n1))
      {
        if (n1 >= 0 && n1 <= Int32Max) return n1;
      }

      var mEmbed = Regex.Match(t, @"M(\d+)", RegexOptions.IgnoreCase);
      if (mEmbed.Success && int.TryParse(mEmbed.Groups[1].Value, out var n2))
      {
        if (n2 >= 0 && n2 <= Int32Max) return n2;
      }

      var motTs = Regex.Match(t, @"^MOT-(\d+)$", RegexOptions.IgnoreCase);
      if (motTs.Success && int.TryParse(motTs.Groups[1].Value, out var n3))
      {
        if (n3 < 0) return null;
        if (n3 > Int32Max) n3 = n3 % Int32Max;
        if (n3 == 0) n3 = 1;
        return n3;
      }

      foreach (Match m in Regex.Matches(t, @"\d+"))
      {
        if (int.TryParse(m.Value, out var n4) && n4 >= 0 && n4 <= Int32Max) return n4;
      }

      return null;
    }
  }
}
