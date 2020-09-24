using Org.BouncyCastle.Security;
using SPOCSimulator.Models;
using SPOCSimulator.Simulation.Ticker.Helper;
using SPOCSimulator.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SPOCSimulator.Simulation.Ticker
{
    public class EmployeeTicker : ITicker
    {
        private readonly int id;
        private MultiLevelTicketQueue inputQueue;

        private TicketQueue doneOutputQueue;

        private EmployeeType employeeType;

        private int shiftEnd;

        private EmployeeTickerState employeeTickerState;

        private EquallyDistributedNumber equallyDistributedNumber = new EquallyDistributedNumber(1, 100);

        private double EfficencyDecayValue = BoundaryConditions.EmployeeEfficencyDecayStartValue;

        public bool HasTicket
        {
            get => currentTicket != null;
        }

        private TicketEntity currentTicket;

        private int ticksToFinish;

        private int workedTicks = 0;

        private int doneTickets = 0;

        private Accounting accounting;


        public EmployeeTicker(int id, MultiLevelTicketQueue inputQueue, TicketQueue doneOutputQueue, EmployeeType employeeType, int shiftEnd, Accounting accounting)
        {
            this.id = id;
            this.inputQueue = inputQueue;
            this.doneOutputQueue = doneOutputQueue;
            this.employeeType = employeeType;
            this.shiftEnd = shiftEnd;

            employeeTickerState = EmployeeTickerState.WarmUp;
            ticksToFinish = (int)(BoundaryConditions.EmployeeWarmUpDuration * employeeType.DurationFactor);
            this.accounting = accounting;
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
            if(workedTicks > BoundaryConditions.EmployeeEfficencyDecayStartTicks)
            {
                if(workedTicks % BoundaryConditions.EmployeeEfficencyDecayInterval == 0)
                {
                    EfficencyDecayValue = EfficencyDecayValue * BoundaryConditions.EmployeeEfficencyDecayFactor;
                }
            }
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
                        ticksToFinish = (int)(BoundaryConditions.EmployeeCleanUpDuration * employeeType.DurationFactor);
                    }
                    else
                    {
                        // No ticket? Get a new one!
                        currentTicket = inputQueue.Dequeue(employeeType.Level, out var lowerLevel);
                        if (!HasTicket) return;

                        ticksToFinish = (int)(currentTicket.TicksToSolve(employeeType.Level) * employeeType.DurationFactor);
                        /*if(lowerLevel)
                        {
                            ticksToFinish += currentTicket.TicksToSolve(employeeType.Level-1);
                        }*/
                        var oldTicksToFinish = ticksToFinish;
                        if (workedTicks > BoundaryConditions.EmployeeEfficencyDecayStartTicks)
                        {
                            ticksToFinish = (int)(ticksToFinish * (1d + EfficencyDecayValue));
                        }
                        ticksToFinish = (int)Math.Min((currentTicket.TicksToSolve(employeeType.Level) * 3), ticksToFinish);
                        // Dirty security hack in case of over/underflow
                        if (oldTicksToFinish > ticksToFinish) ticksToFinish = oldTicksToFinish;
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
                    if(equallyDistributedNumber.Next() > (employeeType.SuccessRate * 100))
                    {
                        Console.WriteLine(string.Format("{0} > ID {1} [{2}] failed to solve ticket ", ticks, id, employeeType.Level, workedTicks, currentTicket.Duration));
                        currentTicket.StopSolving(ticks);
                        inputQueue.Enqueue(employeeType.Level, currentTicket);
                    }
                    else if (currentTicket.MoreDifficultyThen(employeeType.Level))
                    {
                        if (employeeType.Level == SupportLevel.Level2nd) Debugger.Break();
                        currentTicket.StopSolving(ticks);
                        Console.WriteLine(string.Format("{0} > ID {1} [{2}] escalated ticket after ", ticks, id, employeeType.Level, workedTicks, currentTicket.Duration));
                        inputQueue.Enqueue(employeeType.Level+1, currentTicket);
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
                    accounting.Book(employeeType.HourlyWage, workedTicks);
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
