using SPOCSimulator.ContentManager;
using SPOCSimulator.Generator;
using SPOCSimulator.Simulation.Ticker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Simulation
{
    public class SimulationManager : ITickerManager
    {
        private WorkshiftsCM workshiftsCM;
        private TicketGenerationPlan plan;
        private readonly int daysToSimulate;
        private TicketQueue primaryInputQueue = new TicketQueue();
        private TicketQueue doneQueue = new TicketQueue();
        private TicketQueue firstToSecondQueue = new TicketQueue();

        public delegate void LogDelegate(string text);
        public event LogDelegate LogEvent;

        public SimulationManager(WorkshiftsCM workshiftsCM, TicketGenerationPlan plan, int daysToSimulate)
        {
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
            }
        }
    }
}
