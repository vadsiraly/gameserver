using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.Effects
{
    public enum Status
    {
        None,
        Disarmed,
        Weakened,
        Silenced,
        Stunned,
        Vulnerable,
        Dazed,
        Blind
    }

    public abstract class Effect
    {
        public Unit Owner { get; protected set; }
        public abstract string Name { get; }
        public abstract int Duration { get; protected set; }

        public abstract void ApplyEffect(IUnit target);

        public abstract void RemoveEffect(IUnit target);

        public void Tick(IUnit target)
        {
            if (--Duration <= 0)
            {
                RemoveEffect(target);
            }
        }
    }
}
