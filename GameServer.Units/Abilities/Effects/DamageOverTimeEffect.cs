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
            var combinedDamage = CombinedDamage.Zero;
            combinedDamage.DamageCollection.Add((Source, Damage));
            target.TakeEffectDamage(Source, combinedDamage);

            if (--Duration <= 0)
            {
                RemoveEffect(target);
            }
        }
    }
}
