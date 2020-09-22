using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Statistics
{
    public class SimulationDatapoint
    {
        public readonly int Day;
        public readonly int Tick;
        public readonly int OpenTickets;
        public readonly int Open2ndLevelTickets;
        public readonly int DoneTickets;
        public readonly int EmployeesWarmUp;
        public readonly int EmployeesCleanUp;
        public readonly int EmployeesWarmUpOrCleanUp;
        public readonly int Employees1stLevelWorking;
        public readonly int Employees1stLevelWaiting;
        public readonly int Employees2ndLevelWorking;
        public readonly int Employees2ndLevelWaiting;
        public readonly double AverageTicketWaitTime;
        public readonly double AverageTicketSolveTime;
        public readonly double AverageNumberOfStarts;

        public SimulationDatapoint()
        {

        }
    }
}
