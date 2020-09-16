using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Simulation
{
    interface ITicker
    {
        public void Tick(int day, int ticks);
    }
}
