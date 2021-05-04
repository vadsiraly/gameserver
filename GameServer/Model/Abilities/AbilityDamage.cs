using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities
{
    public class AbilityDamage
    {
        public AbilityDamage(Damage damage, Damage crit, Damage bonus, Damage bonusNoCrit)
        {
            DamagePart = damage;
            CriticalPart = crit;
            BonusDamagePart = bonus;
            BonusDamageNoCritPart = bonusNoCrit;
        }

        public Damage DamagePart { get; private set; }
        public Damage CriticalPart { get; private set; }
        public Damage BonusDamagePart { get; private set; }
        public Damage BonusDamageNoCritPart { get; private set; }

        public List<Damage> DamageList
        {
            get
            {
                var result = new List<Damage>();
                result.Add(DamagePart ?? new Damage(0, DamageType.Physical));
                result.Add(CriticalPart ?? new Damage(0, DamageType.Physical));
                result.Add(BonusDamagePart ?? new Damage(0, DamageType.Physical));
                result.Add(BonusDamageNoCritPart ?? new Damage(0, DamageType.Physical));

                return result.GroupBy(x => x.Type).Select(x => new Damage(x.Sum(y => y.Value), x.Key)).ToList();
            }
        }
    }
}
