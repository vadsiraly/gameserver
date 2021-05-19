using GameServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces.Events
{
    public class AttackEventArgs
    {
        public AttackEventArgs(IUnit caster, List<ITargetable> target, IAbility basicAttack)
        {
            Caster = caster;
            Target = target;
            BasicAttack = basicAttack;
        }

        public IUnit Caster { get; private set; }
        public List<ITargetable> Target { get; private set; }
        public IAbility BasicAttack { get; private set; }
    }
}
