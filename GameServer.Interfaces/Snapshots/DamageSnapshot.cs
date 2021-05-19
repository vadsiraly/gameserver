using GameServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces.Snapshots
{
    public class DamageSnapshot
    {
        public double Physical { get; set; }
        public double Magical { get; set; }
        public double Composite { get; set; }
        public double Pure { get; set; }

        public bool IsCritical { get; set; }
    }
}
