using System;
using System.Collections.Generic;
using System.Linq;

namespace Zork
{
    public class Chance
    {
        private static Random rng = new Random();

        public static bool Percentage(int percentChance)
        {
            return rng.Next(0, 100) < percentChance;
        }

        public static T RandomElement<T>(T[] options)
        {
            return RandomElement(options.ToList());
        }

        public static T RandomElement<T>(List<T> options)
        {
            return options[rng.Next(0, options.Count)];
        }

        public static int Between(int minValue, int maxValue)
        {
            return rng.Next(minValue, maxValue);
        }
    }
}
