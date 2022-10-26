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

    private bool RecEnemy(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if(i.getType() == "enemy")
        {
            return true;
        }

        else
        { 
            return false;
        }
    }

    private bool RecAlly(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if(i.getType() == "ally")
        {
            return true;
        }

        else
        { 
            return false;
        }

    }

    private bool RecSeparator(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if(i.getType() == "separator")
        {
            return true;
        }

        else
        { 
            return false;
        }

    }

    private bool RecDelimiter(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if(i.getType() == "delimiter")
        {
            return true;
        }

        else
        { 
            return false;
        }

    }

    private bool RecAllyGroup(List<Token> _token_list)
    {
        Token i = this.token_list[0];

        if(RecAlly(_token_list) == true)
        {
            this.token_list.RemoveAt(0);

            if(RecSeparator(_token_list) == true)
            {
                this.token_list.RemoveAt(0);

                return RecAllyGroup(_token_list);
            }

            else
            {
                return true;
            }
        }

        else
        { 
            return false;
        }

    }

    private bool RecSelf(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if(i.getType() == "self")
        {
            return true;
        }

        else
        { 
            return false;
        }

    }

    private bool RecEndLine(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if(i.getType() == "endLine")
        {
            return true;
        }

        else
        { 
            return false;
        }

    }

    public bool FS()
    {

        foreach (var t in this.token_list)
        {
            Console.WriteLine(t.getType());
        }   

        Console.WriteLine("_______________________________________________");

        RecAllyGroup(this.token_list);

        if(RecDelimiter(this.token_list) == true)
        {
            this.token_list.RemoveAt(0);
        }

        if(RecSelf(this.token_list) == true)
        {
            this.token_list.RemoveAt(0);
        }

        foreach (var t in this.token_list)
        {
            Console.WriteLine(t.getType());
        }   

        return true;
    }
    
}