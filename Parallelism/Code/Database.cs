using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallelism.Code
{
    public class Database
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string SumXY()
        {
            return (this.X + this.Y).ToString();
        }
        public static List<Database> Fetch(int max)
        {
            List<Database> databases = new List<Database>();
            for (int i = 0; i < max; i++) databases.Add(new Database() { X = i, Y = -1 * i });
            return databases;
        }
        public int Minimum()
        {
            return X < Y ? X : Y;
        }
        public int Check(System.Threading.CancellationToken cancellationToken)
        {
            for(int i=0; i<100; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(4);
            }
            return 0;
        }
    }
}
