using GameServer.Interfaces.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces
{
    public interface IModifiedDamage
    {
        (IDamageSource Source, IDamage Damage) BaseDamage { get; }
        List<(IDamageSource Source, IDamage Damage)> Modifications { get;  }

        void AddReduction(double armor, double resistance);

        IDamage Aggregate();

        static IModifiedDamage Zero { get; }

        ModifiedDamageSnapshot Snapshot();
    }
}
