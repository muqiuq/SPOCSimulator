using SPOCSimulator.ContentManager;
using SPOCSimulator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Generator
{
    public class ExampleGenerator 
    {

        public static EmployeeTypeContentManager GetEmployeeTypeContentManager()
        {
            var e = new EmployeeTypeContentManager();

            e.EmployeeTypes.Add(new Models.EmployeeType("Susanne", level: Models.SupportLevel.Level1st, hourlyWage: 100000, successRate: 1, durationFactor: 1));
            e.EmployeeTypes.Add(new Models.EmployeeType("Peter", level: Models.SupportLevel.Level2nd, hourlyWage: 100000, successRate: 1, durationFactor: 1));

            return e;
        }

        public static Workshifts GetWorkshifts()
        {
            var ws = new Workshifts();

            ws.Add(new Workshift() {StartTime = "09:00", StopTime="18:00", EmployeeNames = { { "Susanne" , 10 }, { "Peter", 5} } });

            return ws;
        }

    }
}
