using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SPOCSimulator.Utils
{
    public class Helper
    {


        public static bool DiffersFromThreshold(int threshold, params bool[] bools)
        { 
            return new List<bool>(bools).Count(b => b) != threshold; 
        }

        private static Dictionary<string, TValue> ToStringKeyDictionary<TKey, TValue>(IDictionary<TKey, TValue> map)
        {
            var rv = new Dictionary<string, TValue>();
            foreach(var kp in map)
            {
                var val = kp.Value;
                rv.Add(kp.Key.ToString(), kp.Value);
            }
            return rv;
        }

        private static Dictionary<int, TValue> ToIntegerDictionary<TValue>(Dictionary<string, TValue> map)
        {
            var rv = new Dictionary<int, TValue>();
            foreach (var kp in map)
            {
                rv.Add(int.Parse(kp.Key), kp.Value);
            }
            return rv;
        }

        public static string TicksToTimeString(int ticks)
        {
            var hours = (ticks / 60) % 23;
            var min = ticks % 60;
            return string.Format("{0:00}:{1:00}", hours, min);
        }
    }
}
