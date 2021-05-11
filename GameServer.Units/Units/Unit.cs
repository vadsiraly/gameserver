using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Damages;
using GameServer.Model.Abilities.Effects;
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
        public event EventHandler<EventArgs> OnDeathEvent;

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
                    Die();
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
        public List<(Effect Effect, int Stack)> Buffs { get; protected set; } = new List<(Effect Effect, int Stack)>();
        public List<(Effect Effect, int Stack)> Debuffs { get; protected set; } = new List<(Effect Effect, int Stack)>();

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
        public virtual void OnDeath(EventArgs e)
        {
            OnDeathEvent?.Invoke(this, e);
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
                var buff = Buffs[i];
                buff.Effect.Tick(this, buff.Stack);
            }

            for (int i = 0; i < Debuffs.Count; ++i)
            {
                var debuff = Debuffs[i];
                debuff.Effect.Tick(this, debuff.Stack);
            }
        }

        public void AddBuff(Effect effect)
        {
            BeforeEffectAdded(new EffectEventArgs(this, effect));

            var buff = Buffs.FirstOrDefault(x => x.Effect.Name == effect.Name && x.Effect.Source.Owner.Name == effect.Source.Owner.Name);
            if (buff == default)
            {
                Buffs.Add((effect, 1));
            }
            else
            {
                if (buff.Stack < effect.MaxStack)
                {
                    buff.Stack++;
                }

                buff.Effect.Duration = effect.Duration;
            }

            AfterEffectAdded(new EffectEventArgs(this, effect));
        }

        public void RemoveBuff(Effect effect)
        {
            BeforeEffectRemoved(new EffectEventArgs(this, effect));

            var buff = Buffs.FirstOrDefault(x => x.Effect.Name == effect.Name && x.Effect.Source.Owner.Name == effect.Source.Owner.Name);
            if (buff != default)
            {
                Buffs.Remove(buff);
            }

            AfterEffectRemoved(new EffectEventArgs(this, effect));
        }

        public void AddDebuff(Effect effect)
        {
            BeforeEffectAdded(new EffectEventArgs(this, effect));

            var debuff = Debuffs.FirstOrDefault(x => x.Effect.Name == effect.Name && x.Effect.Source.Owner.Name == effect.Source.Owner.Name);
            if (debuff == default)
            {
                Debuffs.Add((effect, 1));
            }
            else
            {
                if (debuff.Stack < effect.MaxStack)
                {
                    debuff.Stack++;
                }

                debuff.Effect.Duration = effect.Duration;
            }

            AfterEffectAdded(new EffectEventArgs(this, effect));
        }

        public void RemoveDebuff(Effect effect)
        {
            BeforeEffectRemoved(new EffectEventArgs(this, effect));

            var debuff = Debuffs.FirstOrDefault(x => x.Effect.Name == effect.Name && x.Effect.Source.Owner.Name == effect.Source.Owner.Name);
            if (debuff != default)
            {
                Debuffs.Remove(debuff);
            }

            AfterEffectRemoved(new EffectEventArgs(this, effect));
        }

        public void Attack(List<Unit> targets)
        {
            if (IsDead) return;

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

        public CombinedDamage TakeDamage(Ability source, CombinedDamage combinedDamage)
        {
            BeforeAttacked(new AttackedEventArgs(source.Owner, combinedDamage));

            var reducedCombinedDamage = ReduceCombinedDamage(combinedDamage);
            var reducedDamage = reducedCombinedDamage.Aggregate();
            Health -= reducedDamage.Sum;
            Console.WriteLine($"{source.Owner.Name}'s {source.Name} dealt {reducedDamage} damage to {Name}{(combinedDamage.DamageCollection.Any(x => x.Damage.IsCritical) ? " (CRIT)" : "")} ({Health:F2}/{MaxHealth})");
            if (IsDead)
            {
                Console.WriteLine($"{Name} has died.");
            }

            AfterAttacked(new AttackedEventArgs(source.Owner, reducedCombinedDamage));

            return reducedCombinedDamage;
        }

        public CombinedDamage TakeEffectDamage(Ability source, CombinedDamage combinedDamage)
        {
            var reducedCombinedDamage = ReduceCombinedDamage(combinedDamage);
            var reducedDamage = reducedCombinedDamage.Aggregate();
            Health -= reducedDamage.Sum;
            Console.WriteLine($"{source.Owner.Name}'s {source.Name} dealt {reducedDamage} damage to {Name}{(combinedDamage.DamageCollection.Any(x => x.Damage.IsCritical) ? " (CRIT)" : "")} ({Health:F2}/{MaxHealth})");
            if (IsDead)
            {
                Console.WriteLine($"{Name} has died.");
            }

            return reducedCombinedDamage;
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

        public CombinedDamage ReduceCombinedDamage(CombinedDamage combinedDamage)
        {
            var reducedCombinedDamage = new CombinedDamage((combinedDamage.BaseDamage.Source, combinedDamage.BaseDamage.Damage.Mitigate(Armor, Resistance)));
            foreach (var damage in combinedDamage.DamageCollection)
            {
                reducedCombinedDamage.DamageCollection.Add((damage.Source, damage.Damage.Mitigate(Armor, Resistance)));
            }

            return reducedCombinedDamage;
        }

        public void Die()
        {
            OnDeath(new EventArgs());

            Debuffs.Clear();
            Buffs.Clear();
        }
    }
}
