using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities
{
    public class ResistanceReduction : Ability
    {
        public ResistanceReduction(Unit owner, Random random) : base(owner, random)
        {
            Id = 3;

            Reference = "target_resistance_reduction";
            Name = "Target Resistance Reduction";

            Damage = Damages.Damage.Zero;
        }
    }
}
