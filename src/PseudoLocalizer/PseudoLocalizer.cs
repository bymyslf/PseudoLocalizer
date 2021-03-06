﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PseudoLocalizer
{
    public class PseudoLocalizer
    {
        private static readonly Dictionary<char, char> _replacements = new Dictionary<char, char>()
        {
            { '!', '\u00a1' },
            { '"', '\u2033' },
            { '#', '\u266f' },
            { '$', '\u20ac' },
            { '%', '\u2030' },
            { '&', '\u214b' },
            { '\'', '\u00b4' },
            { '*', '\u204e' },
            { '+', '\u207a' },
            { ',', '\u060c' },
            { '-', '\u2010' },
            { '.', '\u00b7' },
            { '/', '\u2044' },
            { '0', '\u24ea' },
            { '1', '\u2460' },
            { '2', '\u2461' },
            { '3', '\u2462' },
            { '4', '\u2463' },
            { '5', '\u2464' },
            { '6', '\u2465' },
            { '7', '\u2466' },
            { '8', '\u2467' },
            { '9', '\u2468' },
            { ':', '\u2236' },
            { ';', '\u204f' },
            { '<', '\u2264' },
            { '=', '\u2242' },
            { '>', '\u2265' },
            { '?', '\u00bf' },
            { '@', '\u055e' },
            { 'A', '\u00c5' },
            { 'B', '\u0181' },
            { 'C', '\u00c7' },
            { 'D', '\u00d0' },
            { 'E', '\u00c9' },
            { 'F', '\u0191' },
            { 'G', '\u011c' },
            { 'H', '\u0124' },
            { 'I', '\u00ce' },
            { 'J', '\u0134' },
            { 'K', '\u0136' },
            { 'L', '\u013b' },
            { 'M', '\u1e40' },
            { 'N', '\u00d1' },
            { 'O', '\u00d6' },
            { 'P', '\u00de' },
            { 'Q', '\u01ea' },
            { 'R', '\u0154' },
            { 'S', '\u0160' },
            { 'T', '\u0162' },
            { 'U', '\u00db' },
            { 'V', '\u1e7c' },
            { 'W', '\u0174' },
            { 'X', '\u1e8a' },
            { 'Y', '\u00dd' },
            { 'Z', '\u017d' },
            { '[', '\u2045' },
            { '\\', '\u2216' },
            { ']', '\u2046' },
            { '^', '\u02c4' },
            { '_', '\u203f' },
            { '`', '\u2035' },
            { 'a', '\u00e5' },
            { 'b', '\u0180' },
            { 'c', '\u00e7' },
            { 'd', '\u00f0' },
            { 'e', '\u00e9' },
            { 'f', '\u0192' },
            { 'g', '\u011d' },
            { 'h', '\u0125' },
            { 'i', '\u00ee' },
            { 'j', '\u0135' },
            { 'k', '\u0137' },
            { 'l', '\u013c' },
            { 'm', '\u0271' },
            { 'n', '\u00f1' },
            { 'o', '\u00f6' },
            { 'p', '\u00fe' },
            { 'q', '\u01eb' },
            { 'r', '\u0155' },
            { 's', '\u0161' },
            { 't', '\u0163' },
            { 'u', '\u00fb' },
            { 'v', '\u1e7d' },
            { 'w', '\u0175' },
            { 'x', '\u1e8b' },
            { 'y', '\u00fd' },
            { 'z', '\u017e' },
            { '|', '\u00a6' },
            { '~', '\u02de' },
        };

        private static readonly HashSet<char> _vowels = new HashSet<char>()
            { 'a', 'e', 'i', 'o', 'u', 'y', 'A', 'E', 'I', 'O', 'U', 'Y' };

        public static string PseudoLocalize(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return text;
            }

            var sb = new StringBuilder(text.Length * 2);

            sb.Append('[');

            for (var i = 0; i < text.Length; i++)
            {
                var ch = text[i];

                // Potential placeholder(e.g. "{0}" or "{0:dd/MM/yyyy}")
                if (ch == '{' && i < text.Length - 2)
                {
                    sb.Append(ch);

                    while (i < text.Length - 1 && text[++i] != '}')
                    {
                        sb.Append(text[i]);
                    }

                    sb.Append(text[i]);
                }
                // Potential HTML tag (e.g. "<a/>" or "<p></p>")
                else if (ch == '<' && i < text.Length - 2)
                {
                    var next = text[i + 1];
                    var indexBefore = i;

                    sb.Append(ch);

                    if (char.IsLetter(next) || next == '/')
                    {
                        while (i < text.Length - 1 && text[++i] != '>')
                        {
                            sb.Append(text[i]);
                        }
                    }

                    if (i != indexBefore)
                    {
                        sb.Append(text[i]);
                    }
                }
                else
                {
                    var transformed = Transform(ch);
                    sb.Append(transformed);

                    // Duplicate vowels to emulate ~30% longer text
                    if (IsVowel(ch))
                    {
                        sb.Append(transformed);
                    }
                }
            }

            sb.Append(']');

            return sb.ToString();

            bool IsVowel(char ch)
               => _vowels.Contains(ch);

            char Transform(char ch)
            {
                if (_replacements.TryGetValue(ch, out char result))
                {
                    return result;
                }

                return ch;
            }
        }
    }
}
