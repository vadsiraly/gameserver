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

namespace GameServer.Model
{
    public class Unit
    {
        private double _baseHealth;
        private double _baseMana;
        private double _baseMaxHealth;

        public int Id { get; private set; }
        public string Name { get; private set; }
        private double BaseMaxMana { get; set; }
        private double BaseDamage { get; set; }
        private double BaseArmor { get; set; }
        private double BaseResistance { get; set; }
        private double BaseSpeed { get; set; }
        private double BaseCriticalChance { get; set; }
        private double BaseCriticalMultiplier { get; set; }

        private double BaseHealth
        { 
            get => _baseHealth; 
            set
            {
                if (value > MaxHealth)
                {
                    _baseHealth = MaxHealth;
                } 
                else if (value < 0)
                {
                    _baseHealth = 0;
                }
                else
                {
                    _baseHealth = value;
                }
            }
        }

        private double BaseMana
        {
            get => _baseMana;
            set
            {
                if (value > MaxMana)
                {
                    _baseMana = MaxMana;
                }
                else if (value < 0)
                {
                    _baseMana = 0;
                }
                else
                {
                    _baseMana = value;
                }
            }
        }

        private double BaseMaxHealth
        {
            get => _baseMaxHealth;
            set
            {
                if (value < 1)
                {
                    _baseMaxHealth = 1;
                }
                else
                {
                    _baseMaxHealth = value;
                }
            }
        }

        [JsonConstructor]
        public Unit(int id, string name, double baseHealth, double baseMana, double baseMaxHealth, double baseMaxMana, double baseDamage, double baseArmor, double baseResistance, double baseSpeed, double baseCriticalChance, double baseCriticalMultiplier, List<string> abilities)
        {
            Id = id;
            Name = name;
            BaseMaxHealth = baseMaxHealth;
            BaseMaxMana = baseMaxMana;
            BaseHealth = baseHealth;
            BaseMana = baseMana;
            BaseDamage = baseDamage;
            BaseArmor = baseArmor;
            BaseResistance = baseResistance;
            BaseSpeed = baseSpeed;
            BaseCriticalChance = baseCriticalChance;
            BaseCriticalMultiplier = baseCriticalMultiplier;

            var abilityFactory = new AbilityFactory();
            foreach (var ability in abilities)
            {
                Abilities.Add(abilityFactory.GetByReference(ability));
            }
        }

        public Team Team { get; set; }

        public bool IsDead => Health <= 0;

        // properties
        public double MaxHealth => ApplyPersistentEffects(BaseMaxHealth, EffectTargetAttribute.MaxHealth);
        public double MaxMana => ApplyPersistentEffects(BaseMaxMana, EffectTargetAttribute.MaxMana);
        public double Health => ApplyPersistentEffects(BaseHealth, EffectTargetAttribute.Health);
        public double Mana => ApplyPersistentEffects(BaseMana, EffectTargetAttribute.Mana);
        public double Damage => ApplyPersistentEffects(BaseDamage, EffectTargetAttribute.Damage);
        public double Armor => ApplyPersistentEffects(BaseArmor, EffectTargetAttribute.Armor);
        public double Resistance => ApplyPersistentEffects(BaseResistance, EffectTargetAttribute.Resistance);
        public double Speed => ApplyPersistentEffects(BaseSpeed, EffectTargetAttribute.Speed);
        public double CriticalChance => ApplyPersistentEffects(BaseCriticalChance, EffectTargetAttribute.CriticalChance);
        public double CriticalMultiplier => ApplyPersistentEffects(BaseCriticalMultiplier, EffectTargetAttribute.CriticalMultiplier);
        
        [JsonIgnore]
        public Ability BasicAttack => Abilities[0];
        public List<Ability> Abilities { get; set; } = new List<Ability>();

        // effects
        public List<(Unit Source, Ability SourceAbility, Effect Effect)> AppliedEffects { get; set; } = new List<(Unit Source, Ability SourceAbility, Effect Effect)>();
        public List<(Unit Source, Ability SourceAbility, Effect Effect)> Effects { get; set; } = new List<(Unit Source, Ability SourceAbility, Effect Effect)>();

        //stats
        [JsonIgnore]
        public double DamageDone { get; set; }

        [JsonIgnore]
        public double HealingDone { get; set; }

        public void BeginRound(Random random)
        {
            ProcessBeforeRoundEffects(random);
        }

