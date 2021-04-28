using GameServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
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
        public Team Attacker { get; private set; }
        public Team Defender { get; private set; }
        public int Rounds { get; private set; } = 0;
        public IEnumerable<Unit> AllUnits { get; private set; }
        public IEnumerable<Unit> AliveUnits => AllUnits.Where(x => !x.IsDead);

        public int RoundLimit { get; set; } = 200;

        public Battle(Team attacker, Team defender)
        {
            Attacker = attacker;
            Defender = defender;
            AllUnits = Attacker.Units.Union(Defender.Units);
        }

        public Outcome Fight(int seed)
        {
            var ps = new PrintService();
            var random = new Random(seed);

            var allAttackersDead = false;
            var allDefendersDead = false;
            while (!allAttackersDead && !allDefendersDead && Rounds < RoundLimit)
            {

                ps.WrapPrint(() => { }, Attacker, Defender, $"Starting Round {Rounds + 1}");

                foreach (var current in AllUnits)
                {
                    current.BeginRound(random);
                }

                foreach (var current in AliveUnits.OrderBy(x => x.Speed))
                {
                    if (IsAttacker(current))
                    {
                        ps.WrapPrint(() => { current.BasicAttack.Use(current, Defender, random); }, Attacker, Defender, $"{current.Name} uses {current.BasicAttack.Name}");          
                        if (current.Abilities[1].ActiveCooldown == 0)
                        {
                            ps.WrapPrint(() => { current.Abilities[1].Use(current, Defender, random); }, Attacker, Defender, $"{current.Name} uses {current.Abilities[1].Name}");
                        }
                    }

                    else if (IsDefender(current))
                    {
                        ps.WrapPrint(() => { current.BasicAttack.Use(current, Attacker, random); }, Attacker, Defender, $"{current.Name} uses {current.BasicAttack.Name}");
                        if (current.Abilities[1].ActiveCooldown == 0)
                        {
                            ps.WrapPrint(() => { current.Abilities[1].Use(current, Attacker, random); }, Attacker, Defender, $"{current.Name} uses {current.Abilities[1].Name}");
                        }
                    }
                }

                foreach (var current in AllUnits)
                {
                    current.EndRound(random);
                }

                ps.WrapPrint(() => { }, Attacker, Defender, $"Ending Round {Rounds + 1}");

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

            return Outcome.Inconclusive;
        }

        private bool IsAttacker(Unit u)
        {
            return Attacker.Units.Contains(u);
        }

        private bool IsDefender(Unit u)
        {
            return Defender.Units.Contains(u);
        }
    }
}
