using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces.Events
{
    public class RoundEventArgs
    {
        public RoundEventArgs(int round)
        {
            Round = round;
        }

        public int Round { get; private set; }
    }
}
