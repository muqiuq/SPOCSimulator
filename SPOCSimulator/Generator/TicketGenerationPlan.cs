using SPOCSimulator.Simulation.Ticker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SPOCSimulator.Generator
{
    public class TicketGenerationPlan
    {

        public List<TicketEntity> Tickets { get; } = new List<TicketEntity>();

        public TicketGenerationPlan()
        {

        }

        public void Save(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                foreach (var ticket in Tickets)
                {
                    writer.WriteLine(ticket.ToCSV());
                }
            }
        }

        public void Load(string filename)
        {
            using(StreamReader reader = new StreamReader(filename, Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine().Trim();
                    if (line == "") continue;
                    Tickets.Add(TicketEntity.FromCSV(line));
                }
            }
        }
    }
}
