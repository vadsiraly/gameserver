using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.Damages
{
    public class DamageReduction
    {
        public DamageReduction() : this(0, 0) { }

        public DamageReduction(double armor, double resistance)
        {
            Armor = armor;
            Resistance = resistance;
        }

        public double Armor { get; set; }
        public double Resistance { get; private set; }

        public static DamageReduction None => new DamageReduction(); 
    }
}
