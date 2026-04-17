using System.Text.RegularExpressions;

namespace CliCloud.Application.Services.Core.EmailService.Helpers;

public static class EmailTemplateRenderer
{
    public static string Render(string template, Dictionary<string, string> values)
    {
        var result = template ?? string.Empty;

        foreach(var kv in values.OrderByDescending(x => x.Key.Length))
        {
            var escaped = Regex.Escape(kv.Key);
            result = Regex.Replace(
                result,
                $@"@{escaped}@",
                kv.Value ?? string.Empty,
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
            );
        }

        return result;
    }
}