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
        public AbilityDamage(Damage damage)
        {
            DamagePart = damage;
        }

        public Damage DamagePart { get; set; } = Damage.Undefined;
        public Damage CriticalPart { get; set; } = Damage.Undefined;
        public List<Damage> BonusDamagePart { get; set; } = new List<Damage>();

        public List<Damage> DamageList
        {
            get
            {
                var result = new List<Damage>();
                result.Add(DamagePart ?? Damage.Undefined);
                result.Add(CriticalPart ?? Damage.Undefined);

                foreach(var part in BonusDamagePart) 
                {
                    result.Add(part ?? Damage.Undefined);
                }

                return result.GroupBy(x => x.Type).Select(x => new Damage(x.Sum(y => y.Value), x.Key)).ToList();
            }
        }
    }
}
