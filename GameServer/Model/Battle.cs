using GameServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.BaseTypes
{
    public enum Outcome
    {
        Win,
        Loss,
        Tie,
        Inconclusive
    }

    public class Battle
    {
        public Team Attacker { get; set; }
        public Team Defender { get; set; }
        public int Rounds { get; set; } = 0;
        public List<Unit> AllUnits
        {
            get
            {
                return Attacker.Units.Union(Defender.Units).ToList();
            }
        }

        public int RoundLimit { get; set; } = 200;

        public Battle(Team attacker, Team defender)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public Outcome Fight(int seed)
        {
            var ps = new PrintService();
            var random = new Random(seed);

            var allAttackersDead = false;
            var allDefendersDead = false;
            while (!allAttackersDead && !allDefendersDead && Rounds < RoundLimit)
            {
                Console.WriteLine($"\n----------------------------------------\nStarting Round {Rounds+1}...\n----------------------------------------");
                ps.Print(Attacker, Defender);
                Console.ReadKey();

                foreach (var current in AllUnits)
                {
                    current.BeginRound(random);
                }

                foreach (var current in AllUnits.Where(x => !x.IsDead).OrderBy(x => x.Speed).ToArray().Shuffle())
                {
                    if (Attacker.Units.Contains(current))
                    {
                        ps.WrapPrint(() => { current.BasicAttack.Use(current, Defender, current.BasicAttack, random); }, Attacker, Defender, $"{current.Name} uses {current.BasicAttack.Name}");          
                        if (current.Abilities[0].ActiveCooldown == 0)
                        {
                            ps.WrapPrint(() => { current.Abilities[0].Use(current, Defender, current.Abilities[0], random); }, Attacker, Defender, $"{current.Name} uses {current.Abilities[0].Name}");
                        }
                    }
                    else
                    {
                        ps.WrapPrint(() => { current.BasicAttack.Use(current, Attacker, current.BasicAttack, random); }, Attacker, Defender, $"{current.Name} uses {current.BasicAttack.Name}");
                        if (current.Abilities[0].ActiveCooldown == 0)
                        {
                            ps.WrapPrint(() => { current.Abilities[0].Use(current, Attacker, current.Abilities[0], random); }, Attacker, Defender, $"{current.Name} uses {current.Abilities[0].Name}");
                        }
                    }
                }

                foreach (var current in AllUnits)
                {
                    current.EndRound(random);
                }

                Console.WriteLine($"\n----------------------------------------\nEnding Round {Rounds + 1}...\n----------------------------------------");
                new PrintService().Print(Attacker, Defender);
                Console.ReadKey();

                allAttackersDead = Attacker.Units.All(x => x.IsDead);
                allDefendersDead = Defender.Units.All(x => x.IsDead);

                if (allAttackersDead && !allDefendersDead)
                {
                    return Outcome.Loss;
                }
                if (allDefendersDead && !allAttackersDead)
                {
                    return Outcome.Win;
                }
                if (allAttackersDead && allDefendersDead)
                {
                    return Outcome.Tie;
                }

                ++Rounds;
            }

            Console.WriteLine($"\n----------------------------------------\nEND RESULT\n----------------------------------------");
            new PrintService().Print(Attacker, Defender);
            return Outcome.Inconclusive;
        }
    }
}
