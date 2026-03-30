using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CliCloud.Application.Common;

/// <summary>
/// Obtém o nome de apresentação de enums via [Display(Name = "...")].
/// Fonte de verdade: atributos no Domain.
/// </summary>
public static class EnumDisplayHelper
{
    public static string GetDisplayName(Enum? value)
    {
        if (value == null) return string.Empty;
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();
        var attr = field.GetCustomAttribute<DisplayAttribute>();
        return attr?.GetName() ?? value.ToString();
    }
}
