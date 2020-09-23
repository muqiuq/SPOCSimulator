using CommandLine;
using SPOCSimulator.ContentManager;
using SPOCSimulator.Generator;
using SPOCSimulator.Simulation;
using SPOCSimulator.Simulation.Ticker;
using SPOCSimulator.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Commands
{
    [Verb("run", HelpText = "Run simulation")]
    public class RunCommand : DbBaseCommand, ICommandVerb
    {
        [Option('e', "employeetypes", HelpText = "path to employee types json file", Required = true)]
        public string EmployeeTypesFilename { get; set; }

        [Option('w', "workshifts", HelpText = "path to workshifts json file", Required = true)]
        public string WorkshiftsFilename { get; set; }

        [Option('d', "days", HelpText = "days to simulate", Required = false, Default = null)]
        public int? Days { get; set; }

        [Option('t', "tickets", HelpText = "path to ticket generation plan file", Required = true)]
        public string TicketGenerationPlan { get; set; }

        [Option("usedb", HelpText = "Use database to store datapoints")]
        public bool UseDatabase { get; set; }

        private List<SimulationDatapoint> datapoints = new List<SimulationDatapoint>();

        public int Run()
        {

            if(UseDatabase)
            {
                ConnectDb();
            }

            EmployeeTypesCM employeeTypesCM = new EmployeeTypesCM();
            employeeTypesCM.Load(EmployeeTypesFilename);

            WorkshiftsCM workshifts = new WorkshiftsCM(employeeTypesCM);
            workshifts.Load(WorkshiftsFilename);

            TicketGenerationPlan ticketGenerationPlan = new TicketGenerationPlan();
            ticketGenerationPlan.Load(TicketGenerationPlan);

            Print("All files loaded");

            Print(string.Format("Total tickets: {0}", ticketGenerationPlan.TotalTickets));

            if(Days == null)
            {
                Days = ticketGenerationPlan.NumberOfDays;
                Print("Found {0} days in ticket plan", Days);
                
            }

            

            SimulationManager sm = new SimulationManager("test",workshifts, ticketGenerationPlan, Days.Value);

            if(UseDatabase)
            {
                sm.NewDatapoint += Sm_NewDatapoint;
            }


            sm.LogEvent += Print;

            sm.Run();


            Print("Inserting {} datapoints into db if connected", datapoints.Count);
            if(datapoints.Count > 0)
            {
                
            }

            List<TicketEntity> tickets = ticketGenerationPlan.Tickets;

            Print("Solved tickets: {0}/{1} ", tickets.Where(t => t.Solved).Count(), tickets.Count());
            Print("Deployed tickets: {0} ", tickets.Where(t => t.Deployed).Count());
            Print("Started tickets: {0} ", tickets.Where(t => t.Started).Count());
            Print("Open 1st level tickets: {0} ", tickets.Where(t => !t.Solved && t.Difficulty == Models.SupportLevel.Level1st).Count());
            Print("Open 2nd level tickets: {0} ", tickets.Where(t => !t.Solved && t.Difficulty == Models.SupportLevel.Level2nd).Count());
            Print("Average Duration: {0} ", tickets.Where(t => t.Solved).Average(i => i.Duration));

            return 0;
        }

        private void Sm_NewDatapoint(SimulationDatapoint point)
        {
            datapoints.Add(point);
        }
    }
}
