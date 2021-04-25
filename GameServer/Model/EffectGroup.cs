using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    public enum EffectGroupTarget
    {
        RandomFriendly,
        RandomEnemy,
        AllEnemy,
        AllFriendly
    }

    public class EffectGroup
    {
        public EffectGroupTarget Target = EffectGroupTarget.RandomEnemy;
        public int TargetCount { get; set; } = 1;
        public List<Effect> Effects { get; set; } = new List<Effect>();
    }
}
