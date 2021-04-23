using GameServer.Model.BaseTypes;
using GameServer.Services;
using Newtonsoft.Json;
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
            var json = JsonConvert.SerializeObject(new Unit[] { new Lyra(), new Toxin() }, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            });
            File.WriteAllText("out.txt", json);

            return;
        */
        static void Main(string[] args)
        {
            var unitManager = new UnitManager();

            int attackersWon = 0;
            int defendersWon = 0;
            int matchCount = 100000;
            for (int j = 0; j < matchCount; j++)
            {
                var seed = new Random().Next(0, 1000000);
                Console.WriteLine($"Match {j+1}/{matchCount} ({seed})");
                var random = new Random(seed);

                var attacker = new Unit[6];
                var defender = new Unit[6];

                for (int i = 0; i < 6; i++)
                {
                    if (random.Next(2) % 2 == 0)
                    {
                        attacker[i] = unitManager.Get("Lyra");
                    }
                    else
                    {
                        attacker[i] = unitManager.Get("Toxin");
                    }
                }

                for (int i = 0; i < 6; i++)
                {
                    if (random.Next(2) % 2 == 0)
                    {
                        defender[i] = unitManager.Get("Lyra");
                    }
                    else
                    {
                        defender[i] = unitManager.Get("Toxin");
                    }
                }

                while (!(attacker.All(x => x.IsDead()) || defender.All(x => x.IsDead())))
                {
                    foreach (var current in attacker.Union(defender).OrderBy(x => x.Speed))
                    {
                        if (!attacker.Any(x => !x.IsDead()) || !defender.Any(x => !x.IsDead())) break;

                        if (attacker.Contains(current))
                        {
                            var target = defender.Where(x => !x.IsDead()).ToArray()[random.Next(0, defender.Count(x => !x.IsDead()))];
                            current.Abilities[0].Use(current, target, random);
                        }
                        else
                        {
                            var target = attacker.Where(x => !x.IsDead()).ToArray()[random.Next(0, attacker.Count(x => !x.IsDead()))];
                            current.Abilities[0].Use(current, target, random);
                        }
                    }
                }

                if (!attacker.Any(x => !x.IsDead()))
                {
                    attackersWon++;
                }
                else
                {
                    defendersWon++;
                }
            }

            Console.WriteLine($"Attackers won {attackersWon}/1000");
            Console.WriteLine($"Defenders won {defendersWon}/1000");
        }
    }
}
