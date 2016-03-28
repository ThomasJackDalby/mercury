using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
    public struct Point
    {
        public int X { get { return x; } }
        public int Y { get { return y; } }

        private readonly int x;
        private readonly int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return String.Format("[{0}, {1}]", X, Y);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point)) return false;
            Point other = (Point)obj;

            if (!X.Equals(other.X)) return false;
            if (!Y.Equals(other.Y)) return false;

            return true;
        }
    }
}
