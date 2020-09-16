using Newtonsoft.Json;
using SPOCSimulator.ContentManager;
using SPOCSimulator.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace SPOCSimulator.Generator
{
    public class TicketsPerNDays : BaseContentManager
    {
        private Dictionary<int, Dictionary<int, int>> ticketsPerNDays = new Dictionary<int, Dictionary<int, int>>();

        public TicketsPerNDays()
        {

        }

        private void CheckOrGenerateDay(int day)
        {
            if(!ticketsPerNDays.ContainsKey(day))
            {
                ticketsPerNDays.Add(day, new Dictionary<int, int>());
                for (int h = 0; h < TicketsPerDayDistributionGenerator.HOURS_PER_DAY; h++)
                {
                    ticketsPerNDays[day].Add(h, 0);
                }
            }
        }

        public void Set(int day, int hour, int tickets)
        {
            CheckOrGenerateDay(day);
            ticketsPerNDays[day][hour] = tickets;
        }

        public int Get(int day, int hour)
        {
            return ticketsPerNDays[day][hour];
        }

        public void Load(string filename)
        {
            var l = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>(File.ReadAllText(filename));
            ticketsPerNDays = l;
        }

        public void Save(string filename)
        {
            var jsonString = JsonConvert.SerializeObject(ticketsPerNDays, Formatting.Indented);
            File.WriteAllText(filename, jsonString);
        }

    }
}
