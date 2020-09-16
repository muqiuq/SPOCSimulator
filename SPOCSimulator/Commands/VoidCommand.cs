using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Commands
{
    [Verb("void", HelpText = "does absolutely nothing")]
    public class VoidCommand : ICommandVerb
    {
        public int Run()
        {
            Console.WriteLine("Void");
            return 0;
        }
    }
}
