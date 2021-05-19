using GameServer.Damages;
using GameServer.Interfaces;
using GameServer.Interfaces.Events;
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

            Damage = new Damage(physical: 18);
            CanCriticalHit = true;

            DamageSelfHealPercentage = 0.1;

            Description = $"Deals {Damage} damage to the target. {Name} heals herself by 10% of the damage dealt.";
        }

        public double DamageSelfHealPercentage { get; private set; }

        public override void Use(List<ITargetable> targets)
        {
            ModifiedDamage modifiedDamage;
            if (CanCriticalHit)
            {
                modifiedDamage = new ModifiedDamage((this, Damage.TryCrit(Owner.CriticalHitChance, Owner.CriticalHitMultiplier, _random)));
            }
            else
            {
                modifiedDamage = new ModifiedDamage((this, Damage));
            }

            BeforeAbilityUse(new AbilityUseEventArgs(Owner, targets, modifiedDamage));

            var reducedDamage = ModifiedDamage.Zero;
            foreach (var target in targets)
            {
                reducedDamage = target.TakeDamage(this, modifiedDamage);

                var selfHeal = new ModifiedDamage((this, new Damage(pure: reducedDamage.Aggregate().Sum * DamageSelfHealPercentage)));
                Owner.Heal(this, selfHeal);

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

            AfterAbilityUse(new AbilityUseEventArgs(Owner, targets, modifiedDamage));
        }
    }
}
