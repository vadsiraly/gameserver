using GameServer.Model.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    public enum EffectGroupTarget
    {
        Self,
        RandomFriendly,
        RandomEnemy,
        AllEnemy,
        AllFriendly
    }

    public class EffectGroup
    {
        public EffectGroupTarget Target { get; set; } = EffectGroupTarget.RandomEnemy;
        public int TargetCount { get; set; } = 1;
        public List<Effect> Effects { get; set; } = new List<Effect>();
    }
}
