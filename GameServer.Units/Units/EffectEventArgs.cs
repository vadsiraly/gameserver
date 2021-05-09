using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units
{
    public class EffectEventArgs
    {
        public EffectEventArgs(Unit self, Effect effect)
        {
            Self = self;
            Effect = effect;
        }

        public Unit Self { get; private set; }
        public Effect Effect { get; private set; }
    }
}