        public void EndRound(Random random)
        {
            ProcessDelayedEffects(random);
            ProcessAfterRoundEffects(random);
            ProcessPersistentEffects(random);
            DecreaseCooldowns();

            Effects.RemoveAll(x => x.Effect.Delay == 0 || x.Effect.Duration == 0);
        }

        public void ProcessDelayedEffects(Random random)
        {
            var delayedEffect = Effects.Where(x => x.Effect.Schedule == EffectSchedule.Delayed).ToList();
            foreach(var effect in delayedEffect)
            {
                effect.Effect.Delay--;
                if (effect.Effect.Delay == 0)
                {
                    ApplyEffect(effect, random);

                    Effects.Remove(effect);
                    AppliedEffects.Add(effect);
                }
            }
        }

        public void DecreaseCooldowns()
        {
            foreach (var ability in Abilities.Where(x => x != null))
            {
                if (ability.ActiveCooldown > 0)
                {
                    ability.ActiveCooldown--;
                }
            }
        }

        public void ProcessPersistentEffects(Random random)
        {
            var persistentEffects = Effects.Where(x => x.Effect.Schedule == EffectSchedule.Continuous).ToList();
            foreach(var effect in persistentEffects)
            {
                ApplyEffect(effect, random);

                effect.Effect.Duration -= 1;
                if (effect.Effect.Duration == 0)
                {
                    Effects.Remove(effect);
                    AppliedEffects.Add(effect);
                }
            }
        }

        public void ProcessBeforeRoundEffects(Random random)
        {
            var beforeRoundEffects = Effects.Where(x => x.Effect.Schedule == EffectSchedule.BeforeRound).ToList();
            foreach(var effect in beforeRoundEffects)
            {
                ApplyEffect(effect, random);

                effect.Effect.Duration -= 1;
                if (effect.Effect.Duration == 0)
                {
                    Effects.Remove(effect);
                    AppliedEffects.Add(effect);
                }
            }
        }

        public void ProcessAfterRoundEffects(Random random)
        {
            var afterRoundEffects = Effects.Where(x => x.Effect.Schedule == EffectSchedule.AfterRound).ToList();
            foreach (var effect in afterRoundEffects)
            {
                ApplyEffect(effect, random);

                effect.Effect.Duration -= 1;
                if (effect.Effect.Duration == 0)
                {
                    Effects.Remove(effect);
                    AppliedEffects.Add(effect);
                }
            }
        }

        private double ApplyPersistentEffects(double value, EffectTargetAttribute attribute)
        {
            var persistentEffects = Effects.Where(x => x.Effect.Schedule == EffectSchedule.Continuous);
            foreach (var effect in persistentEffects)
            {
                if (effect.Effect.TargetAttribute == attribute)
                {
                    switch (effect.Effect.ValueType)
                    {
                        case EffectValueType.Value:
                            value += FinalizeValue(effect.Source, effect.Effect, null, out bool _);
                            break;
                        case EffectValueType.Percentage:
                            value += FinalizePercentage(effect.Effect);
                            break;
                        default:
                            break;
                    }
                }
            }

            return value;
        }

        private double ApplyDamage((Unit Source, Ability SourceAbility, Effect Effect) effect, double originalValue, Random random)
        {
            var value = originalValue;
            switch (effect.Effect.ValueType)
            {
                case EffectValueType.Value:
                    {
                        var actualDamage = FinalizeValue(effect.Source, effect.Effect, random, out bool isCritical);
                        value += actualDamage;
                        //Console.WriteLine($"{effect.Source.Name}'s {effect.SourceAbility.Name} {(actualDamage > 0 ? "heals" : "hits")} {Name} for {actualDamage:N2} {(effect.Effect.DamageType == DamageType.Pure ? "" : $"({(Math.Abs(effect.Effect.Value) - Math.Abs(actualDamage)):N2} {(effect.Effect.DamageType == DamageType.Magical ? "resisted" : "blocked")})")} {(isCritical ? " Crit!" :"")}");
                        //Thread.Sleep(500);
                    }
                    break;
                case EffectValueType.Percentage:
                    {
                        var actualDamage = value * FinalizePercentage(effect.Effect);
                        value += actualDamage;
                    }
                    break;
                default:
                    break;
            }

            return value;
        }

