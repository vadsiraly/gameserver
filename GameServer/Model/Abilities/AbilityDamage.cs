using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities
{
    public enum AbilityResult
    {
        Hit,
        Miss
    }

    public class AbilityDamage
    {
        public AbilityDamage(Damage damage, Damage crit, Damage bonus, Damage bonusNoCrit, AbilityResult abilityResult)
        {
            DamagePart = damage;
            CriticalPart = crit;
            BonusDamagePart = bonus;
            BonusDamageNoCritPart = bonusNoCrit;
            AbilityResult = abilityResult;
        }

        public AbilityResult AbilityResult { get; set; }
        public Damage DamagePart { get; set; }
        public Damage CriticalPart { get; set; }
        public Damage BonusDamagePart { get; set; }
        public Damage BonusDamageNoCritPart { get; set; }

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
