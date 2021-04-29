using GameServer.Model.Abilities.Effects;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities
{
    public class BasicAttack : IAbility
    {
        private Random _random;
        public BasicAttack(Unit owner, Random random)
        {
            Owner = owner;
            _random = random;
        }

        public Unit Owner { get; private set; }

        public int Id => 0;

        public string Reference => "basic_attack";

        public string Name => "Basic Attack";

        public int ManaCost => 0;

        public int Cooldown => 0;

        public int Damage => 15;

        public DamageType DamageType => DamageType.Physical;

        public List<Effect> Effects => null;

        public void Use(List<Unit> targets)
        {
            foreach(var target in targets)
            {
                target.TakeDamage(Owner, Damage);
            }
        }
    }
}
