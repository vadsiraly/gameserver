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
        public double MaxHealth { get; set; }
        public double MaxMana { get; set; }
        public double Health { get => ApplyPersistentEffects(health, EffectTargetAttribute.Health); set => health = value; }
        public double Mana { get => ApplyPersistentEffects(mana, EffectTargetAttribute.Mana); set => mana = value; }
        public double Damage { get => ApplyPersistentEffects(damage, EffectTargetAttribute.Damage); set => damage = value; }
        public double Armor { get => ApplyPersistentEffects(armor, EffectTargetAttribute.Armor); set => armor = value; }
        public double Resistance { get => ApplyPersistentEffects(resistance, EffectTargetAttribute.Resistance); set => resistance = value; }
        public double Speed { get => ApplyPersistentEffects(speed, EffectTargetAttribute.Speed); set => speed = value; }
        public double CriticalChance { get => ApplyPersistentEffects(criticalChance, EffectTargetAttribute.CriticalChance); set => criticalChance = value; }
        public double CriticalMultiplier { get => ApplyPersistentEffects(criticalMultiplier, EffectTargetAttribute.CriticalMultiplier); set => criticalMultiplier = value; }
        public Ability[] Abilities { get; set; } = new Ability[4];
        public Ability BasicAttack { 
            get
            {
                // yuck!
                return new Ability()
                {
                    Name = "Basic attack",
                    ManaCost = 0,
                    Cooldown = 0,
                    EffectGroups = new List<EffectGroup>
                    {
                        new EffectGroup
                        {
                            Target = EffectGroupTarget.RandomEnemy,
                            Effects = new List<Effect>
                            {
                                new Effect
                                {
                                    Schedule = EffectSchedule.Permanent,
                                    TargetAttribute = EffectTargetAttribute.Health,
                                    CanCrit = true,
                                    Type = DamageType.Physical,
                                    ValueType = EffectValueType.Value,
                                    Value = Damage,
                                }
                            }
                        }
                    }
                };
            }
        }

        // effects
        public List<(Unit Source, Ability SourceAbility, Effect Effect)> PermanentEffects { get; set; } = new List<(Unit Source, Ability SourceAbility, Effect Effect)>();
        public List<(Unit Source, Ability SourceAbility, Effect Effect, int Delay)> DelayedEffects { get; set; } = new List<(Unit Source, Ability SourceAbility, Effect Effect, int Delay)>();
        public List<(Unit Source, Ability SourceAbility, Effect Effect, int Duration)> PersistentEffects { get; set; } = new List<(Unit Source, Ability SourceAbility, Effect Effect, int Duration)>();
        public List<(Unit Source, Ability SourceAbility, Effect Effect, int Duration)> BeforeRoundEffects { get; set; } = new List<(Unit Source, Ability SourceAbility, Effect Effect, int Duration)>();
        public List<(Unit Source, Ability SourceAbility, Effect Effect, int Duration)> AfterRoundEffects { get; set; } = new List<(Unit Source, Ability SourceAbility, Effect Effect, int Duration)>();

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
            ProcesDelayedEffects(random);
            ProcessAfterRoundEffects(random);
            ProcessPersistentEffects(random);
            DecreaseCooldowns();
        }

        public void ProcesDelayedEffects(Random random)
        {
            for (int i = 0; i < DelayedEffects.Count; i++)
            {
                DelayedEffects[i] = (DelayedEffects[i].Source, DelayedEffects[i].SourceAbility, DelayedEffects[i].Effect, DelayedEffects[i].Delay - 1);
                if (DelayedEffects[i].Delay == 0)
                {
                    DelayedEffects[i].Effect.Apply(DelayedEffects[i].Source, this, random);
                }
            }

            DelayedEffects.RemoveAll(x => x.Delay == 0);
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
            for (int i = 0; i < PersistentEffects.Count; i++)
            {
                PersistentEffects[i] = (PersistentEffects[i].Source, PersistentEffects[i].SourceAbility, PersistentEffects[i].Effect, PersistentEffects[i].Duration - 1);
            }
            PersistentEffects.RemoveAll(x => x.Duration == 0);
        }

        public void ProcessBeforeRoundEffects(Random random)
        {
            for (int i = 0; i < BeforeRoundEffects.Count; i++)
            {
                BeforeRoundEffects[i].Effect.Apply(BeforeRoundEffects[i].Source, this, random);
                BeforeRoundEffects[i] = (BeforeRoundEffects[i].Source, BeforeRoundEffects[i].SourceAbility, BeforeRoundEffects[i].Effect, BeforeRoundEffects[i].Duration - 1);
            }

            BeforeRoundEffects.RemoveAll(x => x.Duration == 0);
        }

        public void ProcessAfterRoundEffects(Random random)
        {
            for (int i = 0; i < AfterRoundEffects.Count; i++)
            {
                AfterRoundEffects[i].Effect.Apply(AfterRoundEffects[i].Source, this, random);
                AfterRoundEffects[i] = (AfterRoundEffects[i].Source, AfterRoundEffects[i].SourceAbility, AfterRoundEffects[i].Effect, AfterRoundEffects[i].Duration - 1);
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
                        default:
                            break;
                    }
                }
            }

            return value;
        }

        public void ApplyEffect(Unit source, Ability sourceAbility, List<Effect> effects, Random random)
        {
            foreach (var effect in effects)
            {
                switch (effect.Schedule)
                {
                    case EffectSchedule.Permanent:
                        PermanentEffects.Add((source, sourceAbility, effect));
                        effect.Apply(source, this, random);
                        break;
                    case EffectSchedule.Persistent:
                        PersistentEffects.Add((source, sourceAbility, effect, effect.Duration));
                        break;
                    case EffectSchedule.BeforeRound:
                        BeforeRoundEffects.Add((source, sourceAbility, effect, effect.Duration));
                        break;
                    case EffectSchedule.AfterRound:
                        AfterRoundEffects.Add((source, sourceAbility, effect, effect.Duration));
                        break;
                    case EffectSchedule.Delayed:
                        DelayedEffects.Add((source, sourceAbility, effect, effect.Delay));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
