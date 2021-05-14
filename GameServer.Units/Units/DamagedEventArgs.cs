using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Damages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units
{
    public class DamagedEventArgs
    {
        public DamagedEventArgs(Ability source, CombinedDamage combinedDamage)
        {
            Source = source;
            CombinedDamage = combinedDamage;
        }

        public Ability Source { get; private set; }
        public CombinedDamage CombinedDamage { get; private set; }
    }
}
