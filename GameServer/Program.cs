using GameServer.Model;
using GameServer.Model.BaseTypes;
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
            var unitManager = new UnitManager();

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
                var defender = new Team();

                while (attacker.Units.Count != 6)
                {
                    var remainder = random.Next(1, 7) % 6;
                    switch (remainder)
                    {
                        case 0:
                            attacker.Add(unitManager.Get("Lyra"));
                            break;
                        case 1:
                            attacker.Add(unitManager.Get("Toxin"));
                            break;
                        case 2:
                            attacker.Add(unitManager.Get("Gulp"));
                            break;
                        case 3:
                            attacker.Add(unitManager.Get("Mermaid"));
                            break;
                        case 4:
                            attacker.Add(unitManager.Get("Plague Doctor"));
                            break;
                        case 5:
                            attacker.Add(unitManager.Get("Secretive Girl"));
                            break;
                        default:
                            break;
                    }
                }

                while(defender.Units.Count != 6)
                {
                    var remainder = random.Next(1, 7) % 6;
                    switch (remainder)
                    {
                        case 0:
                            defender.Add(unitManager.Get("Lyra"));
                            break;
                        case 1:
                            defender.Add(unitManager.Get("Toxin"));
                            break;
                        case 2:
                            defender.Add(unitManager.Get("Gulp"));
                            break;
                        case 3:
                            defender.Add(unitManager.Get("Mermaid"));
                            break;
                        case 4:
                            defender.Add(unitManager.Get("Plague Doctor"));
                            break;
                        case 5:
                            defender.Add(unitManager.Get("Secretive Girl"));
                            break;
                        default:
                            break;
                    }
                }

                var battle = new Battle(attacker, defender);
                var outcome = battle.Fight(seed);
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
                }
            }

            Console.WriteLine($"Attackers won {attackersWon}/{matchCount}");
            Console.WriteLine($"Defenders won {defendersWon}/{matchCount}");
            Console.WriteLine($"Tie {tie}/{matchCount}");
            Console.WriteLine($"Inconclusive {inconclusive}/{matchCount}");
        }
    }
}
