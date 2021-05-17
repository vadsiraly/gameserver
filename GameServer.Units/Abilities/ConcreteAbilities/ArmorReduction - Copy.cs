using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities
{
    public class ArmorReduction : Ability
    {
        public ArmorReduction(Unit owner, Random random) : base(owner, random)
        {
            Id = 3;

            Reference = "target_armor_reduction";
            Name = "Target Armor Reduction";

            Damage = Damages.Damage.Zero;
        }
    }
}
