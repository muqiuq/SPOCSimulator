using MathNet.Numerics.Distributions;
using SPOCSimulator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.Simulation.Ticker
{
    public class TicketEntity
    {
        public int Number;

        public SupportLevel Difficulty { get; private set; }

        public Dictionary<SupportLevel, int> DifficultyToSolveDurationMin = new Dictionary<SupportLevel, int>();

        public int createAtTicks { get; private set; }

        public TicketEntity(int number, SupportLevel difficulty, Dictionary<SupportLevel, int> difficultyToSolveDurationMin, int createAtTicks)
        {
            Number = number;
            Difficulty = difficulty;
            DifficultyToSolveDurationMin = difficultyToSolveDurationMin;
            this.createAtTicks = createAtTicks;
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
            string s = string.Format("N° {0} at {1}tick ({2}) Diffs:", Number, createAtTicks, Difficulty);
            foreach(var dm in DifficultyToSolveDurationMin)
            {
                s += string.Format("{0} => {1} ticks, ", dm.Key, dm.Value);
            }
            return s;
        }

        public string ToCSV()
        {
            var baseInfo = string.Format("{0};{1};{2};",
                Number,
                Difficulty,
                createAtTicks);
            foreach (var dm in DifficultyToSolveDurationMin)
            {
                baseInfo += string.Format("{0};{1};", dm.Key, dm.Value);
            }
            return baseInfo;
        }

        public static TicketEntity FromCSV(string csv)
        {
            var parts = csv.Split(";");
            if (parts.Length < 3) throw new FormatException("Invalid Format for TicketEntity(1)");
            Dictionary<SupportLevel, int> difficultyToSolveDurationMin = new Dictionary<SupportLevel, int>();
            int pLength = parts.Length;
            if (pLength % 2 == 0) pLength -= 1;
            for(int i = 3; i < pLength; i += 2)
            {
                difficultyToSolveDurationMin.Add((SupportLevel)Enum.Parse(typeof(SupportLevel), parts[i]), Int32.Parse(parts[i + 1]));
            }

            return new TicketEntity(
                int.Parse(parts[0]),
                (SupportLevel)Enum.Parse(typeof(SupportLevel), parts[1]),
                difficultyToSolveDurationMin,
                int.Parse(parts[2])
                );
        }
    }
}
