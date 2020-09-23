using SPOCSimulator.Models;
using SPOCSimulator.Simulation.Ticker.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SPOCSimulator.Simulation.Ticker
{
    public class EmployeeTicker : ITicker
    {
        private readonly int id;
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

        private int doneTickets = 0;
        

        public EmployeeTicker(int id, TicketQueue inputQueue, TicketQueue doneOutputQueue, TicketQueue escalateOutputQueue, EmployeeType employeeType, int shiftEnd)
        {
            this.id = id;
            this.inputQueue = inputQueue;
            this.doneOutputQueue = doneOutputQueue;
            this.escalateOutputQueue = escalateOutputQueue;
            this.employeeType = employeeType;
            this.shiftEnd = shiftEnd;

            employeeTickerState = EmployeeTickerState.WarmUp;
            ticksToFinish = BoundaryConditions.EmployeeWarmUpDuration;
        }


        public bool Working
        {
            get
            {
                return employeeTickerState == EmployeeTickerState.Working;
            }
        }

        public bool Productive
        {
            get
            {
                return employeeTickerState == EmployeeTickerState.Working && currentTicket != null;
            }
        }

        public bool WarmUp
        {
            get
            {
                return employeeTickerState == EmployeeTickerState.WarmUp;
            }
        }

        public bool CleanUp
        {
            get
            {
                return employeeTickerState == EmployeeTickerState.CleanUp;
            }
        }

        public SupportLevel Level
        {
            get
            {
                return employeeType.Level;
            }
        }

        public void Tick(int day, int ticks)
        {
            if (employeeTickerState == EmployeeTickerState.WarmUp)
            {
                
                ticksToFinish--;
                if (ticksToFinish <= 0)
                {
                    Console.WriteLine(string.Format("{0} > ID {1} [{2}] WarmUp Done after {3}", ticks, id, employeeType.Level, workedTicks));
                    employeeTickerState = EmployeeTickerState.Working;
                }
                workedTicks++;
            }
            else if (employeeTickerState == EmployeeTickerState.Working)
            {
                workedTicks++;
                if (!HasTicket)
                {
                    if (ticks >= shiftEnd)
                    {
                        Console.WriteLine(string.Format("{0} > ID {1} [{2}] shifts done after {3}", ticks, id, employeeType.Level, workedTicks));
                        // Shift done go into cleanup
                        employeeTickerState = EmployeeTickerState.CleanUp;
                        ticksToFinish = BoundaryConditions.EmployeeCleanUpDuration;
                    }
                    else
                    {
                        // No ticket? Get a new one!
                        currentTicket = inputQueue.Dequeue();
                        if (!HasTicket) return;
                        ticksToFinish = currentTicket.TicksToSolve(employeeType.Level);
                        currentTicket.StartSolving(ticks);
                        Console.WriteLine(string.Format("{0} > ID {1} [{2}] Getting new ticket ({3})", ticks, id, employeeType.Level, workedTicks, ticksToFinish));
                    }
                }
                else if (ticksToFinish > 0)
                {
                    ticksToFinish--;
                }
                else
                {
                    doneTickets++;
                    if (currentTicket.MoreDifficultyThen(employeeType.Level))
                    {
                        if (employeeType.Level == SupportLevel.Level2nd) Debugger.Break();
                        currentTicket.StopSolving(ticks);
                        Console.WriteLine(string.Format("{0} > ID {1} [{2}] escalated ticket after ", ticks, id, employeeType.Level, workedTicks, currentTicket.Duration));
                        escalateOutputQueue.Enqueue(currentTicket);
                    }
                    else
                    {
                        currentTicket.StopSolving(ticks);
                        Console.WriteLine(string.Format("{0} > ID {1} [{2}] finished ticket after ", ticks, id, employeeType.Level, workedTicks, currentTicket.Duration));
                        doneOutputQueue.Enqueue(currentTicket);
                    }
                    currentTicket = null;
                }
            }
            else if (employeeTickerState == EmployeeTickerState.CleanUp)
            {
                workedTicks++;
                ticksToFinish--;
                if (ticksToFinish <= 0)
                {
                    Console.WriteLine(string.Format("{0} > ID {1} [{2}] Going home after {3} and doing {4} tickets", ticks, id, employeeType.Level, workedTicks, doneTickets));
                    employeeTickerState = EmployeeTickerState.Done;
                }
            }
        }



        public bool Destroyable()
        {
            if(employeeTickerState == EmployeeTickerState.Done)
            {
                Console.WriteLine(string.Format("[{2}] worked for {0} and did {1} tickets", workedTicks, doneTickets, employeeType.Level));
            }
            return employeeTickerState == EmployeeTickerState.Done;
        }
    }

}
