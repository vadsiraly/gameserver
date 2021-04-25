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
        private double damage;
        private double armor;
        private double resistance;
        private double speed;
        private double criticalChance;
        private double criticalMultiplier;
        private double mana;
        private double health;

        [JsonIgnore]
        public Team Team { get; set; }

        [JsonIgnore]
        public bool IsDead => Health <= 0;

        // properties
        public int Id { get; set; }
        public string Name { get; set; }
        public double Health { get => ApplyPersistentEffects(health, EffectTargetAttribute.Health); set => health = value; }
        public double Mana { get => ApplyPersistentEffects(mana, EffectTargetAttribute.Mana); set => mana = value; }
        public double Damage { get => ApplyPersistentEffects(damage, EffectTargetAttribute.Damage); set => damage = value; }
        public double Armor { get => ApplyPersistentEffects(armor, EffectTargetAttribute.Armor); set => armor = value; }
        public double Resistance { get => ApplyPersistentEffects(resistance, EffectTargetAttribute.Resistance); set => resistance = value; }
        public double Speed { get => ApplyPersistentEffects(speed, EffectTargetAttribute.Speed); set => speed = value; }
        public double CriticalChance { get => ApplyPersistentEffects(criticalChance, EffectTargetAttribute.CriticalChance); set => criticalChance = value; }
        public double CriticalMultiplier { get => ApplyPersistentEffects(criticalMultiplier, EffectTargetAttribute.CriticalMultiplier); set => criticalMultiplier = value; }
        public Ability[] Abilities { get; set; } = new Ability[4];

        // effects
        public List<(Unit Source, Effect Effect)> PermanentEffects { get; set; } = new List<(Unit Source, Effect Effect)>();
        public List<(Unit Source, Effect Effect, int Duration)> PersistentEffects { get; set; } = new List<(Unit Source, Effect Effect, int Duration)>();
        public List<(Unit Source, Effect Effect, int Duration)> BeforeRoundEffects { get; set; } = new List<(Unit Source, Effect Effect, int Duration)>();
        public List<(Unit Source, Effect Effect, int Duration)> AfterRoundEffects { get; set; } = new List<(Unit Source, Effect Effect, int Duration)>();
        public List<(Unit Source, Effect Effect, int Delay, int Duration)> DelayedEffects { get; set; } = new List<(Unit Source, Effect Effect, int Delay, int Duration)>();

        //stats
        public void BeginRound(Random random)
        {
            ProcessBeforeRoundEffects(random);
        }

        public void EndRound(Random random)
        {
            ProcessAfterRoundEffects(random);
            ProcessPersistentEffects(random);
        }

        public void ProcessPersistentEffects(Random random)
        {
            for (int i = 0; i < PersistentEffects.Count; i++)
            {
                PersistentEffects[i] = (PersistentEffects[i].Source, PersistentEffects[i].Effect, PersistentEffects[i].Duration - 1);
            }
            PersistentEffects.RemoveAll(x => x.Duration == 0);
        }

        public void ProcessBeforeRoundEffects(Random random)
        {
            for (int i = 0; i < BeforeRoundEffects.Count; i++)
            {
                BeforeRoundEffects[i].Effect.Apply(BeforeRoundEffects[i].Source, this, random);
                BeforeRoundEffects[i] = (BeforeRoundEffects[i].Source, BeforeRoundEffects[i].Effect, BeforeRoundEffects[i].Duration - 1);
            }

            BeforeRoundEffects.RemoveAll(x => x.Duration == 0);
        }

        public void ProcessAfterRoundEffects(Random random)
        {
            for (int i = 0; i < BeforeRoundEffects.Count; i++)
            {
                AfterRoundEffects[i].Effect.Apply(AfterRoundEffects[i].Source, this, random);
                AfterRoundEffects[i] = (AfterRoundEffects[i].Source, AfterRoundEffects[i].Effect, AfterRoundEffects[i].Duration - 1);
            }

            AfterRoundEffects.RemoveAll(x => x.Duration == 0);
        }

        private double ApplyPersistentEffects(double value, EffectTargetAttribute attribute)
        {
            foreach(var persistentEffect in PersistentEffects)
            {
                if (persistentEffect.Effect.TargetAttribute == attribute)
                {
                    switch (persistentEffect.Effect.ValueType)
                    {
                        case EffectValueType.Value:
                            if (persistentEffect.Effect.TargetAttribute == EffectTargetAttribute.Health && !persistentEffect.Effect.Positive)
                            {
                                var reducedDamage = BaseTypes.Damage.Calculate(
                                    new Damage
                                    {
                                        Amount = persistentEffect.Effect.Value * (persistentEffect.Effect.Positive ? 1 : -1),
                                        Type = persistentEffect.Effect.Type
                                    },
                                    this);
                                value += reducedDamage.Amount;
                            }
                            else
                            {
                                value += persistentEffect.Effect.Value * (persistentEffect.Effect.Positive ? 1 : -1);
                            }
                            break;
                        case EffectValueType.Percentage:
                            value *= persistentEffect.Effect.Percentage;
                            break;
                        case EffectValueType.BasedOnSelfDamage:
                            if (persistentEffect.Effect.TargetAttribute == EffectTargetAttribute.Health && !persistentEffect.Effect.Positive)
                            {
                                var reducedDamage = BaseTypes.Damage.Calculate(
                                    new Damage
                                    {
                                        Amount = persistentEffect.Source.Damage * persistentEffect.Effect.DamageMultiplier * (persistentEffect.Effect.Positive ? 1 : -1),
                                        Type = persistentEffect.Effect.Type
                                    },
                                    this);
                                value += reducedDamage.Amount;
                            }
                            else
                            {
                                value += persistentEffect.Source.Damage * persistentEffect.Effect.DamageMultiplier * (persistentEffect.Effect.Positive ? 1 : -1);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return value;
        }

        public void ApplyEffect(Unit source, List<Effect> effects, Random random)
        {
            foreach (var effect in effects)
            {
                switch (effect.Schedule)
                {
                    case EffectSchedule.Permanent:
                        PermanentEffects.Add((source, effect));
                        effect.Apply(source, this, random);
                        break;
                    case EffectSchedule.Persistent:
                        PersistentEffects.Add((source, effect, effect.Duration));
                        break;
                    case EffectSchedule.BeforeRound:
                        BeforeRoundEffects.Add((source, effect, effect.Duration));
                        break;
                    case EffectSchedule.AfterRound:
                        AfterRoundEffects.Add((source, effect, effect.Duration));
                        break;
                    case EffectSchedule.Delayed:
                        DelayedEffects.Add((source, effect, effect.Delay, effect.Duration));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
