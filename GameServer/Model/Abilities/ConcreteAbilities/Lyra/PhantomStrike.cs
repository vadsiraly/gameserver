using GameServer.Model.Abilities.Effects;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities.Lyra
{
    public class PhantomStrike : Ability
    {
        public PhantomStrike(Unit owner, Random random) : base(owner, random)
        {
            Id = 3;
            Reference = "lyra_phantom_strike";
            Name = "Phantom Strike";

            IsActive = true;

            ManaCost = 20;
            Cooldown = 2;

            Damage = 18;
            DamageType = DamageType.Physical;
            CanCriticalHit = true;

            DamageSelfHealPercentage = 0.1;

            Description = $"Deals {Damage} {DamageType} damage to the target. {Name} heals herself by 10% of the damage dealt.";
        }

        public double DamageSelfHealPercentage { get; private set; }

        public override void Use(List<Unit> targets)
        {
            var damage = new AbilityDamage(new Damage(Damage, DamageType));

            var criticalDamage = 0d;
            if (CanCriticalHit && _random.NextDouble() < Owner.CriticalHitChance)
            {
                criticalDamage = Damage * Owner.CriticalHitMultiplier - Damage;
            }

            damage.CriticalPart = new Damage(criticalDamage, DamageType);

            BeforeAbilityUse(new AbilityUseEventArgs(Owner, targets, damage));

            var actualDamage = 0d;
            foreach (var target in targets)
            {
                actualDamage = target.TakeDamage(Owner, damage);

                foreach (var buff in Buffs)
                {
                    buff.ApplyEffect(target);
                }

                foreach (var debuff in Debuffs)
                {
                    debuff.ApplyEffect(target);
                }
            }

            var selfHeal = new AbilityHealing(actualDamage * DamageSelfHealPercentage, 0, 0, 0);
            Owner.Heal(Owner, selfHeal);

            _activeCooldown = Cooldown;

            AfterAbilityUse(new AbilityUseEventArgs(Owner, targets, damage));
        }
    }
}
