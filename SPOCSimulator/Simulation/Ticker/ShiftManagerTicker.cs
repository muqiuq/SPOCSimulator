using SPOCSimulator.ContentManager;
using SPOCSimulator.Simulation.Ticker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOCSimulator.Simulation.Ticker
{
    public class ShiftManagerTicker : ITicker
    {
        private WorkshiftsCM workshiftsCM;
        private ITickerManager tickerManager;
        private TicketQueue primaryInputQueue;
        private TicketQueue doneQueue;
        private TicketQueue firstToSecondLevelQueue;

        private int ContinousEmployeeId = 0;

        public ShiftManagerTicker(
            ITickerManager tickerManager, 
            WorkshiftsCM workshiftsCM,
            TicketQueue primaryInputQueue,
            TicketQueue doneQueue,
            TicketQueue firstToSecondLevelQueue)
        {
            this.workshiftsCM = workshiftsCM;
            this.tickerManager = tickerManager;
            this.primaryInputQueue = primaryInputQueue;
            this.doneQueue = doneQueue;
            this.firstToSecondLevelQueue = firstToSecondLevelQueue;
        }

        public bool Destroyable()
        {
            return false;
        }

        public void Tick(int day, int ticks)
        {
            var ticksInDay = ticks % BoundaryConditions.DayLength;
            var startingShifts = workshiftsCM.GetAll().Where(i => i.Begin == ticksInDay).ToList();
            if (!startingShifts.Any()) return;
            foreach(var startingShift in startingShifts)
            {
                foreach(var employeeTypeAndAmount in startingShift.EmployeeTypes)
                {
                    for(int a = 0; a < employeeTypeAndAmount.Value; a++)
                    {
                        tickerManager.Add(new EmployeeTicker(ContinousEmployeeId, 
                            primaryInputQueue, 
                            doneQueue, 
                            firstToSecondLevelQueue, 
                            employeeTypeAndAmount.Key, 
                            day * BoundaryConditions.DayLength +  startingShift.End));

                        ContinousEmployeeId++;
                    }
                }
            }
        }
    }
}
