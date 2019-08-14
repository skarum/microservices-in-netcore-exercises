using System;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            new EventSubscriber("localhost:5000");
            Console.ReadLine();
        }
    }
}