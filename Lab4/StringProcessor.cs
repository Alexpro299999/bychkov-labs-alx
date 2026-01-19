using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public static class StringProcessor
    {
        public static string DuplicateSpecialWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            var parts = text.Split(' ');
            var result = new List<string>();

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part)) continue;

                if (char.ToLower(part[0]) == char.ToLower(part[part.Length - 1]))
                {
                    result.Add(part);
                    result.Add(part);
                }
                else
                {
                    result.Add(part);
                }
            }

            return string.Join(" ", result);
        }

        public static string ReverseWordsInLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return line;

            var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Array.Reverse(words);
            return string.Join(" ", words);
        }
    }
}