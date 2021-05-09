using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    public class Team
    {
        public List<Unit> Units { get; set; } = new List<Unit>();

        public void Add(Unit u)
        {
            if (Units.Count < 6)
            {
                Units.Add(u);
                u.SetTeam(this);
            }
        }
    }
}
