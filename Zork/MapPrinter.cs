using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.UIContext;

namespace Zork
{
    public class MapPrinter
    {
        private Maze _maze;

        public MapPrinter(Maze maze)
        {
            _maze = maze;
        }

        /// <summary>
        /// Print the maze as a map created by 0's -'s and P for player characters.
        /// </summary>
        /// <param name="playerLocation"></param>
        public void Print(Point playerLocation)
        {
            for (int yi = 0; yi < _maze.Height; ++yi)
            {
                for (int xi = 0; xi < _maze.Width; ++xi)
                {
                    PrintHorizontal(xi, yi, playerLocation);
                }
                PrintLowerHalf(yi);
            }
            Console.WriteLine();
        }

        private void PrintLowerHalf(int yi)
        {
            Console.WriteLine();
            if (yi < _maze.Height - 1)
            {
                for (int xi = 0; xi < _maze.Width; ++xi)
                {
                    PrintVertical(xi, yi);
                }
                Console.WriteLine();
            }
        }

        private void PrintVertical(int xi, int yi)
        {
            bool CanGoSouth = _maze.Rooms[xi, yi].CanGoThere[Direction.South];
            if (yi < _maze.Height - 1)
            {
                Debug.Assert(_maze.Rooms[xi, yi + 1].CanGoThere[Direction.North] == CanGoSouth);
            }
            var ConnectionColor = CanGoSouth ? ColourContext.MapAvailableSquare : ColourContext.MapWall;
            using (new ColourContext(ColourContext.MapAvailableSquare, ConnectionColor))
            {
                Console.Write(" ");
            }
            if (xi < _maze.Width - 1)
            {
                using (new ColourContext(ColourContext.MapAvailableSquare, ColourContext.MapWall))
                {
                    Console.Write(" ");
                }
            }
        }

        private void PrintHorizontal(int xi, int yi, Point playerLocation)
        {
            if (playerLocation.X == xi && playerLocation.Y == yi)
            {
                using (new ColourContext(ColourContext.MapPlayerLocation, ColourContext.MapAvailableSquare))
                {
                    Console.Write("X");
                }
            }
            else
            {
                using (new ColourContext(ColourContext.MapAvailableSquare, ColourContext.MapAvailableSquare))
                {
                    Console.Write(" ");
                }
            }
            if (_maze.Rooms[xi, yi].CanGoThere[Direction.East])
            {
                Debug.Assert(xi == _maze.Width - 1 || _maze.Rooms[xi + 1, yi].CanGoThere[Direction.West]);
                using (new ColourContext(ColourContext.MapAvailableSquare, ColourContext.MapAvailableSquare))
                {
                    Console.Write(" ");
                }
            }
            else if (xi < _maze.Width - 1)
            {
                using (new ColourContext(ColourContext.MapAvailableSquare, ColourContext.MapWall))
                {
                    Console.Write(" ");
                }
            }
        }
    }
}
