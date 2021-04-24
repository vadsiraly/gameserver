using GameServer.Enumerations.Damage;
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
        Health,
        Mana,
        Armor,
        Resistance,
        Damage,
        CriticalChance,
        CriticalMultiplier,
        Speed
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
        public double Percentage { get; set; } = 1;
        public bool BasedOnSelfDamage { get; set; } = false;
        public double DamageMultiplier { get; set; } = 1;

        public void Apply(Unit source, Unit target, Random random)
        {
            var damage = Damage.Calculate(new Damage { Amount = source.Damage * DamageMultiplier, Type = Type }, target);
            if (!Positive)
            {
                damage.Amount *= -1;
            }

            switch (TargetAttribute)
            {
                case EffectTargetAttribute.Health:
                    target.Health += damage.Amount;
                    break;
                case EffectTargetAttribute.Mana:
                    target.Mana += damage.Amount;
                    break;
                case EffectTargetAttribute.Armor:
                    target.Armor += damage.Amount;
                    break;
                case EffectTargetAttribute.Resistance:
                    target.Resistance += damage.Amount;
                    break;
                case EffectTargetAttribute.Damage:
                    target.Damage += damage.Amount;
                    break;
                case EffectTargetAttribute.CriticalChance:
                    target.CriticalChance += damage.Amount;
                    break;
                case EffectTargetAttribute.CriticalMultiplier:
                    target.CriticalMultiplier += damage.Amount;
                    break;
                case EffectTargetAttribute.Speed:
                    target.Speed += damage.Amount;
                    break;
                default:
                    break;
            }
        }
    }
}
