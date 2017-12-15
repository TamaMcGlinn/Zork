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
            maze.AddItemToRandomRoom(new Clue("money", "Half a pound of good English money!"));
            maze.AddItemToRandomRoom(new Clue("iron_skillet", "A cast-iron skillet, quite heavy. There's some blood on the bottom."));
        }

        private static void AddHealthPickups(Maze maze)
        {
            for (int i = 0; i < 4; ++i)
            {
                maze.AddItemToRandomRoom(new HealthPickup("Green vial", 50, "Some sort of potion; might be toxic."));
                maze.AddItemToRandomRoom(new HealthPickup("Green vial", -50, "Some sort of potion; might be toxic."));
            }
            for (int i = 0; i < 4; ++i)
            {
                maze.AddItemToRandomRoom(new HealthPickup("Red vial", -90, "Wonderful life-saving stuff. Probably."));
            }
            for (int i = 0; i < 12; ++i)
            {
                maze.AddItemToRandomRoom(new HealthPickup("Apple", 5, "Looks ripe."));
            }
            for (int i = 0; i < 6; ++i)
            {
                maze.AddItemToRandomRoom(new HealthPickup("Bandage", 40, "Could save your life."));
            }
            for (int i = 0; i < 12; ++i)
            {
                maze.AddItemToRandomRoom(new HealthPickup("Brown rag", 20, "Just what you need when you're bleeding."));
            }
        }

        private static void AddWeapons(Maze maze)
        {
            maze.AddItemToRandomRoom(new Weapon("Longsword", 30, "Best suited to spilling the blood of your enemies."));
            maze.AddItemToRandomRoom(new Weapon("Knife", 12, "Crude, but will probably get the job done."));
            maze.AddItemToRandomRoom(new Weapon("Butterknife", 5, "Very lethal if you happen to encounter someone made of butter."));
            maze.AddItemToRandomRoom(new Weapon("Chair", 8, "A wooden chair."));
            maze.AddItemToRandomRoom(new Weapon("Broken glass", 11, "A broken glass; it stinks of ale."));
            maze.AddItemToRandomRoom(new Weapon("Broom", 8, "A long wooden handle with straw bound in rope."));
            maze.AddItemToRandomRoom(new Weapon("Shoe", 3, "A large black boot."));
            maze.AddItemToRandomRoom(new Weapon("Brick", 5, "A stone brick."));
            maze.AddItemToRandomRoom(new Weapon("Flail", 13, "Sweeet. Time to do some murdering."));
            maze.AddItemToRandomRoom(new Weapon("Mace", 18, "Spikey."));
            maze.AddItemToRandomRoom(new Weapon("Hammer", 25, "The heavy sort; could probably kill a human fairly quickly."));
            int revolverDropChance = 33;
            if (Chance.Percentage(revolverDropChance))
            {
                maze.AddItemToRandomRoom(new Weapon(".32 Rimfire Revolver", 70, "Holy smokes! a bloody revolver!"));
            }
        }
    }
}
