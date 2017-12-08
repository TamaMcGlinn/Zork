using System;

namespace Zork
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Game gameObject = new Game();
            try
            {
                gameObject.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
