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

        public int SimulationDatapointInterval { get; set; } = 30;

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
            Add(new ShiftManagerTicker(this, workshiftsCM, primaryInputQueue, doneQueue, firstToSecondQueue));
            Add(new NewTicketTicker(plan, primaryInputQueue));
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
            for(int tick = 0; tick < ticksToSimulate; tick++)
            {
                
                var day = tick / BoundaryConditions.DayLength;
                if(day != lastDay)
                {
                    LogEvent?.Invoke(string.Format("At day: {0}", day));
                    lastDay = day;
                }
                foreach(var ticker in Tickers.ToList() )
                {
                    ticker.Tick(day, tick);
                    if(ticker.Destroyable())
                    {
                        Tickers.Remove(ticker);
                    }
                }
                if (tick % SimulationDatapointInterval == 0)
                {
                    var activeEmployees = Tickers.Where(t => t.GetType() == typeof(EmployeeTicker)).Select(t => (EmployeeTicker)t).ToList();
                    NewDatapoint?.Invoke(new SimulationDatapoint(marker, day, tick,
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
                        plan.Tickets.Where(t => t.Started).Average(t => t.WaitingTime),
                        plan.Tickets.Where(t => t.Solved).Average(t => t.Duration),
                        plan.Tickets.Where(t => t.Solved && t.Difficulty == Models.SupportLevel.Level1st).Average(t => t.Duration),
                        plan.Tickets.Where(t => t.Solved && t.Difficulty == Models.SupportLevel.Level2nd).Average(t => t.Duration),
                        plan.Tickets.Where(t => t.Solved).Average(t => t.NumberOfStarts)));
                }
            }
        }
    }
}
