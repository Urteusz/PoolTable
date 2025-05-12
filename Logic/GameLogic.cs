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
        private Timer _timer;

        public GameLogic(Table t)
        {
            tableAPI = t;
            _timer = new Timer(16);
            _timer.Elapsed += Move;
        }

        public void StartTimer()
        {
            if (_timer != null)
            {
                _timer.Start();
            }
        }

        private void Move(object sender, ElapsedEventArgs e)
        {
            foreach (var ball in tableAPI.balls)
            {
                ball.SetPosition();
            }

            foreach (var ball in tableAPI.balls)
            {
                HandleWallCollision(ball);
            }

            for (int i = 0; i < tableAPI.CountBalls(); i++)
            {
                for (int j = i + 1; j < tableAPI.CountBalls(); j++)
                {
                    ResolveCollision(tableAPI.balls[i], tableAPI.balls[j]);
                }
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
                    float vx = Random.Shared.Next(-5, 5);
                    float vy = Random.Shared.Next(-5, 5);
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

        private void ResolveCollision(IBall ball1, IBall ball2)
        {
            float dx = ball1.x - ball2.x;
            float dy = ball1.y - ball2.y;
            float distance = MathF.Sqrt(dx * dx + dy * dy);

            if (distance >= ball1.r + ball2.r) return; // Brak kolizji
            if (distance == 0) return; // uniknięcie dzielenia przez zero

            float nx = dx / distance;
            float ny = dy / distance;

            float tx = -ny;
            float ty = nx;

            float dpTan1 = ball1.vx * tx + ball1.vy * ty;
            float dpTan2 = ball2.vx * tx + ball2.vy * ty;

            float dpNorm1 = ball1.vx * nx + ball1.vy * ny;
            float dpNorm2 = ball2.vx * nx + ball2.vy * ny;

            float m1 = ball1.r;
            float m2 = ball2.r;

            // Nowe składowe normalne prędkości (sprężyste zderzenie)
            float new_dpNorm1 = (dpNorm1 * (m1 - m2) + 2f * m2 * dpNorm2) / (m1 + m2);
            float new_dpNorm2 = (dpNorm2 * (m2 - m1) + 2f * m1 * dpNorm1) / (m1 + m2);

            ball1.vx = tx * dpTan1 + nx * new_dpNorm1;
            ball1.vy = ty * dpTan1 + ny * new_dpNorm1;
            ball2.vx = tx * dpTan2 + nx * new_dpNorm2;
            ball2.vy = ty * dpTan2 + ny * new_dpNorm2;

        }
        private void HandleWallCollision(IBall ball)
        {
            if (ball.x - ball.r < 0 && ball.vx < 0)
            {
                ball.vx *= -1;
                ball.x = ball.r;
            }
            else if (ball.x + ball.r > tableAPI.width && ball.vx > 0)
            {
                ball.vx *= -1;
                ball.x = tableAPI.width - ball.r;
            }

            if (ball.y - ball.r < 0 && ball.vy < 0)
            {
                ball.vy *= -1;
                ball.y = ball.r;
            }
            else if (ball.y + ball.r > tableAPI.height && ball.vy > 0)
            {
                ball.vy *= -1;
                ball.y = tableAPI.height - ball.r;
            }
        }


    }
}
