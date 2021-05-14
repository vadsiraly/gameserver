using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Snapshots
{
    public class TeamSnapshot
    {
        public List<UnitSnapshot> Units { get; private set; } = new List<UnitSnapshot>();
    }
}
