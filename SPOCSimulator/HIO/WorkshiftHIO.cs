using Newtonsoft.Json;
using SPOCSimulator.HIO.Utils;
using SPOCSimulator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.HIO
{
    // HIO = Humon Input Object
    public class WorkshiftHIO : IHIO
    {

        [JsonProperty]
        [JsonRequired]
        public string ShiftBegin { get; set; }

        [JsonProperty]
        [JsonRequired]
        public string ShiftEnd { get; set; }

        [JsonProperty]
        [JsonRequired]
        public IDictionary<string, int> EmployeeTypeNames = new Dictionary<string, int>();




            
        
    }
}
