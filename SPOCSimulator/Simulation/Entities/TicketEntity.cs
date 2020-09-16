using MathNet.Numerics.Distributions;
using SPOCSimulator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Simulation.Entities
{
    public class TicketEntity
    {
        public int Number;

        public SupportLevel Difficulty { get; private set; }

        public Dictionary<SupportLevel, int> DifficultyToSolveDurationMin = new Dictionary<SupportLevel, int>();

        public int createTicks { get; private set; }

        public TicketEntity(int number, SupportLevel difficulty, Dictionary<SupportLevel, int> difficultyToSolveDurationMin, int createTicks)
        {
            Number = number;
            Difficulty = difficulty;
            DifficultyToSolveDurationMin = difficultyToSolveDurationMin;
            this.createTicks = createTicks;
        }


        public int? startTicks = null;
        public int? stopTicks = null;

        public void StartSolving(int ticks)
        {
            if (startTicks == null) startTicks = ticks;
        }

        public void StopSolving(int ticks)
        {
            if (stopTicks == null || stopTicks < ticks) stopTicks = ticks;
        }

        public bool MoreDifficultyThen(SupportLevel sp)
        {
            return sp < Difficulty;
        }

        public int TicksToSolve(SupportLevel sp)
        {
            return DifficultyToSolveDurationMin[sp];
        }

        public override string ToString()
        {
            string s = string.Format("N° {0} at {1}tick ({2}) Diffs:", Number, createTicks, Difficulty);
            foreach(var dm in DifficultyToSolveDurationMin)
            {
                s += string.Format("{0} => {1} ticks, ", dm.Key, dm.Value);
            }
            return s;
        }
    }
}
