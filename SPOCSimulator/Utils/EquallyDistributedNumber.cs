using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Utils
{
    public class EquallyDistributedNumber
    {

        private readonly int min;
        private readonly int max;

        private Queue<int> numbers = new Queue<int>();

        public EquallyDistributedNumber(int min, int max)
        {
            this.min = min;
            this.max = max;

            List<int> allNumbers = new List<int>();

            for(int a = min; a <= max; a++)
            {
                allNumbers.Add(a);
            }

            Random r = new Random();

            while (allNumbers.Count > 0)
            {
                var pos = r.Next(0, allNumbers.Count);
                if(pos < allNumbers.Count)
                {
                    numbers.Enqueue(allNumbers[pos]);
                    allNumbers.RemoveAt(pos);
                }
            }
        }

        public int Next()
        {
            lock(numbers)
            {
                var n = numbers.Dequeue();
                numbers.Enqueue(n);
                return n;
            }
        }
    }
}
