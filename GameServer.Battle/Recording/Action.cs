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
        EffectDamage,
        ApplyEffect,
        ExpireEffect,
        ApplyStatus,
        ExpireStatus
    }

    public class Action
    {
        public ActionType ActionType { get; set; }
        public string CasterRef { get; set; }
        public string TargetRef { get; set; }
        public string AbilityRef { get; set; }
        public ModifiedDamageSnapshot Damage { get; set; }
    }
}
