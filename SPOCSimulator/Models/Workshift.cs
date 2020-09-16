using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Workshift
    {

        public int? _start = null;
        public int? _stop = null;

        public int Start { get
            {
                if (_start == null)
                {
                    TimeSpan ts = TimeSpan.Parse(StartTime);
                    _start = (int)ts.TotalMinutes;
                }
                return _start.Value;
            }
        }
        public int Stop
        {
            get
            {
                if (_start == null)
                {
                    TimeSpan ts = TimeSpan.Parse(StartTime);
                    _stop = (int)ts.TotalMinutes;
                }
                return _start.Value;
            }
        }

        [JsonProperty]
        public string StartTime { get; set; }
        
        [JsonProperty]
        public string StopTime { get; set; }

        
        public IDictionary<EmployeeType, int> EmployeeTypes = new Dictionary<EmployeeType, int>();

        [JsonProperty]
        public IDictionary<string, int> EmployeeNames = new Dictionary<string, int>();

    }
}
