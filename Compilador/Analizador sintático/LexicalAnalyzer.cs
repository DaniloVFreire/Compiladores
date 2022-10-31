using System;
using System.Collections.Generic;
using Utilities;
using static TokenTypes;

public class Lexical_analizer
{
    private List<Token> token_list;
    private Utils utils;
    private int stringInsertionPosition;
    private int wordStartPosition;
    private string word;
    bool Error;
    public Lexical_analizer(Utils _utils)
    {
        this.utils = _utils;
        this.token_list = new List<Token>();
        stringInsertionPosition = 0;
        wordStartPosition = 0;
        word = "";
        Error = false;
    }
    private bool CheckNumber(string s)
    {
        if (s.Equals("0") ||
            s.Equals("1") ||
            s.Equals("2") ||
            s.Equals("3") ||
            s.Equals("4") ||
            s.Equals("5") ||
            s.Equals("6") ||
            s.Equals("7") ||
            s.Equals("8") ||
            s.Equals("9"))
        {
            return true;
        }
        return false;
    }
    public void RunLexicalAnalizer(string input_line, int line)
    {
        utils.Verbose("Lendo linha");
        for (int i = 0; i < input_line.Length; ++i)
        {
            if (/*input_line[i] != ' ' && */String.IsNullOrEmpty(word))
            {//inicializa a cadeia do token em word
                this.wordStartPosition = i;
                utils.Verbose("Inicializando cadeia com caractere: " + Char.ToString(input_line[i]));

                this.word = word.Insert(stringInsertionPosition, Char.ToString(input_line[i]));
                this.stringInsertionPosition++;

                if (input_line[i] == '.' || input_line[i] == ',' ||
                input_line[i] == '(' || input_line[i] == ')')
                {//se o símbolo incial da cadeia for um "fechador"
                    utils.Verbose("cadeia de tamanho 1 finalizada com: " + input_line[i]);
                    generateAndAppendToken(line, this.wordStartPosition, word, true);
                }
            }
            else if (!String.IsNullOrEmpty(word) &&
                !(input_line[i] == ' ' || input_line[i] == '.' || input_line[i] == ',' ||
                input_line[i] == '(' || input_line[i] == ')'))
            {//continuando a cadeia do token em word
                utils.Verbose("reconhecendo caracteres do token: " + Char.ToString(input_line[i]));

                this.word = word.Insert(stringInsertionPosition, Char.ToString(input_line[i]));
                this.stringInsertionPosition++;
            }
            else if (!String.IsNullOrEmpty(word))
            {//finaliza a cadeia do token
                utils.Verbose("token encontrado: " + word);
                utils.Verbose("cadeia finalizada com: " + input_line[i]);

                generateAndAppendToken(line, this.wordStartPosition, word, false);
                generateAndAppendToken(line, this.wordStartPosition + word.Length, Char.ToString(input_line[i]), true);
            }
            if (Error)
            {
                Console.WriteLine("Finalizando análize lexica com erro");
                break;
            }
        }
        if (!String.IsNullOrEmpty(word))
        {//finaliza a cadeia do token
            utils.Verbose("token encontrado fora do loop: " + word);

            generateAndAppendToken(line, this.wordStartPosition, word, true);
        }
    }

    private TokenTypes defineTokentipe(string lexeme)
    {
        if (lexeme.Equals(","))
        {
            return separator;
        }
        else if (lexeme.Equals("."))
        {
            return endLine;
        }
        else if (lexeme.Equals("("))
        {
            return open_parentesis;
        }
        else if (lexeme.Equals(")"))
        {
            return close_parentesis;
        }
        else if (lexeme.Equals(" "))
        {
            return delimiter;
        }
        else if (CheckNumber(lexeme))
        {
            return number;
        }
        else if (lexeme.Contains("allyN") && lexeme.Length == 7 &&
            Char.IsLetter(lexeme[lexeme.Length - 2]) &&
            Char.IsDigit(lexeme[lexeme.Length - 1]))
        {
            return ally;
        }
        else if (lexeme.Contains("enemyN") && lexeme.Length == 8 &&
            Char.IsLetter(lexeme[lexeme.Length - 2]) &&
            Char.IsDigit(lexeme[lexeme.Length - 1]))
        {
            return enemy;
        }
        else if (lexeme.Equals("moveTowards") ||
            lexeme.Equals("explore") ||
            lexeme.Equals("sendBall") ||
            lexeme.Equals("sayOk") ||
            lexeme.Equals("sayNo") ||
            lexeme.Equals("sayPosition") ||
            lexeme.Equals("help"))
        {
            return action;
        }
        else if (lexeme.Equals("carryingBall") || lexeme.Contains("marked") ||
            lexeme.Contains("position") || lexeme.Contains("neighbors"))
        {
            return condition;
        }
        else if (lexeme.Equals("askAction") || lexeme.Equals("askInfo"))
        {
            return ask;
        }
        else if (lexeme.Equals("allyGoal") || lexeme.Equals("enemyGoal"))
        {
            return objects;
        }
        else if (lexeme.Equals("self"))
        {
            return self;
        }
        else
        {
            Console.WriteLine("Token Com erro/não definido: " + word);

            //para parar caso encontre erro descomente as duas linhas abaixo
            //this.Error=true;
            //clearTokenList();
            return error;

        }
    }

    private void generateAndAppendToken(int line, int column, string lexem, bool clean_buffer)
    {//recebendo dados iniciais de geração do token para unificar a geração
     //e evitar duplicidade de código

        //Adicionando um a coluna e lina pois não tem posição 0,0 em um texto e sim 1,1
        Tuple<int, int> tokenPosition = new Tuple<int, int>(line + 1, column + 1);

        //Adicionando token a lista
        this.token_list.Add(new Token(tokenPosition, lexem, defineTokentipe(lexem)));
        if (clean_buffer)
        {
            //reiniciando os separadores dos lexemas
            this.stringInsertionPosition = 0;
            this.wordStartPosition = 0;
            this.word = "";
        }

    }
    public List<Token> getTokenList()
    {//retorna a lista de tokens do estado atual
        return this.token_list;
    }
    private void clearTokenList()
    {//retorna a lista de tokens do estado atual
        this.token_list = new List<Token>();
    }
}