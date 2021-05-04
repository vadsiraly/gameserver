using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Effects;
using GameServer.Model.Battles;
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
        public event EventHandler<EffectEventArgs> AfterEffectAddedEvent;
        public event EventHandler<EffectEventArgs> BeforeEffectRemovedEvent;
        public event EventHandler<EffectEventArgs> AfterEffectRemovedEvent;

        public bool IsDead => Health <= 0;
        public Team Team { get; protected set; }

        public abstract string Name { get; }
        public abstract double Health { get; protected set; }
        public abstract double Mana { get; protected set; }
        public abstract double Speed { get; protected set; }

        public abstract Dictionary<Status, int> Statuses { get; protected set; }

        public abstract double Armor { get; protected set; }
        public abstract double Resistance { get; protected set; }

        public abstract double CriticalHitChance { get; protected set; }
        public abstract double CriticalHitMultiplier { get; protected set; }

        public abstract IAbility BasicAttack { get; protected set; }
        public List<IAbility> Abilities { get; protected set; } = new List<IAbility>();
        public List<Effect> Buffs { get; protected set; } = new List<Effect>();
        public List<Effect> Debuffs { get; protected set; } = new List<Effect>();

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
            AfterAttackedEvent?.Invoke(this, e);
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
        public virtual void AfterEffectAdded(EffectEventArgs e)
        {
            AfterEffectAddedEvent?.Invoke(this, e);
        }
        public virtual void BeforeEffectRemoved(EffectEventArgs e)
        {
            BeforeEffectRemovedEvent?.Invoke(this, e);
        }
        public virtual void AfterEffectRemoved(EffectEventArgs e)
        {
            AfterEffectRemovedEvent?.Invoke(this, e);
        }

        public virtual void RoundBegin(object sender, RoundEventArgs e)
        {
            foreach (var ability in Abilities)
            {
                ability.Tick();
            }
        }

        public virtual void RoundEnd(object sender, RoundEventArgs e)
        {
            for (int i = 0; i < Buffs.Count; ++i)
            {
                Buffs[i].Tick(this);
            }

            for (int i = 0; i < Debuffs.Count; ++i)
            {
                Debuffs[i].Tick(this);
            }
        }

        public void AddBuff(Effect effect)
        {
            BeforeEffectAdded(new EffectEventArgs(this, effect));

            Buffs.Add(effect);

            AfterEffectAdded(new EffectEventArgs(this, effect));
        }

        public void RemoveBuff(Effect effect)
        {
            BeforeEffectRemoved(new EffectEventArgs(this, effect));

            Buffs.Remove(effect);

            AfterEffectRemoved(new EffectEventArgs(this, effect));
        }

        public void AddDebuff(Effect effect)
        {
            BeforeEffectAdded(new EffectEventArgs(this, effect));

            Debuffs.Add(effect);

            AfterEffectAdded(new EffectEventArgs(this, effect));
        }

        public void RemoveDebuff(Effect effect)
        {
            BeforeEffectRemoved(new EffectEventArgs(this, effect));

            Debuffs.Remove(effect);

            AfterEffectRemoved(new EffectEventArgs(this, effect));
        }

        public void Attack(List<Unit> targets)
        {
            if (Abilities.Any(x => x.Available))
            {
                foreach (var ability in Abilities.Where(x => x.Available))
                {
                    UseAbility(targets, ability);
                    break;
                }
            }
            else
            {
                if (!Statuses.ContainsKey(Status.Disarmed))
                {
                    BeforeAttack(new AttackEventArgs(this, targets, BasicAttack));
                    BasicAttack.Use(targets);
                    AfterAttack(new AttackEventArgs(this, targets, BasicAttack));
                }
            }
        }

        public void UseAbility(List<Unit> targets, IAbility ability)
        {
            BeforeAbilityUsed(new AttackEventArgs(this, targets, ability));

            ability.Use(targets);

            AfterAbilityUsed(new AttackEventArgs(this, targets, ability));
        }

        public void AddStatus(Status status)
        {
            if (Statuses.ContainsKey(status))
            {
                Statuses[status]++;
            }
            else
            {
                Statuses.Add(status, 1);
            }

            Console.WriteLine($"Status added: {status}");
        }

        public void RemoveStatus(Status status)
        {
            if (Statuses.ContainsKey(status))
            {
                if (Statuses[status] == 1)
                {
                    Statuses.Remove(status);
                }
                else
                {
                    Statuses[status]--;
                }

                Console.WriteLine($"Status removed: {status}");
            }
        }

        public void TakeDamage(Unit attacker, AbilityDamage damage)
        {
            BeforeAttacked(new AttackedEventArgs(attacker, damage));

            if (damage != null)
            {
                var actualDamage = ReduceDamage(damage.DamageList);
                Health -= actualDamage;
                Console.WriteLine($"Damage dealt: {actualDamage}");
            }

            AfterAttacked(new AttackedEventArgs(attacker, damage));
        }

        public void SetTeam(Team t)
        {
            Team = t;
        }

        public double ReduceDamage(List<Damage> damages)
        {
            var actualDamage = 0d;
            foreach(var damage in damages)
            {
                switch (damage.Type)
                {
                    case DamageType.Physical:
                        if (Armor > 0)
                        {
                            var multiplier = 100 / (100 + Armor);
                            actualDamage += damage.Value * multiplier;
                        }
                        else
                        {
                            var multiplier = 2 - 100 / (100 - Armor);
                            actualDamage += damage.Value * multiplier;
                        }
                        break;
                    case DamageType.Magical:
                        if (Resistance > 0)
                        {
                            var multiplier = 100 / (100 + Resistance);
                            actualDamage += damage.Value * multiplier;
                        }
                        else
                        {
                            var multiplier = 2 - 100 / (100 - Resistance);
                            actualDamage += damage.Value * multiplier;
                        }
                        break;
                    case DamageType.Composite:
                        var halfDamage = damage.Value / 2;
                        if (Armor > 0)
                        {
                            var multiplier = 100 / (100 + Armor);
                            actualDamage += halfDamage / 2 * multiplier;
                        }
                        else
                        {
                            var multiplier = 2 - 100 / (100 - Armor);
                            actualDamage += halfDamage * multiplier;
                        }

                        if (Resistance > 0)
                        {
                            var multiplier = 100 / (100 + Resistance);
                            actualDamage += halfDamage * multiplier;
                        }
                        else
                        {
                            var multiplier = 2 - 100 / (100 - Resistance);
                            actualDamage += halfDamage * multiplier;
                        }
                        break;
                    case DamageType.Pure:
                        actualDamage += damage.Value;
                        break;
                }
            }

            return actualDamage;
        }
    }
}
