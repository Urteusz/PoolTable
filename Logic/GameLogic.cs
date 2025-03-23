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
            pilki.Add(new Ball(5, 5, 25));
            pilki.Add(new Ball(3, 7, 25));
            pilki.Add(new Ball(8, 2, 25));
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
        public void Move(Ball pilka, int x_dodaj, int y_dodaj)
        {
            int temp_x = pilka.x + x_dodaj + pilka.r;
            int temp_y = pilka.y + y_dodaj + pilka.r;
            if(temp_x > 0 && temp_x < table.width && temp_y > 0 && temp_y < table.height)
            {
                pilka.x = pilka.x + x_dodaj;
                pilka.y = pilka.y + y_dodaj;
            }

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
