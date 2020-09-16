using Newtonsoft.Json;
using SPOCSimulator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SPOCSimulator.ContentManager
{
    public class Workshifts
    {
        private IList<Workshift> Data = new List<Workshift>();

        public void Add(Workshift w)
        {
            Data.Add(w);
        }

        public void Load(string filename)
        {
            var l = JsonConvert.DeserializeObject<List<Workshift>>(File.ReadAllText(filename));
            Data = l;
        }

        public void Save(string filename)
        {
            var jsonString = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(filename, jsonString);
        }
    }
}
