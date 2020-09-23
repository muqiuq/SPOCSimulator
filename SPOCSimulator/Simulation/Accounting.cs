using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Simulation
{
    public class Accounting
    {
        public long TotalExpenses { get; private set; }

        public double TotalWorkingHours { get; private set; }

        public Accounting()
        {

        }

        public void Book(long amount)
        {
            TotalExpenses += amount;
        }

        public void Book(long hourlyWage, int ticks)
        {
            TotalExpenses += (int)(hourlyWage * (double)ticks / 60);
            TotalWorkingHours += (double)ticks / 60;
        }
    }
}
