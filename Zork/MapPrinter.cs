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
        private const int BorderSize = 2;
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
            PrintEnclosingBorder();
            for (int yi = 0; yi < _maze.Height; ++yi)
            {
                PrintLineEnclosingBorder();
                for (int xi = 0; xi < _maze.Width; ++xi)
                {
                    PrintHorizontal(xi, yi, playerLocation);
                }
                PrintLineEnclosingBorder();
                Console.WriteLine();
                PrintLowerHalf(yi);
            }
            PrintEnclosingBorder();
            Console.WriteLine();
        }

        private void PrintLineEnclosingBorder()
        {
            for (int border = 0; border < BorderSize; ++border)
            {
                PlaceBorder();
            }
        }

        private void PrintEnclosingBorder()
        {
            for (int verticalBorder = 0; verticalBorder < BorderSize; ++verticalBorder)
            {
                for (int border = 0; border < (_maze.Width + BorderSize) * 2 - 1; ++border)
                {
                    PlaceBorder();
                }
                Console.WriteLine();
            }
        }

        private void PlaceBorder()
        {
            using (new ColorContext(ColorContext.MapWall, ColorContext.MapWall))
            {
                Console.Write(" ");
            }
        }

        private void PrintLowerHalf(int yi)
        {
            if (yi < _maze.Height - 1)
            {
                PrintLineEnclosingBorder();
                for (int xi = 0; xi < _maze.Width; ++xi)
                {
                    PrintVertical(xi, yi);
                }
                PrintLineEnclosingBorder();
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
            var ConnectionColor = CanGoSouth ? ColorContext.MapAvailableSquare : ColorContext.MapWall;
            using (new ColorContext(ColorContext.MapAvailableSquare, ConnectionColor))
            {
                Console.Write(" ");
            }
            if (xi < _maze.Width - 1)
            {
                using (new ColorContext(ColorContext.MapAvailableSquare, ColorContext.MapWall))
                {
                    Console.Write(" ");
                }
            }
        }

        private void PrintHorizontal(int xi, int yi, Point playerLocation)
        {
            if (playerLocation.X == xi && playerLocation.Y == yi)
            {
                using (new ColorContext(ColorContext.MapPlayerLocation, ColorContext.MapAvailableSquare))
                {
                    Console.Write("X");
                }
            }
            else
            {
                using (new ColorContext(ColorContext.MapAvailableSquare, ColorContext.MapAvailableSquare))
                {
                    Console.Write(" ");
                }
            }
            if (_maze.Rooms[xi, yi].CanGoThere[Direction.East])
            {
                Debug.Assert(xi == _maze.Width - 1 || _maze.Rooms[xi + 1, yi].CanGoThere[Direction.West]);
                using (new ColorContext(ColorContext.MapAvailableSquare, ColorContext.MapAvailableSquare))
                {
                    Console.Write(" ");
                }
            }
            else if (xi < _maze.Width - 1)
            {
                using (new ColorContext(ColorContext.MapAvailableSquare, ColorContext.MapWall))
                {
                    Console.Write(" ");
                }
            }
        }
    }
}
