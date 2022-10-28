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
        Token i = _token_list[0];

        if (i.getType() == "close_parentesis")
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    
    private bool RecParentesisOpen(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if (i.getType() == "open_parentesis")
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void ClearLine(List<Token> _token_list, List<Token> _token_aux)
    {
        if (_token_list.Count > 0)
        {
            while (this.token_list[0].getType() != "endLine")   token_list.RemoveAt(0);

            token_list.RemoveAt(0);
        }
        if (_token_aux.Count > 0)
        {
            while (this.token_aux.Count > 0) token_aux.RemoveAt(0);

            //token_stack.RemoveAt(0);
        }
    }

    public bool FS()
    {
        while (token_list.Count != 0)
        {
            bool erro = false;

            if (RecAction(this.token_list) == true)
            {
                if (token_list[0].getValue().Equals("moveTowards"))
                {
                    this.token_aux.Add(token_list[0]);
                    this.token_list.RemoveAt(0);

                    if (RecDelimiter(this.token_list) == true)
                    {
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);
                    }
                    else
                    {
                        if (erro == false) ClearLine(this.token_list, this.token_aux);
                        erro = true;
                    }

                    if (erro == false && RecParentesisOpen(this.token_list) == true)
                    {
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);
                    }
                    else
                    {
                        if (erro == false) ClearLine(this.token_list, this.token_aux);
                        erro = true;
                    }

                    if (erro == false && RecEnemy(this.token_list) == true ||
                        erro == false && RecAlly (this.token_list) == true ||
                        erro == false && RecSelf (this.token_list) == true ||
                        erro == false && RecGoal (this.token_list) == true)
                    {
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);
                    }
                    else
                    {
                        if (erro == false) ClearLine(this.token_list, this.token_aux);
                        erro = true;
                    }

                    if (erro == false && RecParentesisClosed(this.token_list) == true)
                    {
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);
                    }
                    else
                    {
                        if (erro == false) ClearLine(this.token_list, this.token_aux);
                        erro = true;
                    }

                    if (erro == false && RecDelimiter(this.token_list) == true)
                    {
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);
                    }
                    else
                    {
                        if (erro == false) ClearLine(this.token_list, this.token_aux);
                        erro = true;
                    }

                    if (erro == false && RecEndLine(this.token_list) == true)
                    {
                        this.token_aux.Add(token_list[0]);
                        this.token_list.RemoveAt(0);
                    }
                    else
                    {
                        if (erro == false)
                        {
                            erro = true;
                            Console.WriteLine("Esperando um '.' ao final do comando 'MoveTowards");
                            Console.WriteLine("Linha ignorada");
                        }
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
                        Console.WriteLine("Error\n");
                    }
                }
                //if (token_list[0].getValue().Equals("sendBall"))
            }

            else if (RecCondition(this.token_list) == true)
            {

            }

            else if (RecAsk(this.token_list) == true)
            {

            }

            else
            {
                Console.WriteLine("ERROR: PRECISA INICIAR UMA LINHA DE COMANDO COM UMA 'action', 'condition' ou 'ask'");
            }
        }

        foreach (var t in this.token_stack)
        {
            if (t.getType() != "endLine")   Console.WriteLine(t.getType());
            else                            Console.WriteLine(t.getType()+"\n");
        }

        return true;
    }
}