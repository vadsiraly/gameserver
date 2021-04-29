using GameServer.Model;
using GameServer.Model.Battles;
using GameServer.Model.Units.ConcreteUnits.Gulp;
using GameServer.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int attackersWon = 0;
            int defendersWon = 0;
            int tie = 0;
            int inconclusive = 0;
            int matchCount = 1000;
            var seedGenerator = new Random();
            for (int j = 0; j < matchCount; j++)
            {
                var seed = seedGenerator.Next(0, 1000000);
                Console.WriteLine($"Match {j + 1}/{matchCount} ({seed})");
                var random = new Random(seed);

                var attacker = new Team();
                attacker.Add(new Gulp(random));
                attacker.Add(new Gulp(random));
                attacker.Add(new Gulp(random));
                attacker.Add(new Gulp(random));
                attacker.Add(new Gulp(random));
                attacker.Add(new Gulp(random));
                var defender = new Team();
                defender.Add(new Gulp(random));
                defender.Add(new Gulp(random));
                defender.Add(new Gulp(random));
                defender.Add(new Gulp(random));
                defender.Add(new Gulp(random));
                defender.Add(new Gulp(random));

                var battle = new Battle(attacker, defender);
                var outcome = battle.Fight(random);
                if (outcome == Outcome.Win)
                {
                    attackersWon++;
                }
                else if (outcome == Outcome.Loss)
                {
                    defendersWon++;
                }
                else if (outcome == Outcome.Tie)
                {
                    tie++;
                }
                else if (outcome == Outcome.Inconclusive)
                {
                    inconclusive++;
                }

                /*
                Console.WriteLine("Result:");
                Console.WriteLine($"Rounds: {battle.Rounds}");
                Console.WriteLine("Attackers:");
                foreach(var unit in attacker.Units)
                {
                    Console.WriteLine($"{unit.Name}\n\tDamage done: {Math.Abs(unit.DamageDone)}\n\tHealing done: {unit.HealingDone}");
                }
                Console.WriteLine("Defenders:");
                foreach (var unit in defender.Units)
                {
                    Console.WriteLine($"{unit.Name}\n\tDamage done: {Math.Abs(unit.DamageDone)}\n\tHealing done: {unit.HealingDone}");
                }*/
            }

            Console.WriteLine($"Attackers won {attackersWon}/{matchCount}");
            Console.WriteLine($"Defenders won {defendersWon}/{matchCount}");
            Console.WriteLine($"Tie {tie}/{matchCount}");
            Console.WriteLine($"Inconclusive {inconclusive}/{matchCount}");
        }
    }
}
