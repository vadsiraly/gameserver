using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Damages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units
{
    public class AttackedEventArgs
    {
        public AttackedEventArgs(Unit attacker, CombinedDamage combinedDamage)
        {
            Attacker = attacker;
            CombinedDamage = combinedDamage;
        }

        public Unit Attacker { get; private set; }
        public CombinedDamage CombinedDamage { get; private set; }
    }
}
