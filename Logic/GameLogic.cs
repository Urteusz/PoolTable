using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public class GameLogic
    {
        private bool isRunning = false;
        private List<Ball> pilki;

        public void Start()
        {
            pilki = new List<Ball>
            {
                new Ball(5, 5),
                new Ball(3, 7),
                new Ball(8, 2)
            };

            isRunning = true;

            // Uruchomienie pętli gry w tle
            Task.Run(Loop);

            // Uruchomienie nasłuchiwania klawiatury w tle
            Task.Run(HandleInput);
        }

        private void Loop()
        {
            while (isRunning)
            {
                CheckBallsPositions();
                Thread.Sleep(10);
            }
        }

        private void CheckBallsPositions()
        {
            Console.Clear();
            Console.WriteLine("Pozycje piłek:");
            foreach (var pilka in pilki)
            {
                Console.WriteLine($"Piłka na pozycji: X = {pilka.x}, Y = {pilka.y}");
            }
        }

        private void MoveBalls()
        {
            foreach (var pilka in pilki)
            {
                pilka.Move(pilka.x + 1, pilka.y);
            }
        }

        private void HandleInput()
        {
            Console.CursorVisible = false; // Ukrycie kursora, żeby nie migał
            while (isRunning)
            {
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Q)
                    {
                        Stop();
                    }
                    if (key.Key == ConsoleKey.W)
                    {
                        MoveBalls();
                    }
                }
                Thread.Sleep(50);
            }
        }

        public void Stop()
        {
            isRunning = false;
            Console.WriteLine("Gra zatrzymana.");
        }
    }
}
