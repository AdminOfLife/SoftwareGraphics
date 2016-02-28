using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericMathematics
{
    internal static class FormatParser
    {
        public static Tuple<char, int> FirstSpecifier(ref string format)
        {
            if (format == null)
                throw new ArgumentNullException(format);
            if (format.Length == 0)
                throw new ArgumentException(TextResources.FormatParser_FormatIsEmpty, "format");

            char specifier = format[0];
            if (specifier >= '0' && specifier <= '9')
                throw new FormatException();

            int i = 1;
            while (i < format.Length &&
                ((format[i] >= '0' && format[i] <= '9') || format[i] == '+' || format[i] == '-'))
            {
                i++;
            }

            int specifierValue = -1;
            if (i > 1)
            {
                if (!int.TryParse(format.Substring(1, i - 1), out specifierValue))
                    throw new FormatException();
            }

            format = format.Substring(i);

            return new Tuple<char, int>(specifier, specifierValue);
        }

        public static Tuple<char, int> FirstSpecifierOrDefault(
            ref string format, char defaultSpecifier, int defaultValue)
        {
            var spec = new Tuple<char, int>(defaultSpecifier, defaultValue);
            
            if (!string.IsNullOrEmpty(format))
                spec = FirstSpecifier(ref format);
            else
                format = null;

            return spec;
        }
    }
}
