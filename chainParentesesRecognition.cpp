#include <bits/stdc++.h>
#include <string>
using namespace std;

int FS1(string entrada, int posic, int pilha);
int FSIf(string entrada, int posic, int pilha);
int FSc();

int main()
{
    string entrada;
    getline(cin, entrada, '\n');
    if (FSIf(entrada, 0, 0))
    {
        cout << "Cadeia aceita" << endl;
    }
    else
    {
        cout << "Cadeia não aceita" << endl;
    }
    return 0;
}

int FSIf(string entrada, int posic, int pilha)
{

    if (entrada.size() >= posic + 1)
    {
        cout << posic << ", " << entrada[posic] << ", " << pilha << endl;
        if (entrada[posic] == ')' && pilha <= 0)
        {
            return FSc();
        }
        else if (entrada[posic] == ')')
        {
            return FSIf(entrada, posic + 1, pilha - 1);
        }
        else if (entrada[posic] == '(')
        {
            return FS1(entrada, posic + 1, pilha + 1);
        }
    }
    else if (pilha > 0)
    {
        return 0;
    }
    return 1;
}

int FS1(string entrada, int posic, int pilha)
{

    if (entrada.size() >= posic + 1)
    {
        cout << posic << ", " << entrada[posic] << ", " << pilha << endl;
        if (entrada[posic] == '(')
        {
            pilha++;
            return FSIf(entrada, posic + 1, pilha + 1);
        }
        else if (entrada[posic] == ' ')
        {
            return FSIf(entrada, posic + 1, pilha);
        }
        else if (entrada[posic] == ')')
        {
            return FSIf(entrada, posic + 1, pilha - 1);
        }
    }
    return 0;
}

int FSc()
{
    cout << "Erro na leitura, char ) adicionado incorretamente" << endl;
    return 0;
}