using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces
{
    public interface IDamageSource
    {
        int Id { get; }
        string Reference { get; }
        string Name { get; }
    }
}
