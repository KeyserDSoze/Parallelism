using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallelism.Code
{
    public class PLinqConfiguration
    {
        //http://reedcopsey.com/2010/02/11/parallelism-in-net-part-9-configuration-in-plinq-and-tpl/
        public static PLinqConfiguration Instance
        {
            get
            {
                int maxThread = Math.Max(Environment.ProcessorCount / 2, 1);
                //options can add number of processor to be used to
                ParallelOptions options = new ParallelOptions();
                options.MaxDegreeOfParallelism = maxThread;
                Parallel.ForEach(Database.Fetch(10), options, row =>
                {
                    Console.WriteLine($"Sum: {row.SumXY()}");
                });

                //in PLinq is possible to set a maxThread usage
                int min = Database.Fetch(10)
                 .AsParallel()
                 .WithDegreeOfParallelism(maxThread)
                 .Min(item => item.Minimum());
                Console.WriteLine($"Min: {min}");

                //In this case parallelism is not used by PLinq
                //1) Queries that contain a Select, indexed Where, indexed SelectMany, or ElementAt clause after an ordering or filtering operator that has removed or rearranged original indices.
                //2) Queries that contain a Take, TakeWhile, Skip, SkipWhile operator and where indices in the source sequence are not in the original order.
                //3) Queries that contain Zip or SequenceEquals, unless one of the data sources has an originally ordered index and the other data source is indexable (i.e.an array or IList(T)).
                //4) Queries that contain Concat, unless it is applied to indexable data sources.
                //5) Queries that contain Reverse, unless applied to an indexable data source.
                //But it's possible to force parallelism
                var reversed = Database.Fetch(10)
                  .AsParallel()
                  .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                  .Select(i => i.Minimum())
                  .Reverse();
                Console.WriteLine($"Forced parallelism: {string.Join(",", reversed)}");


                //without buffer
                reversed = Database.Fetch(10)
                 .AsParallel()
                 .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                 .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                 .Select(i => i.Minimum())
                 .Reverse();
                Console.WriteLine($"Forced parallelism without buffer: {string.Join(",", reversed)}");

                //with buffer
                reversed = Database.Fetch(10)
                 .AsParallel()
                 .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                 .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                 .Select(i => i.Minimum())
                 .Reverse();
                Console.WriteLine($"Forced parallelism with buffer: {string.Join(",", reversed)}");

                return null;
            }
        }
    }
}
