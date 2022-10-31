using System;
using System.Collections.Generic;
using static Lexical_analizer;
using static Syntatic;

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
        string[] archive_lines;
        int line_counter = 0;

        archive_lines = System.IO.File.ReadAllLines(@"C:\Users\rfc77\source\Repos\Compiladores\Analizador sintático\Analizador sintático\WriteText.txt");

        foreach (string input_line in archive_lines)
        {
            Console.WriteLine(input_line);
            Console.WriteLine(input_line.Length);
            if (!String.IsNullOrEmpty(input_line))
            {//Recebimento de uma string e envio pra processamento pelo analizador lexico
                lexical_analizer_instance.RunLexicalAnalizer(input_line, line_counter);
                line_counter++;
            }
            //if (verbose) Console.WriteLine("Fim da análize léxica");
        }

        this.token_list = lexical_analizer_instance.getTokenList();
        if (verbose && token_list.Count > 0)
        {//mostra a lista recebida do analizador léxico
         //Console.WriteLine("Tokens recebidos no compilador");
            foreach (var token in token_list)
            {
                Console.WriteLine(token);
            }
        }

        Console.WriteLine("Error = " + lexical_analizer_instance.getError());
        Console.WriteLine("________________________________________________________");
        if (lexical_analizer_instance.getError() == true)
        {
            Console.WriteLine("Analizado Léxico finalizado com Erro, portanto não seguira com a análise sintática");
        }
        else
        {
            syntatic_instance.RunSyntaticAnalizer(this.token_list);
        }
        //syntatic_instance.RunSyntaticAnalizer(this.token_list);

        Console.ReadLine();
        return 0;
    }

}
