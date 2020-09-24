using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Statistics
{
    public class SimulationDatapoint
    {
        public readonly string Marker;
        public readonly int Run;
        public readonly int Day;
        public readonly int Tick;
        public readonly long Time;
        public readonly int DeployedUnsolvedTickets;
        public readonly int OpenTickets;
        public readonly int Open2ndLevelTickets;
        public readonly int DoneTickets;
        public readonly int Ongoing1stLevelTickets;
        public readonly int Ongoing2ndLevelTickets;
        public readonly int EmployeesWarmUp;
        public readonly int EmployeesCleanUp;
        public readonly int EmployeesWarmUpOrCleanUp;
        public readonly int Employees1stLevelWorking;
        public readonly int Employees1stLevelWaiting;
        public readonly int Employees2ndLevelWorking;
        public readonly int Employees2ndLevelWaiting;
        public readonly double MinWaitTime;
        public readonly double MinSolveTime1stLevel;
        public readonly double MinSolveTime2ndLevel;
        public readonly double AverageTicketWaitTime;
        public readonly double AverageTicketSolveTime;
        public readonly double AverageTicketSolveTime1stLevel;
        public readonly double AverageTicketSolveTime2ndLevel;
        public readonly double AverageNumberOfStarts;


        public SimulationDatapoint(
            string marker,
            int run,
            long time,
            int day,
            int tick,
            int deployedUnsolvedTickets,
            int openTickets,
            int open2ndLevelTickets,
            int doneTickets,
            int employeesWarmUp,
            int employeesCleanUp,
            int employeesWarmUpOrCleanUp,
            int employees1stLevelWorking,
            int employees1stLevelWaiting,
            int employees2ndLevelWorking,
            int employees2ndLevelWaiting,
            double averageTicketWaitTime,
            double averageTicketSolveTime,
            double averageTicketSolveTime1stLevel,
            double averageTicketSolveTime2ndLevel,
            double averageNumberOfStarts, double minSolveTime2ndLevel, double minSolveTime1stLevel, double minWaitTime, int ongoing1stLevelTickets, int ongoing2ndLevelTickets)
        {
            Marker = marker;
            Run = run;
            Day = day;
            Tick = tick;
            Time = time;
            DeployedUnsolvedTickets = deployedUnsolvedTickets;
            OpenTickets = openTickets;
            Open2ndLevelTickets = open2ndLevelTickets;
            DoneTickets = doneTickets;
            EmployeesWarmUp = employeesWarmUp;
            EmployeesCleanUp = employeesCleanUp;
            EmployeesWarmUpOrCleanUp = employeesWarmUpOrCleanUp;
            Employees1stLevelWorking = employees1stLevelWorking;
            Employees1stLevelWaiting = employees1stLevelWaiting;
            Employees2ndLevelWorking = employees2ndLevelWorking;
            Employees2ndLevelWaiting = employees2ndLevelWaiting;
            AverageTicketWaitTime = averageTicketWaitTime;
            AverageTicketSolveTime = averageTicketSolveTime;
            AverageTicketSolveTime1stLevel = averageTicketSolveTime1stLevel;
            AverageTicketSolveTime2ndLevel = averageTicketSolveTime2ndLevel;
            AverageNumberOfStarts = averageNumberOfStarts;
            MinSolveTime2ndLevel = minSolveTime2ndLevel;
            MinSolveTime1stLevel = minSolveTime1stLevel;
            MinWaitTime = minWaitTime;
            Ongoing1stLevelTickets = ongoing1stLevelTickets;
            Ongoing2ndLevelTickets = ongoing2ndLevelTickets;
        }

    }
}
