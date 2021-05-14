using GameServer.Model.Snapshots;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;

namespace GameServer.Model
{
    [Serializable]
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
