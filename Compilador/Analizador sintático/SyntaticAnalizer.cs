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

    private int pos_Line = 1;

    public Syntatic(Utils _utils)
    {
        this.utils = _utils;
    }
    public string RunSyntaticAnalizer(List<Token> _token_list)
    {
        this.input_token_list = _token_list;
        utils.Verbose("printing input token list");
        foreach(Token token in input_token_list)
        {
            utils.Verbose(token.getValue());
        }
        utils.Verbose("End of the input token list");
        FS();
        return "teste";
    }

    // Função principal do analisador sintático, responsável por chamar as outras

    public bool FS()
    {
        while (input_token_list.Count != 0)
        {
            bool erro = false;

            if (RecAction(this.input_token_list[0]) == true)// Analisando se existe uma ação na lista de tokens
            {
                if (recActionType() == true)
                {
                    if (RecEndLine(this.input_token_list[0]) == true)
                    {
                        if (erro == false)
                        {
                            utils.Verbose(this.input_token_list[0].ToString());
                            this.new_token_list.Add(this.input_token_list[0]);
                        }

                        this.input_token_list.RemoveAt(0);
                    }
                    else
                    {
                        if (erro == false)
                        {
                            erro = true;
                            Console.WriteLine("Esperando um '.' ao final do comando 'action'");
                            Console.WriteLine("Linha ignorada");
                        }
                    }
                }
                else
                {
                    if(input_token_list.Count != 0){
                        this.input_token_list.RemoveAt(0);
                    }
                    else{
                        Console.WriteLine("Erro, ponto não encontrado");
                    }
                    Console.WriteLine("Não foi possivel identificar uma ação na frase");
                }
            }
            else if (RecCondition(this.input_token_list[0]) == true)// Analisando se existe alguma condição na lista de tokens
            {
                if (recConditionType() == true)
                {
                    if (RecEndLine(this.input_token_list[0]) == true)
                    {
                        if (erro == false)
                        {
                            this.new_token_list.Add(this.input_token_list[0]);
                        }
                        
                        this.input_token_list.RemoveAt(0);
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
                    this.input_token_list.RemoveAt(0);
                    Console.WriteLine("Não foi possivel identificar uma condição na frase");
                }
            }
            else if (RecAsk(this.input_token_list[0]) == true) // Analisando se existe alguma pergunta na lista de tokens
            {
                // Reconhecendo e removendo o ponto final da frase
                if (recAskType() == true)
                {
                    if (RecEndLine(this.input_token_list[0]) == true)
                    {
                        if (erro == false)
                        {
                            this.new_token_list.Add(this.input_token_list[0]);
                        }
                        
                        this.input_token_list.RemoveAt(0);
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
                    this.input_token_list.RemoveAt(0);
                    Console.WriteLine("Não foi possivel identificar uma pergunta na frase");
                }

            }
            else // Print a error if there is no action, condition or ask
            {
                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'action', 'condition' ou 'ask'", TokenTypes.error));

                ClearLineList();

                Console.WriteLine("ERROR: PRECISA INICIAR UMA LINHA DE COMANDO COM UMA 'action', 'condition' ou 'ask'");
            }

            //incrementando a posição dos comandos
            pos_Line++;
        }

        // Print the token stack

        foreach (var t in this.errors_list)
        {
            Console.WriteLine(t);
        }
        Console.WriteLine("-------------------------------------------------");
        utils.Verbose("new token list");
        for (int i = 0; i < new_token_list.Count; i++)
        {
            utils.Verbose("Comando: " + new_token_list[i].getPosition()
             + " Identificador: " + (new_token_list[i].getValue() == " "? "space": new_token_list[i].getValue())
             + " Tipo: " + new_token_list[i].getType());
        }
        return true;
    }

    // Função que testa as várias possibilidades de ações

    private bool recActionType()
    {
        // Tentando encontrar a ação realizada, entre os diversos tipos de ações

        if (input_token_list[0].getValue().Equals("explore"))
        {
            return true;
        }
        else if (input_token_list[0].getValue().Equals("moveTowards"))
        {
            return recMoveTowards();
        }
        else if (input_token_list[0].getValue().Equals("sendBall"))
        {
            return recSendBall();
        }
        else if (input_token_list[0].getValue().Equals("sayOk"))
        {
            return recSayOK();
        }
        else if (input_token_list[0].getValue().Equals("sayNo"))
        {
            return recSayNO();
        }
        else if (input_token_list[0].getValue().Equals("sayPosition"))
        {
            return recSayPosition();
        }
        else if (input_token_list[0].getValue().Equals("help"))
        {
            return recHelp();
        }
        else
        {
            return false;
        }

    }

        // Funções que realizam o reconhecimento das ações

    private bool recMoveTowards()
    {
        // Essa função reconhece a ação moveTowards
        bool erro = false;

        if (input_token_list[0].getValue().Equals("moveTowards"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecEnemy(this.input_token_list[0]) == true ||
                 RecAlly(this.input_token_list[0]) == true ||
                 RecSelf(this.input_token_list[0]) == true ||
                 RecObjects(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID agente' ou 'Goal'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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
        
        if (input_token_list[0].getValue().Equals("sendBall"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));

                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecAlly(this.input_token_list[0]) == true ||
                input_token_list[0].getValue().Equals("enemyGoal") ||
                RecSelf(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado' ou 'auto ID' ou 'ID inimigo'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);

                erro = true;
            }

            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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

        if (input_token_list[0].getValue().Equals("sayOk"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);
            
            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));

                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }
            
            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                //this.input_token_list.RemoveAt(0);
                erro = true;
            }
            
            if (RecAlly(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                Console.WriteLine("skdjf0");
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);

                erro = true;
            }
            
            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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

        if (input_token_list[0].getValue().Equals("sayNo"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecAlly(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

           if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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

        if (input_token_list[0].getValue().Equals("sayPosition"))
        {
            // Com isso já reconhecemos a palavra sayPosition
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            // Agora estamos tentando reconhecer o espaço depois da palavra

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecEnemy(this.input_token_list[0]) == true ||
                 RecAlly(this.input_token_list[0]) == true ||
                 RecSelf(this.input_token_list[0]) == true ||
                 RecObjects(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID agente' ou 'Goal'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            // Tentando reconhecer as virgulas 
            if (RecSeparator(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ','", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }
            // Fim do reconhecimento das virgulas

            // Tentando reconhecer o numero
            if (RecNumber(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'num'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }
            // Fim do reconhecimento do numero

            // Tentando reconhecer as virgulas 
            if (RecSeparator(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ','", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }
            // Fim do reconhecimento das virgulas

            // Tentando reconhecer o numero
            if (RecNumber(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'num'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }
            // Fim do reconhecimento do numero

            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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

        if (input_token_list[0].getValue().Equals("help"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));

                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecAlly(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);

                erro = true;
            }

            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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

    // Funções auxiliares para ajudar a reconhecer a ordem, e o tipo dos tokenss
    
    private bool RecAction(Token _token)
    {
        return _token.getType() == TokenTypes.action.ToString();
    }

    private bool RecAsk(Token _token)
    {
        return _token.getType() == TokenTypes.ask.ToString();
    }

    private bool RecCondition(Token _token)
    {
        return _token.getType() == TokenTypes.condition.ToString();
    }

    private bool RecEnemy(Token _token)
    {
        return _token.getType() == TokenTypes.enemy.ToString();
    }

    private bool RecAlly(Token _token)
    {
        return _token.getType() == TokenTypes.ally.ToString();
    }

    private bool RecSeparator(Token _token)
    {
        return _token.getType() == TokenTypes.separator.ToString();
    }

    private bool RecDelimiter(Token _token)
    {
        return _token.getType() == TokenTypes.delimiter.ToString();
    }
    
    private bool RecSelf(Token _token)
    {
        return _token.getType() == TokenTypes.self.ToString();
    }

    private bool RecEndLine(Token _token)
    {
        return _token.getType() == TokenTypes.endLine.ToString();
    }

    private bool RecParentesisClosed(Token _token)
    {
        return _token.getType() == TokenTypes.close_parentesis.ToString();
    }

    private bool RecParentesisOpen(Token _token)
    {
        return _token.getType() == TokenTypes.open_parentesis.ToString();
    }

    private bool RecNumber(Token _token)
    {
        return _token.getType() == TokenTypes.number.ToString();
    }

    private bool RecObjects(Token _token)
    {
        return _token.getType() == TokenTypes.objects.ToString();
    }

    private bool RecAllyGroup(List<Token> _token_list)
    {
        Token i = this.input_token_list[0];

        if (RecAlly(_token_list[0]) == true)
        {
            this.input_token_list.RemoveAt(0);

            if (RecSeparator(_token_list[0]) == true)
            {
                this.input_token_list.RemoveAt(0);

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
    {
        //quando da erro, limpa a linha e a lista aux até o momento

        if (input_token_list.Count > 0)
        {
            while (this.input_token_list[0].getType() != "endLine") input_token_list.RemoveAt(0);
            
            input_token_list.RemoveAt(0);
        }
    }
    private void ClearListAux(List<Token> _token_aux)
    {
        //quando da erro, limpa a linha e a lista aux até o momento

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

        if (input_token_list[0].getValue().Equals("carryingBall"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));

                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecAlly (this.input_token_list[0]) == true ||
                RecEnemy(this.input_token_list[0]) == true ||
                RecSelf (this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID agente'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);

                erro = true;
            }

            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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

        if (input_token_list[0].getValue().Equals("marked"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));

                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecAlly (this.input_token_list[0]) == true ||
                RecEnemy(this.input_token_list[0]) == true ||
                RecSelf (this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID agente'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);

                erro = true;
            }

            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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
        // Essa função tenta reconhecer a condição 'position'

        bool erro = false;

        if (input_token_list[0].getValue().Equals("position"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));

                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecAlly (this.input_token_list[0]) == true ||
                RecEnemy(this.input_token_list[0]) == true ||
                RecSelf (this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID agente'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);

                erro = true;
            }

            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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

    private bool recNeighbors()
    {
        // Essa função tenta reconhecer a condição 'neighbors'

        bool erro = false;

        if (input_token_list[0].getValue().Equals("neighbors"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));

                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecAlly (this.input_token_list[0]) == true ||
                RecEnemy(this.input_token_list[0]) == true ||
                RecSelf (this.input_token_list[0]) == true ||
                RecObjects(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID agente'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);

                erro = true;
            }

            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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

        if (input_token_list[0].getValue().Equals("carryingBall"))
        {
            return recCarryingBall();
        }
        else if (input_token_list[0].getValue().Equals("marked"))
        {
            return recMarked();
        }
        else if (input_token_list[0].getValue().Equals("position"))
        {
            return recPosition();
        }
        else if (input_token_list[0].getValue().Equals("neighbors"))
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

        if (input_token_list[0].getValue().Equals("askAction"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            // Reconhecendo o primeiro delimitador

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));

                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }
            
            // Tentando reconhecer uma ação

            if (RecAction(this.input_token_list[0]) == true)
            {
                if (recActionType() == false)
                {
                    erro = true;
                    this.input_token_list.RemoveAt(0);
                    Console.WriteLine("Não foi possivel identificar uma ação na frase");
                }
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'action'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            //OBS: o segundo delimitador já é reconhecido na função que reconhece a ação

            //Tentando reconhecer open_parentesis
            
            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecAlly(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);

                erro = true;
            }

            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

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

        if (input_token_list[0].getValue().Equals("askInfo"))
        {
            this.recognized_token_list.Add(input_token_list[0]);
            this.input_token_list.RemoveAt(0);

            // Reconhecendo o primeiro delimitador

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));

                this.input_token_list.RemoveAt(0);*/

                erro = true;
            }

            // Tentando reconhecer uma condição

            if (RecCondition(this.input_token_list[0]) == true)
            {
                if (recConditionType() == false)
                {
                    erro = true;
                    this.input_token_list.RemoveAt(0);
                    Console.WriteLine("Não foi possivel identificar uma condição na frase");
                }
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'condition'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            //OBS: o segundo delimitador já é reconhecido na função que reconhece a condição

            //Tentando reconhecer open_parentesis

            if (RecParentesisOpen(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando '('", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecAlly(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'ID aliado'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);

                erro = true;
            }

            if (RecParentesisClosed(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando ')'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);
                erro = true;
            }

            if (RecDelimiter(this.input_token_list[0]) == true)
            {
                if (erro == false)  this.recognized_token_list.Add(input_token_list[0]);
                this.input_token_list.RemoveAt(0);
            }
            else
            {
                if (erro == false) ClearListAux(this.recognized_token_list);

                /*
                this.errors_list.Add(new Token(input_token_list[0].getPositionInt(), "Esperando 'blank'", TokenTypes.error));
                
                this.input_token_list.RemoveAt(0);*/
                erro = true;
            }

            // Removendo os tokens da lista aux, e adicionando na stack

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

    // Função que testa as diversas possibilidades de perguntas

    private bool recAskType()
    {
        // Tentando encontrar a pergunta correta entre os diversos tipos de perguntas

        if (input_token_list[0].getValue().Equals("askAction"))
        {
            return recAskAction();
        }
        else if (input_token_list[0].getValue().Equals("askInfo"))
        {
            return recAskInfo();
        }
        else
        {
            return false;
        }
    }
}