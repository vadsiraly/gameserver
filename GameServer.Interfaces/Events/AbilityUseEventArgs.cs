using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces.Events
{
    public class AbilityUseEventArgs
    {
        public AbilityUseEventArgs(IUnit caster, List<ITargetable> target, IModifiedDamage modifiedDamage)
        {
            Caster = caster;
            Target = target;
            ModifiedDamage = modifiedDamage;
        }

        public IUnit Caster { get; private set; }
        public List<ITargetable> Target { get; private set; }
        public IModifiedDamage ModifiedDamage { get; private set; }
    }
}
