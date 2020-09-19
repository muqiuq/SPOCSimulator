using SPOCSimulator.ContentManager;
using SPOCSimulator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Generator
{
    public class ExampleGenerator 
    {

        public static EmployeeTypesCM GetEmployeeTypeContentManager()
        {
            var e = new EmployeeTypesCM();

            e.EmployeeTypes.Add(new Models.EmployeeType("Susanne", level: Models.SupportLevel.Level1st, hourlyWage: 100000, successRate: 1, durationFactor: 1));
            e.EmployeeTypes.Add(new Models.EmployeeType("Peter", level: Models.SupportLevel.Level2nd, hourlyWage: 100000, successRate: 1, durationFactor: 1));

            return e;
        }

        public static WorkshiftsCM GetWorkshifts()
        {
            var ws = new WorkshiftsCM(null);

            EmployeeType etS = new EmployeeType("Susanne", SupportLevel.Level1st, 60, 1, 1);
            EmployeeType etP = new EmployeeType("Peter", SupportLevel.Level1st, 60, 1, 1);

            ws.Add(new Workshift() { Begin = 540, End = 1080, EmployeeTypes = { { etS, 3 }, { etP, 2 } } });
            ws.Add(new Workshift() {Begin = 720, End = 1260, EmployeeTypes = { { etS, 3 }, { etP, 2 } } });

            return ws;
        }

    }
}
