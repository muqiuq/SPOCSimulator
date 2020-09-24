using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SPOCSimulator.Commands
{
    public abstract class BaseCommand
    {

        [Option("conditions", HelpText = "file to load boundary conditions from", Default = null)]
        public string BoundaryConditionsFile { get; set; }

        protected void init()
        {
            if(BoundaryConditionsFile != null)
            {
                var json = File.ReadAllText("boundary.json");
                JsonConvert.DeserializeObject<BoundaryConditions>(json);
                Print("Loaded boundary conditions!");
            }
        }

        public void Print(string text)
        {
            Console.WriteLine(text);
        }

        public void Print(string text, params object[] objs)
        {
            Console.WriteLine(string.Format(text, objs));
        }

    }
}
