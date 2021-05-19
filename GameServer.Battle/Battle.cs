using GameServer.Battles.Recording;
using GameServer.Interfaces;
using GameServer.Interfaces.Events;
using GameServer.Model;
using GameServer.Model.Units;
using GameServer.Recording;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace GameServer.Battles
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
        private BattleRecorder _battleRecorder = new BattleRecorder();

        public event EventHandler OnBattleBegin;
        public event EventHandler OnBattleEnd;
        public event EventHandler<RoundEventArgs> OnRoundBegin;
        public event EventHandler<RoundEventArgs> OnRoundEnd;

        public Team Attacker { get; private set; }
        public Team Defender { get; private set; }
        public int Rounds { get; private set; } = 0;
        public IEnumerable<IUnit> AllUnits { get; private set; }
        public IEnumerable<IUnit> AliveUnits => AllUnits.Where(x => !x.IsDead);

        public int RoundLimit { get; set; } = 200;

        public Battle(Team attacker, Team defender)
        {
            Attacker = attacker;
            Defender = defender;

            RegisterTeam(Attacker);
            RegisterTeam(Defender);

            AllUnits = Attacker.Units.Union(Defender.Units);

            _battleRecorder.RecordUnits(Attacker, Defender);

            OnRoundBegin += Battle_OnRoundBegin;
            OnRoundEnd += Battle_OnRoundEnd;
        }

        private void Battle_OnRoundEnd(object sender, RoundEventArgs e)
        {
            _battleRecorder.EndRound();
        }

        private void Battle_OnRoundBegin(object sender, RoundEventArgs e)
        {
            _battleRecorder.StartRound();
            Console.WriteLine($"======== ROUND {e.Round} ========");
        }

        public void RegisterTeam(Team team)
        {
            foreach(var unit in team.Units)
            {
                OnRoundBegin += unit.RoundBegin;
                OnRoundEnd += unit.RoundEnd;
            }
        }

        public Outcome Fight(Random random)
        {
            OnBattleBegin?.Invoke(this, new EventArgs());

            while (Outcome == Outcome.Inconclusive && ++Rounds <= RoundLimit)
            { 
                OnRoundBegin?.Invoke(this, new RoundEventArgs(Rounds));

                foreach (var current in AliveUnits.OrderBy(x => x.Speed))
                {
                    if (current.IsDead) continue;

                    if (IsAttacker(current))
                    {
                        var aliveDefenders = Defender.Units.Where(x => !x.IsDead).ToList();
                        if (aliveDefenders.Count > 0)
                        {
                            var defender = aliveDefenders[random.Next(0, aliveDefenders.Count)];
                            current.Attack(new List<ITargetable> { defender });
                        }
                    }

                    else if (IsDefender(current))
                    {
                        var aliveAttackers = Attacker.Units.Where(x => !x.IsDead).ToList();
                        if (aliveAttackers.Count > 0)
                        {
                            var attacker = aliveAttackers[random.Next(0, aliveAttackers.Count)];
                            current.Attack(new List<ITargetable> { attacker });
                        }
                    }
                }

                OnRoundEnd?.Invoke(this, new RoundEventArgs(Rounds));
            }

            OnBattleEnd?.Invoke(this, new EventArgs());

            XmlSerializer xsSubmit = new XmlSerializer(typeof(BattleRecord));

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww, new XmlWriterSettings { Indent = true }))
                {
                    xsSubmit.Serialize(writer, _battleRecorder.Finish());
                    var xml = sww.ToString();

                    File.WriteAllText("battle.txt", xml);
                }
            }

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

        private bool IsAttacker(IUnit u)
        {
            return Attacker.Units.Contains(u);
        }

        private bool IsDefender(IUnit u)
        {
            return Defender.Units.Contains(u);
        }
    }
}
