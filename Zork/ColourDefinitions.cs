using System;
using Zork.UIContext;

namespace Zork
{
    public static class ColorDefinitions
    {
        /// <summary>
        /// Show all available console colors for background and foreground
        /// </summary>
        public static void TestColors()
        {
            foreach (ConsoleColor backgroundColor in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.BackgroundColor = backgroundColor;
                foreach (ConsoleColor foregroundColor in Enum.GetValues(typeof(ConsoleColor)))
                {
                    Console.ForegroundColor = foregroundColor;
                    ColorContext.WriteFullLine($"{foregroundColor} on {backgroundColor}");
                }
            }
        }
    }
}
