using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Compiler;

namespace CompilerS
{
    class Program
    {
        static void Main(string[] args)
        {
            //Gerando os casos testes básicos
            //TestGenerator example = new TestGenerator();
            //example.FS();
            //Console.ReadLine();
            Utils utils = new Utils(false);
            Compiler compiler_instance = new Compiler(utils);
            compiler_instance.RunCompilation();
        }
    }
    
}
