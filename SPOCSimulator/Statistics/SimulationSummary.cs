using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Statistics
{
    public class SimulationSummary
    {
        public readonly string Marker;
        public readonly int SolvedTickets;
        public readonly int DeployedTickets;
        public readonly int StartedTickets;
        public readonly int TotalTickets;
        public readonly int Open1stLevelTickets;
        public readonly int Open2ndLevelTickets;
        public readonly double AverageTicketSolveDuration;
        public readonly long TotalCosts;
        public readonly double TotalWorkingHours;
        public readonly double AverageHourlyWage;

        public SimulationSummary(string marker, 
            int solvedTickets, 
            int deployedTickets, 
            int startedTickets, 
            int totalTickets, 
            int open1stLevelTickets, 
            int open2ndLevelTickets, 
            double averageTicketSolveDuration, 
            long totalCosts, 
            double totalWorkingHours, 
            double averageHourlyWage)
        {
            Marker = marker;
            SolvedTickets = solvedTickets;
            DeployedTickets = deployedTickets;
            StartedTickets = startedTickets;
            TotalTickets = totalTickets;
            Open1stLevelTickets = open1stLevelTickets;
            Open2ndLevelTickets = open2ndLevelTickets;
            AverageTicketSolveDuration = averageTicketSolveDuration;
            TotalCosts = totalCosts;
            TotalWorkingHours = totalWorkingHours;
            AverageHourlyWage = averageHourlyWage;
        }
    }
}
