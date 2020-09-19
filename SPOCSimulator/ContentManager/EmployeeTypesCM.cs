using Newtonsoft.Json;
using SPOCSimulator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPOCSimulator.ContentManager
{
    public class EmployeeTypesCM : BaseContentManager
    {
        public List<EmployeeType> EmployeeTypes = new List<EmployeeType>();

        public EmployeeTypesCM()
        {

        }

        public EmployeeType GetByName(string name)
        {
            return EmployeeTypes.Where(et => et.Name == name).Single();
        }

        public void Load(string filename)
        {
            var l = JsonConvert.DeserializeObject<List<EmployeeType>>(File.ReadAllText(filename));
            EmployeeTypes.AddRange(l);
        }

        public void Save(string filename)
        {
            var jsonString = JsonConvert.SerializeObject(EmployeeTypes, Formatting.Indented);
            File.WriteAllText(filename, jsonString);
        }

    }
}
