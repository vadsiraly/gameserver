using GameServer.Model.Abilities.Damages;
using GameServer.Model.Abilities.Effects;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities.Gulp
{
    public class DevourWeapon : Ability
    {
        public DevourWeapon(Unit owner, Random random) : base(owner, random)
        {
            Id = 1;
            Reference = "gulp_devour_weapon";
            Name = "Devour Weapon";

            IsActive = true;

            ManaCost = 20;
            Cooldown = 6;
            Damage = new Damage(physical: 30);

            CanCriticalHit = true;

            EffectChance = 0.2;
            Debuffs.Add(new StatusEffect(this, "Devour weapons", 4, Status.Disarmed, EffectChance, random));

            Description = $"Deals {Damage} damage to the target. Has {EffectChance * 100}% chance to devour the target's weapons, disarming them for 4 rounds.";
        }
    }
}
