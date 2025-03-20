using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Ball
    {
        public int color;
        public int x;
        public int y;
        public int speed;


        public Ball(int x, int y)
        {
            this.color = 0;
            this.x = x;
            this.y = y;
            this.speed = 0;
        }

        public void Move(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