        private void ApplyEffect((Unit Source, Ability SourceAbility, Effect Effect) effect, Random random)
        {
            switch (effect.Effect.TargetAttribute)
            {
                case EffectTargetAttribute.MaxHealth:
                    BaseMaxHealth = ApplyDamage(effect, BaseMaxHealth, random);
                    break;
                case EffectTargetAttribute.MaxMana:
                    BaseMaxMana = ApplyDamage(effect, BaseMaxMana, random);
                    break;
                case EffectTargetAttribute.Health:
                    BaseHealth = ApplyDamage(effect, BaseHealth, random);
                    break;
                case EffectTargetAttribute.Mana:
                    BaseMana = ApplyDamage(effect, BaseMana, random);
                    break;
                case EffectTargetAttribute.Armor:
                    BaseArmor = ApplyDamage(effect, BaseArmor, random);
                    break;
                case EffectTargetAttribute.Resistance:
                    BaseResistance = ApplyDamage(effect, BaseResistance, random);
                    break;
                case EffectTargetAttribute.Damage:
                    BaseDamage = ApplyDamage(effect, BaseDamage, random);
                    break;
                case EffectTargetAttribute.CriticalChance:
                    BaseCriticalChance = ApplyDamage(effect, BaseCriticalChance, random);
                    break;
                case EffectTargetAttribute.CriticalMultiplier:
                    BaseCriticalMultiplier = ApplyDamage(effect, BaseCriticalMultiplier, random);
                    break;
                case EffectTargetAttribute.Speed:
                    BaseSpeed = ApplyDamage(effect, BaseSpeed, random);
                    break;
                default:
                    break;
            }
        }

        private double FinalizeValue(Unit Source, Effect effect, Random random, out bool isCritical)
        {
            isCritical = false;
            var amount = effect.Value * (effect.Positive ? 1 : -1);
            if (effect.CanCrit && random != null)
            {
                var roll = random.NextDouble();
                if (roll < Source.CriticalChance)
                {
                    amount = amount * Source.CriticalMultiplier;
                    isCritical = true;
                }
            }

            switch (effect.TargetAttribute)
            {
                case EffectTargetAttribute.Health:
                    if (amount >= 0)
                    {
                        return amount;
                    }

                    switch (effect.DamageType)
                    {
                        case DamageType.Physical:
                            return amount * (1 - Armor / 100);
                        case DamageType.Magical:
                            return amount * (1 - Resistance / 100);
                        case DamageType.Pure:
                        default:
                            return amount;
                    }

                case EffectTargetAttribute.MaxHealth:
                case EffectTargetAttribute.MaxMana:
                case EffectTargetAttribute.Mana:
                case EffectTargetAttribute.Armor:
                case EffectTargetAttribute.Resistance:
                case EffectTargetAttribute.Damage:
                case EffectTargetAttribute.CriticalChance:
                case EffectTargetAttribute.CriticalMultiplier:
                case EffectTargetAttribute.Speed:
                default:
                    return amount;
            }
        }

        private double FinalizePercentage(Effect effect)
        {
            var percentage = effect.Percentage * (effect.Positive ? 1 : -1);
            switch (effect.TargetAttribute)
            {
                case EffectTargetAttribute.Health:
                    if (percentage >= 0)
                    {
                        return percentage;
                    }

                    switch (effect.DamageType)
                    {
                        case DamageType.Physical:
                            return percentage * (1 - Armor / 100);
                        case DamageType.Magical:
                            return percentage * (1 - Resistance / 100);
                        case DamageType.Pure:
                        default:
                            return percentage;
                    }

                case EffectTargetAttribute.MaxHealth:
                case EffectTargetAttribute.MaxMana:
                case EffectTargetAttribute.Mana:
                case EffectTargetAttribute.Armor:
                case EffectTargetAttribute.Resistance:
                case EffectTargetAttribute.Damage:
                case EffectTargetAttribute.CriticalChance:
                case EffectTargetAttribute.CriticalMultiplier:
                case EffectTargetAttribute.Speed:
                default:
                    return percentage;
            }
        }

        public void AddEffect(Unit source, Ability sourceAbility, List<Effect> effects, Random random)
        {
            foreach (var effect in effects)
            {
                switch (effect.Schedule)
                {
                    case EffectSchedule.Permanent:
                        var appliedEffect = (source, sourceAbility, effect.Clone());
                        AppliedEffects.Add(appliedEffect);
                        ApplyEffect(appliedEffect, random);
                        break;
                    case EffectSchedule.Continuous:
                        Effects.Add((source, sourceAbility, effect.Clone()));
                        break;
                    case EffectSchedule.BeforeRound:
                        Effects.Add((source, sourceAbility, effect.Clone()));
                        break;
                    case EffectSchedule.AfterRound:
                        Effects.Add((source, sourceAbility, effect.Clone()));
                        break;
                    case EffectSchedule.Delayed:
                        Effects.Add((source, sourceAbility, effect.Clone()));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
