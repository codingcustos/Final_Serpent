using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Serpent
{
    internal class Food
    {
        public Point Position;
        public int NutriVal;
        public Food(Point position, int nutriVal = 1)
        {
            Position = position;
            NutriVal = nutriVal;
        }
    }
}
