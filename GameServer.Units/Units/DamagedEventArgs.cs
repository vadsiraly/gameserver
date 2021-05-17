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
        public DamagedEventArgs(Ability source, ModifiedDamage modifiedDamage)
        {
            Source = source;
            ModifiedDamage = modifiedDamage;
        }

        public Ability Source { get; private set; }
        public ModifiedDamage ModifiedDamage { get; private set; }
    }
}
