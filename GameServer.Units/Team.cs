using GameServer.Interfaces;
using GameServer.Interfaces.Snapshots;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;

namespace GameServer.Model
{
    [Serializable]
    public class Team : ITeam
    {
        public List<IUnit> Units { get; set; } = new List<IUnit>();

        public void Add(IUnit u)
        {
            if (Units.Count < 6)
            {
                Units.Add(u);
                u.SetTeam(this);
            }
        }

        public bool IsMember(IUnit unit)
        {
            return Units.Contains(unit);
        }

        public TeamSnapshot Snapshot()
        {
            var snapshot = new TeamSnapshot();
            foreach (var unit in Units)
            {
                snapshot.Units.Add(unit.Snapshot());
            }
            return snapshot;
        }
    }
}
