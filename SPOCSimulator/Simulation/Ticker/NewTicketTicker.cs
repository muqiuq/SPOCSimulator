using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Simulation.Ticker
{
    public class NewTicketTicker : ITicker
    {

        public NewTicketTicker()
        {

        }

        public bool Destroyable()
        {
            return false;
        }

        public void Tick(int day, int ticks)
        {
            throw new NotImplementedException();
        }
    }
}
