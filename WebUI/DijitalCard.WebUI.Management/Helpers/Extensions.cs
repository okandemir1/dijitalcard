using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DijitalCard.WebUI.Management.Helpers
{
    public static class Extensions
    {
        public static string ToSlug(this string value)
        {
            if (value == null)
                value = "";

            string str = value.ClearPunctuation().ToLowerInvariant();

            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s+", "-").Trim();

            str = str.Replace("ı", "i")
                .Replace("ğ", "g")
                .Replace("ö", "o")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("$", "s")
                .Replace("ç", "c");

            return str;
        }

        private static string ClearPunctuation(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var exludeChars = new List<char>() { '-' };
            var list = new List<Char>();
            foreach (char c in value)
            {
                if (Char.IsPunctuation(c) && !exludeChars.Contains(c))
                    continue;

                list.Add(c);
            }

            value = string.Concat(list.ToArray());

            return value;
        }
    }
}
