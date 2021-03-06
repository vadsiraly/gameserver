using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities
{
    public class UnknownAbility : Ability
    {
        public UnknownAbility(Unit owner, Random random) : base(owner, random)
        {
            Id = 4;

            Reference = "ability_unknown";
            Name = "Unknown ability";

            Damage = Damages.Damage.Zero;
        }
    }
}
