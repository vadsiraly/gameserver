using GameServer.Model.Abilities.Effects;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities
{
    public class BasicAttack : Ability
    {
        public BasicAttack(Unit owner, Random random) : base(owner, random)
        {
            Id = 0;

            Reference = "basic_attack";
            Name = "Basic Attack";

            IsActive = true;

            ManaCost = 0;
            Cooldown = 0;

            Damage = 15;
            DamageType = DamageType.Physical;
            CanCriticalHit = true;
        }
    }
}
