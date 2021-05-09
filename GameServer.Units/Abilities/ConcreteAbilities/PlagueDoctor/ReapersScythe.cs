﻿using GameServer.Model.Abilities.Effects;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities.PlagueDoctor
{
    public class ReapersScythe : Ability
    {
        public ReapersScythe(Unit owner, Random random) : base(owner, random)
        {
            Id = 4;
            Reference = "plague_doctor_reapers_scythe";
            Name = "Reaper's Scythe";

            IsActive = true;

            ManaCost = 0;
            Cooldown = 2;

            Damage = 0;
            DamageType = DamageType.Undefined;
            CanCriticalHit = false;

            EffectChance = 1;
            EffectDuration = 5;
            EffectDamage = 1;
            EffectDamageType = DamageType.Magical;
            EffectMaxStack = 99;

            Description = $"{Owner.Name} swings his Scythe and covers each enemy unit with radioactive dust. Every affected unit suffers {EffectDamage} {EffectDamageType} damage at the end of the turn for {EffectDuration} turns. This effect stacks indefinitely.";

            Debuffs.Add(new DamageOverTimeEffect(this, Name, EffectDuration, EffectDamage, EffectDamageType, EffectMaxStack, _random));
        }

        public int EffectDuration { get; private set; }
        public double EffectDamage { get; private set; }
        public DamageType EffectDamageType { get; private set; }
        public int EffectMaxStack { get; private set; }

        public override void Use(List<Unit> targets)
        {
            BeforeAbilityUse(new AbilityUseEventArgs(Owner, targets, new AbilityDamage(Abilities.Damage.Undefined)));

            var enemyTeam = targets.FirstOrDefault()?.Team.Units ?? new List<Unit>();
            foreach (var unit in enemyTeam)
            {
                foreach (var debuff in Debuffs)
                {
                    unit.AddDebuff(this, debuff);
                }
            }

            _activeCooldown = Cooldown;

            AfterAbilityUse(new AbilityUseEventArgs(Owner, targets, new AbilityDamage(Abilities.Damage.Undefined)));
        }
    }
}