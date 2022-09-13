using System;

public class Syntatic
{
    public string SyntaticAnalizer(string s)
    {

        string R = FAllyGroup(s);

        return R;
    }

    public bool FLetter(char c)
    {
        return char.IsLetter(c) && char.IsLower(c);
    }
    public bool FNumber(char c)
    {
        return char.IsDigit(c);
    }

    public string FEnemy(string s)
    {
        string pattern = "enemyN";
        int i;
        for (i = 0; i < 6; ++i)
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

        return "<" + s + ", enemyN>";
    }

    public string FAlly(string s)
    {
        string pattern = "allyN";
        int i;
        for (i = 0; i < 5; ++i)
        {
            if (s[i] != pattern[i])
            {
                Console.WriteLine("Error, Invalid allyy token, expected 'allyN'");
                return "Error";
            }
        }
        if (!FNumber(s[i]))
        {
            Console.WriteLine("Error, Invalid number token, expected number after 'allyyN'");
            return "Error";
        }
        if (!FLetter(s[i + 1]))
        {
            Console.WriteLine("Error, Invalid letter token, expected letter after number");
            return "Error";
        }

        return "<" + s + ", allyN>";
    }

    public string FAllyGroup(string s)
    {
        char separator = ',';
        string[] LAlly = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string a in LAlly)
        {

            Console.WriteLine(FAlly(a));
        }

        return null;
    }
}