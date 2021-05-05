using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities
{
    public enum DamageType
    {
        Undefined,
        Physical,
        Magical,
        Pure,
        Composite
    }

    public class Damage
    {
        public Damage(double value, DamageType type)
        {
            Value = value;
            Type = type;
        }

        public DamageType Type { get; private set; }
        public double Value { get; private set; }
    }
}
