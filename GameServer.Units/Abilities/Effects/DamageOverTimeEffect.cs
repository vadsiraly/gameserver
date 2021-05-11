using GameServer.Model.Abilities.Damages;
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
        public DamageOverTimeEffect(Ability source, string name, int duration, Damage damage, int maxStack, Random random) : base(source, random)
        {
            Name = name;
            Duration = duration;
            Damage = damage;
            MaxStack = maxStack;
        }

        public Damage Damage { get; private set; }

        public override void ApplyEffect(Unit target)
        {
            target.AddDebuff(this);
        }

        public override void RemoveEffect(Unit target)
        {
            target.RemoveDebuff(this);
        }

        public override void Tick(Unit target, int stack)
        {
            var combinedDamage = new CombinedDamage((Source, Damage * stack));
            target.TakeEffectDamage(Source, combinedDamage);

            if (--Duration <= 0)
            {
                RemoveEffect(target);
            }
        }
    }
}
