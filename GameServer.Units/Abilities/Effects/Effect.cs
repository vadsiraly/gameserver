using GameServer.Interfaces;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.Effects
{
    public abstract class Effect : IEffect
    {
        protected Random _random;

        public Effect(IAbility source, Random random)
        {
            Source = source;
            _random = random;
        }

        public IAbility Source { get; protected set; }
        public string Name { get; protected set; }
        public int MaxStack { get; protected set; }
        public int Duration { get; set; }

        public abstract void ApplyEffect(ITargetable target);

        public abstract void RemoveEffect(ITargetable target);

        public abstract void Tick(ITargetable target, int stack);
    }
}
