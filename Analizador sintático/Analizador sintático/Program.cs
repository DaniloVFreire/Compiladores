using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Analizador_sintático
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Entre com a string a ser identificada");
            Sintatico algo = new Sintatico();
            string s = Console.ReadLine();
            Console.WriteLine(SyntaticAnalizer(s));
        }
    }
}
