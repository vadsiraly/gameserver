using GameServer.Battles.Recording;
using GameServer.Model;
using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Damages;
using GameServer.Model.Snapshots;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = GameServer.Battles.Recording.Action;

namespace GameServer.Recording
{
    public class BattleRecorder
    {
        private BattleRecord Record { get; set; } = new BattleRecord();
        private Tick CurrentTick { get; set; }
        private Round CurrentRound { get; set; }

        public void RecordUnits(Team attackers, Team defenders)
        {
            foreach(var unit in attackers.Units)
            {
                unit.AfterDamagedEvent += Unit_AfterDamagedEvent;
                unit.AfterEffectDamagedEvent += Unit_AfterEffectDamagedEvent;
            }

            foreach (var unit in defenders.Units)
            {
                unit.AfterDamagedEvent += Unit_AfterDamagedEvent;
                unit.AfterEffectDamagedEvent += Unit_AfterEffectDamagedEvent;
            }

            Record.Attackers = attackers.Snapshot();
            Record.Defenders = defenders.Snapshot();
        }

        private void Unit_AfterEffectDamagedEvent(object sender, DamagedEventArgs e)
        {
            var target = sender as Unit;

            StartTick();

            AddAction(ActionType.Effect, e.Source.Owner.Snapshot(), target.Snapshot(), e.Source.Reference, e.ModifiedDamage.Snapshot());

            EndTick();
        }

        private void Unit_AfterDamagedEvent(object sender, DamagedEventArgs e)
        {
            var target = sender as Unit;

            StartTick();

            AddAction(ActionType.Ability, e.Source.Owner.Snapshot(), target.Snapshot(), e.Source.Reference, e.ModifiedDamage.Snapshot());

            EndTick();
        }

        public void StartTick()
        {
            CurrentTick = new Tick();
        }

        public void AddAction(ActionType actionType, UnitSnapshot caster, UnitSnapshot target, string sourceRef, ModifiedDamageSnapshot damage)
        {
            var action = new Action { ActionType = actionType, CasterRef = caster.Reference, TargetRef = target.Reference, AbilityRef = sourceRef, Damage = damage };
            CurrentTick.Actions.Add(action);
        }

        public void EndTick()
        {
            CurrentRound.Ticks.Add(CurrentTick);
        }

        public void StartRound()
        {
            CurrentRound = new Round();
        }

        public void EndRound()
        {
            Record.Rounds.Add(CurrentRound);
        }

        public BattleRecord Finish()
        {
            CurrentRound.Ticks.Add(CurrentTick);
            Record.Rounds.Add(CurrentRound);

            return Record;
        }
    }
}
