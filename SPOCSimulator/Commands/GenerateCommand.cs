using CommandLine;
using MathNet.Numerics.Distributions;
using SPOCSimulator.Generator;
using SPOCSimulator.Models;
using SPOCSimulator.Simulation.Ticker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Commands
{
    [Verb("generate", HelpText = "Generate random simulation base data")]
    public class GenerateCommand : BaseCommand, ICommandVerb
    {
        [Option("days", HelpText = "number of days to generate", Required = true)]
        public int DaysToGenerate { get; set; }

        [Option("tickets", HelpText = "mean number of tickets per day", Required = true)]
        public int NumOfTickets { get; set; }

        [Option("dist", HelpText = "path to tickets per day distribution json file", Required = true)]
        public string TicketsPerDayDistributionFileName { get; set; }

        [Option("outplan", HelpText = "output ticket generation plan", Default = "ticket-generation.csv")]
        public string FilenameTicketGeneration { get; set; }

        [Option("outdist", HelpText = "output ticket distribution plan", Default = "ticket-disribution.json")]
        public string FilenameTicketDistribution { get; set; }

        public int Run()
        {
            var distributioGenerator = new TicketsPerDayDistributionGenerator();
            distributioGenerator.Load(TicketsPerDayDistributionFileName);

            var g = new Normal(NumOfTickets, NumOfTickets/100);

            var ticketsPerNDay = new TicketsPerNDays();

            for (int day = 0; day < DaysToGenerate; day++)
            {
                int ticketsForThatDay = (int)g.Sample();
                for(int h = 0; h < TicketsPerDayDistributionGenerator.HOURS_PER_DAY; h++)
                {
                    var hourDist = Decimal.ToDouble(distributioGenerator.Get(h)) / 100;
                    var gHourDist = new Normal(hourDist, hourDist >= 1 ? 0.1 : hourDist / 100);
                    var ticketsForHour = gHourDist.Sample() * ticketsForThatDay;
                    ticketsPerNDay.Set(day, h, (int)ticketsForHour);
                }
            }

            ticketsPerNDay.Save(FilenameTicketDistribution);

            Print("Generated Tickets per n day file");

            int ticketNumber = 0;
            int ticks = 0;

            var gSupportLevel = new Normal(BoundaryConditions.LevelDistributionFactor, 1);

            TicketGenerationPlan tge = new TicketGenerationPlan();

            for (int day = 0; day < DaysToGenerate; day++)
            {
                for (int h = 0; h < TicketsPerDayDistributionGenerator.HOURS_PER_DAY; h++)
                {
                    var ticketsToSolve = ticketsPerNDay.Get(day, h);
                    for(int t = 0; t < ticketsToSolve; t++)
                    {
                        var time = ticks + ((double)t / (double)ticketsToSolve) * (double)60;
                        var supportLevel = SupportLevel.Level1st;
                        if(gSupportLevel.Sample() >= 2)
                        {
                            supportLevel = SupportLevel.Level2nd;
                            
                        }
                        var ticket = new TicketEntity(ticketNumber, supportLevel, GetLevelDifficulties(), (int)time);
                        tge.Tickets.Add(ticket);
                        ticketNumber++;
                    }
                    ticks += 60;
                }
            }

            var avg1StLevel = tge.Tickets.Average(i => i.DifficultyToSolveDurationMin[SupportLevel.Level1st]);
            var avg2ndLevel = tge.Tickets.Average(i => i.DifficultyToSolveDurationMin[SupportLevel.Level2nd]);
            var numOf1stLevel = tge.Tickets.Count(i => i.Difficulty == SupportLevel.Level1st);
            var numOf2ndLevel = tge.Tickets.Count(i => i.Difficulty == SupportLevel.Level2nd);
            var percentageOf1stLevel = (double)numOf1stLevel / (double)(numOf1stLevel + numOf2ndLevel) * 100;
            Console.WriteLine("Avg 1st: {0:0.00} ticks 2nd: {1:0.00} ticks Tickets: 1st: {2:0.00} ({3:0.00}%) 2nd: {4:0.00}", 
                avg1StLevel, 
                avg2ndLevel, 
                numOf1stLevel,
                percentageOf1stLevel,
                numOf2ndLevel);

            tge.Save(FilenameTicketGeneration);

            return 0;
        }

        Normal g1stLevelDuration = new Normal(BoundaryConditions.TicketResolvTime1stLevelMean, BoundaryConditions.TicketResolvTime1stLevelStdDev);
        Normal g2ndLevelDuration = new Normal(BoundaryConditions.TicketResolvTime2ndLevelMean, BoundaryConditions.TicketResolvTime2ndLevelStdDev);

        public Dictionary<SupportLevel, int> GetLevelDifficulties()
        {
            Dictionary<SupportLevel, int> difficultyToSolveDurationMin = new Dictionary<SupportLevel, int>() {
                { SupportLevel.Level1st, Math.Max(2,(int)Math.Abs(g1stLevelDuration.Sample()))},
                { SupportLevel.Level2nd, Math.Max(2,(int)Math.Abs(g2ndLevelDuration.Sample()))},
            };

            return difficultyToSolveDurationMin;
        }
    }
}
