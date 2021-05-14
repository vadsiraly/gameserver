﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Snapshots
{
    public class CombinedDamageSnapshot
    {
        public (string Source, DamageSnapshot Damage) BaseDamage { get; set; }
        public List<(string Source, DamageSnapshot Damage)> DamageCollection { get; set; }
    }
}
