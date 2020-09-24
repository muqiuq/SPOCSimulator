using SPOCSimulator.Generator;
using SPOCSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Simulation.Ticker
{
    public class NewTicketTicker : ITicker
    {
        private readonly MultiLevelTicketQueue inputQueue;
        private List<TicketEntity> tickets;

        public NewTicketTicker(TicketGenerationPlan plan, MultiLevelTicketQueue inputQueue)
        {
            tickets = plan.Tickets.OrderBy(t => t.createAtTicks).ToList();
            this.inputQueue = inputQueue;
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
                inputQueue.Enqueue(SupportLevel.Level1st, ticket);
            }
        }
    }
}
