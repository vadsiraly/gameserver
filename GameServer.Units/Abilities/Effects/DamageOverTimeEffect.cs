using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.Effects
{
    public class DamageOverTimeEffect : Effect
    {
        public DamageOverTimeEffect(Ability source, string name, int duration, double damage, DamageType damageType, int maxStack, Random random) : base(source, random)
        {
            Name = name;
            Duration = duration;
            Damage = damage;
            DamageType = damageType;
            MaxStack = maxStack;
        }

        public double Damage { get; }
        public DamageType DamageType { get; }
        public int Duration { get; private set; }
        public int MaxStack { get; private set; }

        public override void ApplyEffect(Unit target)
        {
            if (target.Buffs.Count(x => x.Name == Name && x.Source.Owner.Name == Source.Owner.Name) < MaxStack)
            {
                target.AddDebuff(Source, this);
            }
        }

        public override void RemoveEffect(Unit target)
        {
        }

        public override void Tick(Unit target)
        {
            var abilityDamage = new AbilityDamage(new Damage(Damage, DamageType));
            target.TakeEffectDamage(Source, abilityDamage);

            if (--Duration <= 0)
            {
                RemoveEffect(target);
            }
        }
    }
}
