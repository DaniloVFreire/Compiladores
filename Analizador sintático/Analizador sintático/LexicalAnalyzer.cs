using System;
using System.Collections.Generic;

public enum TokenClasses
{
    action,
    ally,
    ask,
    condition,
    enemy,
    objects,
}
public class Lexical_analizer
{
    private List<Token> token_list;
    public Lexical_analizer()
	{
        this.token_list = new List<Token>();

    }
    public void RunLexicalAnalizer(string input_line)
    {

    }
    public List<Token> getTokenList()
    {
        return this.token_list;
    }
}
