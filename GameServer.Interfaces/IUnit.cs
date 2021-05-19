using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.Interfaces.Events;
using GameServer.Interfaces.Snapshots;

namespace GameServer.Interfaces
{
    public interface IUnit : ITargetable
    {
        public event EventHandler<AttackEventArgs> BeforeBasicAttackEvent;
        public event EventHandler<AttackEventArgs> AfterBasicAttackEvent;
        public event EventHandler<AttackEventArgs> BeforeAbilityUsedEvent;
        public event EventHandler<AttackEventArgs> AfterAbilityUsedEvent;
        public event EventHandler<DamagedEventArgs> BeforeDamagedEvent;
        public event EventHandler<DamagedEventArgs> AfterDamagedEvent;
        public event EventHandler<DamagedEventArgs> BeforeEffectDamagedEvent;
        public event EventHandler<DamagedEventArgs> AfterEffectDamagedEvent;
        public event EventHandler<EffectEventArgs> BeforeEffectAddedEvent;
        public event EventHandler<EffectEventArgs> AfterEffectAddedEvent;
        public event EventHandler<EffectEventArgs> BeforeEffectRemovedEvent;
        public event EventHandler<EffectEventArgs> AfterEffectRemovedEvent;
        public event EventHandler<StatusEventArgs> BeforeStatusAppliedEvent;
        public event EventHandler<StatusEventArgs> AfterStatusAppliedEvent;
        public event EventHandler<StatusEventArgs> BeforeStatusRemovedEvent;
        public event EventHandler<StatusEventArgs> AfterStatusRemovedEvent;
        public event EventHandler<EventArgs> OnDeathEvent;

        public bool IsDead { get; }
        public ITeam Team { get; }

        public string Name { get; set; }
        public string Reference { get; }
        public double MaxHealth { get; set; }
        public double MaxMana { get; set; }
        public double Health { get; set; }
        public double Mana { get; set; }

        public double Speed { get; set; }

        public double Armor { get; set; }
        public double Resistance { get; set; }

        public double CriticalHitChance { get; set; }
        public double CriticalHitMultiplier { get; set; }

        public List<(IAbility Source, Status Status)> Statuses { get; }

        public IAbility BasicAttack { get; set; }
        public List<IAbility> Abilities { get; }
        public List<(IEffect Effect, int Stack)> Buffs { get; }
        public List<(IEffect Effect, int Stack)> Debuffs { get; }

        public void RoundBegin(object sender, RoundEventArgs e);

        public void RoundEnd(object sender, RoundEventArgs e);


        public void Attack(List<ITargetable> targets);
        public void UseAbility(List<ITargetable> targets, IAbility ability);

        public void SetTeam(ITeam t);

        public void Die();

        public UnitSnapshot Snapshot();
    }
}
