using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallelism.Code
{
    public class PLinq
    {
        public static PLinq Instance
        {
            get
            {
                Console.WriteLine("Normal calculation of Minimum in parallel");
                // Safe, and fast!
                int min = int.MaxValue;
                // Make a "lock" object
                object syncObject = new object();
                Parallel.ForEach(
                    Database.Fetch(100),
                    // First, we provide a local state initialization delegate.
                    () => int.MaxValue,
                    // Next, we supply the body, which takes the original item, loop state,
                    // and local state, and returns a new local state
                    (item, loopState, localState) =>
                    {
                        int value = item.Minimum();
                        return Math.Min(localState, value);
                    },
                    // Finally, we provide an Action<TLocal>, to "merge" results together
                    localState =>
                    {
                            // This requires locking, but it's only once per used thread
                            lock (syncObject)
                            min = System.Math.Min(min, localState);
                    }
                );
                Console.WriteLine("Minimum is: " + min.ToString());

                Console.WriteLine("PLinq calculation of Minimum");
                min = int.MaxValue;
                min = Database.Fetch(100).AsParallel().Min(x => x.Minimum());
                Console.WriteLine("Plinq minimum is: " + min.ToString());

                Console.WriteLine("Parallelize with an order and non-parallelize when some operations need to");
                min = int.MaxValue;
                //select in ordered parallel operation and after search minimum in a non-parallel operation
                min = Database.Fetch(100).AsParallel().AsOrdered().Select(x => x.Minimum()).AsEnumerable().Min(x => x);
                Console.WriteLine("Plinq ordered and non-parallel minimum is: " + min.ToString());

                Console.WriteLine("Linq query in parallel");
                min = int.MaxValue;
                var tempCollection = from item in Database.Fetch(100).AsParallel()
                                     let e = item.Minimum()
                                     where (item.X > 4)
                                     select e;
                min = tempCollection.AsEnumerable().Min(item => item);
                Console.WriteLine($"Linq query minimum: {min}");

                //Plinq not ordered
                var t = Database.Fetch(100).AsParallel().Take(20);
                Console.WriteLine($"Plinq not ordered and not predictable: {string.Join(",", t)}");

                //Plinq ordered
                t = Database.Fetch(100).AsParallel().AsOrdered().Take(20);
                Console.WriteLine($"Plinq ordered and predictable: {string.Join(",", t)}");

                //Plinq ForAll
                //var z = Database.Fetch(7).AsParallel().Where(x => x.X > 5);
                //Parallel.ForEach(z, (x) => 
                //{
                //    Console.WriteLine(x.Minimum());
                //});
                Database.Fetch(7).AsParallel().Where(x => x.X > 5).ForAll(x => Console.WriteLine($"Minimum in parallel ForAll: {x.Minimum()}"));


                return null;
            }
        }
    }
}
