using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using Data;
using Timer = System.Timers.Timer;

namespace Logic
{
    public class GameLogic : IGameLogic
    {
        private readonly ITable tableAPI;
        private float _friction;
        private Timer _timer;


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
            _timer = new Timer(20);
            _timer.Elapsed += Move;
            _timer.Start();
        }
        private void Move(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < tableAPI.CountBalls(); i++)
            {
                IBall pilka = tableAPI.balls[i];
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
        }

        public bool CreateBalls(int count)
        {
            if (count <= 0) return false;

            int createdBalls = 0;
            int maxTriesPerBall = 100;

            while (createdBalls < count)
            {
                bool placed = false;
                int tries = 0;

                while (!placed && tries < maxTriesPerBall)
                {
                    float x = Random.Shared.Next(0, (int) tableAPI.width);
                    float y = Random.Shared.Next(0, (int) tableAPI.height);
                    float vx = Random.Shared.Next(-20, 20);
                    float vy = Random.Shared.Next(-20, 20);
                    IBall ball = new Ball(x, y, 25, vx, vy);

                    if (AddBallCheck(ball))
                    {
                        tableAPI.AddBall(ball);
                        placed = true;
                        createdBalls++;
                    }

                    tries++;
                }

                if (!placed)
                {
                    Console.WriteLine($"Nie udało się dodać kuli numer {createdBalls + 1} po {maxTriesPerBall} próbach.");
                    return false;
                }
            }

            return true;
        }


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

        void IGameLogic.Move(object sender, ElapsedEventArgs e)
        {
            Move(sender, e);
        }

        Timer IGameLogic.getTimer()
        {
            return _timer;
        }

        IBall getBall(Guid id)
        {
            foreach (var ball in tableAPI.balls)
            {
                if (ball.Id_ball == id)
                {
                    return ball;
                }
            }
            return null;
        }

        IBall IGameLogic.getBall(Guid id)
        {
            return getBall(id);
        }

        public void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }
    }
}
