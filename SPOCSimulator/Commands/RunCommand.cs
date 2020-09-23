using CommandLine;
using MySql.Data.MySqlClient;
using SPOCSimulator.ContentManager;
using SPOCSimulator.Generator;
using SPOCSimulator.Simulation;
using SPOCSimulator.Simulation.Ticker;
using SPOCSimulator.Statistics;
using SPOCSimulator.Utils;
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

        [Option("truncate", HelpText = "Truncate table first")]
        public bool TruncateFirst { get; set; }

        [Option("dropfirst", HelpText = "Drop and create table first")]
        public bool DropAndCreateFirst { get; set; }

        [Option('n', "Name", HelpText = "Name (Tag) for the current run.", Required = true)]
        public string Name { get; set; }

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

            

            SimulationManager sm = new SimulationManager(Name,workshifts, ticketGenerationPlan, Days.Value);

            if(UseDatabase)
            {
                sm.NewDatapoint += Sm_NewDatapoint;
                if(DropAndCreateFirst)
                {
                    DropTables(true);

                    CreateTables();
                }
                if(TruncateFirst)
                {
                    TruncateTables();
                }
            }

            sm.LogEvent += Print;

            sm.Run();

            if(UseDatabase)
            {
                Print("Inserting {0} datapoints into db if connected", datapoints.Count);
                int inserts = 0;
                if (datapoints.Count > 0)
                {
                    inserts += InternalMySqlHelper.GetBulkInsert(conn, "datapoints", datapoints.ToArray()).ExecuteNonQuery();
                }
                Print("Inserts: {0}", inserts);
            }
            

            List<TicketEntity> tickets = ticketGenerationPlan.Tickets;

            SimulationSummary ss = new SimulationSummary(
                Name,
                tickets.Where(t => t.Solved).Count(),
                tickets.Where(t => t.Deployed).Count(),
                tickets.Where(t => t.Started).Count(),
                tickets.Count(),
                tickets.Where(t => !t.Solved && t.Difficulty == Models.SupportLevel.Level1st).Count(),
                tickets.Where(t => !t.Solved && t.Difficulty == Models.SupportLevel.Level2nd).Count(),
                tickets.Where(t => t.Solved).Average(i => i.Duration),
                sm.Accounting.TotalExpenses,
                sm.Accounting.TotalWorkingHours,
                sm.Accounting.TotalExpenses / sm.Accounting.TotalWorkingHours
                );

            var oldSummaryDeleteCommand = new MySqlCommand(string.Format("DELETE FROM {0} WHERE Marker=@Marker",Statics.TableSummaries), conn);
            oldSummaryDeleteCommand.Parameters.Add("@Marker", MySqlDbType.VarChar).Value = Name;
            Print("Deleted old summary (if exists) ({0})", oldSummaryDeleteCommand.ExecuteNonQuery());

            var summaryInsert = InternalMySqlHelper.GetInsert(conn, TablePrefix + Statics.TableSummaries, ss);
            Print("Insert summary ({0})", summaryInsert.ExecuteNonQuery());
                            
            Print("Solved tickets: {0}/{1}", ss.SolvedTickets, ss.TotalTickets);
            Print("Deployed tickets: {0} ", ss.DeployedTickets);
            Print("Started tickets: {0} ", ss.StartedTickets);
            Print("Open 1st level tickets: {0} ", ss.Open1stLevelTickets);
            Print("Open 2nd level tickets: {0} ", ss.Open2ndLevelTickets);
            Print("Average Duration: {0} ", ss.AverageTicketSolveDuration);
            Print("Total Costs: {0:C} ", ss.TotalCosts);
            Print("Total Costs: {0:#,##0} h ", ss.TotalCosts);
            Print("Average hourly wage: {0:C}", ss.AverageHourlyWage);

            return 0;
        }

        private void Sm_NewDatapoint(SimulationDatapoint point)
        {
            datapoints.Add(point);
        }
    }
}
