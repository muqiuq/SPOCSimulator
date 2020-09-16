using SPOCSimulator.Simulation.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Simulation
{
    public class TicketQueue
    {

        private ConcurrentQueue<TicketEntity> queue = new ConcurrentQueue<TicketEntity>();

        public bool Available()
        {
            return !queue.IsEmpty;
        }

        public void Enqueue(TicketEntity entity)
        {
            queue.Enqueue(entity);
        }

        public TicketEntity Dequeue()
        {
            lock(queue)
            {
                if (queue.TryDequeue(out var rv))
                {
                    return rv;
                }
                return null;
            }
        }

    }
}
