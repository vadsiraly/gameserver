using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.Effects
{
    public enum EffectSchedule
    {
        Permanent,
        Continuous,
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

    public enum DamageType
    {
        Physical,
        Magical,
        Pure
    }

    public class Effect
    {
        public EffectSchedule Schedule { get; set; } = EffectSchedule.Permanent;
        public EffectTargetAttribute TargetAttribute { get; set; }

        public int Delay { get; set; } = 0;
        public int Duration { get; set; } = 1;
        public bool Positive { get; set; } = false;
        public bool CanCrit { get; set; } = false;
        public DamageType DamageType { get; set; }

        public EffectValueType ValueType { get; set; } = EffectValueType.Value;
        public double DamageMultiplier { get; set; } = 1;
        public double Percentage { get; set; } = 1;
        public double Value { get; set; } = 0;

        public Effect Clone()
        {
            return (Effect)MemberwiseClone();
        }
    }
}
