using GameServer.Model.Abilities.Damages;
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
        public AbilityUseEventArgs(Unit caster, List<Unit> target, CombinedDamage combinedDamage)
        {
            Caster = caster;
            Target = target;
            CombinedDamage = combinedDamage;
        }

        public Unit Caster { get; private set; }
        public List<Unit> Target { get; private set; }
        public CombinedDamage CombinedDamage { get; private set; }
    }
}
