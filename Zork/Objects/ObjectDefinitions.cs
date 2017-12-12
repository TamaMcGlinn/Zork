using System;

namespace Zork.Objects
{
    public static class ObjectDefinitions
    {
        public static void AddItems(Maze maze)
        {
            AddClues(maze);
            AddWeapons(maze);
            AddHealthPickups(maze);
        }

        private static void AddClues(Maze maze)
        {
            maze.AddItemToRandomRoom(new Clue("Chesspiece", "A white wooden rook"));
        }

        private static void AddHealthPickups(Maze maze)
        {
            maze.AddItemToRandomRoom(new HealthPickup("Green vial", 50, "Some sort of potion; might be toxic."));
            maze.AddItemToRandomRoom(new HealthPickup("Green vial", -50, "Some sort of potion; might be toxic."));
            maze.AddItemToRandomRoom(new HealthPickup("Red vial", -90, "Wonderful life-saving stuff. Probably."));
            for (int i = 0; i < 25; ++i)
            {
                maze.AddItemToRandomRoom(new HealthPickup("Apple", 5, "Looks ripe."));
            }
            for (int i = 0; i < 10; ++i)
            {
                maze.AddItemToRandomRoom(new HealthPickup("Bandage", 40, "Could save your life."));
            }
            for (int i = 0; i < 15; ++i)
            {
                maze.AddItemToRandomRoom(new HealthPickup("Brown rag", 20, "Just what you need when you're bleeding."));
            }
        }

        private static void AddWeapons(Maze maze)
        {
            maze.AddItemToRandomRoom(new Weapon("Longsword", 40, "Best suited to spilling the blood of your enemies."));
            maze.AddItemToRandomRoom(new Weapon("Knife", 20, "Crude, but will probably get the job done."));
            maze.AddItemToRandomRoom(new Weapon("Butterknife", 5, "Very lethal if you happen to encounter someone made of butter."));
            maze.AddItemToRandomRoom(new Weapon("Broom", 8, "A long wooden handle with straw bound in rope."));
            maze.AddItemToRandomRoom(new Weapon("Hammer", 25, "The heavy sort; could probably kill a human fairly quickly."));
            maze.AddItemToRandomRoom(new Weapon("Pan", 10, "A cast-iron skillet, quite heavy."));
            int revolverDropChance = 33;
            if (DropChanceByPercentage(revolverDropChance))
            {
                maze.AddItemToRandomRoom(new Weapon(".32 Rimfire Revolver", 50, "Holy smokes! a bloody revolver!"));
            }
        }

        public static bool DropChanceByPercentage(int chancePercentage)
        {
            Random r = new Random();
            if(r.Next(0, 100) < chancePercentage)
            {
                return true;
            }
            return false;
        }
    }
}
