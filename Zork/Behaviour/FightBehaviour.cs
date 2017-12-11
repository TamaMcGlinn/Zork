using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork.Behaviour
{
    public class FightBehaviour
    {
        public FleeBehaviour FleeBehaviour { get; set; }
        public Character Enemy { get; set; }
        public Character Player { get; set; }
        public FightBehaviour(Character enemy, Character player)
        {
            Enemy = enemy;
            Player = player;
            FleeBehaviour = new FleeBehaviour(5);
        }

        /// <summary>
        /// Fights the chosen enemy untill someone dies, if player dies he loses all his items, 
        /// if enemy dies player picks up all his items.
        /// </summary>
        /// <returns>A boolean indicating wether the player won the fight</returns>
        public BattleOutcomeEnum Fight(Point currentRoom)
        {
            while (Player.Health > 0 && Enemy.Health > 0 && !FleeBehaviour.Fled)
            {
                FightOneRound();
                FleeBehaviour.Turn();
            }
            if (FleeBehaviour.Fled)
            {
                FleeBehaviour.Flee(currentRoom);
                return BattleOutcomeEnum.PlayerFled;
            }
            return CheckWhoWon();
        }

        private  BattleOutcomeEnum CheckWhoWon()
        {
            if (Player.Health < 0)
            {
                Player.Inventory.Clear();
                Player.ResetHealth();
                Console.WriteLine("You died! But luckily you've returned without items.");
                return BattleOutcomeEnum.EnemyWon;
            }
            else
            {
                Player.Inventory.AddRange(Enemy.Inventory);
                Console.WriteLine($"You've won! You've picked up all {Enemy.Name}'s items, check your inventory!");
                return BattleOutcomeEnum.PlayerWon;
            }
        }

        public  void FightOneRound()
        {
            int playerDamage = GenerateDamage(Player);
            int enemyDamage = GenerateDamage(Enemy);

            Console.WriteLine("You hit eachother...");
            Player.TakeDamage(enemyDamage);
            Enemy.TakeDamage(playerDamage);

            Console.Write("You hit for: " + playerDamage + GetPlayerWeaponString());
            Console.Write($"\n{Enemy.Name} hits you for:" + enemyDamage + GetEnemyWeaponString());
            Console.WriteLine($"\nYou have {Player.Health} hp left, he has {Enemy.Health} hp left.");
        }

        private  string GetPlayerWeaponString()
        {
            if (Player.EquippedWeapon != null)
            {
                Console.Write($" with your mighty {Player.EquippedWeapon.Name} ");
            }
            return "";
        }

        private string GetEnemyWeaponString()
        {
            if (Enemy.EquippedWeapon != null)
            {
                Console.Write($" with his stupid {Enemy.EquippedWeapon.Name} ");
            }
            return "";
        }

        private int GenerateDamage(Character character)
        {
            Random turnBonusDamageGenerator = new Random();
            const int maxBonusDamage = 10;
            int bonusDamage = turnBonusDamageGenerator.Next(0, maxBonusDamage);

            int damage = character.Strength + bonusDamage;
            if (character.EquippedWeapon != null)
            {
                damage += character.EquippedWeapon.Strength;
            }
            return damage;
        }

    }
}
