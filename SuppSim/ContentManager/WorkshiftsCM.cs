using Newtonsoft.Json;
using SPOCSimulator.HIO;
using SPOCSimulator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SPOCSimulator.ContentManager
{
    public class WorkshiftsCM
    {
        private IList<Workshift> Data = new List<Workshift>();
        private EmployeeTypesCM employeeTypeContentManager;

        public WorkshiftsCM(EmployeeTypesCM employeeTypeContentManager)
        {
            this.employeeTypeContentManager = employeeTypeContentManager;
        }

        public void Add(Workshift w)
        {
            Data.Add(w);
        }

        public IList<Workshift> GetAll()
        {
            return Data;
        }

        public Workshift ParseWorkshiftHIO(WorkshiftHIO wsHIO)
        {
            var ws = new Workshift()
            {
                Begin = (int)TimeSpan.Parse(wsHIO.ShiftBegin).TotalMinutes,
                End = (int)TimeSpan.Parse(wsHIO.ShiftEnd).TotalMinutes
            };
            foreach (var employeeName in wsHIO.EmployeeTypeNames)
            {
                ws.EmployeeTypes.Add(employeeTypeContentManager.GetByName(employeeName.Key), employeeName.Value);
            }
            return ws;
        }

        public void Load(string filename)
        {
            var l = JsonConvert.DeserializeObject<List<WorkshiftHIO>>(File.ReadAllText(filename));
            Data = l.Select(i => ParseWorkshiftHIO(i)).ToList();
        }

        public void Save(string filename)
        {
            var jsonString = JsonConvert.SerializeObject(Data.Select(d => d.ToHIO()).ToList(), Formatting.Indented);
            File.WriteAllText(filename, jsonString);
        }
    }
}
