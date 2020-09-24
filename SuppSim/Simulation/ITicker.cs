using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Simulation
{
    public interface ITicker
    {
        public void Tick(int day, int ticks);

        public bool Destroyable();
    }
}
