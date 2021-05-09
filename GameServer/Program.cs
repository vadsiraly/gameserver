using GameServer.Battles;
using GameServer.Model;
using GameServer.Model.Units.ConcreteUnits;
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
            int matchCount = 1;
            var seedGenerator = new Random();
            for (int j = 0; j < matchCount; j++)
            {
                var seed = seedGenerator.Next(0, 1000000);
                Console.WriteLine($"Match {j + 1}/{matchCount} ({seed})");
                var random = new Random(seed);

                var attacker = new Team();
                while (attacker.Units.Count < 6)
                {
                    switch(random.Next(1,5) % 4)
                    {
                        case 0:
                            attacker.Add(new Gulp(random));
                            break;
                        case 1:
                            attacker.Add(new Lyra(random));
                            break;
                        case 2:
                            attacker.Add(new SecretiveGirl(random));
                            break;
                        case 3:
                            attacker.Add(new PlagueDoctor(random));
                            break;
                    }
                }
                var defender = new Team();
                while (defender.Units.Count < 6)
                {
                    switch (random.Next(1, 5) % 4)
                    {
                        case 0:
                            defender.Add(new Gulp(random));
                            break;
                        case 1:
                            defender.Add(new Lyra(random));
                            break;
                        case 2:
                            defender.Add(new SecretiveGirl(random));
                            break;
                        case 3:
                            defender.Add(new PlagueDoctor(random));
                            break;
                    }
                }

                Console.WriteLine($"Attackers:\n\t{attacker.Units.Select(x => x.Name).Aggregate((c, n) => c + "\n" + "\t" + n)}");
                Console.WriteLine($"Defenders:\n\t{defender.Units.Select(x => x.Name).Aggregate((c, n) => c + "\n" + "\t" + n)}");

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
