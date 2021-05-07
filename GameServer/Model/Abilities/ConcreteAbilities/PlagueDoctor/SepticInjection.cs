using GameServer.Model.Abilities.Effects;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities.PlagueDoctor
{
    public class SepticInjection : Ability
    {
        public SepticInjection(Unit owner, Random random) : base(owner, random)
        {
            Id = 4;
            Reference = "plague_doctor_septic_injection";
            Name = "Septic Injection";

            IsActive = false;

            ManaCost = 0;
            Cooldown = 1;

            Damage = 0;
            DamageType = DamageType.Undefined;
            CanCriticalHit = false;

            EffectChance = 1;
            EffectDuration = 4;
            EffectDamage = 2;
            EffectDamageType = DamageType.Magical;
            EffectMaxStack = 5;

            Description = $"{Name} casuses {owner.Name}'s attacks to inflict {EffectDamage} {EffectDamageType} damage at the end of the round for {EffectDuration} rounds. This effect stacks up to {EffectMaxStack}.";

            Debuffs.Add(new DamageOverTimeEffect(this, Name, EffectDuration, EffectDamage, EffectDamageType, EffectMaxStack, _random));
            Owner.BasicAttack.AfterAbilityUseEvent += BasicAttack_AfterAbilityUseEvent;
        }

        public int EffectDuration { get; private set; }
        public double EffectDamage { get; private set; }
        public DamageType EffectDamageType { get; private set; }
        public int EffectMaxStack { get; private set; }

        private void BasicAttack_AfterAbilityUseEvent(object sender, AbilityUseEventArgs e)
        {
            foreach (var target in e.Target)
            {
                foreach (var debuff in Debuffs)
                {
                    target.AddDebuff(this, debuff);
                }
            }
        }
    }
}
