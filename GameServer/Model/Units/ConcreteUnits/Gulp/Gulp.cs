using GameServer.Model.Abilities;
using GameServer.Model.Abilities.ConcreteAbilities;
using GameServer.Model.Abilities.ConcreteAbilities.Gulp;
using GameServer.Model.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units.ConcreteUnits.Gulp
{
    public class Gulp : Unit
    {
        private double health;
        private double mana;
        private double speed;

        private Dictionary<Status, int> statuses = new Dictionary<Status, int>();

        private double armor;
        private double resistance;

        private double criticalHitChance;
        private double criticalHitMultiplier;

        public Gulp(Random random)
        {
            Health = 50;
            Mana = 100;
            Speed = 80;

            Armor = 100;
            Resistance = 100;

            CriticalHitChance = 0.2;
            CriticalHitMultiplier = 2;

            BasicAttack = new BasicAttack(this, random);
            Abilities.Add(new DevourWeapon(this, random));
        }

        public override string Name => "Gulp";
        public override double Health { get => health; protected set => health = value; }
        public override double Mana { get => mana; protected set => mana = value; }
        public override double Speed { get => speed; protected set => speed = value; }

        public override Dictionary<Status, int> Statuses { get => statuses; protected set => statuses = value; } 

        public override double Armor { get => armor; protected set => armor = value; }
        public override double Resistance { get => resistance; protected set => resistance = value; }

        public override double CriticalHitChance { get => criticalHitChance; protected set => criticalHitChance = value; }
        public override double CriticalHitMultiplier { get => criticalHitMultiplier; protected set => criticalHitMultiplier = value; }

        public override IAbility BasicAttack { get; protected set; }
    }
}
