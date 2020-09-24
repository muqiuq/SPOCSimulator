using CommandLine;
using Newtonsoft.Json;
using SPOCSimulator.Generator;
using SPOCSimulator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SPOCSimulator.Commands
{
    [Verb("examples", HelpText="Generate example configuration files")]
    class ExampleCommand : BaseCommand, ICommandVerb
    {

        [Option('e', "employeetype", HelpText="Generate employeetype example file")]
        public bool GenerateEmployeeType { get; set; }

        [Option('d', "ticketsperday", HelpText = "Generate Tickets Per Day distribution")]
        public bool GenerateTicketsPerDayDistribution { get; set; }

        [Option('w', "workshifts", HelpText = "Workshifts example file")]
        public bool GenerateWorkshifts { get; set; }

        [Option('b', "boundary", HelpText = "write boundary conditions")]
        public bool BoundaryConditions { get; set; }

        [Option('o', "output", HelpText = "Output filename", Default = "example.json")]
        public string Filename { get; set; }

        public int Run()
        {
            if(Helper.DiffersFromThreshold(1, GenerateEmployeeType, GenerateTicketsPerDayDistribution, GenerateWorkshifts, BoundaryConditions))
            {
                Print("You may only select one generator function!");
                return 1;
            }
            if(GenerateEmployeeType)
            {
                var etcm = ExampleGenerator.GetEmployeeTypeContentManager();
                etcm.Save(Filename);

                Print("Saved example employee types to " + Filename);
            }
            if(GenerateTicketsPerDayDistribution)
            {
                var gentic = new TicketsPerDayDistributionGenerator();
                gentic.Save(Filename);
                Print("Saved tickets per day distribution " + Filename);
            }
            if(GenerateWorkshifts)
            {
                var ws = ExampleGenerator.GetWorkshifts();
                ws.Save(Filename);
                Print("Saved workshifts example files to " + Filename);
            }
            if(BoundaryConditions)
            {
                var json = JsonConvert.SerializeObject(new BoundaryConditions(), Formatting.Indented);
                File.WriteAllText(Filename, json);
            }

            return 0;
        }
    }
}
