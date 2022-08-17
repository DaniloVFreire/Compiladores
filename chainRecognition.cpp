#include <bits/stdc++.h>
#include <string>
using namespace std;

int FSf(string entrada, int posic);
int FSi(string entrada, int posic);

int main(){
    string entrada;
    getline(cin, entrada, '\n');
    if(FSi(entrada, 0)){
        cout << "Cadeia aceita"<< endl;
    }
    else{
        cout << "Cadeia Incorreta"<< endl;
    }
    return 0;
}

int FSf(string entrada, int posic){
    if(entrada[posic] == '0'){
        return FSi(entrada, posic+1);
    }
    else if(entrada[posic] == '1' || entrada[posic] == ' '){
        return FSf(entrada, posic+1);
    }
    return 1;
}

int FSi(string entrada, int posic){
    if(entrada[posic] == '0' || entrada[posic] == ' '){
        return FSi(entrada, posic+1);
    }
    else if(entrada[posic] == '1'){
        return FSf(entrada, posic+1);
    }
    return 0;
}
