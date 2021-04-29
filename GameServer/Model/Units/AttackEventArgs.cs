using GameServer.Model.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units
{
    public class AttackEventArgs
    {
        public AttackEventArgs(Unit caster, List<Unit> target, IAbility basicAttack)
        {
            Caster = caster;
            Target = target;
            BasicAttack = basicAttack;
        }

        public Unit Caster { get; private set; }
        public List<Unit> Target { get; private set; }
        public IAbility BasicAttack { get; private set; }
    }
}
