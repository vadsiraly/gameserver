using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Interfaces;

namespace GameServer.Interfaces.Events
{
    public class StatusEventArgs
    {
        public StatusEventArgs(IAbility source, Status status)
        {
            Source = source;
            Status = status;
        }

        public IAbility Source { get; private set; }
        public Status Status { get; private set; }
    }
}
