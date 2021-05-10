using GameServer.Model.Abilities;
using GameServer.Model.Abilities.ConcreteAbilities;
using GameServer.Model.Abilities.ConcreteAbilities.SecretiveGirl;
using GameServer.Model.Abilities.Damages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units.ConcreteUnits
{
    public class SecretiveGirl : Unit
    {
        public SecretiveGirl(Random random) : base(random)
        {
            Name = $"Secretive Girl{random.Next(0, 1000)}";

            MaxHealth = 40;
            MaxMana = 100;
            Mana = 0;
            Speed = 40;

            Armor = 25;
            Resistance = 25;

            CriticalHitChance = 0;
            CriticalHitMultiplier = 3;

            BasicAttack = new BasicAttack(this, new Damage(physical: 5), random);
            Abilities.Add(new Transform(this, random));
        }
    }
}
