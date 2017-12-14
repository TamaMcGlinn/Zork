using System;

namespace Zork
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth - 18, Console.LargestWindowHeight - 8);
            Game gameObject = new Game();
            gameObject.Run();
        }
    }
}
