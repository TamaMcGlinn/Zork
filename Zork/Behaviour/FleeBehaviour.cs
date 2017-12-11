using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Characters;

namespace Zork
{
    public class FleeBehaviour
    {
        public bool Fled { get; set; } = false;
        private int TurnsPerAskFlee { get; set; }
        private int TurnsPassed { get; set; }

        public FleeBehaviour(int turnsPerAskFlee)
        {
            this.TurnsPerAskFlee = turnsPerAskFlee;
        }

        public void Turn()
        {
            TurnsPassed++;
            if (TurnsPassed == TurnsPerAskFlee)
            {
                TurnsPassed = 0;
                AskFlee();
            }
        }

        protected void AskFlee()
        {
            Console.WriteLine("Do you want to flee? Y/N");
            char userInputCharacter = (char)Console.Read();
            if (userInputCharacter == 'y' || userInputCharacter == 'Y')
            {
                Fled = true;
            }
            else
            {
                Fled = false;
            }
        }

        public void Flee(ref Point currentRoom)
        {
            Random r = new Random();
            int X = currentRoom.X;
            int Y = currentRoom.Y;
            while(X == currentRoom.X )
            {
                 X = r.Next(0, Game.Width);
               
            }
            while(Y == currentRoom.Y)
            {
                 Y = r.Next(0, Game.Height);
            }
            currentRoom.X = X;
            currentRoom.Y = Y;

            Console.WriteLine("...What ...Where am i?");
        }
    }
}
