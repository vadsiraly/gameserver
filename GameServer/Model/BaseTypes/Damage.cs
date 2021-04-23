using GameServer.Enumerations.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.BaseTypes
{
    public class Damage
    {
        public DamageType Type { get; set; }
        public double Amount { get; set; }
        public static Damage operator*(Damage damage, double multiplier)
        {
            return new Damage
            { 
                Amount = damage.Amount * multiplier, 
                Type = damage.Type 
            };
        }
    }
}
