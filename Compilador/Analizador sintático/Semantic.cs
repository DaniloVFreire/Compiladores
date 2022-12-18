using System;
using System.Collections.Generic;
using System.IO;
using Utilities;
using static TokenTypes;
public class Semantic
{
    List<string> allyTeam = new List<string>();
    List<string> enemyTeam = new List<string>();

    public Semantic()
    {
        allyTeam = IniciarAllyTeam();
        enemyTeam = IniciarEnemyTeam();
        //Print(enemyTeam);
    }

    public string Execution(List<Token> token_list_input)
    {
        foreach (Token token in token_list_input)
        {
            if (token.getType().Equals("ally"))
            {
                string saida = CheckAlly(token, "allyNa0");

                if (saida != null) return "ERROR: "+saida;
            }
            else if (token.getType().Equals("ask"))
            {
                if (token.getValue().Equals("askAction"))
                {
                    string saida = CheckAction(token_list_input, token, token_list_input.IndexOf(token));

                    if (saida != null) return "ERROR: " + saida;
                }
            }
        }

        return "Semantico executado com sucesso";
    }

    private string CheckAction (List<Token> list, Token token, int index)
    {
        bool check = CheckAllySelf(list[index+2].getValue(), list[index+3].getValue());
        if (!check) return token + " pedindo para jogador falar sozinho";

        return null;
    }

    private bool CheckAllySelf (string allySecond, string allyThird)
    {
        if (!allySecond.Equals(allyThird))
        {
            return true;
        }
        return false;
    }
    private string CheckAlly(Token token, string allyatual)
    {
        string testNull = CheckNullPlayer(token);
        if (testNull != null) return "Jogador inexistente: " + testNull;

        string testSelf = CheckSelf(token, allyatual);

        if (testSelf != null) return "Jogador "+ allyatual + " falando consigo";

        return null;
    }
    private string CheckNullPlayer(Token tokentest)
    {
        foreach (string token in allyTeam)
        {
            if (tokentest.getValue().Equals(token)) return null;
        }
        foreach (string token in enemyTeam)
        {
            if (tokentest.getValue().Equals(token)) return null;
        }

        return tokentest.getValue();
    }

    private string CheckSelf (Token token, string self)
    {
        if (token.getValue().Equals(self)) return self;

        return null;
    }
    private List<string> IniciarAllyTeam()
    {
        List<string> list = new List<string>();
        list.Add("allyNa0");
        list.Add("allyNb1");
        list.Add("allyNc2");
        list.Add("allyNd3");
        list.Add("allyNe4");

        return list;
    }

    private List<string> IniciarEnemyTeam()
    {
        List<string> list = new List<string>();
        list.Add("enemyNf0");
        list.Add("enemyNg1");
        list.Add("enemyNh2");
        list.Add("enemyNi3");
        list.Add("enemyNj4");

        return list;
    }

    public void Print(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(list[i]);
        }
    }
}