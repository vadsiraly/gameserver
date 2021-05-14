using GameServer.Model.Abilities;
using GameServer.Model.Abilities.Damages;
using GameServer.Model.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units
{
    public class StatusEventArgs
    {
        public StatusEventArgs(Ability source, Status status)
        {
            Source = source;
            Status = status;
        }

        public Ability Source { get; private set; }
        public Status Status { get; private set; }
    }
}
