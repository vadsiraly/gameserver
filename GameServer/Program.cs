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
        /*
            var json = JsonConvert.SerializeObject(
                    new Unit[] { unitManager.Get("Lyra"), unitManager.Get("Toxin") },
                    Formatting.Indented, 
                    new[] { new StringEnumConverter() } 
                );
            File.WriteAllText("out.txt", json);

            return;
        */
        static void Main(string[] args)
        {
            var unitManager = new UnitManager();

            int attackersWon = 0;
            int defendersWon = 0;
            int matchCount = 10000;
            var seedGenerator = new Random();
            for (int j = 0; j < matchCount; j++)
            {
                var seed = seedGenerator.Next(0, 1000000);
                Console.WriteLine($"Match {j + 1}/{matchCount} ({seed})");
                var random = new Random(seed);

                var attacker = new Team();
                var defender = new Team();

                for (int i = 0; i < 6; i++)
                {
                    var remainder = random.Next(1, 4) % 3;
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
                        default:
                            break;
                    }
                }

                for (int i = 0; i < 6; i++)
                {
                    var remainder = random.Next(1, 4) % 3;
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
                        default:
                            break;
                    }
                }

                var outcome = new Battle(attacker, defender).Fight(seed);
                if (outcome == Outcome.Win)
                {
                    attackersWon++;
                }
                else
                {
                    defendersWon++;
                }
            }

            Console.WriteLine($"Attackers won {attackersWon}/{matchCount}");
            Console.WriteLine($"Defenders won {defendersWon}/{matchCount}");
        }
    }
}
