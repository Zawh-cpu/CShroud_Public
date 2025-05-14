using System.Globalization;
using System.Text;
using System.Text.Json;

namespace CShroudApp.Infrastructure.Data.Json.Policies;

/// <summary>
/// Converts property names from PascalCase or camelCase to snake_case.
/// </summary>
public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        var builder = new StringBuilder();
        var previousCategory = default(UnicodeCategory?);

        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];
            var currentCategory = char.GetUnicodeCategory(c);

            if (char.IsUpper(c))
            {
                // Avoid leading underscore
                if (i > 0 && previousCategory != UnicodeCategory.SpaceSeparator)
                {
                    builder.Append('_');
                }

                builder.Append(char.ToLowerInvariant(c));
            }
            else
            {
                builder.Append(c);
            }

            previousCategory = currentCategory;
        }

        return builder.ToString();
    }
}
