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
            var random = new Random(seed);

            var allAttackersDead = false;
            var allDefendersDead = false;
            var rounds = 0;
            while (!allAttackersDead && !allDefendersDead && rounds < RoundLimit)
            {
                foreach(var current in AllUnits)
                {
                    current.BeginRound(random);
                }

                foreach (var current in AllUnits.OrderBy(x => x.Speed).ToArray().Shuffle())
                {
                    if (Attacker.Units.Contains(current))
                    {
                        if (current.Abilities[0].ActiveCooldown == 0)
                        {
                            current.Abilities[0].Use(current, Defender, random);
                        }
                    }
                    else
                    {
                        if (current.Abilities[0].ActiveCooldown == 0)
                        {
                            current.Abilities[0].Use(current, Attacker, random);
                        }
                    }

                    allAttackersDead = Attacker.Units.All(x => x.IsDead);
                    allDefendersDead = Defender.Units.All(x => x.IsDead);

                    if (allAttackersDead && !allDefendersDead) return Outcome.Loss;
                    if (allDefendersDead && !allAttackersDead) return Outcome.Win;
                    if (allAttackersDead && allDefendersDead) return Outcome.Tie;
                }

                foreach (var current in AllUnits)
                {
                    current.EndRound(random);
                }

                ++rounds;
            }

            return Outcome.Inconclusive;
        }
    }
}
