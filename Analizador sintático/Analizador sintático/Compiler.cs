using System;
using System.Collections.Generic;
using static Lexical_analizer;

public class Compiler
{
    private List<Token> token_list;
    private bool verbose;
    public Compiler()
    {
        this.token_list = new List<Token>();
        this.verbose = false;
    }
    public Compiler(bool _verbose)
    {
        this.token_list = new List<Token>();
        this.verbose = _verbose;
    }
    public int RunCompilation()
    {
        Lexical_analizer lexical_analizer_instance = new Lexical_analizer(verbose);
        Syntatic syntatic_instance = new Syntatic();
        string inputLine = "";
        int line_counter=0;
        while (true)
        {
            inputLine = Console.ReadLine();
            
            if (!String.IsNullOrEmpty(inputLine))
            {//Recebimento de uma string e envio pra processamento pelo analizador lexico
                lexical_analizer_instance.RunLexicalAnalizer(inputLine, line_counter);
                line_counter++;
            }
            else
            {//Finalização da análize lexica
                if (verbose) Console.WriteLine("Fim da análize léxica");

                this.token_list = lexical_analizer_instance.getTokenList();
                if (verbose && token_list.Count>0)
                {//mostra a lista recebida do analizador léxico
                    Console.WriteLine("Tokens recebidos no compilador");
                    foreach (var token in token_list)
                    {
                        Console.WriteLine(token);
                    }
                    Console.ReadLine();
                }
                break;
            }
        }
        return 0;
    }
}
