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
        private int cooldown = 0;

        public DevourWeapon(Unit owner, Random random)
        {
            Owner = owner;
            _random = random;

            Debuffs.Add(new StatusEffect("Devour weapons", 4, Status.Disarmed));
        }

        public Unit Owner { get; private set; }
        public int Id => 1;
        public string Reference => "gulp_devour_weapon";
        public string Name => "Devour Weapon";

        public int ManaCost => 20;
        public int Cooldown => 6;
        public double Damage => 30;

        public bool Available => cooldown == 0;

        public DamageType DamageType => DamageType.Physical;
        public bool CanCriticalHit => true;
        public List<Effect> Buffs { get; private set; } = new List<Effect>();
        public List<Effect> Debuffs { get; private set; } = new List<Effect>();

        public void Tick()
        {
            if (cooldown > 0)
            {
                --cooldown;
            }
        }

        public void Use(List<Unit> targets)
        {
            var criticalDamage = 0d;
            if (CanCriticalHit && _random.NextDouble() < Owner.CriticalHitChance)
            {
                criticalDamage = Damage * Owner.CriticalHitMultiplier - Damage;
            }

            var damage = new AbilityDamage(new Damage(Damage, DamageType), new Damage(criticalDamage, DamageType), null, null);

            foreach (var target in targets)
            {
                target.TakeDamage(Owner, damage);

                foreach (var buff in Buffs)
                {
                    buff.ApplyEffect(target);
                }

                foreach (var debuff in Debuffs)
                {
                    debuff.ApplyEffect(target);
                }
            }

            cooldown = Cooldown;
        }
    }
}
