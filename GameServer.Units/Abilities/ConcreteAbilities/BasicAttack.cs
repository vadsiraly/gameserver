﻿using GameServer.Damages;
using GameServer.Interfaces;
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
        public BasicAttack(IUnit owner, IDamage damage, Random random) : base(owner, random)
        {
            Id = 0;

            Reference = "basic_attack";
            Name = "Basic Attack";

            Damage = damage;

            CanCriticalHit = true;
        }
    }
}
