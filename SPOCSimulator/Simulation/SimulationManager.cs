using SPOCSimulator.ContentManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Simulation
{
    public class SimulationManager : ITickerManager
    {
        private WorkshiftsCM workshiftsCM;

        private TicketQueue primaryInputQueue = new TicketQueue();
        private TicketQueue doneQueue = new TicketQueue();
        private TicketQueue firstToSecondQueue = new TicketQueue();


        public List<ITicker> Tickers { get; private set; } = new List<ITicker>();

        public void Add(ITicker ticker)
        {
            Tickers.Add(ticker);
        }

        public void Run()
        {

        }
    }
}
