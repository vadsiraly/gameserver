using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Effects;
using GameServer.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Model.Units
{
    public abstract class Unit : IUnit
    {
        public event EventHandler<AttackEventArgs> BeforeAttackEvent;
        public event EventHandler<AttackEventArgs> AfterAttackEvent;
        public event EventHandler<AttackEventArgs> BeforeAbilityUsedEvent;
        public event EventHandler<AttackEventArgs> AfterAbilityUsedEvent;
        public event EventHandler<AttackedEventArgs> BeforeAttackedEvent;
        public event EventHandler<AttackedEventArgs> AfterAttackedEvent;
        public event EventHandler<EffectEventArgs> BeforeEffectAddedEvent;
        public event EventHandler<EffectEventArgs> AfterEffectRemovedEvent;

        public bool IsDead => Health <= 0;
        public Team Team { get; protected set; }
        public abstract string Name { get; }
        public abstract double Health { get; protected set; }
        public abstract double Mana { get; protected set; }
        public abstract double Speed { get; protected set; }

        public abstract IAbility BasicAttack { get; protected set; }
        public abstract List<IAbility> Abilities { get; protected set; }

        public abstract List<Effect> Buffs { get; protected set; }
        public abstract List<Effect> Debuffs { get; protected set; }

        public virtual void BeforeAttack(AttackEventArgs e)
        {
            BeforeAttackEvent?.Invoke(this, e);
        }
        public virtual void AfterAttack(AttackEventArgs e)
        {
            AfterAttackEvent?.Invoke(this, e);
        }

        public virtual void BeforeAttacked(AttackedEventArgs e)
        {
            BeforeAttackedEvent?.Invoke(this, e);
        }
        public virtual void AfterAttacked(AttackedEventArgs e)
        {
            BeforeAttackedEvent?.Invoke(this, e);
        }
        public virtual void BeforeAbilityUsed(AttackEventArgs e)
        {
            BeforeAbilityUsedEvent?.Invoke(this, e);
        }
        public virtual void AfterAbilityUsed(AttackEventArgs e)
        {
            AfterAbilityUsedEvent?.Invoke(this, e);
        }
        public virtual void BeforeEffectAdded(EffectEventArgs e)
        {
            BeforeEffectAddedEvent?.Invoke(this, e);
        }
        public virtual void AfterEffectRemoved(EffectEventArgs e)
        {
            AfterEffectRemovedEvent?.Invoke(this, e);
        }

        public void AddEffect(Unit caster, Effect effect)
        {
            BeforeEffectAdded(new EffectEventArgs(this, effect));
        }

        public abstract void Attack(List<Unit> targets);

        public abstract void UseAbility(List<Unit> targets, IAbility ability);

        public void TakeDamage(Unit attacker, double amount)
        {
            BeforeAttacked(new AttackedEventArgs(attacker, amount));

            Health -= amount;

            AfterAttacked(new AttackedEventArgs(attacker, amount));
        }

        public void SetTeam(Team t)
        {
            Team = t;
        }
    }
}
