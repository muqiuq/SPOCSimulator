using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Commands
{
    [Verb("run", HelpText = "Run simulation")]
    public class RunCommand : BaseCommand, ICommandVerb
    {
        [Option("workshifts", HelpText = "path to workshifts json file", Required = true)]
        public string WorkshiftsFilename { get; set; }

        [Option("tickets", HelpText = "path to tickets per n day json file", Required = true)]
        public string TicketsPerNDayFilename { get; set; }

        public int Run()
        {
            

            return 0;
        }
    }
}
