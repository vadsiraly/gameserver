using GameServer.Model.Abilities.ConcreteAbilities;
using GameServer.Model.Abilities.ConcreteAbilities.Lyra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units.ConcreteUnits
{
    public class Lyra : Unit
    {
        public Lyra(Random random) : base(random)
        {
            Name = $"Lyra{random.Next(0, 1000)}";

            MaxHealth = 100;
            MaxMana = 100;
            Mana = 0;
            Speed = 80;

            Armor = 60;
            Resistance = 25;

            CriticalHitChance = 0.15;
            CriticalHitMultiplier = 1.5;

            BasicAttack = new BasicAttack(this, random);
            Abilities.Add(new PhantomStrike(this, random));
            Abilities.Add(new DivineAssistance(this, random));
        }
    }
}
