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
                g.Run();
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
