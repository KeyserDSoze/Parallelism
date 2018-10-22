using Parallelism.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallelism
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = Normal.Instance;
            var y = BreakStop.Instance;
            var z = PLinq.Instance;
            var w = PLinqConfiguration.Instance;
            var u = CancellationToken.Instance;
        }
    }
}
