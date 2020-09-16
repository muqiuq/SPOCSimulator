using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Commands
{
    public abstract class BaseCommand
    {
        public void Print(string text)
        {
            Console.WriteLine(text);
        }

    }
}
