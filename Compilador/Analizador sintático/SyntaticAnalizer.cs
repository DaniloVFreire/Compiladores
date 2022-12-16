using System;
using System.Collections.Generic;
using System.IO;
using Utilities;
using static TokenTypes;

public class Syntatic
{
    private List<Token> errors_list = new List<Token>();
    private List<Token> input_token_list = new List<Token>();
    private List<Token> recognized_token_list = new List<Token>();

    private Utils utils;

    private List<Token> new_token_list = new List<Token>();

    private Token tracking_token = new Token(new Tuple<int,int>(1,1), " ", TokenTypes.error);

    public Syntatic(Utils _utils)
    {
        this.utils = _utils;
    }
    public List<Token> RunSyntaticAnalizer(List<Token> _token_list)
    {
        this.input_token_list = _token_list;
        utils.Verbose("printing input token list");
        foreach(Token token in input_token_list)
        {
            utils.Verbose(token.getValue());
        }
        utils.Verbose("End of the input token list");
        FS();
        return this.new_token_list;
    }

    // Função principal do analisador sintático, responsável por chamar as outras

    public bool FS()
    {
        while (input_token_list.Count > 0)
        {
            bool erro = false;

            if (RecAction() == true)// Analisando se existe uma ação na lista de tokens
            {
                utils.Verbose("FS() recActionType() == true");
                if (recActionType() == true)
                {
                    erro = RecEndLine(erro);
                }
                else
                {
                    utils.Verbose("FS() recActionType() == false");

                    clearCurrentPosition();

                    Console.WriteLine("Não foi possivel identificar uma ação na frase");
                }
            }
            else if (RecCondition() == true)// Analisando se existe alguma condição na lista de tokens
            {
                if (recConditionType() == true)
                {
                    erro = RecEndLine(erro);
                }
                else
                {
                    clearCurrentPosition();
                    Console.WriteLine("Não foi possivel identificar uma condição na frase");
                }
            }
            else if (RecAsk() == true) // Analisando se existe alguma pergunta na lista de tokens
            {
                // Reconhecendo e removendo o ponto final da frase
                if (recAskType() == true)
                {
                    utils.Verbose("FS() recAskType() == true");
                    erro = RecEndLine(erro);
                    
                }
                else
                {
                    clearCurrentPosition();
                    Console.WriteLine("Não foi possivel identificar uma pergunta na frase");
                }

            }
            else // Print a error if there is no action, condition or ask
            {
                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'action', 'condition' ou 'ask'", TokenTypes.error));

                ClearLineList();

                Console.WriteLine("ERROR: PRECISA INICIAR UMA LINHA DE COMANDO COM UMA 'action', 'condition' ou 'ask'");
            }

        }

        // Print the token stack
        Console.WriteLine("Iniciando lista de tokens com erro");
        foreach (var t in this.errors_list)
        {
            Console.WriteLine(t);
        }
        Console.WriteLine("Fim da lista de tokens com erro");
        Console.WriteLine("-------------------------------------------------");
        utils.Verbose("Lista de tokens reconhecidos");
        for (int i = 0; i < new_token_list.Count; i++)
        {
            utils.Verbose("Comando: " + new_token_list[i].getPosition()
             + " Identificador: " + (new_token_list[i].getValue() == " "? "space": new_token_list[i].getValue())
             + " Tipo: " + new_token_list[i].getType());
        }
        utils.Verbose("Fim da Lista de tokens reconhecidos");
        return true;
    }

    // Função que testa as várias possibilidades de ações

