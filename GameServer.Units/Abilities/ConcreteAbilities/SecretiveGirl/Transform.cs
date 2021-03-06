using GameServer.Model.Abilities.Damages;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.ConcreteAbilities.SecretiveGirl
{
    public class Transform : Ability
    {
        public Transform(Unit owner, Random random) : base(owner, random)
        {
            Id = 3;
            Reference = "secretive_girl_transform";
            Name = "Transform";

            IsActive = true;

            ManaCost = 20;
            Cooldown = 999;
            _activeCooldown = 4;

            Damage = Damage.Zero;
            CanCriticalHit = false;

            HealthBonus = 150;
            DamageBonus = new Damage(physical: 25);
            ArmorBonus = 100;
            ResistanceBonus = 100;
            CriticalChanceBonus = 0.2;

            Description = $"After {_activeCooldown} rounds {Name} transforms, increasing her health by {HealthBonus}, damage by {DamageBonus}, armor by {ArmorBonus}, resistance by {ResistanceBonus}, and critical chance by {CriticalChanceBonus * 100}%.";
        }

        public double HealthBonus { get; private set; }
        public Damage DamageBonus { get; private set; }
        public double ArmorBonus { get; private set; }
        public double ResistanceBonus { get; private set; }
        public double CriticalChanceBonus { get; private set; }

        public override void Use(List<Unit> targets)
        {
            BeforeAbilityUse(new AbilityUseEventArgs(Owner, targets, ModifiedDamage.Zero));

            Console.WriteLine($"{Owner.Name} has transformed into a Horrid Monstrosity.");

            Owner.MaxHealth += HealthBonus;
            Owner.BasicAttack.Damage.Add(DamageBonus);
            Owner.Armor += ArmorBonus;
            Owner.Resistance += ResistanceBonus;
            Owner.CriticalHitChance += CriticalChanceBonus;
            Owner.Name = Owner.Name.Replace("Secretive Girl", "Horrid Monstrosity");

            AfterAbilityUse(new AbilityUseEventArgs(Owner, targets, ModifiedDamage.Zero));

            _activeCooldown = Cooldown;

        }
    }
}
