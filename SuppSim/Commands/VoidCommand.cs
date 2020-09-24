using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Text;

namespace SPOCSimulator.Commands
{
    [Verb("void", HelpText = "does absolutely nothing")]
    public class VoidCommand : BaseCommand, ICommandVerb
    {
        public int Run()
        {
            Print("OK");
            return 0;
        }
    }
}
