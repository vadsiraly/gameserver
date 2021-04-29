using GameServer.Model.Abilities.Effects;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities.Gulp
{
    public class DevourWeapon : IAbility
    {
        private Random _random;
        public DevourWeapon(Unit owner, Random random)
        {
            Owner = owner;
            _random = random;
        }

        public Unit Owner { get; private set; }
        public int Id => 1;
        public string Reference => "gulp_devour_weapon";
        public string Name => "Devour Weapon";
        public int ManaCost => 20;
        public int Cooldown => 4;
        public int Damage => 15;
        public DamageType DamageType => DamageType.Physical;
        public List<Effect> Effects { get; private set; } = new List<Effect>();

        public void Use(List<Unit> targets)
        {
            throw new NotImplementedException();
        }
    }
}
