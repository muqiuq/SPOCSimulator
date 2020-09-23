using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Statistics
{
    public class SimulationDatapoint
    {
        public readonly string Marker;
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
        public readonly double AverageTicketSolveTime1stLevel;
        public readonly double AverageTicketSolveTime2ndLevel;
        public readonly double AverageNumberOfStarts;


        public SimulationDatapoint(
            string marker, 
            int day, 
            int tick, 
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
            double averageNumberOfStarts)
        {
            Marker = marker;
            Day = day;
            Tick = tick;
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
        }

    }
}
