using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Models
{
    public class EmployeeType
    {
        public EmployeeType(string name, SupportLevel level, int hourlyWage, decimal successRate, decimal durationFactor)
        {
            Name = name;
            Level = level;
            HourlyWage = hourlyWage;
            SuccessRate = successRate;
            DurationFactor = durationFactor;
        }

        public string Name { get; private set; }

        public SupportLevel Level { get; private set; }

        public int HourlyWage { get; private set; }

        public decimal SuccessRate { get; private set; }
        public decimal DurationFactor { get; private set; }
    }
}
