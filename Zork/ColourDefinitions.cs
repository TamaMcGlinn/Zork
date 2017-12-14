using System;
using Zork.UIContext;

namespace Zork
{
    public static class ColourDefinitions
    {
        /// <summary>
        /// Show all available console colours for background and foreground
        /// </summary>
        public static void TestColors()
        {
            foreach (ConsoleColor backgroundColor in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.BackgroundColor = backgroundColor;
                foreach (ConsoleColor foregroundColor in Enum.GetValues(typeof(ConsoleColor)))
                {
                    Console.ForegroundColor = foregroundColor;
                    ColourContext.WriteFullLine($"{foregroundColor} on {backgroundColor}");
                }
            }
        }
    }
}
