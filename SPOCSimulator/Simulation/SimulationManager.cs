using SPOCSimulator.ContentManager;
using SPOCSimulator.Generator;
using SPOCSimulator.Simulation.Ticker;
using SPOCSimulator.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Simulation
{
    public class SimulationManager : ITickerManager
    {
        private readonly string marker;
        private WorkshiftsCM workshiftsCM;
        private TicketGenerationPlan plan;
        private readonly int daysToSimulate;
        private TicketQueue primaryInputQueue = new TicketQueue();
        private TicketQueue doneQueue = new TicketQueue();
        private TicketQueue firstToSecondQueue = new TicketQueue();
        private int unixTimestamp;
        public Accounting Accounting { get; private set; } = new Accounting();

        public int SimulationDatapointInterval { get; set; } = 5;

        public delegate void LogDelegate(string text);
        public event LogDelegate LogEvent;

        public delegate void NewDatapointDelegate(SimulationDatapoint point);
        public event NewDatapointDelegate NewDatapoint;

        public SimulationManager(string marker, WorkshiftsCM workshiftsCM, TicketGenerationPlan plan, int daysToSimulate)
        {
            this.marker = marker;
            this.workshiftsCM = workshiftsCM;
            this.plan = plan;
            this.daysToSimulate = daysToSimulate;
            Add(new ShiftManagerTicker(this, workshiftsCM, primaryInputQueue, doneQueue, firstToSecondQueue, Accounting));
            Add(new NewTicketTicker(plan, primaryInputQueue));
            //unixTimestamp = (Int32)(DateTime.Today.Subtract(new DateTime(1970, 1, 1).ToUniversalTime())).TotalSeconds;
            unixTimestamp = (Int32)((DateTimeOffset)DateTime.Today.ToUniversalTime()).ToUnixTimeSeconds();
        }

        public List<ITicker> Tickers { get; private set; } = new List<ITicker>();

        public TicketQueue GetDoneTicketQueue()
        {
            return doneQueue;
        }


        public void Add(ITicker ticker)
        {
            Tickers.Add(ticker);
        }

        public void Run()
        {
            int ticksToSimulate = daysToSimulate * BoundaryConditions.DayLength;
            int lastDay = 0;
            for (int tick = 0; tick < ticksToSimulate; tick++)
            {

                var day = tick / BoundaryConditions.DayLength;
                if (day != lastDay)
                {
                    LogEvent?.Invoke(string.Format("At day: {0}", day));
                    lastDay = day;
                }
                foreach (var ticker in Tickers.ToList())
                {
                    ticker.Tick(day, tick);
                    if (ticker.Destroyable())
                    {
                        Tickers.Remove(ticker);
                    }
                }
                if (tick % SimulationDatapointInterval == 0)
                {
                    var activeEmployees = Tickers.Where(t => t.GetType() == typeof(EmployeeTicker)).Select(t => (EmployeeTicker)t).ToList();
                    var startedTickets = plan.Tickets.Where(t => t.Started);
                    var solvedTickets = plan.Tickets.Where(t => t.Solved);
                    var solved1stLevel = solvedTickets.Where(t => t.Difficulty == Models.SupportLevel.Level1st);
                    var solved2ndLevel = solvedTickets.Where(t => t.Difficulty == Models.SupportLevel.Level2nd);

                    NewDatapoint?.Invoke(new SimulationDatapoint(marker,
                                unixTimestamp + tick * 60,
                                day, tick,
                                plan.Tickets.Where(t => t.Deployed && !t.Solved).Count(),
                                primaryInputQueue.Count,
                                firstToSecondQueue.Count,
                                doneQueue.Count,
                                activeEmployees.Count(t => t.WarmUp),
                                activeEmployees.Count(t => t.CleanUp),
                                activeEmployees.Count(t => t.CleanUp || t.WarmUp),
                                activeEmployees.Count(t => t.Level == Models.SupportLevel.Level1st && t.Productive),
                                activeEmployees.Count(t => t.Level == Models.SupportLevel.Level1st && !t.Productive),
                                activeEmployees.Count(t => t.Level == Models.SupportLevel.Level2nd && t.Productive),
                                activeEmployees.Count(t => t.Level == Models.SupportLevel.Level2nd && !t.Productive),
                                startedTickets.Any() ? startedTickets.Average(t => t.WaitingTime) : 0,
                                solvedTickets.Any() ? solvedTickets.Average(t => t.Duration) : 0,
                                solved1stLevel.Any() ? solved1stLevel.Average(t => t.Duration) : 0,
                                solved2ndLevel.Any() ? solved2ndLevel.Average(t => t.Duration) : 0,
                                solvedTickets.Any() ? solvedTickets.Average(t => t.NumberOfStarts) : 0,
                                solved2ndLevel.Any() ? solvedTickets.Min(t => t.Duration) : 0,
                                solved1stLevel.Any() ? solvedTickets.Min(t => t.Duration) : 0,
                                startedTickets.Any() ? startedTickets.Min(t => t.WaitingTime) : 0,
                                plan.Tickets.Where(t => t.Difficulty == Models.SupportLevel.Level1st && t.Started && !t.Solved).Count(),
                                plan.Tickets.Where(t => t.Difficulty == Models.SupportLevel.Level1st && t.Started && !t.Solved).Count()
                                ));
                }
            }
        }
    }
}
