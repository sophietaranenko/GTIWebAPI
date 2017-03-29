using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GTIWebAPI.Providers
{
    public class TransliterationProvider
    {
        private Dictionary<char, string> mapToLatin = new Dictionary<char, string>
        {
            { 'А', "A" },
            { 'Б', "B" },
            { 'В', "V" },
            { 'Г', "G" },
            { 'Д', "D" },
            { 'Е', "E" },
            { 'Ё', "E" },
            { 'Ж', "Zh" },
            { 'З', "Z" },
            { 'И', "I" },
            { 'Й', "I" },
            { 'І', "I" },
            { 'Ї', "I" },
            { 'Є', "E" },
            { 'К', "K" },
            { 'Л', "L" },
            { 'М', "M" },
            { 'Н', "N" },
            { 'О', "O" },
            { 'П', "P" },
            { 'Р', "R" },
            { 'С', "S" },
            { 'Т', "T" },
            { 'У', "U" },
            { 'Ф', "F" },
            { 'Х', "Kh" },
            { 'Ц', "Ts" },
            { 'Ч', "Ch" },
            { 'Ш', "Sh" },
            { 'Щ', "Shch" },
            { 'Ь', "" },
            { 'Ы', "Y" },
            { 'Ъ', "Ie" },
            { 'Э', "E" },
            { 'Ю', "Iu" },
            { 'Я', "Ia" },
            { 'Ґ', "G"},
            { 'а', "a" },
            { 'б', "b" },
            { 'в', "v" },
            { 'г', "g" },
            { 'д', "d" },
            { 'е', "e" },
            { 'ё', "e" },
            { 'ж', "zh" },
            { 'з', "z" },
            { 'и', "i" },
            { 'й', "i" },
            { 'і', "i" },
            { 'ї', "i" },
            { 'є', "e" },
            { 'к', "k" },
            { 'л', "l" },
            { 'м', "m" },
            { 'н', "n" },
            { 'о', "o" },
            { 'п', "p" },
            { 'р', "r" },
            { 'с', "s" },
            { 'т', "t" },
            { 'у', "u" },
            { 'ф', "f" },
            { 'х', "kh" },
            { 'ц', "ts" },
            { 'ч', "ch" },
            { 'ш', "sh" },
            { 'щ', "shch" },
            { 'ь', "" },
            { 'ы', "y" },
            { 'ъ', "ie" },
            { 'э', "e" },
            { 'ю', "iu" },
            { 'я', "ia" },
            { 'ґ', "g"},
            { '\'', "" }
     };

        public string ToLatin(string value)
        {
            var result = string.Concat(value.Select(c => ToLatinChar(c)));
            result = result.Replace("ii", "i");
            result = Regex.Replace(result, "[^A-Za-z _]", "");
            return result;
        }

        public string ToLatinChar(char character)
        {
            string latin = "";

            if (mapToLatin.ContainsKey(character))
            {
                latin = mapToLatin[character];
            }
            else
            {
                latin = latin + character;
            }
            return latin;
        }

    }
}
