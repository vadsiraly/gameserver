using GameServer.Model.Abilities;
using GameServer.Model.Abilities.ConcreteAbilities;
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
    public abstract class Unit
    {
        protected Random _random;

        public Unit(Random random)
        {
            _random = random;

            MaxHealth = 0;
            MaxMana = 0;
        }

        public event EventHandler<AttackEventArgs> BeforeBasicAttackEvent;
        public event EventHandler<AttackEventArgs> AfterBasicAttackEvent;
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

        public string Name { get; internal set; }

        private double _maxHealth;
        public double MaxHealth 
        {
            get => _maxHealth; 
            internal set
            {
                var difference = value - _maxHealth;
                _maxHealth = value;

                if(difference > 0)
                {
                    Health += difference;
                }
                else
                {
                    Health = Health;
                }
            } 
        }

        private double _maxMana;
        public double MaxMana
        {
            get => _maxMana;
            internal set
            {
                var difference = value - _maxMana;
                _maxMana = value;
                Mana = Mana;
            }
        }

        private double _health;
        public double Health 
        {
            get => _health; 
            protected set
            {
                if (value > MaxHealth)
                {
                    _health = MaxHealth;
                }
                else if (value <= 0)
                {
                    _health = 0;
                }
                else
                {
                    _health = value;
                }
            } 
        }

        private double _mana;
        public double Mana
        {
            get => _mana;
            protected set
            {
                if (value > MaxMana)
                {
                    _mana = MaxMana;
                }
                else if (value < 0)
                {
                    _mana = 0;
                }
                else
                {
                    _mana = value;
                }
            }
        }

        public double Speed { get; internal set; }

        public double Armor { get; internal set; }
        public double Resistance { get; internal set; }

        public double CriticalHitChance { get; internal set; }
        public double CriticalHitMultiplier { get; internal set; }

        public Dictionary<Status, int> Statuses { get; protected set; } = new Dictionary<Status, int>();

        public Ability BasicAttack { get; internal set; }
        public List<Ability> Abilities { get; protected set; } = new List<Ability>();
        public List<Effect> Buffs { get; protected set; } = new List<Effect>();
        public List<Effect> Debuffs { get; protected set; } = new List<Effect>();

        public virtual void BeforeBasicAttack(AttackEventArgs e)
        {
            BeforeBasicAttackEvent?.Invoke(this, e);
        }
        public virtual void AfterBasicAttack(AttackEventArgs e)
        {
            AfterBasicAttackEvent?.Invoke(this, e);
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

        public void AddBuff(Ability source, Effect effect)
        {
            BeforeEffectAdded(new EffectEventArgs(this, effect));

            Buffs.Add(effect);

            AfterEffectAdded(new EffectEventArgs(this, effect));
        }

        public void RemoveBuff(Ability source, Effect effect)
        {
            BeforeEffectRemoved(new EffectEventArgs(this, effect));

            Buffs.Remove(effect);

            AfterEffectRemoved(new EffectEventArgs(this, effect));
        }

        public void AddDebuff(Ability source, Effect effect)
        {
            BeforeEffectAdded(new EffectEventArgs(this, effect));

            Debuffs.Add(effect);

            AfterEffectAdded(new EffectEventArgs(this, effect));
        }

        public void RemoveDebuff(Ability source, Effect effect)
        {
            BeforeEffectRemoved(new EffectEventArgs(this, effect));

            Debuffs.Remove(effect);

            AfterEffectRemoved(new EffectEventArgs(this, effect));
        }

        public void Attack(List<Unit> targets)
        {
            if (Abilities.Any(x => x.IsActive && x.Available))
            {
                foreach (var ability in Abilities.Where(x => x.IsActive && x.Available))
                {
                    UseAbility(targets, ability);
                    break;
                }
            }
            else
            {
                if (!Statuses.ContainsKey(Status.Disarmed))
                {
                    BeforeBasicAttack(new AttackEventArgs(this, targets, BasicAttack));
                    if (!Statuses.ContainsKey(Status.Blinded) || _random.NextDouble() > EffectConstants.STATUS_BLINDED_MISS_CHANCE)
                    {
                        BasicAttack.Use(targets);
                        AfterBasicAttack(new AttackEventArgs(this, targets, BasicAttack));
                    }
                    else
                    {
                        Console.WriteLine($"{Name}'s basic attack missed on {targets.Select(x => x.Name).Aggregate((c,n) => c + "," + n)} (BLINDED)");
                    }
                }
            }
        }

        public void UseAbility(List<Unit> targets, Ability ability)
        {
            BeforeAbilityUsed(new AttackEventArgs(this, targets, ability));

            ability.Use(targets);

            AfterAbilityUsed(new AttackEventArgs(this, targets, ability));
        }

        public void AddStatus(Ability source, Status status)
        {
            if (Statuses.ContainsKey(status))
            {
                Statuses[status]++;
            }
            else
            {
                Statuses.Add(status, 1);
            }

            Console.WriteLine($"{Name} became {status} by {source.Owner.Name}'s {source.Name}");
        }

        public void RemoveStatus(Ability source, Status status)
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

                Console.WriteLine($"{source.Owner.Name}'s {source.Name} wore off, {Name} is no longer {status}");
            }
        }

        public double TakeDamage(Ability source, AbilityDamage abilityDamage)
        {
            BeforeAttacked(new AttackedEventArgs(source.Owner, abilityDamage));

            var actualDamage = ReduceDamage(abilityDamage.DamageList);
            Health -= actualDamage;
            Console.WriteLine($"{source.Owner.Name}'s {source.Name} dealt {actualDamage:F2} damage to {Name}{(abilityDamage.CriticalPart.Value > 0 ? " (CRIT)" : "")} ({Health:F2}/{MaxHealth})");
            if (IsDead)
            {
                Console.WriteLine($"{Name} has died.");
            }

            AfterAttacked(new AttackedEventArgs(source.Owner, abilityDamage));

            return actualDamage;
        }

        public void Heal(Ability source, AbilityHealing abilityHealing)
        {
            Health += abilityHealing.Healing;
            Console.WriteLine($"{source.Owner.Name}'s {source.Name} restored {abilityHealing.Healing:F2} health to {Name}{(abilityHealing.CriticalPart > 0 ? " (CRIT)" : "")} ({Health:F2}/{MaxHealth})");
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
