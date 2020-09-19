using SPOCSimulator.Models;
using SPOCSimulator.Simulation.Ticker.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Simulation.Ticker
{
    public class EmployeeTicker : ITicker
    {
        private TicketQueue inputQueue;

        private TicketQueue doneOutputQueue;

        private TicketQueue escalateOutputQueue;

        private EmployeeType employeeType;

        private int shiftEnd;

        private EmployeeTickerState employeeTickerState;


        public bool HasTicket
        {
            get => currentTicket != null;
        }

        private TicketEntity currentTicket;

        private int ticksToFinish;

        private int workedTicks = 0;
        

        public EmployeeTicker(TicketQueue inputQueue, TicketQueue doneOutputQueue, TicketQueue escalateOutputQueue, EmployeeType employeeType, int shiftEnd)
        {
            this.inputQueue = inputQueue;
            this.doneOutputQueue = doneOutputQueue;
            this.escalateOutputQueue = escalateOutputQueue;
            this.employeeType = employeeType;
            this.shiftEnd = shiftEnd;

            employeeTickerState = EmployeeTickerState.WarmUp;
            ticksToFinish = BoundaryConditions.EmployeeWarmUpDuration;
        }

        public void Tick(int day, int ticks)
        {
            

            if (employeeTickerState == EmployeeTickerState.WarmUp)
            {
                ticksToFinish--;
                if (ticksToFinish <= 0) employeeTickerState = EmployeeTickerState.Working;
                workedTicks++;
            }
            else if (employeeTickerState == EmployeeTickerState.Working)
            {
                workedTicks++;
                if (!HasTicket)
                {
                    if (ticks >= shiftEnd)
                    {
                        // Shift done go into cleanup
                        employeeTickerState = EmployeeTickerState.Working;
                        ticksToFinish = BoundaryConditions.EmployeeCleanUpDuration;
                    }
                    else
                    {
                        // No ticket? Get a new one!
                        currentTicket = inputQueue.Dequeue();
                        if (!HasTicket) return;
                        ticksToFinish = currentTicket.TicksToSolve(employeeType.Level);
                        currentTicket.StartSolving(ticks);
                    }
                }
                else if (ticksToFinish > 0)
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
            else if (employeeTickerState == EmployeeTickerState.CleanUp)
            {
                workedTicks++;
                ticksToFinish--;
                if (ticksToFinish <= 0) employeeTickerState = EmployeeTickerState.Done;
            }
        }

        public bool Destroyable()
        {
            return employeeTickerState == EmployeeTickerState.Done;
        }
    }

}
