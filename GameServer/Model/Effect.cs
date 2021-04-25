using GameServer.Model.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    public enum EffectSchedule
    {
        Permanent,
        Persistent,
        BeforeRound,
        AfterRound,
        Delayed
    }

    public enum EffectTargetAttribute
    {
        MaxHealth,
        MaxMana,
        Health,
        Mana,
        Armor,
        Resistance,
        Damage,
        CriticalChance,
        CriticalMultiplier,
        Speed
    }

    public enum EffectValueType
    {
        Value,
        Percentage
    }

    public class Effect
    {
        public EffectSchedule Schedule { get; set; } = EffectSchedule.Permanent;
        public EffectTargetAttribute TargetAttribute { get; set; }

        public int Delay { get; set; } = 0;
        public int Duration { get; set; } = 1;
        public bool Positive { get; set; } = false;
        public bool CanCrit { get; set; } = false;
        public DamageType Type { get; set; }

        public EffectValueType ValueType { get; set; } = EffectValueType.Value;
        public double DamageMultiplier { get; set; } = 1;
        public double Percentage { get; set; } = 1;
        public double Value { get; set; } = 0;

        public void Apply(Unit source, Unit target, Random random)
        {
            switch (TargetAttribute)
            {
                case EffectTargetAttribute.MaxHealth:
                    {
                        var previousMaxHealth = target.MaxHealth;
                        var value = ApplyEffect(source, target, target.MaxHealth);

                        var difference = value - previousMaxHealth;
                        if (difference > 0)
                        {
                            target.Health += difference;
                        }

                        if (target.Health > target.MaxHealth)
                        {
                            target.Health = target.MaxHealth;
                        }
                    }
                    break;
                case EffectTargetAttribute.MaxMana:
                    {
                        var previousMaxMana = target.MaxMana;
                        var value = ApplyEffect(source, target, target.MaxMana);

                        var difference = value - previousMaxMana;
                        if (difference > 0)
                        {
                            target.Mana += difference;
                        }

                        if (target.Mana > target.MaxMana)
                        {
                            target.Mana = target.MaxMana;
                        }
                    }
                    break;
                case EffectTargetAttribute.Health:
                    {
                        var value = ApplyEffect(source, target, target.Health);
                        target.Health = value;
                    }
                    break;
                case EffectTargetAttribute.Mana:
                    {
                        var value = ApplyEffect(source, target, target.Mana);
                        target.Mana = value;
                    }
                    break;
                case EffectTargetAttribute.Armor:
                    {
                        var value = ApplyEffect(source, target, target.Armor);
                        target.Armor = value;
                    }
                    break;
                case EffectTargetAttribute.Resistance:
                    {
                        var value = ApplyEffect(source, target, target.Resistance);
                        target.Resistance = value;
                    }
                    break;
                case EffectTargetAttribute.Damage:
                    {
                        var value = ApplyEffect(source, target, target.Damage);
                        target.Damage = value;
                    }
                    break;
                case EffectTargetAttribute.CriticalChance:
                    {
                        var value = ApplyEffect(source, target, target.CriticalChance);
                        target.CriticalChance = value;
                    }
                    break;
                case EffectTargetAttribute.CriticalMultiplier:
                    {
                        var value = ApplyEffect(source, target, target.CriticalMultiplier);
                        target.CriticalMultiplier = value;
                    }
                    break;
                case EffectTargetAttribute.Speed:
                    {
                        var value = ApplyEffect(source, target, target.Speed);
                        target.Speed = value;
                    }
                    break;
                default:
                    break;
            }
        }

        private double Limit(double baseValue, double addition, double minValue, double maxValue)
        {
            if (baseValue + addition > maxValue) return maxValue - baseValue;
            if (baseValue + addition < minValue) return -baseValue;
            return addition;
        }

        private double ApplyEffect(Unit source, Unit target, double value)
        {
            switch (ValueType)
            {
                case EffectValueType.Value:
                    if (TargetAttribute == EffectTargetAttribute.Health)
                    {
                        if (!Positive)
                        {
                            var reducedDamage = Damage.Calculate(
                                new Damage
                                {
                                    Amount = Value * (Positive ? 1 : -1),
                                    Type = Type
                                },
                                target);

                            var damage = Limit(value, reducedDamage.Amount, 0, target.MaxHealth);
                            value += damage;

                            source.DamageDone += damage;
                        }
                        else
                        {
                            var damage = Limit(value, Value * (Positive ? 1 : -1), 0, target.MaxHealth);
                            value += damage;

                            source.HealingDone += damage;
                        }
                    }
                    else
                    {
                        value += Value * (Positive ? 1 : -1);
                    }
                    break;
                case EffectValueType.Percentage:
                    value *= Percentage;
                    break;
                default:
                    break;
            }

            return value;
        }
    }
}
