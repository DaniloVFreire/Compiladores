using System;
using System.Collections.Generic;

public class Syntatic
{
    private List<Token> token_stack;
    private List<Token> token_list;
    private List<Token> token_aux = null;
    public Syntatic()
    {
        int buffer_position = 0;
        this.token_stack = new List<Token>();
        this.token_aux = new List<Token>();
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
    private void ClearLine (List<Token> _token_list)
    {
        while (token_list[0].getType() != "endLine") this.token_list.RemoveAt(0);

        this.token_list.RemoveAt(0);
    }
    private bool RecGoal (List<Token> _token_list)
    {
        Token i = _token_list[0];

        if (i.getType() == "objects")
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private bool RecAction (List<Token> _token_list)
    {
        Token i = _token_list[0];

        if (i.getType() == "action")
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private bool RecAsk(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if (i.getType() == "ask")
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private bool RecCondition(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if (i.getType() == "condition")
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private bool RecEnemy(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if (i.getType() == "enemy")
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

        if (i.getType() == "ally")
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

        if (i.getType() == "separator")
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

        if (i.getType() == "delimiter")
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

        if (RecAlly(_token_list) == true)
        {
            this.token_list.RemoveAt(0);

            if (RecSeparator(_token_list) == true)
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

        if (i.getType() == "self")
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
        foreach (var t in this.token_list)
        {
            Console.WriteLine(t.getType());
        }
        Console.WriteLine("##############################");
        Console.WriteLine("------------"+_token_list.Count+"-------------");

        Token i = _token_list[0];

        if (i.getType() == "endLine")
        {
            return true;
        }

        else
        {
            return false;
        }

    }

    private bool RecParentesisClosed(List<Token> _token_list)
    {
        Token i = _token_list[1];

        if (i.getType() == "close_parentesis")
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    
    private int RecParentesisOpen(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if (i.getType() == "open_parentesis")
        {
            if (!RecParentesisClosed(_token_list))
            {
                this.token_list.RemoveAt(2); //como tenho ctz que o parentesis n esta vazio, mas so posso remover o fecha
                                             //parenteses quando tiver um abre, ja removo aqui
                //Console.WriteLine("return 1");
                return 1; // parenteses aberto com algum token depois diferente de parenteses fechado 
            }
            else
            {
                //Console.WriteLine("return 2");
                return 2; // parenteses aberto com parenteses fechado em seguida, ou seja, vazio, error
            }
        }

        else
        {
            //Console.WriteLine("return 0");
            return 0; //Não é abre parênteses
        }
    }
    
    public bool FS()
    {

        foreach (var t in this.token_list)
        {
            Console.WriteLine(t);
        }

        Console.WriteLine("_______________________________________________");

        while (token_list.Count != 0)
        {
            bool erro = false;

            if (RecAction(this.token_list) == true)
            {
                if (token_list[0].getValue().Equals("moveTowards"))
                {
                    this.token_aux.Add(token_list[0]);
                    this.token_list.RemoveAt(0);

                    foreach (var t in this.token_list)
                    {
                        Console.WriteLine(t.getType());
                    }
                    Console.WriteLine("##############################");

                    if (RecDelimiter(this.token_list) == true)
                    {
                        Console.WriteLine("Entrou1");
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);

                        foreach (var t in this.token_list)
                        {
                            Console.WriteLine(t.getType());
                        }
                        Console.WriteLine("##############################");
                    }
                    else
                    {
                        erro = true;
                        ClearLine(this.token_list);
                    }
                    if (RecParentesisOpen(this.token_list) == 1 && erro == false)
                    {
                        Console.WriteLine("Entrou2");
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);

                        foreach (var t in this.token_list)
                        {
                            Console.WriteLine(t.getType());
                        }
                        Console.WriteLine("##############################");
                    }
                    else
                    {
                        erro = true;
                        ClearLine(this.token_list);
                    }
                    if (RecEnemy(this.token_list) == true && erro == false ||
                        RecAlly (this.token_list) == true && erro == false ||
                        RecSelf (this.token_list) == true && erro == false ||
                        RecGoal (this.token_list) == true && erro == false)
                    {
                        Console.WriteLine("Entrou3");
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);

                        foreach (var t in this.token_list)
                        {
                            Console.WriteLine(t.getType());
                        }
                        Console.WriteLine("##############################");
                    }
                    else
                    {
                        erro = true;
                        ClearLine(this.token_list);
                    }

                    if (RecDelimiter(this.token_list) == true && erro == false)
                    {
                        Console.WriteLine("Entrou5");
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);

                        foreach (var t in this.token_list)
                        {
                            Console.WriteLine(t.getType());
                        }
                        Console.WriteLine("##############################");
                    }
                    else
                    {
                        erro = true;
                        ClearLine(this.token_list);
                    }

                    if (RecEndLine(this.token_list) == true && erro == false)
                    {
                        Console.WriteLine("Entrou4");
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);

                        foreach (var t in this.token_list)
                        {
                            Console.WriteLine(t.getType());
                        }
                        Console.WriteLine("##############################");
                    }
                    else
                    {
                        erro = true;
                        Console.WriteLine("Esperando um '.' ao final do comando 'MoveTowards");
                        Console.WriteLine("Linha ignorada");
                    }

                    if (erro == false)
                    {
                        while (token_aux.Count != 0)
                        {
                            this.token_stack.Add(this.token_aux[0]);
                            this.token_aux.RemoveAt(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error");
                    }
                }
                //if (token_list[0].getValue().Equals("sendBall"))
            }
        }

        foreach (var t in this.token_list)
        {
            Console.WriteLine(t.getType());
        }

        Console.WriteLine("_______________________________________________");

        foreach (var t in this.token_stack)
        {
            Console.WriteLine(t.getType());
        }

        return true;
    }
}