using System;

namespace Analizador_sintático
{
    internal class SyntaticAnalizer
    {

        public SyntaticAnalizer()
        {

        }

        public static bool FLetter(char c)
        {
            return (char.IsLetter(c) && char.IsLower(c));
        }
        public static bool FNumber(char c)
        {
            return (char.IsDigit(c));
        }
        public static string FEnemy(string s)
        {
            string pattern = "enemyN";
            int i;
            for (i = 0 ; i < 6; ++i)
            {
                if (s[i] != pattern[i])
                {
                    Console.WriteLine("Error, Invalid enemy token, expected 'enemyN'");
                    return "Error";
                }
            }
            if (!FNumber(s[i]))
            {
                Console.WriteLine("Error, Invalid number token, expected number after 'enemyN'");
                return "Error";
            }
            if (!FLetter(s[i + 1]))
            {
                Console.WriteLine("Error, Invalid letter token, expected letter after number");
                return "Error";
            }

            return "<" + s + "enemyN>";
        }

    }
}