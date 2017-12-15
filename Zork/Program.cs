using System;

namespace Zork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.SetWindowSize(Console.LargestWindowWidth - 18, Console.LargestWindowHeight - 8);
            Game gameObject = new Game();
            gameObject.Run();
            Console.Read();
        }
    }
}
