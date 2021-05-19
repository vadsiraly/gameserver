using GameServer.Damages;
using GameServer.Interfaces;
using GameServer.Interfaces.Events;
using GameServer.Interfaces.Snapshots;
using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Model.Units
{
    public abstract class Unit : IUnit, ITargetable
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

        public bool IsDead => Health <= 0;
        public ITeam Team { get; set; }

        public string Name { get; set; }
        public string Reference { get; set; }

        private double _maxHealth;
        public double MaxHealth 
        {
            get => _maxHealth;
            set
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
            set
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
            set
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
            set
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

        public double Speed { get; set; }

        public double Armor { get; set; }
        public double Resistance { get; set; }

        public double CriticalHitChance { get; set; }
        public double CriticalHitMultiplier { get; set; }

        public List<(IAbility Source, Status Status)> Statuses { get; set; } = new List<(IAbility Source, Status Status)>();

        public IAbility BasicAttack { get; set; }
        public List<IAbility> Abilities { get; set; } = new List<IAbility>();
        public List<(IEffect Effect, int Stack)> Buffs { get; set; } = new List<(IEffect Effect, int Stack)>();
        public List<(IEffect Effect, int Stack)> Debuffs { get; set; } = new List<(IEffect Effect, int Stack)>();

        protected virtual void BeforeBasicAttack(AttackEventArgs e)
        {
            BeforeBasicAttackEvent?.Invoke(this, e);
        }
        protected virtual void AfterBasicAttack(AttackEventArgs e)
        {
            AfterBasicAttackEvent?.Invoke(this, e);
        }

        protected virtual void BeforeDamaged(DamagedEventArgs e)
        {
            BeforeDamagedEvent?.Invoke(this, e);
        }
        protected virtual void AfterDamaged(DamagedEventArgs e)
        {
            AfterDamagedEvent?.Invoke(this, e);
        }
        protected virtual void BeforeEffectDamaged(DamagedEventArgs e)
        {
            BeforeEffectDamagedEvent?.Invoke(this, e);
        }
        protected virtual void AfterEffectDamaged(DamagedEventArgs e)
        {
            AfterEffectDamagedEvent?.Invoke(this, e);
        }

        protected virtual void BeforeAbilityUsed(AttackEventArgs e)
        {
            BeforeAbilityUsedEvent?.Invoke(this, e);
        }
        protected virtual void AfterAbilityUsed(AttackEventArgs e)
        {
            AfterAbilityUsedEvent?.Invoke(this, e);
        }
        protected virtual void BeforeEffectAdded(EffectEventArgs e)
        {
            BeforeEffectAddedEvent?.Invoke(this, e);
        }
        protected virtual void AfterEffectAdded(EffectEventArgs e)
        {
            AfterEffectAddedEvent?.Invoke(this, e);
        }
        protected virtual void BeforeEffectRemoved(EffectEventArgs e)
        {
            BeforeEffectRemovedEvent?.Invoke(this, e);
        }
        protected virtual void AfterEffectRemoved(EffectEventArgs e)
        {
            AfterEffectRemovedEvent?.Invoke(this, e);
        }
        protected virtual void BeforeStatusApplied(StatusEventArgs e)
        {
            BeforeStatusAppliedEvent?.Invoke(this, e);
        }
        protected virtual void AfterStatusApplied(StatusEventArgs e)
        {
            AfterStatusAppliedEvent?.Invoke(this, e);
        }
        protected virtual void BeforeStatusRemoved(StatusEventArgs e)
        {
            BeforeStatusRemovedEvent?.Invoke(this, e);
        }
        protected virtual void AfterStatusRemoved(StatusEventArgs e)
        {
            AfterStatusRemovedEvent?.Invoke(this, e);
        }

        protected virtual void OnDeath(EventArgs e)
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

        public void AddBuff(IEffect effect)
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

        public void RemoveBuff(IEffect effect)
        {
            BeforeEffectRemoved(new EffectEventArgs(this, effect));

            var buff = Buffs.FirstOrDefault(x => x.Effect.Name == effect.Name && x.Effect.Source.Owner.Name == effect.Source.Owner.Name);
            if (buff != default)
            {
                Buffs.Remove(buff);
            }

            AfterEffectRemoved(new EffectEventArgs(this, effect));
        }

        public void AddDebuff(IEffect effect)
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

        public void RemoveDebuff(IEffect effect)
        {
            BeforeEffectRemoved(new EffectEventArgs(this, effect));

            var debuff = Debuffs.FirstOrDefault(x => x.Effect.Name == effect.Name && x.Effect.Source.Owner.Name == effect.Source.Owner.Name);
            if (debuff != default)
            {
                Debuffs.Remove(debuff);
            }

            AfterEffectRemoved(new EffectEventArgs(this, effect));
        }

        public void Attack(List<ITargetable> targets)
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
                if (!Statuses.Any(x => x.Status == Status.Disarmed))
                {
                    BeforeBasicAttack(new AttackEventArgs(this, targets, BasicAttack));
                    if (!Statuses.Any(x => x.Status == Status.Blinded) || _random.NextDouble() > EffectConstants.STATUS_BLINDED_MISS_CHANCE)
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

        public void UseAbility(List<ITargetable> targets, IAbility ability)
        {
            BeforeAbilityUsed(new AttackEventArgs(this, targets, ability));

            ability.Use(targets);

            AfterAbilityUsed(new AttackEventArgs(this, targets, ability));
        }

        public void AddStatus(IAbility source, Status status)
        {
            BeforeStatusApplied(new StatusEventArgs(source, status));

            Statuses.Add((source, status));

            Console.WriteLine($"{Name} became {status} by {source.Owner.Name}'s {source.Name}");

            AfterStatusApplied(new StatusEventArgs(source, status));
        }

        public void RemoveStatus(IAbility source, Status status)
        {
            BeforeStatusRemoved(new StatusEventArgs(source, status));

            var activeStatus = Statuses.FirstOrDefault(x => x.Source == source && x.Status == status);
            if (activeStatus != default)
            {
                Statuses.Remove(activeStatus);
                Console.WriteLine($"{source.Owner.Name}'s {source.Name} wore off, {Name} is no longer {status}");
            }

            AfterStatusRemoved(new StatusEventArgs(source, status));
        }

        public IModifiedDamage TakeDamage(IAbility source, IModifiedDamage modifiedDamage)
        {
            BeforeDamaged(new DamagedEventArgs(source, modifiedDamage));

            TakeDamageImpl(source, modifiedDamage);

            AfterDamaged(new DamagedEventArgs(source, modifiedDamage));

            return modifiedDamage;
        }

        public IModifiedDamage TakeEffectDamage(IAbility source, IModifiedDamage modifiedDamage)
        {
            BeforeEffectDamaged(new DamagedEventArgs(source, modifiedDamage));

            TakeDamageImpl(source, modifiedDamage);

            AfterEffectDamaged(new DamagedEventArgs(source, modifiedDamage));

            return modifiedDamage;
        }

        private IModifiedDamage TakeDamageImpl(IAbility source, IModifiedDamage modifiedDamage)
        {
            modifiedDamage.AddReduction(Armor, Resistance);
            var damage = modifiedDamage.Aggregate().Sum;
            Health -= damage;
            Console.WriteLine($"{source.Owner.Name}'s {source.Name} dealt {damage:F2} damage to {Name}{(modifiedDamage.Modifications.Any(x => x.Damage.IsCritical) ? " (CRIT)" : "")} ({Health:F2}/{MaxHealth})");
            if (IsDead)
            {
                Die();
                Console.WriteLine($"{Name} has died.");
            }

            return modifiedDamage;
        }

        public IModifiedDamage Heal(IAbility source, IModifiedDamage modifiedDamage)
        {
            var healing = modifiedDamage.Aggregate().Sum;
            Health += healing;
            Console.WriteLine($"{source.Owner.Name}'s {source.Name} restored {healing:F2} health to {Name}{(modifiedDamage.Modifications.Any(x => x.Damage.IsCritical) ? " (CRIT)" : "")} ({Health:F2}/{MaxHealth})");

            return modifiedDamage;
        }

        public void SetTeam(ITeam t)
        {
            Team = t;
        }

        public void Die()
        {
            OnDeath(new EventArgs());

            Debuffs.Clear();
            Buffs.Clear();
        }

        public UnitSnapshot Snapshot()
        {
            var snapshot = new UnitSnapshot();
            snapshot.Name = Name;
            snapshot.Reference = Reference;
            snapshot.Health = Health;
            snapshot.MaxHealth = MaxHealth;
            snapshot.Mana = Mana;
            snapshot.MaxMana = MaxMana;
            snapshot.Speed = Speed;
            snapshot.Armor = Armor;
            snapshot.Resistance = Resistance;
            snapshot.CriticalHitChance = CriticalHitChance;
            snapshot.CriticalHitMultiplier = CriticalHitMultiplier;

            return snapshot;
        }
    }
}
