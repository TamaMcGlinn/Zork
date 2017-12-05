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
            catch (StackOverflowException e)
            {
                Console.WriteLine("Stackoverflow");
                Console.WriteLine(e.Message);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Null reference");
                Console.WriteLine(e.Message);
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("Out of memory");
                Console.WriteLine(e.Message);
            }
        }
    }
}
