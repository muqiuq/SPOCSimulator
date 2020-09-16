using SPOCSimulator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SPOCSimulator.ContentManager
{
    public class EmployeeTypeContentManager : BaseContentManager
    {
        public List<EmployeeType> EmployeeTypes = new List<EmployeeType>();

        public EmployeeTypeContentManager()
        {

        }

        public void Load(string filename)
        {
            var l = JsonSerializer.Deserialize<List<EmployeeType>>(File.ReadAllText(filename), JsonOptions);
            EmployeeTypes.AddRange(l);
        }

        public void Save(string filename)
        {
            var jsonString = JsonSerializer.Serialize(EmployeeTypes, JsonOptions);
            File.WriteAllText(filename, jsonString);
        }

    }
}
