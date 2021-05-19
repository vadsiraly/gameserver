using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces
{
    public interface IEffect
    {
        IAbility Source { get; }
        string Name { get; }
        int MaxStack { get; }
        int Duration { get; set; }

        void ApplyEffect(ITargetable target);
        void RemoveEffect(ITargetable target);
        void Tick(ITargetable target, int stack);
    }
}
