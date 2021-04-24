﻿using GameServer.Model;
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
                    if (random.Next(2) % 2 == 0)
                    {
                        attacker.Units[i] = unitManager.Get("Lyra");
                    }
                    else
                    {
                        attacker.Units[i] = unitManager.Get("Toxin");
                    }
                }

                for (int i = 0; i < 6; i++)
                {
                    if (random.Next(2) % 2 == 0)
                    {
                        defender.Units[i] = unitManager.Get("Lyra");
                    }
                    else
                    {
                        defender.Units[i] = unitManager.Get("Toxin");
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
