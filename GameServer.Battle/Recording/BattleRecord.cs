using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using GameServer.Interfaces;
using GameServer.Interfaces.Snapshots;

namespace GameServer.Battles.Recording
{
    public class BattleRecord
    {
        public TeamSnapshot Attackers { get; set; }
        public TeamSnapshot Defenders { get; set; }
        public List<Round> Rounds { get; private set; } = new List<Round>();
    }
}
