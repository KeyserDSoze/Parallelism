using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallelism.Code
{
    public class TaskClass
    {
        public static TaskClass Instance
        {
            get
            {
                string sample = "SDF";
                Task taskOne = new Task(
                       () =>
                       {
                           throw new ApplicationException("Random Exception!");
                       });
                Task<bool> taskTwo = new Task<bool>(
                        () =>
                        {
                            //with lambda it's possible to scope a variable in the entire method that call this task
                            Console.WriteLine(sample);
                            return true;
                        });

                // Start the tasks
                taskOne.Start();
                taskTwo.Start();

                try
                {
                    //it fires and block the execution of this method
                    Console.WriteLine("Return task2: " + taskTwo.Result);
                    Task.WaitAll(new[] { taskOne, taskTwo });
                    //it doesn't fire cause the exception of first task
                    Console.WriteLine("Return task2: " + taskTwo.Result);
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.InnerExceptions.Count);
                    foreach (var inner in e.InnerExceptions)
                        Console.WriteLine(inner.Message);
                }
                return null;
            }
        }
    }
}
