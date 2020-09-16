using SPOCSimulator.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Simulation.Entities
{
    public class EmployeeTicker : ITicker
    {
        private TicketQueue inputQueue;

        private TicketQueue doneOutputQueue;

        private TicketQueue escalateOutputQueue;

        private EmployeeType employeeType;


        public bool HasTicket
        {
            get => currentTicket != null;
        }

        private TicketEntity currentTicket;

        private int ticksToFinish;

        public EmployeeTicker(TicketQueue inputQueue, TicketQueue doneOutputQueue, TicketQueue escalateOutputQueue, EmployeeType employeeType)
        {
            this.inputQueue = inputQueue;
            this.doneOutputQueue = doneOutputQueue;
            this.escalateOutputQueue = escalateOutputQueue;
            this.employeeType = employeeType;
        }

        public void Tick(int day, int ticks)
        {
            if(!HasTicket)
            {
                // No ticket? Get a new one!
                currentTicket = inputQueue.Dequeue();
                if (!HasTicket) return;
                ticksToFinish = currentTicket.TicksToSolve(employeeType.Level);
                currentTicket.StartSolving(ticks);
            }
            else if(ticksToFinish > 0)
            {
                ticksToFinish--;
            }
            else
            {
                if (currentTicket.MoreDifficultyThen(employeeType.Level))
                {
                    currentTicket.StopSolving(ticks);
                    escalateOutputQueue.Enqueue(currentTicket);
                }
                else
                {
                    doneOutputQueue.Enqueue(currentTicket);
                }
                currentTicket = null;
            }
        }
    }
}
