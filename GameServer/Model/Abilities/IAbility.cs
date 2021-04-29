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
    public enum DamageType
    {
        Physical,
        Magical,
        Pure,
        Composite
    }

    public interface IAbility
    {
        Unit Owner { get; }
        int Id { get; }
        string Reference { get; }
        string Name { get; }
        int ManaCost { get; }
        int Cooldown { get; }
        int Damage { get; }
        DamageType DamageType { get; }
        List<Effect> Effects { get; }

        void Use(List<Unit> targets);
    }
}
