using SPOCSimulator.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Simulation.Ticker
{
    public class NewTicketTicker : ITicker
    {
        private readonly TicketQueue primaryInputQueue;
        private List<TicketEntity> tickets;

        public NewTicketTicker(TicketGenerationPlan plan, TicketQueue primaryInputQueue)
        {
            tickets = plan.Tickets.OrderBy(t => t.createAtTicks).ToList();
            this.primaryInputQueue = primaryInputQueue;
        }

        public bool Destroyable()
        {
            return false;
        }

        public void Tick(int day, int ticks)
        {
            var ticketsToAdd = tickets.Where(i => i.createAtTicks == ticks).ToList();
            foreach(var ticket in ticketsToAdd)
            {
                ticket.SetDeployed(ticks);
                primaryInputQueue.Enqueue(ticket);
            }
        }
    }
}
