using GameServer.Model.Abilities.Effects;
using GameServer.Model.Units;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities
{
    public interface IAbility
    {
        Unit Owner { get; }
        int Id { get; }
        string Reference { get; }
        string Name { get; }

        int ManaCost { get; }
        int Cooldown { get; }
        bool Available { get; }
        double Damage { get; }

        DamageType DamageType { get; }
        bool CanCriticalHit { get; }

        List<Effect> Buffs { get; }
        List<Effect> Debuffs { get; }

        void Tick();

        void Use(List<Unit> targets);
    }
}
