using GameServer.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.BaseTypes
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
                if (effect.Effect.Delay == 0)
                {
                    ApplyEffect(effect);

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
            var persistentEffects = Effects.Where(x => x.Effect.Schedule == EffectSchedule.Persistent).ToList();
            foreach(var effect in persistentEffects)
            {
                ApplyEffect(effect);

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
                ApplyEffect(effect);

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
                ApplyEffect(effect);

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
            var persistentEffects = Effects.Where(x => x.Effect.Schedule == EffectSchedule.Persistent);
            foreach (var effect in persistentEffects)
            {
                if (effect.Effect.TargetAttribute == attribute)
                {
                    switch (effect.Effect.ValueType)
                    {
                        case EffectValueType.Value:
                            value += FinalizeAmount(effect.Effect);
                            break;
                        case EffectValueType.Percentage:
                            value *= effect.Effect.Percentage;
                            break;
                        default:
                            break;
                    }
                }
            }

            return value;
        }

        private double ApplyDamage((Unit Source, Ability SourceAbility, Effect Effect) effect, double value)
        {
            var result = value;
            switch (effect.Effect.ValueType)
            {
                case EffectValueType.Value:
                    {
                        var actualDamage = FinalizeAmount(effect.Effect);
                        result += actualDamage;
                    }
                    break;
                case EffectValueType.Percentage:
                    {
                        var actualDamage = result * effect.Effect.Percentage;
                        result = actualDamage;
                    }
                    break;
                default:
                    break;
            }

            return result;
        }

        private void ApplyEffect((Unit Source, Ability SourceAbility, Effect Effect) effect)
        {
            switch (effect.Effect.TargetAttribute)
            {
                case EffectTargetAttribute.MaxHealth:
                    BaseMaxHealth = ApplyDamage(effect, BaseMaxHealth);
                    break;
                case EffectTargetAttribute.MaxMana:
                    BaseMaxMana = ApplyDamage(effect, BaseMaxMana);
                    break;
                case EffectTargetAttribute.Health:
                    BaseHealth = ApplyDamage(effect, BaseHealth);
                    break;
                case EffectTargetAttribute.Mana:
                    BaseMana = ApplyDamage(effect, BaseMana);
                    break;
                case EffectTargetAttribute.Armor:
                    BaseArmor = ApplyDamage(effect, BaseArmor);
                    break;
                case EffectTargetAttribute.Resistance:
                    BaseResistance = ApplyDamage(effect, BaseResistance);
                    break;
                case EffectTargetAttribute.Damage:
                    BaseDamage = ApplyDamage(effect, BaseDamage);
                    break;
                case EffectTargetAttribute.CriticalChance:
                    BaseCriticalChance = ApplyDamage(effect, BaseCriticalChance);
                    break;
                case EffectTargetAttribute.CriticalMultiplier:
                    BaseCriticalMultiplier = ApplyDamage(effect, BaseCriticalMultiplier);
                    break;
                case EffectTargetAttribute.Speed:
                    BaseSpeed = ApplyDamage(effect, BaseSpeed);
                    break;
                default:
                    break;
            }
        }

        private double FinalizeAmount(Effect effect)
        {
            var amount = effect.Value * (effect.Positive ? 1 : -1);
            switch (effect.TargetAttribute)
            {
                case EffectTargetAttribute.Health:
                    if (amount >= 0)
                    {
                        return amount;
                    }

                    var reducedDamage = amount;
                    switch (effect.DamageType)
                    {
                        case DamageType.Physical:
                            reducedDamage = amount * (1 - Armor / 100);
                            break;
                        case DamageType.Magical:
                            reducedDamage = amount * (1 - Resistance / 100);
                            break;
                        case DamageType.Pure:
                            break;
                        default:
                            break;
                    }

                    return reducedDamage;

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

        public void AddEffect(Unit source, Ability sourceAbility, List<Effect> effects, Random random)
        {
            foreach (var effect in effects)
            {
                switch (effect.Schedule)
                {
                    case EffectSchedule.Permanent:
                        var appliedEffect = (source, sourceAbility, effect.Clone());
                        AppliedEffects.Add(appliedEffect);
                        ApplyEffect(appliedEffect);
                        break;
                    case EffectSchedule.Persistent:
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
