using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator
{
    public static class BoundaryConditions
    {
        // All values in ticks (1 tick = 1 minute)
        public static int TicketResolvTime1stLevelMean = 30;
        public static int TicketResolvTime1stLevelStdDev = 15;
         
        public static int TicketResolvTime2ndLevelMean = 60;
        public static int TicketResolvTime2ndLevelStdDev = 45;

        // Factor of 2 would mean 50% 1st, 50% 2nd, 1.5 is 70% 1st
        public static double LevelDistributionFactor = 1.5;

    }
}
