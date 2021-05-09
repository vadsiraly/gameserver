using GameServer.Model.Abilities;
using GameServer.Model.Abilities.ConcreteAbilities;
using GameServer.Model.Abilities.ConcreteAbilities.Gulp;
using GameServer.Model.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units.ConcreteUnits
{
    public class Gulp : Unit
    {
        public Gulp(Random random) : base(random)
        {
            Name = $"Gulp{random.Next(0, 1000)}";

            MaxHealth = 75;
            MaxMana = 100;
            Mana = 0;

            Speed = 80;

            Armor = 100;
            Resistance = 100;

            CriticalHitChance = 0.2;
            CriticalHitMultiplier = 2;

            BasicAttack = new BasicAttack(this, 15, DamageType.Physical, random);
            Abilities.Add(new DevourWeapon(this, random));
            Abilities.Add(new SlimySkin(this, random));
        }
    }
}
