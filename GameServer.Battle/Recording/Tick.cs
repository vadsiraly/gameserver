using GameServer.Model;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battles.Recording
{
    public class Tick
    {
        public List<Action> Actions { get; set; } = new List<Action>();
    }
}
