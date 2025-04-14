using Logic;
using Model;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using Logic;
using Data;

namespace ModelView
{
    
public class CreateView
    {
        private CanvasModel _canvas;
        private IGameLogic gameLogicAPI;

        public CreateView(int w, int h)
        {
            TableModel tableModel = new TableModel(w, h);
            this.gameLogicAPI = new GameLogic(tableModel.Table, 0.995f);
            this._canvas = new CanvasModel(tableModel);

        }

        public bool CreateBalls(int count)
        {
            if (count <= 0)
            {
                return false;
            }

            int createdBalls = 0;
            int maxTriesPerBall = 100;

            while (createdBalls < count)
            {
                bool placed = false;
                int tries = 0;

                while (!placed && tries < maxTriesPerBall)
                {
                    float x = Random.Shared.Next(0, (int)canvas.tableModel.Table.width);
                    float y = Random.Shared.Next(0, (int)canvas.tableModel.Table.height);
                    float vx = Random.Shared.Next(-20, 20);
                    float vy = Random.Shared.Next(-20, 20);
                    BallModel ballModel = new BallModel(x, y, 25, vx, vy);

                    if (gameLogicAPI.AddBallCheck(ballModel.ball))
                    {
                        _canvas.tableModel.AddBall(ballModel);
                        UpdateBallPosition(ballModel.ball, ballModel.ballShape); // najpierw ustaw pozycję
                        _canvas.addObject(ballModel.ballShape); // potem dodaj do Canvas
                        placed = true;
                        createdBalls++;
                    }


                    tries++;
                }

                if (!placed)
                {
                    // Można zalogować, że nie udało się dodać kuli po wielu próbach
                    Console.WriteLine($"Nie udało się dodać kuli numer {createdBalls + 1} po {maxTriesPerBall} próbach.");
                    return false;
                }
            }

            return true;
        }


        public CanvasModel canvas
        {
            get { return _canvas; }
        }

        public void UpdateBallPosition(IBall ball, Ellipse shape)
        {
            Canvas.SetLeft(shape, ball.x - ball.r);
            Canvas.SetTop(shape, ball.y - ball.r);
        }


        public void UpdateBallMove(object sender, EventArgs e)
        {
            foreach (var ballModel in _canvas.tableModel.Balls)
            {
                gameLogicAPI.Move(ballModel.ball);
                if (ballModel.ball.vx != 0 || ballModel.ball.vy != 0)
                {
                    UpdateBallPosition(ballModel.ball, ballModel.ballShape);
                }
            }
        }

    }
}
