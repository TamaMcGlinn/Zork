using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Objects;
using Zork.Texts;
using System.Drawing;
using System.Diagnostics;

namespace Zork
{
    public class Character
    {

        #region properties
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _strength;

        public int Strength
        {
            get { return _strength; }
            set { _strength = value; }
        }

        private int _health;

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }


        private Weapon _weapon;

        public Weapon EquippedWeapon
        {
            get { return _weapon; }
            set { _weapon = value; }
        }

        private Point _location;

        public Point Location
        {
            get { return _location; }
            set { _location = value; }
        }

        private TextTree _text;

        public TextTree Text
        {
            get { return _text; }
            set { _text = value; }
        }
        
        #endregion properties

        /// <summary>
        /// Character constructor
        /// </summary>
        /// <param name="name">The name of the character</param>
        /// <param name="strength">The characters strength</param>
        /// <param name="health">The characters health</param>
        /// <param name="weapon">The weapon the character has equipped</param>
        /// <param name="location">Current location of the character</param>
        public Character(string name, int strength,int health, Weapon weapon, Point location)
        {
            this.Name = name;
            this.Strength = strength;
            this.Health = health;
            this.EquippedWeapon = weapon;
            this.Location = location;
            this.Text = new TextTree(name + ".txt");
        }

        /// <summary>
        /// Output text and accept player choices until the tree reaches a leaf node
        /// </summary>
        public void Talk()
        {
            bool playerIsTalking = false;
            Node currentNode = Text.RootNode;
            while (true)
            {
                Console.WriteLine(currentNode.Text);
                if (currentNode.Children.Count == 0)
                {
                    return;
                }

                if (!playerIsTalking)
                {
                    int responseNumber = 1;
                    foreach (Node child in currentNode.Children)
                    {
                        Console.WriteLine(responseNumber + "> " + child.Text);
                        ++responseNumber;
                    }
                    Console.Write("> ");
                    int chosenResponse = -1;
                    while (Int32.TryParse(Console.ReadLine(), out chosenResponse) == false || chosenResponse < 0 || chosenResponse > currentNode.Children.Count)
                    {
                        Console.WriteLine("Write a number for one of the responses");
                    }
                    Node playerResponse = currentNode.Children[chosenResponse - 1];
                    Console.WriteLine("> " + playerResponse.Text);
                    if (playerResponse.Children.Count == 1)
                    {
                        currentNode = playerResponse.Children.First();
                    } else
                    {
                        Debug.Assert(playerResponse.Children.Count == 0);
                        return;
                    }
                }
            }
        }
    }
}
