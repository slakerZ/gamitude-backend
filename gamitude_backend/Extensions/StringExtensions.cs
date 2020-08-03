using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace gamitude_backend.Extensions
{
    public static class StringExtensions
    {
        public static string camelToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
        public static string snakeToCamelCase(this string input)
        {
            input = input
                .Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);

            return Char.ToLower(input[0]).ToString()+input.Substring(1);
        }

    }
}