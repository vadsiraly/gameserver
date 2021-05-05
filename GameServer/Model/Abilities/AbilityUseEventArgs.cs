using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities
{
    public class AbilityUseEventArgs
    {
        public AbilityUseEventArgs(Unit caster, List<Unit> target, AbilityDamage abilityDamage)
        {
            Caster = caster;
            Target = target;
            AbilityDamage = abilityDamage;
        }

        public Unit Caster { get; private set; }
        public List<Unit> Target { get; private set; }
        public AbilityDamage AbilityDamage { get; private set; }
    }
}
