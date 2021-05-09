﻿using GameServer.Model.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units
{
    public class AttackedEventArgs
    {
        public AttackedEventArgs(Unit attacker, AbilityDamage damage)
        {
            Attacker = attacker;
            Damage = damage;
        }

        public Unit Attacker { get; private set; }
        public AbilityDamage Damage { get; private set; }
    }
}