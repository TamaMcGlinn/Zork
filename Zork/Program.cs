using System;

namespace Zork
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Game g = new Game();
            try
            {
                g.run();
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
