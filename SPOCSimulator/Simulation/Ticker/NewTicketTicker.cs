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
            var ticket = tickets.FirstOrDefault();
            while(ticket != null && ticket.createAtTicks <= ticks)
            {
                primaryInputQueue.Enqueue(ticket);
                ticket = tickets.FirstOrDefault();
            }
        }
    }
}
