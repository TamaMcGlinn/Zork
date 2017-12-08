using Zork.Characters;

namespace Zork.Objects
{
    public class Clue : BaseObject
    {
        public Clue(string name, string description) : base(name, description)
        {
        }

        public override void PickupObject(Character character)
        {
            base.PickupObject(character);
            Player p = character as Player;
            if (p != null)
            {
                p.Clues.Add(Name);
            }
        }
    }
}
