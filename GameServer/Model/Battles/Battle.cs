using GameServer.Model.Units;
using GameServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Battles
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
        public event EventHandler OnBattleBegin;
        public event EventHandler OnBattleEnd;
        public event EventHandler OnRoundBegin;
        public event EventHandler OnRoundEnd;

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

        public Outcome Fight(Random random)
        {
            var ps = new PrintService();

            OnBattleBegin?.Invoke(this, new EventArgs());

            while (Outcome == Outcome.Inconclusive && Rounds++ < RoundLimit)
            { 
                OnRoundBegin?.Invoke(this, new EventArgs());

                foreach (var current in AliveUnits.OrderBy(x => x.Speed))
                {
                    if (IsAttacker(current))
                    {
                        var aliveDefenders = Defender.Units.Where(x => !x.IsDead).ToList();
                        if (aliveDefenders.Count > 0)
                        {
                            var defender = aliveDefenders[random.Next(0, aliveDefenders.Count)];
                            current.Attack(new List<Unit> { defender });
                        }
                    }

                    else if (IsDefender(current))
                    {
                        var aliveAttackers = Attacker.Units.Where(x => !x.IsDead).ToList();
                        if (aliveAttackers.Count > 0)
                        {
                            var attacker = aliveAttackers[random.Next(0, aliveAttackers.Count)];
                            current.Attack(new List<Unit> { attacker });
                        }
                    }
                }

                OnRoundEnd?.Invoke(this, new EventArgs());
            }

            OnBattleEnd?.Invoke(this, new EventArgs());

            return Outcome;
        }

        private Outcome Outcome
        {
            get
            {
                var allAttackersDead = Attacker.Units.All(x => x.IsDead);
                var allDefendersDead = Defender.Units.All(x => x.IsDead);
                if (allAttackersDead && !allDefendersDead)
                {
                    return Outcome.Loss;
                }
                else if (!allAttackersDead && allDefendersDead)
                {
                    return Outcome.Win;
                }
                else if (allAttackersDead && allDefendersDead)
                {
                    return Outcome.Tie;
                }
                else
                {
                    return Outcome.Inconclusive;
                }
            }
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
