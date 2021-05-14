using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Damages;
using GameServer.Model.Snapshots;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battles.Recording
{
    public enum ActionType
    {
        Ability,
        Effect
    }

    public class Action
    {
        public ActionType ActionType { get; set; }
        public UnitSnapshot Caster { get; set; }
        public UnitSnapshot Target { get; set; }
        public string AbilityRef { get; set; }
        public CombinedDamageSnapshot Damage { get; set; }
    }
}
