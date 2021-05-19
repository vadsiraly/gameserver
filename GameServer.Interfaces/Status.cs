using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces
{
    public enum Status
    {
        None,
        Disarmed,
        Weakened,
        Silenced,
        Stunned,
        Vulnerable,
        Dazed,
        Blinded
    }
}
