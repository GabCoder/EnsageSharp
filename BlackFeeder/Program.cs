using System;

namespace BlackFeeder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Feeder.OnLoad();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }
    }
}