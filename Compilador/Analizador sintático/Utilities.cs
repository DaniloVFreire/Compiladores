using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Utils
    {
        private bool verbose;
        public Utils(bool _verbose)
        {
            this.verbose = _verbose;
        }
        public void Verbose(String Text)
        {
            if (verbose == true)
            {
                Console.WriteLine(Text);
            }
        }
    }
}