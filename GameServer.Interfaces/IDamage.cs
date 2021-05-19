using GameServer.Interfaces.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces
{
    public interface IDamage
    {
        double Physical { get; set; }
        double Magical { get; set; }
        double Composite { get; set; }
        double Pure { get; set; }

        bool IsCritical { get; set; }

        double Sum { get; }

        bool IsZero { get; }

        IDamage TryCrit(double criticalChance, double criticalMultiplier, Random random);

        public void Add(IDamage other);

        public void Multiply(double amount);

        public static IDamage Zero { get; }

        public string ToString();

        public DamageSnapshot Snapshot();
    }
}