    private bool recActionType()
    {
        // Tentando encontrar a ação realizada, entre os diversos tipos de ações
        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("explore"))
        {
            utils.Verbose("recActionType() explore");
            return true;
        }
        else if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("moveTowards"))
        {
            utils.Verbose("recActionType() moveTowards");
            return recMoveTowards();
        }
        else if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("sendBall"))
        {
            utils.Verbose("recActionType() sendBall");
            return recSendBall();
        }
        else if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("sayOk"))
        {
            utils.Verbose("recActionType() sayOk");
            return recSayOK();
        }
        else if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("sayNo"))
        {
            utils.Verbose("recActionType() sayNo");
            return recSayNO();
        }
        else if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("sayPosition"))
        {
            utils.Verbose("recActionType() sayPosition");
            return recSayPosition();
        }
        else if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("help"))
        {
            utils.Verbose("recActionType() help");
            return recHelp();
        }
        else
        {
            utils.Verbose("recActionType() return false");
            return false;
        }
    }
        // Funções que realizam o reconhecimento das ações

    private bool recMoveTowards()
    {
        // Essa função reconhece a ação moveTowards
        bool erro = false;

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("moveTowards"))
        {
            utils.Verbose("recMoveTowards() == true");
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();

            erro = RecDelimiter(erro);
            clearUnexpected(new List<TokenTypes> { TokenTypes.open_parentesis });

            erro = RecParentesisOpen(erro);
            clearUnexpected(new List<TokenTypes> { TokenTypes.ally, TokenTypes.enemy, TokenTypes.self, TokenTypes.objects, TokenTypes.close_parentesis });

            if (this.input_token_list.Count > 0 && (RecEnemy() == true ||
                 RecAlly() == true ||
                 RecSelf() == true ||
                 RecObjects() == true))
            {
                utils.Verbose("recMoveTowards() == true (RecEnemy() == true || RecAlly() == true || RecSelf() == true || RecObjects() == true)");
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                    clearCurrentPosition();
                
            }
            else
            {
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando 'ID agente' ou 'Goal'", TokenTypes.error));
                
                
                erro = true;
            }
            clearUnexpected(new List<TokenTypes> { TokenTypes.close_parentesis });


            erro = RecParentesisClosed(erro);
            clearUnexpected(new List<TokenTypes> { TokenTypes.delimiter });

            erro = RecDelimiter(erro);
            clearUnexpected(new List<TokenTypes> { TokenTypes.endLine });

            if (erro == false)
            {
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
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
        
        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("sendBall"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();
            clearUnexpected(new List<TokenTypes> { TokenTypes.delimiter });

            erro = RecDelimiter(erro);
            clearUnexpected(new List<TokenTypes> { TokenTypes.open_parentesis });

            erro = RecParentesisOpen(erro);
            clearUnexpected(new List<TokenTypes> { TokenTypes.close_parentesis, TokenTypes.ally, TokenTypes.self }, new List<string> { "enemyGoal" });

            if (this.input_token_list.Count > 0 && RecAlly() == true ||
                input_token_list[0].getValue().Equals("enemyGoal") ||
                RecSelf() == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado' ou 'auto ID' ou 'ID inimigo'", TokenTypes.error));
                
                clearCurrentPosition();

                erro = true;
            }


            erro = RecParentesisClosed(erro);

            erro = RecDelimiter(erro);

            if (erro == false)
            {
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
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

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("sayOk"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();
            erro = RecDelimiter(erro);
            erro = RecParentesisOpen(erro);
            if (this.input_token_list.Count > 0 && RecAlly() == true)
            {
                utils.Verbose("recSayOK() RecAlly() == true");
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                utils.Verbose("recSayOK() RecAlly() == false");
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando 'ID aliado'", TokenTypes.error));
                
                if(this.input_token_list.Count > 0 && this.input_token_list[0].getValue().Equals(")") == false) clearCurrentPosition();

                erro = true;
            }


            erro = RecParentesisClosed(erro);

            erro = RecDelimiter(erro);

            if (erro == false)
            {
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
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

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("sayNo"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();

            erro = RecDelimiter(erro);

            erro = RecParentesisOpen(erro);

            if (this.input_token_list.Count > 0 && RecAlly() == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado'", TokenTypes.error));
                
                clearCurrentPosition();
                erro = true;
            }


            erro = RecParentesisClosed(erro);

            erro = RecDelimiter(erro);

            if (erro == false)
            {
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
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

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("sayPosition"))
        {
            // Com isso já reconhecemos a palavra sayPosition
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();

            // Agora estamos tentando reconhecer o espaço depois da palavra
            erro = RecDelimiter(erro);
            erro = RecParentesisOpen(erro);

            if (this.input_token_list.Count > 0 && RecEnemy() == true ||
                 RecAlly() == true ||
                 RecSelf() == true ||
                 RecObjects() == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando 'ID agente' ou 'Goal'", TokenTypes.error));
                
                clearCurrentPosition();
                erro = true;
            }

            // Tentando reconhecer as virgulas 
            if (this.input_token_list.Count > 0 && RecSeparator() == true)
            {
                //if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ','", TokenTypes.error));
                
                clearCurrentPosition();
                erro = true;
            }
            // Fim do reconhecimento das virgulas

            // Tentando reconhecer o numero
            erro = RecNumber(erro);
            // Fim do reconhecimento do numero

            // Tentando reconhecer as virgulas 
            if (this.input_token_list.Count > 0 && RecSeparator() == true)
            {
                //if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ','", TokenTypes.error));
                
                clearCurrentPosition();
                erro = true;
            }
            // Fim do reconhecimento das virgulas

            // Tentando reconhecer o numero
            erro = RecNumber(erro);
            // Fim do reconhecimento do numero

            erro = RecParentesisClosed(erro);

            erro = RecDelimiter(erro);

            if (erro == false)
            {
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
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

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("help"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();

            erro = RecDelimiter(erro);
            erro = RecParentesisOpen(erro);

            if (this.input_token_list.Count > 0 && RecAlly() == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado'", TokenTypes.error));
                
                clearCurrentPosition();

                erro = true;
            }

            erro = RecParentesisClosed(erro);

            erro = RecDelimiter(erro);

            if (erro == false)
            {
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
                }
            }
            else
            {
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool RecAllyGroup(List<Token> _token_list)
    {
        Token i = this.input_token_list[0];

        if (this.input_token_list.Count > 0 && RecAlly() == true)
        {
            clearCurrentPosition();

            if (this.input_token_list.Count > 0 && RecSeparator() == true)
            {
                clearCurrentPosition();

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

    // Função auxiliar que limpa uma linha de código com erro
    private void ClearLineList()
    { //quando da erro, limpa a linha e a lista aux até o momento

        if (this.input_token_list.Count > 0 && input_token_list.Count > 0)
        {
            while (this.input_token_list.Count > 0 && this.input_token_list[0].getType() != "endLine")
             input_token_list.RemoveAt(0);
            
            if(this.input_token_list.Count > 0)
            input_token_list.RemoveAt(0);
        }
    }
    private void ClearRecognizedTokenList(List<Token> _token_aux)
    { //Deve ser chamada ao encontrar um erro, limpa a lista de tokens reconhecidos até o momento

        if (_token_aux.Count > 0)
        {
            while (this.recognized_token_list.Count > 0) recognized_token_list.RemoveAt(0);

            //recognized_token_list.RemoveAt(0);
        }
    }

    //Funções que realizam o reconhecimento das condições

    private bool recCarryingBall()
    {
        bool erro = false;

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("carryingBall"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();

            erro = RecDelimiter(erro);

            erro = RecParentesisOpen( erro);

            if (this.input_token_list.Count > 0 && RecAlly() == true ||
                RecEnemy() == true ||
                RecSelf() == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID agente'", TokenTypes.error));
                
                clearCurrentPosition();

                erro = true;
            }

            erro = RecParentesisClosed(erro);

            erro = RecDelimiter(erro);

            if (erro == false)
            {
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
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

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("marked"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();

            erro = RecDelimiter(erro);
            erro = RecParentesisOpen(erro);

            if (this.input_token_list.Count > 0 && RecAlly() == true ||
                RecEnemy() == true ||
                RecSelf() == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID agente'", TokenTypes.error));
                
                clearCurrentPosition();

                erro = true;
            }

            erro = RecParentesisClosed(erro);

            erro = RecDelimiter(erro);

            if (erro == false)
            {
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
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
        utils.Verbose("recPosition()");
        // Essa função tenta reconhecer a condição 'position'

        bool erro = false;

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("position"))
        {
            utils.Verbose("recPosition() position == true");
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();

            erro = RecDelimiter(erro);
            erro = RecParentesisOpen(erro);

            if (this.input_token_list.Count > 0 && RecAlly() == true ||
                RecEnemy() == true ||
                RecSelf() == true)
            {
                utils.Verbose("recPosition() RecAlly()||RecEnemy()||RecSelf() == true");
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                utils.Verbose("recPosition() RecAlly()||RecEnemy()||RecSelf() == false");
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID agente'", TokenTypes.error));
                
                //clearCurrentPosition();

                erro = true;
            }

            erro = RecParentesisClosed(erro);

            erro = RecDelimiter(erro);

            if (erro == false)
            {
                utils.Verbose("recPosition() erro == false");
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
                }
            }
            else
            {
                utils.Verbose("recPosition() erro == true");
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    private bool recNeighbors()
    {
        // Essa função tenta reconhecer a condição 'neighbors'

        bool erro = false;

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("neighbors"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();

            erro = RecDelimiter(erro);
            erro = RecParentesisOpen(erro);

            if (this.input_token_list.Count > 0 && RecAlly() == true ||
                RecEnemy() == true ||
                RecSelf() == true ||
                RecObjects() == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID agente'", TokenTypes.error));
                
                clearCurrentPosition();

                erro = true;
            }

            erro = RecParentesisClosed(erro);

            erro = RecDelimiter(erro);

            if (erro == false)
            {
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
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
        utils.Verbose("recConditionType()");
        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("carryingBall"))
        {
            utils.Verbose("recConditionType() carryingBall");
            return recCarryingBall();
        }
        else if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("marked"))
        {
            utils.Verbose("recConditionType() marked");
            return recMarked();
        }
        else if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("position"))
        {
            utils.Verbose("recConditionType() position");
            return recPosition();
        }
        else if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("neighbors"))
        {
            utils.Verbose("recConditionType() neighbors");
            return recNeighbors();
        }
        else
        {
            utils.Verbose("recConditionType() false");
            return false;
        }
    }

    // Funções que realizam o reconhecimento das perguntas

    private bool recAskAction()
    {
        // Essa função tenta reconhecer o pedido de uma ação

        bool erro = false;

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("askAction"))
        {
            utils.Verbose("recAskAction() askAction == true");
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();

            // Reconhecendo o primeiro delimitador
            erro = RecDelimiter(erro);
            // Tentando reconhecer uma ação

            if (this.input_token_list.Count > 0 && RecAction() == true)
            {
                utils.Verbose("recAskAction() RecAction() == true");
                if (this.input_token_list.Count > 0 && recActionType() == false)
                {
                    utils.Verbose("recAskAction() RecAction() == true recActionType() == false");
                    erro = true;
                    if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("(") == false)
                    {
                        clearCurrentPosition();
                    }
                    Console.WriteLine("Não foi possivel identificar uma ação na frase");
                }
            }
            else
            {
                utils.Verbose("recAskAction() RecAction() == false");
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'action'", TokenTypes.error));
                
                //clearCurrentPosition();
                erro = true;
            }

            //OBS: o segundo delimitador já é reconhecido na função que reconhece a ação

            //Tentando reconhecer open_parentesis
            erro = RecParentesisOpen(erro);

            if (this.input_token_list.Count > 0 && RecAlly() == true)
            {
                utils.Verbose("recAskAction() RecAlly() == true");
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                utils.Verbose("recAskAction() RecAlly() == false");
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado'", TokenTypes.error));
                
                clearCurrentPosition();

                erro = true;
            }

            if(this.input_token_list.Count > 0)
            {
                erro = RecParentesisClosed(erro);
            }
            erro = RecDelimiter(erro);

            if (erro == false)
            {
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
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

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("askInfo"))
        {
            utils.Verbose("recAskAction() askInfo == true");
            this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();

            // Reconhecendo o primeiro delimitador
            erro = RecDelimiter(erro);
            // Tentando reconhecer uma condição

            if (this.input_token_list.Count > 0 && RecCondition() == true)
            {
                utils.Verbose("recAskAction() RecCondition() == true");
                if (this.input_token_list.Count > 0 && recConditionType() == false)
                {
                    utils.Verbose("recAskAction() RecCondition() == true recConditionType() == false");
                    erro = true;
                    //clearCurrentPosition();
                    Console.WriteLine("Não foi possivel identificar uma condição na frase");
                }
            }
            else
            {
                utils.Verbose("recAskAction() RecCondition() == false");
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'condition'", TokenTypes.error));
                
                //clearCurrentPosition();
                erro = true;
            }

            //OBS: o segundo delimitador já é reconhecido na função que reconhece a condição

            //Tentando reconhecer open_parentesis
            erro = RecParentesisOpen(erro);

            if (this.input_token_list.Count > 0 && RecAlly() == true)
            {
                utils.Verbose("recAskAction() RecAlly() == true");
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                utils.Verbose("recAskAction() RecAlly() == false");
                if (erro == false) ClearRecognizedTokenList(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado'", TokenTypes.error));
                
                //clearCurrentPosition();

                erro = true;
            }

            erro = RecParentesisClosed(erro);

            erro = RecDelimiter(erro);
            // Removendo os tokens da lista aux, e adicionando na stack

            if (erro == false)
            {
                utils.Verbose("recAskAction() erro == false");
                while (recognized_token_list.Count != 0)
                {
                    this.new_token_list.Add(this.recognized_token_list[0]);
                    this.recognized_token_list.RemoveAt(0);
                }
            }
            else
            {
                utils.Verbose("recAskAction() erro == true");
                Console.WriteLine("Error\n");
            }
        }

        return !erro;
    }

    // Função que testa as diversas possibilidades de perguntas

    private bool recAskType()
    {
        // Tentando encontrar a pergunta correta entre os diversos tipos de perguntas

        if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("askAction"))
        {
            utils.Verbose("recAskType() askAction");
            return recAskAction();
        }
        else if (this.input_token_list.Count > 0 && input_token_list[0].getValue().Equals("askInfo"))
        {
            utils.Verbose("recAskType() askInfo");
            return recAskInfo();
        }
        else
        {
            utils.Verbose("recAskType() false");
            return false;
        }
    }
    private void clearCurrentAndUnexpected(List<TokenTypes> tokenTypesList)
    {
        tokenTypesList.Add(TokenTypes.endLine);
        tokenTypesList.Add(TokenTypes.action);
        tokenTypesList.Add(TokenTypes.condition);
        tokenTypesList.Add(TokenTypes.ask);
        clearCurrentPosition();
        clearUnexpected(tokenTypesList);
    }
    private void clearUnexpected(List<TokenTypes> tokenTypesList, List<string> tokenLexemeList)
    {
        bool find = false;
        while (this.input_token_list.Count > 0)
        {
            for (int i = 0; i < tokenTypesList.Count; ++i)
            {
                utils.Verbose(this.input_token_list[0].getType().Equals(tokenTypesList[i].ToString()).ToString());
                if (this.input_token_list[0].getType().Equals(tokenTypesList[i].ToString()))
                {
                    find = true;
                    return;
                }
            }
            for (int i = 0; i < tokenLexemeList.Count; ++i)
            {
                utils.Verbose(this.input_token_list[0].getValue().Equals(tokenLexemeList[i]).ToString());
                if (this.input_token_list[0].getValue().Equals(tokenLexemeList[i]))
                {
                    find = true;
                    return;
                }
            }
            if (find == false)
            {
                utils.Verbose("Removendo unexpected token " + input_token_list[0].ToString());
                this.errors_list.Add(new Token(this.input_token_list[0].getPositionInt(), $"Token não esperado: '{input_token_list[0].getValue()}'", TokenTypes.error));
            }
            clearCurrentPosition();
        }
    }
    private void clearUnexpected(List<TokenTypes> tokenTypesList)
    {
        bool find = false;
        while (this.input_token_list.Count > 0)
        {
            for(int i = 0 ; i < tokenTypesList.Count ; ++i){
                utils.Verbose(this.input_token_list[0].getType().Equals(tokenTypesList[i].ToString()).ToString());
                if(this.input_token_list[0].getType().Equals(tokenTypesList[i].ToString())){
                    find = true;
                    return;
                }
            }
            if(find == false){
                utils.Verbose("Removendo unexpected token " + input_token_list[0].ToString());
                this.errors_list.Add(new Token(this.input_token_list[0].getPositionInt(), $"Token não esperado: '{input_token_list[0].getValue()}'", TokenTypes.error));
            }
            clearCurrentPosition();
        }
    }
    private void clearCurrentPosition(){
        if (this.input_token_list.Count > 0) {
            utils.Verbose("Removendo token " + input_token_list[0].ToString());
            this.tracking_token.setPosition(this.input_token_list[0].getPositionInt());
            this.input_token_list.RemoveAt(0);
        }
    }
    private Tuple<int, int> moveAndGetTrackingTokenPosition()
    {
        int line = this.tracking_token.getPositionInt().Item1;
        int column = this.tracking_token.getPositionInt().Item2;
        this.tracking_token.setPosition(new Tuple<int, int>(line, column+1));
        return this.tracking_token.getPositionInt();
    }
    // Funções auxiliares para ajudar a reconhecer a ordem, e o tipo dos tokenss
    private bool RecAction()
    {
        if(input_token_list.Count > 0){
            return input_token_list[0].getType() == TokenTypes.action.ToString();
        }
        else{
            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando ação", TokenTypes.error));
            return false;
        }
    }

    private bool RecAsk()
    {
        if(input_token_list.Count > 0){
            return input_token_list[0].getType() == TokenTypes.ask.ToString();
        }
        else{
            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando pergunta", TokenTypes.error));
            return false;
        }
    }

    private bool RecCondition()
    {
        if(input_token_list.Count > 0){
            return input_token_list[0].getType() == TokenTypes.condition.ToString();
        }
        else{
            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando condição", TokenTypes.error));
            return false;
        }
    }

    private bool RecEnemy()
    {
        if(input_token_list.Count > 0){
            return input_token_list[0].getType() == TokenTypes.enemy.ToString();
        }
        else{
            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando Enemy", TokenTypes.error));
            return false;
        }
    }

    private bool RecAlly()
    {
        if(input_token_list.Count > 0){
            return input_token_list[0].getType() == TokenTypes.ally.ToString();
        }
        else{
            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando Ally", TokenTypes.error));
            return false;
        }
    }

    private bool RecSeparator()
    {
        if(input_token_list.Count > 0){
            return input_token_list[0].getType() == TokenTypes.separator.ToString();
        }
        else{
            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando separador", TokenTypes.error));
            return false;
        }
    }

    private bool RecDelimiter(bool error)
    {
        if (this.input_token_list.Count > 0 && input_token_list[0].getType() == TokenTypes.delimiter.ToString())
        {
            utils.Verbose("RecDelimiter() == true");
            //this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();
        }
        else
        {
            utils.Verbose("RecDelimiter() == false");
            if (error == false) ClearRecognizedTokenList(this.recognized_token_list);

            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Token esperado ' '", TokenTypes.error));

            error = true;
        }
        return error;
    }
    
    private bool RecSelf()
    {
        if(input_token_list.Count > 0){
            return input_token_list[0].getType() == TokenTypes.self.ToString();
        }
        else{
            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando self", TokenTypes.error));
            return false;
        }
    }

    private bool RecEndLine(bool error)
    {
        if (this.input_token_list.Count > 0 && input_token_list[0].getType() == TokenTypes.endLine.ToString())
        {
            utils.Verbose("RecEndLine() == true");
            if (error == false)
            {
                utils.Verbose("recognized token: "+ this.input_token_list[0].ToString());
                //this.new_token_list.Add(this.input_token_list[0]);
            }
            clearCurrentPosition();
        }
        else
        {
            utils.Verbose("RecEndLine() == false");
            error = true;
            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), $"Esperando '.'", TokenTypes.error));
            Console.WriteLine("Esperando um '.' ao final do comando 'action'");
            //TODO: colocar linha ignorada
            Console.WriteLine($"Linha ignorada");
        }
        return error;
    }

    private bool RecParentesisClosed(bool error)
    {
        if (this.input_token_list.Count > 0 && input_token_list[0].getType() == TokenTypes.close_parentesis.ToString())
        {
            utils.Verbose("RecParentesisClosed() == true");
            //if (error == false)  this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();
        }
        else
        {
            utils.Verbose("RecParentesisClosed() == false");
            if (error == false) ClearRecognizedTokenList(this.recognized_token_list);
            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando ')'", TokenTypes.error));
            
            error = true;
        }
        return error;
    }

    private bool RecParentesisOpen(bool error)
    {
        if (this.input_token_list.Count > 0 && input_token_list[0].getType() == TokenTypes.open_parentesis.ToString())
        {
            utils.Verbose("RecParentesisOpen() == true");
            //if (error == false)  this.recognized_token_list.Add(input_token_list[0]);
            clearCurrentPosition();
        }
        else
        {
            utils.Verbose("RecParentesisOpen() == false");
            if (error == false) ClearRecognizedTokenList(this.recognized_token_list);
            
            this.errors_list.Add(new Token(moveAndGetTrackingTokenPosition(), "Esperando '('", TokenTypes.error));
            
            error = true;
        }
        return error;
    }

    private bool RecNumber(bool error)
    {
        if (this.input_token_list.Count > 0 && input_token_list[0].getType() == TokenTypes.number.ToString())
            {
                //if (error == false)  this.recognized_token_list.Add(input_token_list[0]);
                clearCurrentPosition();
            }
            else
            {
                if (error == false) ClearRecognizedTokenList(this.recognized_token_list);
                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'num'", TokenTypes.error));
                
                error = true;
            }
        return error;
    }

    private bool RecObjects()
    {
        return input_token_list[0].getType() == TokenTypes.objects.ToString();
    }
}