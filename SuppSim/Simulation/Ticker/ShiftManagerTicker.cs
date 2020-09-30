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
        private MultiLevelTicketQueue inputQueue;
        private TicketQueue doneQueue;

        private int ContinousEmployeeId = 0;
        private Accounting accounting;

        public delegate void LogDelegate(string text);
        public event LogDelegate LogEvent;

        public ShiftManagerTicker(
            ITickerManager tickerManager,
            WorkshiftsCM workshiftsCM,
            MultiLevelTicketQueue inputQueue,
            TicketQueue doneQueue,
            Accounting accounting)
        {
            this.workshiftsCM = workshiftsCM;
            this.tickerManager = tickerManager;
            this.inputQueue = inputQueue;
            this.doneQueue = doneQueue;
            this.accounting = accounting;
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
                        var employeeTicker = new EmployeeTicker(ContinousEmployeeId,
                            inputQueue,
                            doneQueue,
                            employeeTypeAndAmount.Key,
                            day * BoundaryConditions.DayLength + startingShift.End,
                            accounting);

                        employeeTicker.LogEvent += EmployeeTicker_LogEvent;

                        tickerManager.Add(employeeTicker);

                        ContinousEmployeeId++;
                    }
                }
            }
        }

        private void EmployeeTicker_LogEvent(string text)
        {
            LogEvent?.Invoke(text);
        }
    }
}
