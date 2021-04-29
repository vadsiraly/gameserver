using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units
{
    public interface IUnit
    {
        event EventHandler<AttackEventArgs> BeforeAttackEvent;
        event EventHandler<AttackEventArgs> AfterAttackEvent;
        event EventHandler<AttackEventArgs> BeforeAbilityUsedEvent;
        event EventHandler<AttackEventArgs> AfterAbilityUsedEvent;
        event EventHandler<AttackedEventArgs> BeforeAttackedEvent;
        event EventHandler<AttackedEventArgs> AfterAttackedEvent;
        event EventHandler<EffectEventArgs> BeforeEffectAddedEvent;
        event EventHandler<EffectEventArgs> AfterEffectRemovedEvent;

        void BeforeAttack(AttackEventArgs e);
        void AfterAttack(AttackEventArgs e);
        void BeforeAbilityUsed(AttackEventArgs e);
        void AfterAbilityUsed(AttackEventArgs e);
        void BeforeAttacked(AttackedEventArgs e);
        void AfterAttacked(AttackedEventArgs e);
        void BeforeEffectAdded(EffectEventArgs e);
        void AfterEffectRemoved(EffectEventArgs e);

        Team Team { get; }

        string Name { get; }
        double Health { get; }
        double Mana { get; }
        double Speed { get; }

        IAbility BasicAttack { get; }
        List<IAbility> Abilities { get; }

        List<Effect> Buffs { get; }
        List<Effect> Debuffs { get; }

        void AddEffect(Unit caster, Effect effect);
        void Attack(List<Unit> targets);
        void UseAbility(List<Unit> targets, IAbility ability);
        void TakeDamage(Unit attacker, double amount);
    }
}
