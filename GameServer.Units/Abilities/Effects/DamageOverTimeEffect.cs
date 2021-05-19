using GameServer.Damages;
using GameServer.Interfaces;
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
        public DamageOverTimeEffect(IAbility source, string name, int duration, IDamage damage, int maxStack, Random random) : base(source, random)
        {
            Name = name;
            Duration = duration;
            Damage = damage;
            MaxStack = maxStack;
        }

        public IDamage Damage { get; private set; }

        public override void ApplyEffect(ITargetable target)
        {
            target.AddDebuff(this);
        }

        public override void RemoveEffect(ITargetable target)
        {
            target.RemoveDebuff(this);
        }

        public override void Tick(ITargetable target, int stack)
        {
            var modifiedDamage = new ModifiedDamage((Source, Damage));
            target.TakeEffectDamage(Source, modifiedDamage);

            if (--Duration <= 0)
            {
                RemoveEffect(target);
            }
        }
    }
}
