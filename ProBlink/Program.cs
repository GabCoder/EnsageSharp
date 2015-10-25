using System;

namespace ProBlink
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Blink.OnLoad();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }
    }
}