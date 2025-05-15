using System;
using System.Collections.Generic;
using System.Diagnostics;
using Data;

namespace Logic
{
    public class GameLogic : IGameLogic
    {
        private readonly ITable tableAPI;
        private readonly object _lockObject = new object(); // Dodano synchronizację

        public GameLogic(Table t)
        {
            tableAPI = t;
        }

        public void Move(object sender, EventArgs e)
        {
            lock (_lockObject) // Dodano synchronizację
            {   
                foreach (var ball in getBalls())
                {
                    ball.SetPosition();
                }

                foreach (var ball in getBalls())
                {
                    HandleWallCollision(ball);
                }

                for (int i = 0; i < getBalls().Count; i++)
                {
                    for (int j = i + 1; j < getBalls().Count; j++)
                    {
                        ResolveCollision(getBalls()[i], getBalls()[j]);
                    }
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
                    float x = Random.Shared.Next(0, (int)tableAPI.width);
                    float y = Random.Shared.Next(0, (int)tableAPI.height);
                    float vx = Random.Shared.Next(-5, 5);
                    float vy = Random.Shared.Next(-5, 5);
                    float r = Random.Shared.Next(10, 20);
                    IBall ball = new Ball(x, y, r, vx, vy);

                    lock (_lockObject) // Dodano synchronizację
                    {
                        if (AddBallCheck(ball))
                        {
                            tableAPI.AddBall(ball);
                            placed = true;
                            createdBalls++;
                        }
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
            int count = getCountBall();
            for (int i = 0; i < count; i++)
            {
                lock (_lockObject) // Dodano synchronizację
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
            lock (_lockObject) // Dodano synchronizację
            {
                List<IBall> balls = new List<IBall>();
                foreach (var ball in tableAPI.balls)
                {
                    balls.Add(ball);
                }
                return balls;
            }
        }

        public int getCountBall()
        {
            return tableAPI.balls.Count;
        }

        public ITable getTable()
        {
            return tableAPI;
        }

        public IBall getBall(Guid id)
        {
            lock (_lockObject) // Dodano synchronizację
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
        }

        private void ResolveCollision(IBall ball1, IBall ball2)
        {
            float dx = ball1.x - ball2.x;
            float dy = ball1.y - ball2.y;
            float distance = MathF.Sqrt(dx * dx + dy * dy);

            if (distance == 0 || distance >= ball1.r + ball2.r) return;

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

            float new_dpNorm1 = (dpNorm1 * (m1 - m2) + 2f * m2 * dpNorm2) / (m1 + m2);
            float new_dpNorm2 = (dpNorm2 * (m2 - m1) + 2f * m1 * dpNorm1) / (m1 + m2);

            ball1.vx = tx * dpTan1 + nx * new_dpNorm1;
            ball1.vy = ty * dpTan1 + ny * new_dpNorm1;
            ball2.vx = tx * dpTan2 + nx * new_dpNorm2;
            ball2.vy = ty * dpTan2 + ny * new_dpNorm2;

            // Separacja kul, by uniknąć ich nakładania się
            float overlap = 0.5f * (ball1.r + ball2.r - distance + 0.1f);
            ball1.x += nx * overlap;
            ball1.y += ny * overlap;
            ball2.x -= nx * overlap;
            ball2.y -= ny * overlap;

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