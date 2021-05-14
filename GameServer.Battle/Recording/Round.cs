using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battles.Recording
{
    public class Round
    {
        public List<Tick> Ticks { get; private set; } = new List<Tick>();
    }
}
