using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Ardalis.Specification;

namespace CliCloud.Application.Common.Specification
{
  // extension to Ardalis.Specification
  // allows OrderBy method to accept a string list of columns for sort ordering, for example: ('Name,-Supplier,Property.Name,Price') -prefix denotes Descending
  // JQuery Datatables and Tanstack table are components that send dynamic column ordering

  public static class ArdalisSpecificationExtensions
  {
    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
      this ISpecificationBuilder<T> specificationBuilder,
      string orderByFields
    )
    {
      Dictionary<string, OrderTypeEnum>? fields = ParseOrderBy(orderByFields);
      if (fields != null)
      {
        foreach (KeyValuePair<string, OrderTypeEnum> field in fields)
        {
          if (string.IsNullOrEmpty(field.Key))
          {
            continue; // Skip empty keys
          }

          Type targetType = typeof(T);
          _ =
            FindNestedProperty(targetType, field.Key.ToLower(CultureInfo.InvariantCulture))
            ?? throw new ArgumentException(
              $"Property '{field.Key}' not found on type {typeof(T).Name}",
              nameof(orderByFields)
            );

          ParameterExpression paramExpr = Expression.Parameter(typeof(T));

          Expression propertyExpr = paramExpr;
          foreach (string member in field.Key.Split('.'))
          {
            propertyExpr = Expression.PropertyOrField(propertyExpr, member);
          }

          Expression<Func<T, object?>> keySelector = Expression.Lambda<Func<T, object?>>(
            Expression.Convert(propertyExpr, typeof(object)),
            paramExpr
          );

          ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions).Add(
            new OrderExpressionInfo<T>(keySelector, field.Value)
          );
        }
      }
      OrderedSpecificationBuilder<T> orderedSpecificationBuilder = new(
        specificationBuilder.Specification
      );

      return orderedSpecificationBuilder;
    }

    // helper method for cases where the column property is nested, for example 'Supplier.Name'
    public static PropertyInfo? FindNestedProperty(Type type, string propertyName)
    {
      string[] propertyNames = propertyName.Split('.'); // Split the property name by dot to handle nesting

      Type currentType = type;
      PropertyInfo? property = null;

      foreach (string name in propertyNames)
      {
        PropertyInfo? nestedProperty = currentType
          .GetProperties()
          .FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

        if (nestedProperty == null)
        {
          return null; // Property not found at this level
        }

        currentType = nestedProperty.PropertyType;
        property = nestedProperty;
      }

      return property;
    }

    // helper method to parse the input string and turn it into something that Ardalis.Specification understands - a list of column names with their sort order
    private static Dictionary<string, OrderTypeEnum>? ParseOrderBy(string orderByFields)
    {
      if (orderByFields is null)
      {
        return null;
      }
      Dictionary<string, OrderTypeEnum> result = [];
      string[] fields = orderByFields.Split(',');
      for (int index = 0; index < fields.Length; index++)
      {
        string field = fields[index];
        OrderTypeEnum orderBy = OrderTypeEnum.OrderBy;
        if (field.StartsWith('-'))
        {
          orderBy = OrderTypeEnum.OrderByDescending;
        }
        if (index > 0)
        {
          orderBy = OrderTypeEnum.ThenBy;
          if (field.StartsWith('-'))
          {
            orderBy = OrderTypeEnum.ThenByDescending;
          }
        }
        if (field.StartsWith('-'))
        {
          field = field[1..];
        }
        result.Add(field, orderBy);
      }
      return result;
    }
  }
}
