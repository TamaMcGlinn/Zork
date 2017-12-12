using Zork.Objects;

namespace Zork.Characters
{
    public class NPC : Character
    {

        public int LetsPlayerFleePerXRounds { get; set; }

        public NPC(string name, string description, int letsPlayerFleePerXRounds) : base(name, description)
        {
            LetsPlayerFleePerXRounds = letsPlayerFleePerXRounds;
        }

        public NPC(string name, int strength, int health, Weapon weapon, string description, int letsPlayerFleePerXRounds) : base(name, strength, health, weapon, description)
        {
            LetsPlayerFleePerXRounds = letsPlayerFleePerXRounds;
        }
    }
}
