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
        Blinded
    }

    public abstract class Effect
    {
        protected Random _random;

        public Effect(Unit owner, Random random)
        {
            Owner = owner;
            _random = random;
        }

        public Unit Owner { get; protected set; }
        public string Name { get; protected set; }
        public double Chance { get; protected set; }

        public abstract void ApplyEffect(Unit target);

        public abstract void RemoveEffect(Unit target);

        public abstract void Tick(Unit target);
    }
}
