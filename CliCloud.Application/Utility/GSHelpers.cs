using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Utility
{
  public static partial class GSHelpers // helper utility methods
  {
    private static readonly Random random = new();

    public static bool BeValidGuid(string value)
    {
      return Guid.TryParse(value, out Guid result) && result != Guid.Empty;
    }

    public static string GenerateHex(int digits) // hex code generator
    {
      byte[] buffer = new byte[digits / 2];
      random.NextBytes(buffer);
      string result = string.Concat(
        buffer.Select(x => x.ToString("X2", CultureInfo.InvariantCulture)).ToArray()
      );
      return digits % 2 == 0
        ? result
        : result + random.Next(16).ToString("X", CultureInfo.InvariantCulture);
    }

    public static string GetEnumDescription(this Enum value) // retrieve enum descriptions
    {
      FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());
      if (fieldInfo == null)
      {
        return value.ToString();
      }

      DescriptionAttribute[] attributes = (DescriptionAttribute[])
        fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

      return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex(); // remove whitespace from strings

    public static string ReplaceWhitespace(this string input, string replacement)
    {
      return WhitespaceRegex().Replace(input, replacement);
    }

    public static string ToUrlSlug(string value) // generate url slug from string
    {
      // first to lower case
      value = value.ToLowerInvariant();

      // remove all accents
      value = RemoveAccents(value);

      // replace spaces
      value = SpacesRegex().Replace(value, "-");

      // remove invalid chars
      value = InvalidCharsRegex().Replace(value, "");

      // trim dashes from end
      value = value.Trim('-', '_');

      // replace double occurences of - or _
      value = DoubleDashUnderscoreRegex().Replace(value, "$1");

      return value;
    }

    [GeneratedRegex(@"\s", RegexOptions.Compiled)]
    private static partial Regex SpacesRegex();

    [GeneratedRegex(@"[^a-z0-9\s-_]", RegexOptions.Compiled)]
    private static partial Regex InvalidCharsRegex();

    [GeneratedRegex(@"([-_]){2,}", RegexOptions.Compiled)]
    private static partial Regex DoubleDashUnderscoreRegex();

    public static string RemoveAccents(string text) // remove accents from string characters
    {
      string normalizedString = text.Normalize(NormalizationForm.FormD);
      StringBuilder stringBuilder = new();

      foreach (char c in normalizedString)
      {
        UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
        if (unicodeCategory != UnicodeCategory.NonSpacingMark)
        {
          _ = stringBuilder.Append(c);
        }
      }
      return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string GenerateOrderByString(PaginationFilter filter)
    {
      // translate a dynamic TanstackColumnOrder List into a string format readable by ardalis specification OrderBy
      // string format example: ('Name,-Supplier,Property.Name,Price') -prefix denotes Descending
      string sortingString = "";
      int numberOfColumns = filter.Sorting.Count;

      int count = 1;
      foreach (TanstackColumnOrder sortColumn in filter.Sorting)
      {
        if (sortColumn.Desc) // prepend a minus if order equals descending
        {
          sortingString += "-" + sortColumn.Id;
        }
        else
        {
          sortingString += sortColumn.Id;
        }

        if (count != numberOfColumns) // append comma if not last in series
        {
          sortingString += ",";
        }
        count++;
      }
      return sortingString;
    }

    public static string GetSpecificChars(string input, int[] positions)
    {
      // Ensure that the input string is long enough to have the required positions
      foreach (int pos in positions)
      {
        if (pos < 1 || pos > input.Length)
        {
          throw new ArgumentException($"Position {pos} is out of bounds.");
        }
      }

      // Retrieve the characters from the specified positions (adjust for zero-based indexing)
      StringBuilder result = new();
      foreach (int pos in positions)
      {
        _ = result.Append(input[pos - 1]); // Convert 1-based index to 0-based
      }

      return result.ToString();
    }
  }
}
