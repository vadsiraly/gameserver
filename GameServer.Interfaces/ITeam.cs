using GameServer.Interfaces.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces
{
    public interface ITeam
    {
        public List<IUnit> Units { get; set; }

        public void Add(IUnit u);

        public bool IsMember(IUnit unit);

        public TeamSnapshot Snapshot();
    }
}
