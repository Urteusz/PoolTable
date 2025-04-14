using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public class GameLogic : IGameLogic
    {
        private readonly ITable tableAPI;
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
            tableAPI = t;
            this._friction = friction;
        }
        public void Move(IBall pilka)
        {
            float new_x = pilka.x + pilka.vx;
            float new_y = pilka.y + pilka.vy;

            // Odbicia od ścian
            if (new_x - pilka.r <= 0 || new_x + pilka.r >= tableAPI.width)
            {
                pilka.vx = -pilka.vx * 0.95f; // odbicie + tłumienie
            }

            if (new_y - pilka.r <= 0 || new_y + pilka.r >= tableAPI.height)
            {
                pilka.vy = -pilka.vy * 0.95f;
            }

            // tarcie
            pilka.vx *= friction;
            pilka.vy *= friction;

            pilka.x += pilka.vx;
            pilka.y += pilka.vy;
        }

        //public void Move(IBall pilka)
        //{

        //    float new_x = pilka.x + pilka.vx;
        //    float new_y = pilka.y + pilka.vy;

        //    if (CheckAllCollision())
        //    {
        //        Debug.WriteLine("Kolizja");
        //        pilka.vx = -pilka.vx;
        //        pilka.vy = -pilka.vy;
        //    }

        //    if (new_x - pilka.r <= 0 || new_x + pilka.r >= tableAPI.width)
        //    {
        //        pilka.vx = -pilka.vx;
        //    }

        //    if (new_y - pilka.r <= 0 || new_y + pilka.r >= tableAPI.height)
        //    {
        //        pilka.vy = -pilka.vy;
        //    }
        //    if(new_x - pilka.r <= 0 || new_x + pilka.r >= tableAPI.width || new_y - pilka.r <= 0 || new_y + pilka.r >= tableAPI.height)
        //    {
        //        pilka.vx = pilka.vx * 0.75f;
        //        pilka.vy = pilka.vy * 0.75f;
        //    }

        //    pilka.vx *= friction;
        //    pilka.vy *= friction;

        //    pilka.x += pilka.vx;
        //    pilka.y += pilka.vy;
        //}

        
        public bool CheckAllCollision()
        {
            int count = tableAPI.CountBalls();
            for (int i = 0; i < count; i++)
            {
                for(int j = 0; j < count; j++)
                {
                    if (i != j)
                    {

                        float distance_x = tableAPI.balls[i].x - tableAPI.balls[j].x;
                        float distance_y = tableAPI.balls[i].y  - tableAPI.balls[j].y;
                        float distance = distance_x * distance_x + distance_y * distance_y;
                        Debug.WriteLine($"Odległość między piłkami {i} i {j} wynosi {distance}");
                        if (distance <= tableAPI.balls[i].r * tableAPI.balls[i].r + tableAPI.balls[j].r * tableAPI.balls[j].r)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckCollision(IBall ball)
        {
            Guid id = ball.Id_ball;
            int count = tableAPI.CountBalls();
            for (int i = 0; i < count; i++)
            {
                if (tableAPI.balls[i].Id_ball != id)
                {
                    float distance_x = tableAPI.balls[i].x - ball.x;
                    float distance_y = tableAPI.balls[i].y - ball.y;
                    float distance = distance_x * distance_x + distance_y * distance_y;
                    float radiusSum = tableAPI.balls[i].r + ball.r;
                    if (distance <= radiusSum * radiusSum)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        public bool AddBallCheck(IBall ball)
        {
            if (CheckCollision(ball))
            {
                return false;
            }
            else
            {
                if (tableAPI.AddBall(ball))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<IBall> getBalls()
        {
            List<IBall> balls = new List<IBall>();
            foreach (var ball in tableAPI.balls)
            {
                balls.Add(ball);
            }
            return balls;
        }

        public int getCountBall()
        {
            return tableAPI.balls.Count;
        }

        public ITable getTable()
        {
            return tableAPI;
        }



    }
}
