using System.Text.RegularExpressions;

namespace Kaigara.Extentions;

public static class StringExtensions
{
    private static readonly Regex SeparatorRegex;

    static StringExtensions()
    {
        const string pattern = @"
                (?<!^) # Not start
                (
                    # Digit, not preceded by another digit
                    (?<!\d)\d 
                    |
                    # Upper-case letter, followed by lower-case letter if
                    # preceded by another upper-case letter, e.g. 'G' in HTMLGuide
                    (?(?<=[A-Z])[A-Z](?=[a-z])|[A-Z])
                )";

        var options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled;

        SeparatorRegex = new Regex(pattern, options);
    }

    public static bool Contains(this String str, String value,
                                StringComparison comparisonType)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (!Enum.IsDefined(typeof(StringComparison), comparisonType))
        {
            throw new ArgumentException("comp is not a member of StringComparison",
                                        nameof(comparisonType));
        }

        return str.IndexOf(value, comparisonType) >= 0;
    }



    public static string SeparateWords(this string value, string separator = " ")
    {
        return SeparatorRegex.Replace(value, separator + "$1");
    }
}
