using SPOCSimulator.Models;
using SPOCSimulator.Simulation.Ticker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Simulation
{
    public class MultiLevelTicketQueue
    {

        private Dictionary<SupportLevel, Queue<TicketEntity>> supportLevelToTicketQueue = new Dictionary<SupportLevel, Queue<TicketEntity>>();

        public MultiLevelTicketQueue()
        {
            foreach(var supportLevel in Enum.GetValues(typeof(SupportLevel)).Cast<SupportLevel>())
            {
                supportLevelToTicketQueue.Add(supportLevel, new Queue<TicketEntity>());
            }
        }

        public bool Available(SupportLevel supportLevel)
        {
            return supportLevelToTicketQueue[supportLevel].Count != 0;
        }

        public int Count(SupportLevel supportLevel)
        {
            return supportLevelToTicketQueue[supportLevel].Count;
        }

        public void Enqueue(SupportLevel supportLevel, TicketEntity entity)
        {
            supportLevelToTicketQueue[supportLevel].Enqueue(entity);
        }

        public void EscalateTo(SupportLevel supportLevel, TicketEntity entity)
        {
            if(Enum.GetValues(typeof(SupportLevel)).Cast<SupportLevel>().Any(t => t == (supportLevel))) {
                supportLevelToTicketQueue[supportLevel].Enqueue(entity);
                return;
            }
            throw new ArgumentException("Invalid support level!");
            
        }

        public TicketEntity Dequeue(SupportLevel supportLevel, out bool lowerLevel)
        {
            
            lock (supportLevelToTicketQueue)
            {
                lowerLevel = false;
                for (SupportLevel startLevel = supportLevel; startLevel  >= SupportLevel.Level1st; startLevel-- )
                {
                    if (supportLevelToTicketQueue[startLevel].TryDequeue(out var rv))
                    {
                        return rv;
                    }
                    lowerLevel = true;
                }
                return null;
            }
        }

    }
}
