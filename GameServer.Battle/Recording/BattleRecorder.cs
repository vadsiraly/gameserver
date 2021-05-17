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

        public void RecordUnits(IEnumerable<Unit> units)
        {
            foreach(var unit in units)
            {
                unit.AfterDamagedEvent += Unit_AfterDamagedEvent;
                unit.AfterEffectDamagedEvent += Unit_AfterEffectDamagedEvent;
            }
        }

        private void Unit_AfterEffectDamagedEvent(object sender, DamagedEventArgs e)
        {
            var target = sender as Unit;

            StartTick();

            AddAction(ActionType.Effect, e.Source.Owner.Snapshot(), target.Snapshot(), e.Source.Reference, e.ModifiedDamage.Snapshot());

            EndTick(e.Source.Owner.Team.Snapshot(), target.Team.Snapshot());
        }

        private void Unit_AfterDamagedEvent(object sender, DamagedEventArgs e)
        {
            var target = sender as Unit;

            StartTick();

            AddAction(ActionType.Ability, e.Source.Owner.Snapshot(), target.Snapshot(), e.Source.Reference, e.ModifiedDamage.Snapshot());

            EndTick(e.Source.Owner.Team.Snapshot(), target.Team.Snapshot());
        }

        public void StartTick()
        {
            CurrentTick = new Tick();
        }

        public void AddAction(ActionType actionType, UnitSnapshot caster, UnitSnapshot target, string sourceRef, ModifiedDamageSnapshot damage)
        {
            var action = new Action { ActionType = actionType, Caster = caster, Target = target, AbilityRef = sourceRef, Damage = damage };
            CurrentTick.Actions.Add(action);
        }

        public void EndTick(TeamSnapshot attackers, TeamSnapshot defenders)
        {
            CurrentTick.Attackers = attackers;
            CurrentTick.Defenders = defenders;
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
