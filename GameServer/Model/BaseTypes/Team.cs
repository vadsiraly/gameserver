using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.BaseTypes
{
    public class Team
    {
        public Unit[] Units { get; set; } = new Unit[6];
    }
}
