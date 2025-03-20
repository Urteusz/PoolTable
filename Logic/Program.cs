using System;

namespace Logic
{
    class Program
    {
        static void Main()
        {
            GameLogic game = new GameLogic();
            game.Start();
            Console.ReadKey();
            game.Stop();
        }
    }
}
