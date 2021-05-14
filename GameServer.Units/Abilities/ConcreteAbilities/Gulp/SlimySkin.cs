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
    public class SlimySkin : Ability
    {
        public SlimySkin(Unit owner, Random random) : base(owner, random)
        {
            Id = 0;

            Reference = "gulp_slimy_skin";
            Name = "Slimy Skin";

            IsActive = false;

            ManaCost = 0;
            Cooldown = 0;

            Damage = Damage.Zero;
            CanCriticalHit = true;

            EffectChance = 0.2;
            Debuffs.Add(new StatusEffect(this, "Slimy Skin", 2, Status.Blinded, EffectChance, random));

            owner.AfterDamagedEvent += Owner_AfterDamagedEvent;

            Description = $"When {Name} is attacked, it has a chance to cover the attacker in slime, causing them to become blinded making them miss their basic attacks 40% of the time.";
        }

        private void Owner_AfterDamagedEvent(object sender, DamagedEventArgs e)
        {
            foreach(var debuff in Debuffs)
            {
                debuff.ApplyEffect(e.Source.Owner);
            }
        }
    }
}
