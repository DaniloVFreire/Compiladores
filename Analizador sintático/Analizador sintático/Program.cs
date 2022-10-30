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
            Compiler compiler_instance = new Compiler(true);
            compiler_instance.RunCompilation();
        }
    }
    
}
