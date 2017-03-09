using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Providers
{
    public static class RandomPasswordGenerator
    {
        private static Random random = new Random();

        public static string GeneratePassword(int passwordLength)
        {
            return RandomString(passwordLength, PasswordSymbolsType.SmallLargeLatinDigits);
        }

        public static string GeneratePassword(int passwordLength, PasswordSymbolsType type)
        {
            return RandomString(passwordLength, type);
        }

        private static string RandomString(int length, PasswordSymbolsType type)
        {
            string chars = "";

            switch (type)
            {
                case PasswordSymbolsType.Digits:
                    chars = "0123456789";
                    break;
                case PasswordSymbolsType.LargeLatin:
                    chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                case PasswordSymbolsType.SmallLargeLatin:
                    chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    break;
                case PasswordSymbolsType.SmallLargeLatinDigits:
                    chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    break;
                case PasswordSymbolsType.SmallLatin:
                    chars = "abcdefghijklmnopqrstuvwxyz";
                    break;
                default:
                    chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    break;
            }

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public enum PasswordSymbolsType
    {
        SmallLargeLatinDigits = 1,
        SmallLatin = 2,
        LargeLatin = 3,
        SmallLargeLatin = 4,
        Digits = 5
    }
}
