using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.BaseTypes
{
    public class Team
    {
        public List<Unit> Units { get; set; } = new List<Unit>();

        public Unit GetRandomAliveUnit(Random random)
        {
            return Units.GetRandomElement(random, x => !x.IsDead());
        }

        public void Add(Unit u)
        {
            if (Units.Count < 6)
            {
                Units.Add(u);
                u.Team = this;
            }
        }
    }
}
