using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public class GameLogic
    {
        private Table table;
        private float _friction;

        public float friction
        {
            get
            {
                return _friction;
            }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException("friction", "Tarcie musi być z przedziału [0, 1]");
                }
                _friction = value;
            }
        }

        public GameLogic(Table t, float friction)
        {
            table = t;
            this._friction = friction;
        }

        public void Start()
        {
            table.pilki.Add(new Ball(100, 100, 25, 0 ,3));
            table.pilki.Add(new Ball(100, 300, 25, 0, 3));
            table.pilki.Add(new Ball(500, 400, 25, 2, 3));
        }

        public void Move(Ball pilka)
        {
            float new_x = pilka.x + pilka.vx;
            float new_y = pilka.y + pilka.vy;

            if (checkCollision())
            {
                Debug.WriteLine("Kolizja");
                pilka.vx = -pilka.vx;
                pilka.vy = -pilka.vy;
            }

            if (new_x - pilka.r <= 0 || new_x + pilka.r >= table.width)
            {
                pilka.vx = -pilka.vx;
            }

            if (new_y - pilka.r <= 0 || new_y + pilka.r >= table.height)
            {
                pilka.vy = -pilka.vy;
            }
            if(new_x - pilka.r <= 0 || new_x + pilka.r >= table.width || new_y - pilka.r <= 0 || new_y + pilka.r >= table.height)
            {
                pilka.vx = pilka.vx * 0.75f;
                pilka.vy = pilka.vy * 0.75f;
            }

            pilka.vx *= friction;
            pilka.vy *= friction;

            pilka.x += pilka.vx;
            pilka.y += pilka.vy;
        }


        public Ball getBall(int choose)
        {
            if (choose < 0 || choose >= getBallsCount())
            {
                throw new ArgumentOutOfRangeException("choose", "Nie ma piłki o takim indeksie");
            }
            return table.pilki[choose];
        }

        public bool checkCollision()
        {
            for(int i = 0; i < table.pilki.Count;i++)
            {
                for(int j = 0; j < table.pilki.Count; j++)
                {
                    if (i != j)
                    {

                        float distance_x = table.pilki[i].x - table.pilki[j].x;
                        float distance_y = table.pilki[i].y  - table.pilki[j].y;
                        float distance = distance_x * distance_x + distance_y * distance_y;
                        Debug.WriteLine($"Odległość między piłkami {i} i {j} wynosi {distance}");
                        if (distance <= table.pilki[i].r * table.pilki[i].r + table.pilki[j].r * table.pilki[j].r)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public int getBallsCount()
        {
            return table.pilki.Count;
        }

    }
}
