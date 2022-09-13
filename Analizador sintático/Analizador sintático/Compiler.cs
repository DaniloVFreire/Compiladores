using System;
using System.Collections.Generic;
using static Lexical_analizer;

public class Compiler
{
    public Compiler()
    {
    }
    public int RunCompilation()
    {
        Lexical_analizer lexical_analizer_instance = new Lexical_analizer();
        string inputLine;
        List<Token> token_list;
        while (true)
        {
            inputLine = Console.ReadLine();

            if (String.IsNullOrEmpty(inputLine))
            {
                Console.WriteLine("Fim da análize léxica");
                token_list = lexical_analizer_instance.getTokenList();
                break;
            }
            else
            {
                lexical_analizer_instance.RunLexicalAnalizer(inputLine);

            }
        }
        
        Syntatic syntatic_instance = new Syntatic();
        
        return 0;
    }
}
