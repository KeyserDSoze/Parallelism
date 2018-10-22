using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallelism.Code
{
    public class CancellationToken
    {
        public static CancellationToken Instance
        {
            get
            {
                //PLinq cancellation token
                CancellationTokenSource cts = new CancellationTokenSource();
                try
                {
                    int min = Database.Fetch(10)
                                    .AsParallel()
                                    .WithCancellation(cts.Token)
                                    .Min(item => item.Check(cts.Token));
                }
                catch (OperationCanceledException e)
                {
                    // Query was cancelled before it finished
                    Console.WriteLine("Cancelled: " + e.ToString());
                }

                //Parallel cancellation token
                try
                {
                    ParallelOptions options = new ParallelOptions()
                    {
                        CancellationToken = cts.Token
                    };
                    Parallel.ForEach(Database.Fetch(20), options, x =>
                    {
                        options.CancellationToken.ThrowIfCancellationRequested();
                        x.Check(cts.Token);
                    });
                }
                catch (OperationCanceledException e)
                {
                    // Parallel.ForEach was cancelled before it finished
                    Console.WriteLine("Cancelled: " + e.ToString());
                }
                return null;
            }
        }
    }
}
