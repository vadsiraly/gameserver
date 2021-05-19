using GameServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces.Snapshots
{
    public class ModifiedDamageSnapshot
    {
        public (string Source, DamageSnapshot Damage) BaseDamage { get; set; }
        public List<(string Source, DamageSnapshot Damage)> Modifications { get; set; }
    }
}
