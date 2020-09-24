using Newtonsoft.Json;
using SPOCSimulator.HIO;
using SPOCSimulator.HIO.Utils;
using SPOCSimulator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Models
{
    public class Workshift : IHIOOutput<WorkshiftHIO>
    {

        public int Begin
        {
            get; set;
        }
        public int End
        {
            get; set;
        }

        /// <summary>
        /// Type and count of each employee type. 
        /// </summary>
        public IDictionary<EmployeeType, int> EmployeeTypes = new Dictionary<EmployeeType, int>();

        public WorkshiftHIO ToHIO()
        {
            return new WorkshiftHIO()
            {
                ShiftBegin = Helper.TicksToTimeString(Begin),
                ShiftEnd = Helper.TicksToTimeString(End),
                EmployeeTypeNames = EmployeeTypes.ToDictionary(o => o.Key.Name, o => o.Value)
            };
        }
    }
}
