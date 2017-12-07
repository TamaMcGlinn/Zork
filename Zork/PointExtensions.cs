using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Zork
{
    public static class PointExtensions
    {
        public static Point Add(this Point origin, Direction dir)
        {
            switch (dir)
            {
                case Direction.North:
                    return new Point(origin.X, origin.Y - 1);
                case Direction.East:
                    return new Point(origin.X + 1, origin.Y);
                case Direction.South:
                    return new Point(origin.X, origin.Y + 1);
                case Direction.West:
                    return new Point(origin.X - 1, origin.Y);
                default:
                    throw new Exception("Unknown direction encountered: " + dir);
            }
        }
    }
}
