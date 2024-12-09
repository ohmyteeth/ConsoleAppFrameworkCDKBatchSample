using System.Text;

namespace Stack.Extensions;

static class StringExtensions {
    internal static string ToKebabCase(this string name) {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < name.Length; i++) {
            if (!char.IsUpper(name[i])) {
                stringBuilder.Append(name[i]);
                continue;
            }
            if (i == 0 || i == name.Length - 1 || char.IsUpper(name[i + 1])) {
                stringBuilder.Append(char.ToLowerInvariant(name[i]));
                continue;
            }
            stringBuilder.Append('-');
            stringBuilder.Append(char.ToLowerInvariant(name[i]));
        }

        return stringBuilder.ToString();
    }
}