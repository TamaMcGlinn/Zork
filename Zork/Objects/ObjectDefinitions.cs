using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork.Objects
{
    public static class ObjectDefinitions
    {
        public static List<Clue> Clues = new List<Clue>()
        {
            new Clue("Chesspiece", "A white wooden rook")
        };

        public static List<Weapon> Weapons = new List<Weapon>()
        {
            new Weapon("Longsword", 40, "Best suited to spilling the blood of your enemies.")
        };

        public static List<HealthPickup> HealthPickups = new List<HealthPickup>()
        {
            new HealthPickup("Green vial", 50, "Some sort of potion; might be toxic."),
            new HealthPickup("Green vial", -50, "Some sort of potion; might be toxic."),
            new HealthPickup("Bandage", 40, "Could save your life."),
            new HealthPickup("Brown rag", 20, "Just what you need when you're bleeding.")
        };
    }
}
