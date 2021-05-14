using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Snapshots
{
    public class DamageSnapshot
    {
        public double Physical { get; set; }
        public double Magical { get; set; }
        public double Composite { get; set; }
        public double Pure { get; set; }

        public double PhysicalReduced { get; set; }
        public double MagicalReduced { get; set; }
        public double CompositeReduced { get; set; }
        public double PureReduced { get; set; }

        public bool IsCritical { get; set; }
    }
}
