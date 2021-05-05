using GameServer.Model.Abilities.Effects;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities
{
    public abstract class Ability
    {
        protected Random _random;
        protected int _activeCooldown = 0;

        public Ability(Unit owner, Random random)
        {
            Owner = owner;
            _random = random;
        }

        public Unit Owner { get; protected set; }
        public int Id { get; protected set; }
        public string Reference { get; protected set; }
        public string Name { get; protected set; }

        public bool IsActive { get; protected set; }

        public int ManaCost { get; protected set; }
        public int Cooldown { get; protected set; }
        public double Damage { get; protected set; }

        public DamageType DamageType { get; protected set; }
        public bool CanCriticalHit { get; protected set; }

        public List<Effect> Buffs { get; protected set; } = new List<Effect>();
        public List<Effect> Debuffs { get; protected set; } = new List<Effect>();
        public double EffectChance { get; protected set; } = 1;

        public virtual bool Available => _activeCooldown == 0;

        public virtual void Tick()
        {
            if (_activeCooldown > 0)
            {
                --_activeCooldown;
            }
        }

        public virtual void Use(List<Unit> targets)
        {
            if (!IsActive) return;

            var criticalDamage = 0d;
            if (CanCriticalHit && _random.NextDouble() < Owner.CriticalHitChance)
            {
                criticalDamage = Damage * Owner.CriticalHitMultiplier - Damage;
            }

            var damage = new AbilityDamage(new Damage(Damage, DamageType), new Damage(criticalDamage, DamageType), null, null, AbilityResult.Hit);

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

            _activeCooldown = Cooldown;
        }
    }
}
