using GameServer.Model.Abilities.Damages;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities.Lyra
{
    public class DivineAssistance : Ability
    {
        public DivineAssistance(Unit owner, Random random) : base(owner, random)
        {
            Id = 4;
            Reference = "lyra_divine_assistance";
            Name = "Divine Assistance";

            IsActive = false;

            ManaCost = 0;
            Cooldown = 1;

            Damage = new Damage();
            CanCriticalHit = false;

            BonusDamagePercentage = 0.1;
            Description = $"Divine Assistance enhances {Name}'s basic attacks. Adding an extra {EffectChance * 10}% pure damage. This bonus is not affected by critical hits.";

            Owner.BasicAttack.BeforeAbilityUseEvent += BasicAttack_BeforeAbilityUseEvent;
        }

        public double BonusDamagePercentage { get; private set; }

        private void BasicAttack_BeforeAbilityUseEvent(object sender, AbilityUseEventArgs e)
        {
            var damage = new Damage(pure: e.CombinedDamage.BaseDamage.Damage.Sum * BonusDamagePercentage);
            e.CombinedDamage.DamageCollection.Add((this, damage));
        }
    }
}
