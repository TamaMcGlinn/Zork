using System;
using System.Collections.Generic;

namespace Zork
{
    public class MazeEnumerator : IEnumerator<Room>
    {
        #region Fields
        private int xi = -1;
        private int yi = 0;
        private Maze maze;
        #endregion

        public MazeEnumerator(Maze maze)
        {
            this.maze = maze;
        }

        public object Current
        {
            get
            {
                return Current;
            }
        }

        Room IEnumerator<Room>.Current
        {
            get
            {
                return maze[xi, yi];
            }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            xi++;
            if(xi == maze.Width)
            {
                xi = 0;
                yi++;
            }
            return yi < maze.Height;
        }

        public void Reset()
        {
            xi = -1;
            yi = 0;
        }
    }
}
