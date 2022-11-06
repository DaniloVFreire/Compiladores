using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Compiler;

namespace CompilerS
{
    class Program
    {
        static void Main(string[] args)
        {
            Utils utils = new Utils(true);
            Compiler compiler_instance = new Compiler(utils);
            compiler_instance.RunCompilation();
        }
    }
    
}
