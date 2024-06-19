using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Final_Serpent
{
    public class Player
    {
        public List<PointF> Positions = new List<PointF>();
        public Directions Direction;
        public Directions TempDirection;
        public Brush SnakeBrush;
        public PointF Head;
        public bool IsAlive { get; set; } = true;
        public float StepSize;
        public int LogicalSerpentLength = 0;

        public Player(PointF spawnPos, int initLen, float stepsize, Directions startDirection, Brush snakeBrush)
        {
            StepSize = stepsize;
            for (int i = 0; i <= (1 + ((initLen - 1) / StepSize) - 1 * StepSize); i++)
            {
                Positions.Add(new PointF(spawnPos.X, spawnPos.Y));
            }
            LogicalSerpentLength = initLen;

            Head = spawnPos;
            SnakeBrush = snakeBrush;
            TempDirection = startDirection;
            Direction = startDirection;
        }

        public void Move(int stepDivisor)
        {
            PointF head = Positions[0];
            PointF newHead = new PointF(head.X, head.Y);
            switch (Direction)
            {
                case Directions.Left:
                    newHead.X -= StepSize;
                    break;
                case Directions.Right:
                    newHead.X += StepSize;
                    break;
                case Directions.Up:
                    newHead.Y -= StepSize;
                    break;
                case Directions.Down:
                    newHead.Y += StepSize;
                    break;
                default:
                    throw new Exception("Invalid direction given");
            }
            newHead = RoundPointF(newHead, 2);
            Positions.Insert(0, newHead);
            Positions.RemoveAt(Positions.Count - 1);

            Head = newHead;
        }

        private PointF RoundPointF(PointF point, int decimalPlaces)
        {
            float x = (float)Math.Round(point.X, decimalPlaces);
            float y = (float)Math.Round(point.Y, decimalPlaces);
            return new PointF(x, y);
        }
    }
}