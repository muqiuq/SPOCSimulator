using Newtonsoft.Json;
using SPOCSimulator.ContentManager;
using SPOCSimulator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPOCSimulator.Generator
{
    public class TicketsPerDayDistributionGenerator : BaseContentManager
    {

        public const int HOURS_PER_DAY = 24;

        Dictionary<int, decimal> TicketsPerHourInDayDistribution = new Dictionary<int, decimal>();

        public bool Validate()
        {
            return (TicketsPerHourInDayDistribution.Values.Sum(x => x) == 100);
        }

        public TicketsPerDayDistributionGenerator()
        {
            for(int a = 0; a < HOURS_PER_DAY; a++)
            {
                TicketsPerHourInDayDistribution.Add(a, 0);
            }
        }

        public decimal Get(int hour)
        {
            return TicketsPerHourInDayDistribution[hour];
        }

        public void Load(string filename)
        {
            var l = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(File.ReadAllText(filename));
            TicketsPerHourInDayDistribution = l;
            if (!Validate()) throw new InvalidDataException("Invalid sum! (should be "  + TicketsPerHourInDayDistribution.Values.Sum(x => x).ToString() +  "/100)");
        }

        public void Save(string filename)
        {
            var jsonString = JsonConvert.SerializeObject(TicketsPerHourInDayDistribution, Formatting.Indented);
            File.WriteAllText(filename, jsonString);
        }


    }
}
