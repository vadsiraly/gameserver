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

        public static Damage Calculate(Damage damage, Unit target)
        {
            var reducedDamage = new Damage { Amount = damage.Amount, Type = damage.Type };
            switch (damage.Type)
            {
                case DamageType.Physical:
                    reducedDamage.Amount = damage.Amount * (1 - target.Armor / 100);
                    break;
                case DamageType.Magical:
                    reducedDamage.Amount = damage.Amount * (1 - target.Resistance / 100);
                    break;
                case DamageType.Pure:
                    break;
                default:
                    break;
            }

            return reducedDamage;
        }
    }
}
