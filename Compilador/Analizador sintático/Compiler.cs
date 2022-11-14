using Utilities;
using System;
using System.Collections.Generic;
using System.IO;
public class Compiler
{
    private List<Token> token_list;
    private Utils utils;
    public Compiler(Utils _utils)
    {
        this.token_list = new List<Token>();
        this.utils = _utils;
    }
    public int RunCompilation()
    {
        Lexical_analizer lexical_analizer_instance = new Lexical_analizer(utils);
        Syntatic syntatic_instance = new Syntatic(utils);
        string[] archive_lines;
        int line_counter = 0;
        string fileName = "WriteText.txt";
        string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);
        utils.Verbose(path);
        archive_lines = File.ReadAllLines(path);

        foreach (string input_line in archive_lines)
        {
            Console.WriteLine(input_line);
            Console.WriteLine(input_line.Length);
            if (!String.IsNullOrEmpty(input_line))
            {//Recebimento de uma string e envio pra processamento pelo analizador lexico
                lexical_analizer_instance.RunLexicalAnalizer(input_line, line_counter);
                line_counter++;
            }
            utils.Verbose("Fim da análize léxica");
        }

        this.token_list = lexical_analizer_instance.getTokenList();
        if (token_list.Count > 0)
        {//mostra a lista recebida do analizador léxico
         //Console.WriteLine("Tokens recebidos no compilador");
            foreach (var token in token_list)
            {
                utils.Verbose(token.ToString());
            }
        }

        utils.Verbose("________________________________________________________");
        Console.WriteLine("Error = " + lexical_analizer_instance.getError());
        if (lexical_analizer_instance.getError() == true)
        {
            Console.WriteLine("Analizado Léxico finalizado com Erro, portanto não seguira com a análise sintática");
        }
        else
        {
            //Console.WriteLine("entrei");
            syntatic_instance.RunSyntaticAnalizer(this.token_list);
        }

        Console.ReadLine();
        return 0;
    }

}
