using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Snapshots
{
    public class UnitSnapshot
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public double Health { get; set; }
        public double MaxHealth { get; set; }
        public double Mana { get; set; }
        public double MaxMana { get; set; }
        public double Speed { get; set; }
        public double Armor { get; set; }
        public double Resistance { get; set; }
        public double CriticalHitChance { get; set; }
        public double CriticalHitMultiplier { get; set; }
    }
}
