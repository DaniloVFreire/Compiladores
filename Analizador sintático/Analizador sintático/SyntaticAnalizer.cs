using System;
using System.Collections.Generic;
using System.IO;

public class Syntatic
{
    private List<Token> token_stack;
    private List<Token> token_list;
    private List<Token> token_aux = null;

    private List<string> token_Id = new List<string>();
    private List<string> token_Tipo = new List<string>();
    private List<string> token_Pos = new List<string>();

    private int pos_Line = 1;

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
        foreach(Token token in token_list)
        {
            Console.Write(token.getValue() + " ");
        }
        Console.WriteLine(" ");
        FS(this.token_list);
        return "teste";
    }

    // Funções auxiliares para ajudar a reconhecer a ordem, e o tipo dos tokens

    private bool RecGoal(List<Token> _token_list)
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

    private bool RecAction(List<Token> _token_list)
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

    private bool RecNumber(List<Token> _token_list)
    {
        Token i = _token_list[0];

        if (i.getType() == "number")
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private bool RecObjects(List<Token> _token_list)
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

    // Função auxiliar que limpa uma linha de código com erro

    private void ClearLine(List<Token> _token_list, List<Token> _token_aux)
    {
        //quando da erro, limpa a linha e a lista aux até o momento
        if (_token_list.Count > 0)
        {
            while (this.token_list[0].getType() != "endLine") token_list.RemoveAt(0);

            token_list.RemoveAt(0);
        }
        if (_token_aux.Count > 0)
        {
            while (this.token_aux.Count > 0) token_aux.RemoveAt(0);

            //token_stack.RemoveAt(0);
        }
    }

    // Funções que realizam o reconhecimento das ações

    private bool recMoveTowards()
    {
        // Essa função reconhece a ação moveTowards
        bool erro = false;

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
                erro == false && RecAlly(this.token_list) == true ||
                erro == false && RecSelf(this.token_list) == true ||
                erro == false && RecGoal(this.token_list) == true)
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

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool recSendBall()
    {
        //Essa função reconhece a ação sendBall

        bool erro = false;

        if (token_list[0].getValue().Equals("sendBall"))
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

            if (erro == false && RecAlly(this.token_list) == true ||
                erro == false && token_list[0].getValue().Equals("enemyGoal") ||
                erro == false && RecSelf(this.token_list) == true)
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

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool recSayOK()
    {
        // Essa função reconhece a ação sayOK

        bool erro = false;

        if (token_list[0].getValue().Equals("sayOk"))
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

            if (erro == false && RecAlly(this.token_list) == true)
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

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool recSayNO()
    {
        // Essa função tenta reconhecer a ação sayNO

        bool erro = false;

        if (token_list[0].getValue().Equals("sayNo"))
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

            if (erro == false && RecAlly(this.token_list) == true)
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

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool recSayPosition()
    {
        // Essa função tenta reconhecer a ação sayPosition

        bool erro = false;

        if (token_list[0].getValue().Equals("sayPosition"))
        {
            // Com isso já reconhecemos a palavra sayPosition
            this.token_aux.Add(token_list[0]);
            this.token_list.RemoveAt(0);

            // Agora estamos tentando reconhecer o espaço depois da palavra

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
                erro == false && RecAlly(this.token_list) == true ||
                erro == false && RecSelf(this.token_list) == true ||
                erro == false && RecGoal(this.token_list) == true)
            {
                this.token_aux.Add(token_list[0]);
                this.token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearLine(this.token_list, this.token_aux);
                erro = true;
            }

            // Tentando reconhecer as virgulas 
            if (erro == false && RecSeparator(this.token_list) == true)
            {
                this.token_aux.Add(token_list[0]);
                this.token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearLine(this.token_list, this.token_aux);
                erro = true;
            }
            // Fim do reconhecimento das virgulas

            // Tentando reconhecer o numero
            if (erro == false && RecNumber(this.token_list) == true)
            {
                this.token_aux.Add(token_list[0]);
                this.token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearLine(this.token_list, this.token_aux);
                erro = true;
            }
            // Fim do reconhecimento do numero

            // Tentando reconhecer as virgulas 
            if (erro == false && RecSeparator(this.token_list) == true)
            {
                this.token_aux.Add(token_list[0]);
                this.token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearLine(this.token_list, this.token_aux);
                erro = true;
            }
            // Fim do reconhecimento das virgulas

            // Tentando reconhecer o numero
            if (erro == false && RecNumber(this.token_list) == true)
            {
                this.token_aux.Add(token_list[0]);
                this.token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearLine(this.token_list, this.token_aux);
                erro = true;
            }
            // Fim do reconhecimento do numero

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

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool recHelp()
    {
        bool erro = false;

        if (token_list[0].getValue().Equals("help"))
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

            if (erro == false && RecAlly(this.token_list) == true)
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

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    // Função que testa as várias possibilidades de ações

    private bool recActionType()
    {
        // Tentando encontrar a ação realizada, entre os diversos tipos de ações

        if (token_list[0].getValue().Equals("explore"))
        {
            return true;
        }
        else if (token_list[0].getValue().Equals("moveTowards"))
        {
            return recMoveTowards();
        }
        else if (token_list[0].getValue().Equals("sendBall"))
        {
            return recSendBall();
        }
        else if (token_list[0].getValue().Equals("sayOk"))
        {
            return recSayOK();
        }
        else if (token_list[0].getValue().Equals("sayNo"))
        {
            return recSayNO();
        }
        else if (token_list[0].getValue().Equals("sayPosition"))
        {
            return recSayPosition();
        }
        else if (token_list[0].getValue().Equals("help"))
        {
            return recHelp();
        }
        else
        {
            return false;
        }

    }

    //Funções que realizam o reconhecimento das condições

    private bool recCarryingBall()
    {
        bool erro = false;

        if (token_list[0].getValue().Equals("carryingBall"))
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
                erro == false && RecAlly(this.token_list) == true ||
                erro == false && RecSelf(this.token_list) == true)
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

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool recMarked()
    {
        bool erro = false;

        if (token_list[0].getValue().Equals("marked"))
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
                erro == false && RecAlly(this.token_list) == true ||
                erro == false && RecSelf(this.token_list) == true)
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

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool recPosition()
    {
        // Essa função tenta reconhecer a condição 'position'

        bool erro = false;

        if (token_list[0].getValue().Equals("position"))
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
                erro == false && RecAlly(this.token_list) == true ||
                erro == false && RecSelf(this.token_list) == true)
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

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool recNeighbors()
    {
        // Essa função tenta reconhecer a condição 'neighbors'

        bool erro = false;

        if (token_list[0].getValue().Equals("neighbors"))
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
                erro == false && RecAlly(this.token_list) == true ||
                erro == false && RecSelf(this.token_list) == true ||
                erro == false && RecGoal(this.token_list) == true)
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

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    // Função que testa as diversas possibilidades de condições

    private bool recConditionType()
    {
        // Tentando encontrar a condição correta entre os diversos tipos de condições

        if (token_list[0].getValue().Equals("carryingBall"))
        {
            return recCarryingBall();
        }
        else if (token_list[0].getValue().Equals("marked"))
        {
            return recMarked();
        }
        else if (token_list[0].getValue().Equals("position"))
        {
            return recPosition();
        }
        else if (token_list[0].getValue().Equals("neighbors"))
        {
            return recNeighbors();
        }
        else
        {
            return false;
        }
    }

    // Funções que realizam o reconhecimento das perguntas

    private bool recAskAction()
    {
        // Essa função tenta reconhecer o pedido de uma ação

        bool erro = false;

        if (token_list[0].getValue().Equals("askAction"))
        {
            this.token_aux.Add(token_list[0]);
            this.token_list.RemoveAt(0);

            // Reconhecendo o primeiro delimitador

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

            // Tentando reconhecer uma ação

            if (RecAction(this.token_list) == true)
            {
                if (recActionType() == false)
                {
                    Console.WriteLine("Não foi possivel identificar uma ação na frase");
                }
            }
            else
            {
                if (erro == false) ClearLine(this.token_list, this.token_aux);
                erro = true;
            }

            //OBS: o segundo delimitador já é reconhecido na função que reconhece a ação

            //Tentando reconhecer open_parentesis

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

            // Tentando reconhecer um aliado

            if (erro == false && RecAlly(this.token_list) == true)
            {
                this.token_aux.Add(token_list[0]);
                this.token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearLine(this.token_list, this.token_aux);
                erro = true;
            }

            // Tentando reconhecer close parentesis
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

            // Tentando reconhecer o terceiro delimitador

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

            // Removendo os tokens da lista, e adicionando na stack

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool recAskInfo()
    {
        bool erro = false;

        if (token_list[0].getValue().Equals("askInfo"))
        {
            this.token_aux.Add(token_list[0]);
            this.token_list.RemoveAt(0);

            // Reconhecendo o primeiro delimitador

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

            // Tentando reconhecer uma condição

            if (RecCondition(this.token_list) == true)
            {
                if (recConditionType() == false)
                {
                    Console.WriteLine("Não foi possivel identificar uma condição na frase");
                }
            }
            else
            {
                if (erro == false) ClearLine(this.token_list, this.token_aux);
                erro = true;
            }

            //OBS: o segundo delimitador já é reconhecido na função que reconhece a condição

            //Tentando reconhecer open_parentesis

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

            // Tentando reconhecer um aliado

            if (erro == false && RecAlly(this.token_list) == true)
            {
                this.token_aux.Add(token_list[0]);
                this.token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearLine(this.token_list, this.token_aux);
                erro = true;
            }

            // Tentando reconhecer close parentesis
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

            // Tentando reconhecer o terceiro delimitador

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

            // Removendo os tokens da lista, e adicionando na stack

            if (erro == false)
            {
                while (token_aux.Count != 0)
                {
                    this.token_Id.Add(this.token_aux[0].getValue());
                    this.token_Tipo.Add(this.token_aux[0].getType());
                    this.token_Pos.Add(this.pos_Line.ToString());
                    this.token_aux.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    // Função que testa as diversas possibilidades de perguntas

    private bool recAskType()
    {
        // Tentando encontrar a pergunta correta entre os diversos tipos de perguntas

        if (token_list[0].getValue().Equals("askAction"))
        {
            return recAskAction();
        }
        else if (token_list[0].getValue().Equals("askInfo"))
        {
            return recAskInfo();
        }
        else
        {
            return false;
        }
    }

    // Função principal do analisador sintático, responsável por chamar as outras

    public bool FS(List<Token> _token_list)
    {
        while (token_list.Count != 0)
        {
            bool erro = false;

            // Analisando se existe uma ação na lista de tokens

            if (RecAction(this.token_list) == true)
            {
                if (recActionType() == true)
                {
                    // Reconhecendo e removendo o ponto final da frase

                    if (erro == false && RecEndLine(this.token_list) == true)
                    {
                        this.token_Id.Add(this.token_list[0].getValue());
                        this.token_Tipo.Add(this.token_list[0].getType());
                        this.token_Pos.Add(this.pos_Line.ToString());
                        this.token_list.RemoveAt(0);
                    }
                    else
                    {
                        if (erro == false)
                        {
                            erro = true;
                            Console.WriteLine("Esperando um '.' ao final do comando 'action'");
                            this.token_stack.Add(token_list[0]);
                            Console.WriteLine("Linha ignorada");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Não foi possivel identificar uma ação na frase");
                }
            }

            // Analisando se existe alguma condição na lista de tokens

            else if (RecCondition(this.token_list) == true)
            {
                if (recConditionType() == true)
                {
                    if (erro == false && RecEndLine(this.token_list) == true)
                    {
                        this.token_Id.Add(this.token_list[0].getValue());
                        this.token_Tipo.Add(this.token_list[0].getType());
                        this.token_Pos.Add(this.pos_Line.ToString());
                        this.token_list.RemoveAt(0);
                    }
                    else
                    {
                        if (erro == false)
                        {
                            erro = true;
                            Console.WriteLine("Esperando um '.' ao final do comando 'condition'");
                            Console.WriteLine("Linha ignorada");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Não foi possivel identificar uma condição na frase");
                }
            }

            // Analisando se existe alguma pergunta na lista de tokens

            else if (RecAsk(this.token_list) == true)
            {
                // Reconhecendo e removendo o ponto final da frase

                if (recAskType() == true)
                {
                    if (erro == false && RecEndLine(this.token_list) == true)
                    {
                        this.token_Id.Add(this.token_list[0].getValue());
                        this.token_Tipo.Add(this.token_list[0].getType());
                        this.token_Pos.Add(this.pos_Line.ToString());
                        this.token_list.RemoveAt(0);
                    }
                    else
                    {
                        if (erro == false)
                        {
                            erro = true;
                            Console.WriteLine("Esperando um '.' ao final do comando 'ask'");
                            Console.WriteLine("Linha ignorada");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Não foi possivel identificar uma pergunta na frase");
                }

            }

            // Print a error if there is no action, condition or ask

            else
            {
                ClearLine(this.token_list, this.token_aux);
                Console.WriteLine("ERROR: PRECISA INICIAR UMA LINHA DE COMANDO COM UMA 'action', 'condition' ou 'ask'");
            }

            //incrementando a posição dos comandos
            pos_Line++;
        }

        // Print the token stack

        /*foreach (var t in this.token_stack)
        {
            if (t.getType() != "endLine")   Console.WriteLine(t.getType());
            else                            Console.WriteLine(t.getType()+"\n");
            
        }*/

        for (int i = 0; i < token_Id.Count; i++)
        {
            if (token_Id[i].Equals(" ")) Console.WriteLine("Comando: " + token_Pos[i] + " Identificador: Space" + " Tipo: " + token_Tipo[i]);
            else Console.WriteLine("Comando: " + token_Pos[i] + " Identificador: " + token_Id[i] + " Tipo: " + token_Tipo[i]);
        }

        return true;
    }
}