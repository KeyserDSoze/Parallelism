using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallelism.Code
{
    public class Normal
    {
        public static Normal Instance
        {
            get
            {
                Console.WriteLine("Normal loop");
                Parallel.ForEach(Database.Fetch(20), (x) =>
                {
                    Console.WriteLine(x.SumXY());
                });
                return null;
            }
        }
    }
}
