using System;
using System.Collections.Generic;

public class Syntatic
{    
    private List<Token> token_stack;
    private List<Token> token_list;
    public Syntatic()
    {
        int buffer_position = 0;
        this.token_stack = new List<Token>();
    }
    private Token GetToken(int buffer_position)
    {
        Token token = token_list[buffer_position];
        buffer_position++;
        return token;
    }
    public string RunSyntaticAnalizer(List<Token> _token_list)
    {
        this.token_list = _token_list;
        Console.WriteLine(this.token_list);
        FS();
        return "teste";
    }
    public bool FS()
    {
        Console.WriteLine(this.token_list);

        foreach (var t in this.token_list)
        {
            Console.WriteLine(t.getType());
        }   

        return true;
    }
    
}