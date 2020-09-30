using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator
{
    public class BoundaryConditions
    {

        public static int DayLength = 1440;

        [JsonProperty]
        public static int EmployeeWarmUpDuration { get; set; } = 20;
        [JsonProperty]
        public static int EmployeeCleanUpDuration { get; set; } = 15;

        // All values in ticks (1 tick = 1 minute)
        [JsonProperty]
        public static int TicketResolvTime1stLevelMin { get; set; } = 5;
        [JsonProperty]
        public static int TicketResolvTime1stLevelMean { get; set; } = 8;
        [JsonProperty]
        public static int TicketResolvTime1stLevelStdDev { get; set; } = 10;
        [JsonProperty]
        public static int TicketResolvTime2ndLevelMin { get; set; } = 15;
        [JsonProperty]
        public static int TicketResolvTime2ndLevelMean { get; set; } = 120;
        [JsonProperty]
        public static int TicketResolvTime2ndLevelStdDev { get; set; } = 45;

        // Factor of 2 would mean 50% 1st, 50% 2nd, 1.5 is 70% 1st
        [JsonProperty]
        public static double LevelDistributionFactor { get; set; } = 1.4;

        [JsonProperty]
        public static int EmployeeEfficencyDecayStartTicks { get; set; } = 360;
        [JsonProperty]
        public static int EmployeeEfficencyDecayInterval { get; set; } = 15;
        [JsonProperty]
        public static double EmployeeEfficencyDecayStartValue { get; set; } = 0.01;
        [JsonProperty]
        public static double EmployeeEfficencyDecayFactor { get; set; } = 1.2;

    }
}
