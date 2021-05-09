using GameServer.Model.Abilities;
using GameServer.Model.Abilities.ConcreteAbilities;
using GameServer.Model.Abilities.ConcreteAbilities.PlagueDoctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units.ConcreteUnits
{
    public class PlagueDoctor : Unit
    {
        public PlagueDoctor(Random random) : base(random)
        {
            Name = $"PlagueDoctor{random.Next(0, 1000)}";

            MaxHealth = 80;
            MaxMana = 100;
            Mana = 0;
            Speed = 110;

            Armor = 40;
            Resistance = 40;

            CriticalHitChance = 0.15;
            CriticalHitMultiplier = 1.5;

            BasicAttack = new BasicAttack(this, 10, DamageType.Magical, random);
            Abilities.Add(new ReapersScythe(this, random));
            Abilities.Add(new SepticInjection(this, random));
        }
    }
}
