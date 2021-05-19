using GameServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities
{
    public class ResistanceReduction : IDamageSource
    {
        public int Id { get; }
        public string Reference { get; }
        public string Name { get; }

        public ResistanceReduction()
        {
            Id = 3;

            Reference = "target_resistance_reduction";
            Name = "Target Resistance Reduction";
        }
    }
}
