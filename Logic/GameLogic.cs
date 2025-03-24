using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public class GameLogic
    {
        private List<Ball> pilki;
        private Table table;

        public GameLogic(Table t)
        {
            pilki = new List<Ball>();
            table = t;
        }

        public void Start()
        {
            pilki.Add(new Ball(100, 100, 25, 2 ,3));
            pilki.Add(new Ball(3, 7, 25, 2, 3));
            pilki.Add(new Ball(8, 2, 25, 2, 3));
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
        public void Move(Ball pilka)
        {
            int new_x = pilka.x + pilka.vx;
            int new_y = pilka.y + pilka.vy;

            // Odbicie od ściany lewej i prawej
            if (new_x - pilka.r <= 0 || new_x + pilka.r >= table.width)
            {
                pilka.vx = -pilka.vx;
            }

            // Odbicie od ściany górnej i dolnej
            if (new_y - pilka.r <= 0 || new_y + pilka.r >= table.height)
            {
                pilka.vy = -pilka.vy;
            }
            if(new_x - pilka.r <= 0 || new_x + pilka.r >= table.width || new_y - pilka.r <= 0 || new_y + pilka.r >= table.height)
            {
                pilka.vx = (int)(pilka.vx * 0.75);
                pilka.vy = (int)(pilka.vy * 0.75);
            }
            pilka.x += pilka.vx;
            pilka.y += pilka.vy;
        }


        public Ball getBall(int choose)
        {
            if (choose < 0 || choose >= pilki.Count)
            {
                throw new ArgumentOutOfRangeException("choose", "Nie ma piłki o takim indeksie");
            }
            return pilki[choose];
        }
    }
}
