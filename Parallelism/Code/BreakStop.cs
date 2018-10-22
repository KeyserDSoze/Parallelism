using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallelism.Code
{
    public class BreakStop
    {
        public static BreakStop Instance
        {
            get
            {
                Console.WriteLine("Break");
                Parallel.ForEach(Database.Fetch(20), (x, parallelLoppState) =>
                {
                    if (x.X > 10) parallelLoppState.Break();
                    Console.WriteLine(x.SumXY());
                });
                Console.WriteLine("Stop");
                Parallel.ForEach(Database.Fetch(20), (x, parallelLoppState) =>
                {
                    if (x.X > 10) parallelLoppState.Stop();
                    Console.WriteLine(x.SumXY());
                });
                return null;
            }
        }
    }
}
